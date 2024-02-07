using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements the cutscene for the intro.
/// </summary>
public class IntroCutscene : Cutscene {

	[SerializeField] private Dialogue dialogues;
	//	[SerializeField] private string[] errorDialogue1;
	//	[SerializeField] private string[] errorDialogue2;

	//	[SerializeField] private GameObject pedestal;
	//	[SerializeField] private GameObject block;

	[SerializeField] private bool skipCutscenes;

	private const float DEFAULT_CG_WAIT = 5.0f;
	private const float FALLING_CG_WAIT = 6.7f;
	private GameObject player;

	private PlayerMovement playerController;
	private PlayerAttack playerAttack;
	private PickupObject pickupObject;
	private bool approachedBox = false;

	[SerializeField] private TriggerSkyboxBreak skyboxBreak;
	[SerializeField] private TriggerSkyfall skyFall;
	[SerializeField] private TriggerMovement triggerMovement;
	[SerializeField] private TriggerPush triggerPush;
	[SerializeField] private TriggerJump triggerJump;
	[SerializeField] private TriggerCurious triggerCurious;
	[SerializeField] private TriggerSkyFragment triggerSkyFragment;


	[SerializeField] private TriggerDoorSpotted triggerDoorSpotted;
	[SerializeField] private TriggerCantTouch triggerCantTouch;
	[SerializeField] private TriggerBoxFix triggerBoxFix;
	[SerializeField] private InteractableTag interactableTag;
	[SerializeField] private StabilityNumberLineAnimatable stabilityParent;

	[SerializeField] private TutorialHandlerCarryingItems handlerCarryingItems;
	[SerializeField] private TutorialHandlerFixingBoxes handlerFixingBoxes;

	void Start() {
		Init ();
	}


	protected override void Init() {
		this.name = "Intro Cutscene";
		//		pickupObject = pedestal.GetComponent<PickupObject> ();
		base.Init ();

		//		if (this.isTriggered) {
		//			Debug.Log ("Tutorial Cutscene isTriggered : Deactivating pededstal");
		//			this.pedestal.SetActive (false);
		//			foreach (Collider2D collider in GetComponentsInChildren<Collider2D>()) {
		//				collider.enabled = false;
		//			}
		//		}
	}

	public StabilityNumberLineAnimatable GetStabilityParent() {
		// Must not enter this, assign ItemContainerAnimatable in inspector;
		if (this.stabilityParent == null) {
			this.stabilityParent = GameObject.FindObjectOfType<StabilityNumberLineAnimatable> ();
		}
		return this.stabilityParent;
	}

	protected override void disablePlayerControls() {
//		ToggleMobileUI (false);
//		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		playerController.canMove (false);
		playerAttack.canAttack (false);
	}



