using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cutscene for the tutorial for picking up and dropping objects and filling ghost blocks with a sky fragment.
/// </summary>
public class PickupAndFillTutorial : Cutscene {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private Dialogue carryOverrideDialogues;
	[SerializeField] private SpawnFragmentPiece spawnFragmentPiece;
//	[SerializeField] private SkyFragmentPiece skyFragmentPiece;
	[SerializeField] private HollowBlock hollowBlock;
	[SerializeField] private StabilityNumberLineAnimatable stabilityParent;
	[SerializeField] private StabilityNumberLine stabilityNumberLine;

	private PlayerYuni player;
	private PlayerAttack playerAttack;
	private PlayerMovement playerController;


	private bool isCarrying;

	[SerializeField] private TriggerDoorUpAhead triggerDoorUpAhead;
	[SerializeField] private TriggerBreakRoom triggerBreakRoom;
	[SerializeField] private TriggerDoorLocked triggerDoorLocked;
	[SerializeField] private TutorialHandlerUnstableRoom handlerUnstableRoom;
	[SerializeField] private TriggerBoxFix triggerBoxFix;
	void Awake() {

		EventBroadcaster.Instance.AddObserver (EventNames.CARRY_OVERRIDE, CarryOverride);
	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.CARRY_OVERRIDE);

	}
	void Start() {
		Init ();
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
	}

	protected override void Init() {
		this.name = "Pickup And Fill Tutorial Cutscene";
		base.Init ();
	}

	protected override void disablePlayerControls() {
//		ToggleMobileUI (false);
		playerController.canMove (false);
		playerAttack.canAttack (false);
	}

	void enableDialogue() {
		playerController.isInDialogue ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
	}
	void enableCarryOverrideDialogue() {
		playerController.isInDialogue ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, carryOverrideDialogues, null);
	}
	public void CarryOverride() {
		this.isCarrying = true;
	}
	public StabilityNumberLineAnimatable GetStabilityParent() {
		// Must not enter this, assign ItemContainerAnimatable in inspector;
		if (this.stabilityParent == null) {
			this.stabilityParent = GameObject.FindObjectOfType<StabilityNumberLineAnimatable> ();
		}
		return this.stabilityParent;
	}
	private StabilityNumberLine GetStabilityNumberLine() {
		if (this.stabilityNumberLine == null) {
			this.stabilityNumberLine = GameObject.FindObjectOfType<StabilityNumberLine> ();
		}
		return this.stabilityNumberLine;
	}
	private IEnumerator FindAWayOut() {
		// Hide the stability meter
		this.GetStabilityParent ().CloseIdle ();


		SoundManager.Instance.Play (AudibleNames.Room.BREAKING, false); // Play breaking sound
		// Safety check. Disable SceneObjectsController when NOT starting from Discord_Main
		if (SceneObjectsController.Instance != null) {
			// Wait for the area to be stable before allowing the player to move
			// Since box should be prefilled

			GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
			while (!this.GetStabilityNumberLine ().IsStable ()) {
				yield return null;
			}
			GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
			playerController.canMove (true);
		}


		// Wait for player to go near the door
		while(!this.GetTriggerDoorUpAhead().IsTriggered()) {
			yield return null;
		}
		this.GetTriggerDoorUpAhead ().Action (); // Play door open animation

		// I see a door up ahead. Maybe we can use it to get out.
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);

		// Mission: Use the door to leave the room
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);



		// Wait for player to enter break area
		while(!this.GetTriggerBreakRoom().IsTriggered()) {
			yield return null;
		}


		SoundManager.Instance.Play (AudibleNames.Room.BREAKING, false); // Play breaking sound


		// What?! The room's breaking!!
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		this.GetTriggerBreakRoom ().Action (); // Break block to break room
		SoundManager.Instance.Play (AudibleNames.BreakableBox.GLASS, false);

		this.GetPlayer().DisableCarry();
		// That box broke as well.. Anyways, let's get out of here!
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);

		// Wait for player to get near the door
		while(!this.GetTriggerDoorLocked().IsTriggered()) {
			// If player tries to carry something
			if (isCarrying) {
				this.isCarrying = false;
				// Play dialogue: Let's get out of here
				enableCarryOverrideDialogue();
				GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
				while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
					disablePlayerControls ();
					yield return null;
				}
				this.carryOverrideDialogues.currentTextIndex = -1;
				GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
				playerController.canMove (true);
			}
			yield return null;
		}


		// Hide: Use the door to leave the room
		GameController_v7.Instance.GetDialogueManager ().CloseMinorHint();

		// Oh no! The door's locked. But it was open awhile ago.
		// It must have locked when the room broke
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		this.GetPlayer().EnableCarry(); // Enable carry


		// UNSTABLE ROOMS - Doors will lock when a room is unstable
		// The meter above shows the room's stability
		// Fixing blocks will add it's value to the room's stability
		// Align the pointer to the target value to stabilize the room

		// While graphic is still open
		this.GetHandlerUnstableRoom ().Action ();
		while (!this.GetHandlerUnstableRoom ().IsFinished ()) {
			yield return null;
		}


		// Glow stability upon graphic close
		this.GetStabilityParent ().Open ();
		GameController_v7.Instance.GetTutorialGlowManager ().GlowStability();
