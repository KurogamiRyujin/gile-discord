using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cutscene tutorial explaining the hammer mechanic.
/// </summary>
public class HammerCutscene : Cutscene {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private Dialogue hintDialogues;
//	[SerializeField] private StabilityNumberLine numberLine;

	private PlayerYuni player;
	private PlayerAttack playerAttack;
	private PlayerMovement playerController;
	private bool hasHammer;
	private bool isStable;
	private bool isBreakableBroken;
	private bool isObserving;


//	[SerializeField] private TriggerBoxFix triggerBoxFix;
//	[SerializeField] private TriggerSkyBlockHit triggerSkyBlockHit;
//	[SerializeField] private SlidingDoor slidingDoor;
//	[SerializeField] private TriggerCharmSpotted triggerCharmSpotted;
//	[SerializeField] private TriggerSkyBlockBroken triggerSkyBlockBroken;
//	[SerializeField] private TutorialHandlerPowerCharm triggerPowerCharm;
	[SerializeField] private TriggerLookDown triggerLookDown;
	[SerializeField] private TriggerSkyBlockSpotted triggerSkyBlockSpotted;
	[SerializeField] private TriggerOnElevator triggerOnElevator;
	[SerializeField] private TriggerSpottedHammer triggerSpottedHammer;
	[SerializeField] private TriggerHollowBlockBroken triggerHollowBlockBroken;
	[SerializeField] private TriggerOnMovingPlatform triggerOnMovingPlatform;
	[SerializeField] private StabilityNumberLine stabilityNumberLine;
	[SerializeField] private TutorialHandlerUsingHammer handlerUsingHammer;
	[SerializeField] private TriggerDoorSpotted triggerSpottedDoor;

	void Awake() {
		this.hasHammer = false;
		this.isStable = false;
		this.isBreakableBroken = false;
		this.isObserving = false;
		EventBroadcaster.Instance.AddObserver (EventNames.PICKUP_ITEM_CLOSE, HammerPickup);
		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, Stabilize);
		EventBroadcaster.Instance.AddObserver (EventNames.BREAKABLE_BROKEN, BreakableBroken);
	}	
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.PICKUP_ITEM_CLOSE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.STABLE_AREA);
		EventBroadcaster.Instance.RemoveObserver (EventNames.BREAKABLE_BROKEN);
	}
	public void Stabilize() {
		this.isStable = true;
	}
	void Start() {
		Init ();
	}
	public void HammerPickup() {
		if (isObserving) {
			this.hasHammer = true;
		}
	}
	public void BreakableBroken() {
		this.isBreakableBroken = true;
	}

	protected override void Init() {
		this.name = "Locked Door Cutscene";
		base.Init ();
	}


	protected override void disablePlayerControls() {
		playerController.canMove (false);
		playerAttack.canAttack (false);
	}

	void enableDialogue() {
		playerController.isInDialogue ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
	}

	public void DialogueStart() {
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		this.disablePlayerControls ();
	}

	public void DialogueEnd() {
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);
		playerAttack.canAttack (true);
	}

	private IEnumerator HammerTutorial() {
//		this.GetSkyBlockHit ().Ignore ();
//		this.GetSkyBlockBroken().Ignore();
		this.GetTriggerHollowBlockBroken().Ignore();
		this.GetTriggerOnMovingPlatform().Ignore();

		// Safety check. Disable SceneObjectsController when NOT starting from Discord_Main
		if (SceneObjectsController.Instance != null) {
			// Wait for the area to be stable before allowing the player to move
			// Since box should be prefilled
			DialogueStart();
//			while (!this.GetStabilityNumberLine ().IsStable ()) {
//				yield return null;
//			}
			while (this.GetStabilityNumberLine ().IsStable ()) {
				yield return new WaitForSeconds(1.0f);
			}
			DialogueEnd ();
		}




//		[D1] 
//		Wait, this block. It says 3/4.
//		This should be enough to stabilize the room.
//		But somehow the current stability says 4/4.
//		There must be another block somewhere that's making the room unstable.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();

		// [H1] Mission: Find the block that's causing the instability
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);



		while (!this.GetTriggerLookDown ().IsTriggered ()) {
			yield return null;
		}


		//		[D2] 
		//		Looks like there's no where else to go but down.
		//		Don't worry, I see a room below.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {

			yield return null;
		}
		this.DialogueEnd ();




		// While instability block is not yet spotted (NOTE: Reused SkyBlockSpotted trigger)
		while (!this.GetTriggerSkyBlockSpotted ().IsTriggered ()) {
			yield return null;
		}

		// Success: Find the block that's causing the instability
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();





		// [D3] This is causing the instability. Find a way to break it.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();




		// [H2] Mission: Find something to help Yuni break the box
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);


		// Check if on elevator
		while (!this.GetTriggerOnElevator ().IsTriggered ()) {
			yield return null;
		}

		// [D4] This is an elevator. We can use it later to go up
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();



		// Check if near hammer
		while (!this.GetTriggerSpottedHammer ().IsTriggered ()) {
			yield return null;
		}

		// Hey that's a hammer. Perfect! I bet it can break that box,
		// let's take it.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();

		this.isObserving = true;
		// Listen for Pick-up item close
		while(!this.hasHammer) {
			yield return null;
		}


		// Success: [H2]
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();



		// While hammer graphic is playing
		this.GetHandlerUsingHammer().Action();
		while (!this.GetHandlerUsingHammer ().IsFinished ()) {
			yield return null;
		}



		// [H3] Mission: Destroy the block using the Hammer
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);

		this.GetTriggerHollowBlockBroken().Observe();
		while (!this.GetTriggerHollowBlockBroken ().IsTriggered ()) {
			yield return null;
		}


		// Success: [H3]
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();



		// [D4] Good job! The room's stable again. I bet the
		// elevator awhile ago is working now.
		// Let's use it to get out of here.


		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();

		// Wait until player get to elevator
		this.GetTriggerOnMovingPlatform().Observe();
		while (!this.GetTriggerOnMovingPlatform ().IsTriggered ()) {
			yield return null;
		}