	private IEnumerator Introduction() {

		if (!this.skipCutscenes) {
			// Sky
			GameController_v7.Instance.GetDialogueManager ().ShowCG ();
			for(int i = 0; i < 6; i++) {
				if (Input.GetButtonDown ("Fire1")) {
					i = 6;
				}
				else {
					yield return new WaitForSecondsRealtime (1.0f);
				}
			}
			GameController_v7.Instance.GetDialogueManager ().HideCG ();

			// Shatter
			GameController_v7.Instance.GetDialogueManager ().ShowCG ();
			SoundManager.Instance.Play (AudibleNames.BreakableBox.GLASS, false);
//			yield return new WaitForSecondsRealtime (4.0f);
			for(int i = 0; i < 4; i++) {
				if (Input.GetButtonDown ("Fire1")) {
					i = 4;
				}
				else {
					yield return new WaitForSecondsRealtime (1.0f);
				}
			}
			GameController_v7.Instance.GetDialogueManager ().HideCG ();

			// Falling
			GameController_v7.Instance.GetDialogueManager ().ShowCG ();
//			yield return new WaitForSecondsRealtime (FALLING_CG_WAIT);
			for(int i = 0; i < 7; i++) {
				if (Input.GetButtonDown ("Fire1")) {
					i = 7;
				}
				else {
					yield return new WaitForSecondsRealtime (1.0f);
				}
			}
			GameController_v7.Instance.GetDialogueManager ().HideCG ();
		}


		// As I looked at the sky
		//		GameController_v7.Instance.GetDialogueManager ().ShowCG ();
		//		yield return new WaitForSecondsRealtime (DEFAULT_CG_WAIT);
		//		GameController_v7.Instance.GetDialogueManager ().HideCG ();



		// Set proper colliders to "Ignore" state
		this.GetSkyboxBreak().Ignore();
		this.GetSkyboxBreak ().HideBlock ();
		this.GetSkyFragmentTrigger ().Ignore ();
		this.GetSkyFragmentTrigger ().HideFragment ();
		this.GetTriggerCurious().Ignore();
		this.GetTriggerCantTouch().Ignore();
		this.GetBoxFixTrigger ().Observe ();

		this.GetStabilityParent ().CloseIdle ();

		// Hide the Help button
		EventBroadcaster.Instance.PostEvent (EventNames.HIDE_HELP_BUTTON);

		enableDialogue ();//Where am I...
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);


		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);


		// Show movement hint
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);
		playerController.canMove (true);
		// Glow arrows
		GameController_v7.Instance.GetTutorialGlowManager ().GlowArrows();


		// Progress when player moves
		while(!this.GetTriggerMovement().IsTriggered()) {
			yield return null;
		}
		// Move goal triggered
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();
		GameController_v7.Instance.GetTutorialGlowManager ().StopArrowGlow();



		// Looks like I can move. Let's try jumping
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);



		// Show jump tutorial when Move goal is triggered
		// Hint: Press jump button
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);
		// Glow jump button
		GameController_v7.Instance.GetTutorialGlowManager ().GlowJump();

		// While player is not yet up ledge
		while(!this.GetTriggerJump().IsTriggered()) {
			yield return null;
		}

		// Move mission completed
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();
		GameController_v7.Instance.GetTutorialGlowManager ().StopJumpGlow();



		// Good. Now let's find a way out of this room
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);






		// Mission: Find a way out
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

		// While player has not spotted door
		while(!this.GetTriggerDoorSpotted().IsTriggered()) {
			yield return null;
		}
		// Success: Find a way out
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();




		// I see a door up ahead, but this platform is too high! May be there's something here that can help me up.
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);




		// Mission: Look for something to help Yuni reach the platform.
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

		// Enable break trigger
		// Break sound
		this.GetSkyboxBreak().Observe(); // Turn the collider on
		while(!this.GetSkyboxBreak().IsTriggered()) {
			yield return null;
		}
		this.GetSkyboxBreak ().Action (); // Play breaking sound



		// What was that noise?
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);



		// Wait for Yuni to spot box
		this.GetTriggerCurious().Observe(); // Turn the collider on
		while(!this.GetTriggerCurious().IsTriggered()) {
			yield return null;
		}

		// Oh, just a box.
		enableDialogue ();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
//		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
//		playerController.canMove (true);


		// Success: Look for something to help Yuni reach the platform.
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();


		// Wait, maybe we can use that to help me reach the platform.
		enableDialogue ();