//		GameController_v7.Instance.GetDialogueManager ().CloseMinorHint();
//		playerController.canMove (true);
		yield return new WaitForSeconds (2.0f);
		GameController_v7.Instance.GetTutorialGlowManager ().StopStabilityGlow();


		// TODO: Dialogue
		// "The meter above shows the room's stability"
		// "We need to fix objects so that it will reach 4/4."
		// "Let's try fixing that box right there using the sky fragments"
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);


		// TODO: Mission: Fix objects to increase the room's stability
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);


		this.GetBoxFixTrigger ().Observe ();
		// While box is not yet fixed
		while (!this.GetBoxFixTrigger ().IsTriggered ()) {
			yield return null;
		}
		// Success: Box is fixed
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();


		// Dialogue: That should do it. Looks like the room is stable again.
		// The door opened! Let's go.
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);


	}
	public TriggerBoxFix GetBoxFixTrigger() {
		if (this.triggerBoxFix == null) {
			this.triggerBoxFix = GetComponentInChildren<TriggerBoxFix> ();
		}
		return this.triggerBoxFix;
	}
	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GameObject.FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}
	private IEnumerator FallenFragment() {

		enableDialogue ();//What is that?

//		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		//Hint to make player pickup fragment
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

		playerController.canMove (true);

		StartCoroutine (FillHollowBox ());
	}


	private IEnumerator FillHollowBox() {
		while (!this.spawnFragmentPiece.IsFragmentCarried ()) {
			yield return null;
		}
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();

		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		enableDialogue ();//Put it in box
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);

		//Hint to fill box
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

		playerController.canMove (true);

		while (!this.hollowBlock.IsSolved())
			yield return null;
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();


		enableDialogue ();//Hollow box is filled

		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			player = other.gameObject.GetComponent<PlayerYuni> ();
			playerAttack = player.GetPlayerAttack ();
			playerController = player.GetPlayerMovement ();
			PlayScenes ();
		}
	}

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
			playerAttack.canAttack (false);
			StartCoroutine (FindAWayOut ());
		}
	}

	public TriggerDoorUpAhead GetTriggerDoorUpAhead() {
		if (this.triggerDoorUpAhead == null) {
			this.triggerDoorUpAhead = GetComponentInChildren<TriggerDoorUpAhead> ();
		}
		return this.triggerDoorUpAhead;
	}
	public TriggerBreakRoom GetTriggerBreakRoom() {
		if (this.triggerBreakRoom == null) {
			this.triggerBreakRoom = GetComponentInChildren<TriggerBreakRoom> ();
		}
		return this.triggerBreakRoom;
	}
	public TriggerDoorLocked GetTriggerDoorLocked() {
		if (this.triggerDoorLocked == null) {
			this.triggerDoorLocked = GetComponentInChildren<TriggerDoorLocked> ();
		}
		return this.triggerDoorLocked;
	}
	public TutorialHandlerUnstableRoom GetHandlerUnstableRoom() {
		if (this.handlerUnstableRoom == null) {
			this.handlerUnstableRoom = GetComponentInChildren<TutorialHandlerUnstableRoom> ();
		}
		return this.handlerUnstableRoom;
	}
}