//		// [D5] Alright, press the HAND button to operate the elevator.
//		this.DialogueStart();
//		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
//			yield return null;
//		}
//		this.DialogueEnd ();

		// [H4] Mission: Go near the elevator and press the HAND button to use it
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);


		while (!this.GetTriggerDoorSpotted ().IsTriggered ()) {
			yield return null;
		}


		// Success: [H4] Go near the elevator and press the HAND button to use it
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();



		// [D6] Is this another dead end?
		// Those boxes blocking our way look week.
		// Hey grab the hammer, I think we can break it.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();


//		// [H5] Mission: Break the boxes using the hammer
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);

		// While not broken
		while (!this.isBreakableBroken) {
			yield return null;
		}

		// Success: [H5]
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();

	}

	private StabilityNumberLine GetStabilityNumberLine() {
		if (this.stabilityNumberLine == null) {
			this.stabilityNumberLine = GameObject.FindObjectOfType<StabilityNumberLine> ();
		}
		return this.stabilityNumberLine;
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni> ()) {
			player = other.gameObject.GetComponent<PlayerYuni> ();
			playerAttack = player.GetPlayerAttack ();
			playerController = player.GetPlayerMovement ();
			PlayScenes ();
		}
	}


	public TriggerLookDown GetTriggerLookDown() {
		if (this.triggerLookDown == null) {
			this.triggerLookDown = GetComponentInChildren<TriggerLookDown> ();
		}
		return this.triggerLookDown;
	}
	public TriggerSpottedHammer GetTriggerSpottedHammer() {
		if (this.triggerSpottedHammer == null) {
			this.triggerSpottedHammer = GetComponentInChildren<TriggerSpottedHammer> ();
		}
		return this.triggerSpottedHammer;
	}
	public TriggerOnElevator GetTriggerOnElevator() {
		if (this.triggerOnElevator == null) {
			this.triggerOnElevator = GetComponentInChildren<TriggerOnElevator> ();
		}
		return this.triggerOnElevator;
	}
	public TriggerHollowBlockBroken GetTriggerHollowBlockBroken() {
		if (this.triggerHollowBlockBroken == null) {
			this.triggerHollowBlockBroken = GetComponentInChildren<TriggerHollowBlockBroken> ();
		}
		return this.triggerHollowBlockBroken;
	}
	public TriggerOnMovingPlatform GetTriggerOnMovingPlatform() {
		if (this.triggerOnMovingPlatform == null) {
			this.triggerOnMovingPlatform = GetComponentInChildren<TriggerOnMovingPlatform> ();
		}
		return this.triggerOnMovingPlatform;
	}
	public TriggerSkyBlockSpotted GetTriggerSkyBlockSpotted() {
		if(this.triggerSkyBlockSpotted == null) {
			this.triggerSkyBlockSpotted = GetComponentInChildren<TriggerSkyBlockSpotted> ();
		}
		return this.triggerSkyBlockSpotted;
	}
	public TutorialHandlerUsingHammer GetHandlerUsingHammer() {
		if(this.handlerUsingHammer == null) {
			this.handlerUsingHammer = GetComponentInChildren<TutorialHandlerUsingHammer> ();
		}
		return this.handlerUsingHammer;
	}

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
			playerAttack.canAttack (false);
			StartCoroutine (HammerTutorial ());
		}
	}
//	public TriggerBoxFix GetBoxFixTrigger() {
//		if (this.triggerBoxFix == null) {
//			this.triggerBoxFix = GetComponentInChildren<TriggerBoxFix> ();
//		}
//		return this.triggerBoxFix;
//	}


	public TriggerDoorSpotted GetTriggerDoorSpotted() {
		if (this.triggerSpottedDoor == null) {
			this.triggerSpottedDoor = GetComponentInChildren<TriggerDoorSpotted> ();
		}
		return this.triggerSpottedDoor;
	}
//	public TriggerSkyBlockHit GetSkyBlockHit() {
//		if (this.triggerSkyBlockHit == null) {
//			this.triggerSkyBlockHit = GetComponentInChildren<TriggerSkyBlockHit> ();
//		}
//		return this.triggerSkyBlockHit;
//	}
//	public TriggerSkyBlockBroken GetSkyBlockBroken() {
//		if (this.triggerSkyBlockBroken == null) {
//			this.triggerSkyBlockBroken = GetComponentInChildren<TriggerSkyBlockBroken> ();
//		}
//		return this.triggerSkyBlockBroken;
//	}
//	public TutorialHandlerPowerCharm GetTriggerPowerCharm() {
//		if (this.triggerPowerCharm == null) {
//			this.triggerPowerCharm = GetComponentInChildren<TutorialHandlerPowerCharm> ();
//		}
//		return this.triggerPowerCharm;
//	}

//	public SlidingDoor GetSlidingDoor() {
//		if(this.slidingDoor == null) {
//			this.slidingDoor = GetComponentInChildren<SlidingDoor> ();
//		}
//		return this.slidingDoor;
//	}

//
//	public TriggerCharmSpotted GetTriggerCharmSpotted() {
//		if(this.triggerCharmSpotted == null) {
//			this.triggerCharmSpotted = GetComponentInChildren<TriggerCharmSpotted> ();
//		}
//		return this.triggerCharmSpotted;
//	}
}
