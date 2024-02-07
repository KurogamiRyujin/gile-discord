using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cutscene for the tutorial explaining the needle mechanic.
/// </summary>
public class NeedleAndHookTutorial : Cutscene {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private Dialogue hintDialogues;


	private PlayerYuni player;
	private PlayerAttack playerAttack;
	private PlayerMovement playerController;


	private bool isCarrying;
	private bool isPickupClosed;

	[SerializeField] private TutorialHandlerUsingNeedle handlerUsingNeedle;
	[SerializeField] private TriggerBoxFix triggerBoxFix;
	[SerializeField] private TriggerBlockFilled3 triggerBlockFilled3;
	[SerializeField] private TriggerShowPickupItem triggerShowPickupItem;
	[SerializeField] private TriggerOnPlatform triggerOnPlatform;

	void Awake() {
		this.isPickupClosed = false;
//		EventBroadcaster.Instance.AddObserver (EventNames.CARRY_OVERRIDE, CarryOverride);
		EventBroadcaster.Instance.AddObserver (EventNames.PICKUP_ITEM_CLOSE, PickupScreenClosed);

	}

	void OnDestroy() {
//		EventBroadcaster.Instance.RemoveObserver (EventNames.CARRY_OVERRIDE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.PICKUP_ITEM_CLOSE);
	}

	void Start() {
		Init ();
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
	}

	protected override void Init() {
		this.name = "Needle And Hook Tutorial Cutscene";
		base.Init ();
	}

	public void CarryOverride() {
		this.isCarrying = true;
	}

	public void PickupScreenClosed() {
		this.isPickupClosed = true;
	}


	private IEnumerator NeedleAndHookRoutine() {
		
		// Hide the stability meter
//		this.GetStabilityParent ().CloseIdle ();


		// Safety check. Disable SceneObjectsController when NOT starting from Discord_Main
//		if (SceneObjectsController.Instance != null) {
//			// Wait for the area to be stable before allowing the player to move
//			// Since box should be prefilled
//			GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
//			while (!this.GetStabilityNumberLine ().IsStable ()) {
//				yield return null;
//			}
//			GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
//			playerController.canMove (true);
//		}


		this.GetTriggerShowPickupItem ().Ignore ();
		this.GetTriggerOnPlatform ().Ignore ();

		// So this room's unstable too, huh.
		// But that box should be enough to stabilize it.
		// There are sky fragments around. Let's use it to fix the box.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();


		// Mission: Place the sky fragments in the hollow block
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);
		// GameController_v7.Instance.GetDialogueManager ().CloseMinorHint();


		// While 3 fragments are not in the hollow block
		this.GetTriggerBlockFilled3().Observe();
		while(!this.GetTriggerBlockFilled3().IsTriggered()) {
			yield return null;
		}

		// Hide: Hide mission
		GameController_v7.Instance.GetDialogueManager ().CloseMinorHint();

		// We're missing a piece..
		// "But there are no fragments left.."
		// "Up there? This is bad, I can't reach that by jumping."
		// "Let's look around, maybe there's something else we can use"
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();


		// Mission: Find something to help Yuni reach the platform
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);


		// Show needle
		this.GetTriggerShowPickupItem ().Observe ();
		while(!this.GetTriggerShowPickupItem().IsTriggered()) {
			yield return null;
		}

		// Hide: Hide mission
		GameController_v7.Instance.GetDialogueManager ().CloseMinorHint();


		// What's that object over there? Let's pick it up
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();



		// Mission: Go near the glowing object and use the HAND button to pick it up
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);

		// Wait for player to pickup needle
		while (!isPickupClosed) {
			yield return null;
		}



		// A needle? How's this supposed to help?
		// There's a thread attached to it. It looks pretty sturdy
		// I have an idea! See that lamp up there?
		// Help me thow the needle towards it
		this.DialogueStart();
		disablePlayerControls (); // Used when player has needle
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();

		// Success: Player pick-up needle
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();


		// While "How to use Needle" graphic is still open
		this.GetHandlerUsingNeedle ().Action ();
		while (!this.GetHandlerUsingNeedle ().IsFinished ()) {
			yield return null;
		}



		// Mission: Throw the needle towards the blue icon in the lamp to reach the platform
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);


		this.GetTriggerOnPlatform ().Observe ();
		while(!this.GetTriggerOnPlatform ().IsTriggered()) {
			yield return null;
		}

		// Wait until Yuni gets up the skyblock fraction
		// Success:
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();


		// This needle's pretty handy! Let's keep it.
		// But more importantly, we can now fix that box with this sky fragment.
		this.DialogueStart();
		disablePlayerControls (); // Used when player has needle
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();


		// Mission: Fix the box
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);


		// Observe if box is fixed
		this.GetBoxFixTrigger ().Observe ();
		// While box is not yet fixed
		while (!this.GetBoxFixTrigger ().IsTriggered ()) {
			yield return null;
		}

		// Success: Fix box
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();

		// Okay, the room's stable. The door should be open now. Let's go.
		this.DialogueStart();
		disablePlayerControls (); // Used when player has needle
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();
	}

	public TriggerBoxFix GetBoxFixTrigger() {
		if (this.triggerBoxFix == null) {
			this.triggerBoxFix = GetComponentInChildren<TriggerBoxFix> ();
		}
		return this.triggerBoxFix;
	}
	public TriggerBlockFilled3 GetTriggerBlockFilled3() {
		if (this.triggerBlockFilled3 == null) {
			this.triggerBlockFilled3 = GetComponentInChildren<TriggerBlockFilled3> ();
		}
		return this.triggerBlockFilled3;
	}
	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GameObject.FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni> () != null) {
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
			StartCoroutine (NeedleAndHookRoutine ());
		}
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
		playerController.canMove (false);
//				disablePlayerControls ();
	}
	public void DialogueEnd() {
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);
		playerAttack.canAttack (true);
	}

	public TutorialHandlerUsingNeedle GetHandlerUsingNeedle() {
		if (this.handlerUsingNeedle == null) {
			this.handlerUsingNeedle = GetComponentInChildren<TutorialHandlerUsingNeedle> ();
		}
		return this.handlerUsingNeedle;
	}
	public TriggerOnPlatform GetTriggerOnPlatform() {
		if (this.triggerOnPlatform == null) {
			this.triggerOnPlatform = GetComponentInChildren<TriggerOnPlatform> ();
		}
		return this.triggerOnPlatform;
	}

	public TriggerShowPickupItem GetTriggerShowPickupItem() {
		if (this.triggerShowPickupItem == null) {
			this.triggerShowPickupItem = GetComponentInChildren<TriggerShowPickupItem> ();
		}
		return this.triggerShowPickupItem;
	}
}