//		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);




		// Mission: Go near the  box and use the hand button to interact with it
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);
		this.GetTriggerCantTouch ().Observe ();
		this.GetInteractableTag ().Observe ();

		// Glow carry
		GameController_v7.Instance.GetTutorialGlowManager ().GlowCarry();


		// Wait for Yuni to go near the box
		while (!this.GetTriggerCantTouch ().IsCarryPressed ()) {
			yield return null;
		}

		this.GetInteractableTag ().Interact ();
		// Stop carry
		GameController_v7.Instance.GetTutorialGlowManager ().StopGlowCarry();
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();

		// What's wrong with this box? I can't touch it. My hand's going through it as if it's not even there...
		// Wait, I remember! Before I fell, I saw the whole sky break into pieces!
		enableDialogue ();
		//		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);


		// ======================== BROKEN OBJECTS
		// The shattered sky caused objects to break! Broken objects can't be touched.

		this.GetSkyFragmentTrigger ().ShowFragment (); // Show sky fragment
		this.GetSkyFragmentTrigger ().Observe ();

		// What's that blue object over there..
		enableDialogue ();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);


		// Carry Item Graphic
		this.GetHandlerCarryingItems ().Action ();
		while (!this.GetHandlerCarryingItems ().IsFinished ()) {
			yield return null;
		}


		// Mission: Approach the blue object and use the hand button to carry it
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

		// Wait for Yuni to interact with the sky fragment
		while (!this.GetSkyFragmentTrigger ().HasCarriedPiece ()) {
			yield return null;
		}


		// Success: Yuni has carried the piece
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();



		GameController_v7.Instance.GetTutorialGlowManager ().GlowItem();
		// This is a piece of the sky!
		enableDialogue ();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		GameController_v7.Instance.GetTutorialGlowManager ().StopGlowItem();
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);




		// I bet bringing enough sky fragments near that broken object will fix it.
		enableDialogue ();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);



		// ======================== FIXING BOXES
		this.GetHandlerFixingBoxes ().Action ();
		while (!this.GetHandlerFixingBoxes ().IsFinished ()) {
			yield return null;
		}




		// Mission: Press the HAND button to carry and drop the sky fragment. Drop it in the box.
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);
	
		// While box is not yet fixed
		while (!this.GetBoxFixTrigger ().IsTriggered ()) {
			yield return null;
		}

		// Success: Box fixed
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();




		// I knew it, it worked! I can now touch the box. Let's use it to get out of here.
		enableDialogue ();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);


		// Mission: Use the box to help Yuni reach the door
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);


		// Waiting for Yuni to push block to certain position
		while (!this.GetTriggerPush ().IsTriggered ()) {
			yield return null;
		}
		// Success: Pushed box to edge of room
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();



		// Mission: Leave the room
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);
	}
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Tutorial Trigger : Tag is "+other.tag);
		if (other.gameObject.GetComponent<PlayerYuni>() != null) { //.CompareTag ("Player")) {
			player = other.gameObject;
			PlayerYuni playerStats = player.GetComponent<PlayerYuni> ();
			playerAttack = playerStats.GetPlayerAttack ();
			playerController = playerStats.GetPlayerMovement ();
			PlayScenes ();
		}
	}

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
			disablePlayerControls ();
			StartCoroutine (Introduction ());
		}
	}

	public TutorialHandlerCarryingItems GetHandlerCarryingItems() {
		if (this.handlerCarryingItems == null) {
			this.handlerCarryingItems = GetComponentInChildren<TutorialHandlerCarryingItems> ();
		}
		return this.handlerCarryingItems;
	}
	public TutorialHandlerFixingBoxes GetHandlerFixingBoxes() {
		if (this.handlerFixingBoxes == null) {
			this.handlerFixingBoxes = GetComponentInChildren<TutorialHandlerFixingBoxes> ();
		}
		return this.handlerFixingBoxes;
	}
	public void ApproachedBox() {
		this.approachedBox = true;
	}
	public TriggerJump GetTriggerJump() {
		if (this.triggerJump == null) {
			this.triggerJump = GetComponentInChildren<TriggerJump> ();
		}
		return this.triggerJump;
	}

	public TriggerCurious GetTriggerCurious() {
		if (this.triggerCurious == null) {
			this.triggerCurious = GetComponentInChildren<TriggerCurious> ();
		}
		return this.triggerCurious;
	}
	public TriggerSkyFragment GetSkyFragmentTrigger() {
		if (this.triggerSkyFragment == null) {
			this.triggerSkyFragment = GetComponentInChildren<TriggerSkyFragment> ();
		}
		return this.triggerSkyFragment;
	}
	public InteractableTag GetInteractableTag() {
		if (this.interactableTag == null) {
			this.interactableTag = GetComponentInChildren<InteractableTag> ();
		}
		return this.interactableTag;
	}
	public TriggerCantTouch GetTriggerCantTouch() {
		if (this.triggerCantTouch == null) {
			this.triggerCantTouch = GetComponentInChildren<TriggerCantTouch> ();
		}
		return this.triggerCantTouch;
	}
	public TriggerDoorSpotted GetTriggerDoorSpotted() {
		if (this.triggerDoorSpotted == null) {
			this.triggerDoorSpotted = GetComponentInChildren<TriggerDoorSpotted> ();
		}
		return this.triggerDoorSpotted;
	}
	public TriggerSkyboxBreak GetSkyboxBreak() {
		if (this.skyboxBreak == null) {
			this.skyboxBreak = GetComponentInChildren<TriggerSkyboxBreak> ();
		}
		return this.skyboxBreak;
	}

	public TriggerSkyfall GetSkyfall() {
		if (this.skyFall == null) {
			this.skyFall = GetComponentInChildren<TriggerSkyfall> ();
		}
		return this.skyFall;
	}
	public TriggerBoxFix GetBoxFixTrigger() {
		if (this.triggerBoxFix == null) {
			this.triggerBoxFix = GetComponentInChildren<TriggerBoxFix> ();
		}
		return this.triggerBoxFix;
	}

	public TriggerMovement GetTriggerMovement() {
		if (this.triggerMovement == null) {
			this.triggerMovement = GetComponentInChildren<TriggerMovement> ();
		}
		return this.triggerMovement;
	}
	public TriggerPush GetTriggerPush() {
		if (this.triggerPush == null) {
			this.triggerPush = GetComponentInChildren<TriggerPush> ();
		}
		return this.triggerPush;
	}
	void enableDialogue() {
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		playerController.isInDialogue ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
	}
}
