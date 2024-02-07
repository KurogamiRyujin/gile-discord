using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cutscene to explain how locked doors work and how to unlock them.
/// </summary>
public class LockedDoorsCutscene : Cutscene {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private Dialogue hintDialogues;
//	[SerializeField] private StabilityNumberLine numberLine;

	private PlayerYuni player;
	private PlayerAttack playerAttack;
	private PlayerMovement playerController;
	private bool hasCharm;
	private bool isStable;


//	[SerializeField] private TriggerBoxFix triggerBoxFix;
	[SerializeField] private TriggerSkyBlockHit triggerSkyBlockHit;
	[SerializeField] private SlidingDoor slidingDoor;
	[SerializeField] private TriggerSkyBlockSpotted triggerSkyBlockSpotted;
	[SerializeField] private TriggerCharmSpotted triggerCharmSpotted;
	[SerializeField] private TriggerSkyBlockBroken triggerSkyBlockBroken;
	[SerializeField] private TutorialHandlerPowerCharm triggerPowerCharm;

	void Awake() {
		this.hasCharm = false;
		this.isStable = false;
		EventBroadcaster.Instance.AddObserver (EventNames.PICKUP_ITEM_CLOSE, CharmPickup);
		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, Stabilize);
	}	
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.PICKUP_ITEM_CLOSE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.STABLE_AREA);
	}
	public void Stabilize() {
		this.isStable = true;
	}
	void Start() {
		Init ();
	}
	public void CharmPickup() {
		this.hasCharm = true;
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

	private IEnumerator SkyBlockTutorial() {
		this.GetSkyBlockHit ().Ignore ();
		this.GetSkyBlockBroken().Ignore();

		// [D1] More broken objects. But I don't see any sky fragments lying around.
		// ... We need to fix those boxes
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();

		// [H1] Mission: Explore the room
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);

		// While sky block is not yet spotted
		while (!this.GetTriggerSkyBlockSpotted ().IsTriggered ()) {
			yield return null;
		}

		// Success: Explore the room
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();

		// [D2] Hey that's a huge sky fragment. I guess it's better to call it a Sky Block
		// It's stuck on the wall though. Let's try to knock it down using the needle
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();


		// [H2] Mission: Knock down the sky block using the needle
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);


		// While player has not hit skyblock
		this.GetSkyBlockHit ().Observe ();
		while (!this.GetSkyBlockHit ().IsTriggered ()) {
			yield return null;
		}


		// Hide: Hide mission
		GameController_v7.Instance.GetDialogueManager ().CloseMinorHint();


		// [D3] No good! The needle's not strong enough... Let's look around.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();


		// Open sliding door
		this.GetSlidingDoor ().Open ();

		// [H3] Mission: Look for something to knock down the sky block
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);


		// While charm is not yet spotted
		while (!this.GetTriggerCharmSpotted ().IsTriggered ()) {
			yield return null;
		}
		// Hide: Hide mission
		GameController_v7.Instance.GetDialogueManager ().CloseMinorHint();

		// [D4] What's that? Let's pick it up.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();


		// [H4] Mission: Pick up the charm
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);
	

		// While pickup UI has not been closed
		while(!this.hasCharm) {
			yield return null;
		}
		// Success: Pick up the charm
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();



		// [D5] Hmm? It's a charm. Like those things people wear for luck.
		// But this has 'Power' written on it and the number 4.
		// ...
		// I don't really believe in this stuff but...
		// I'll tie it on the needle.
		// Let's see if it can make it strong enough to break the block.

		// TUTORIAL CHARM

		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();


		// While "How to use Needle" graphic is still open
		this.GetTriggerPowerCharm ().Action ();
		while (!this.GetTriggerPowerCharm ().IsFinished ()) {
			yield return null;
		}

		// I don't really believe in this stuff but...
		// I'll tie it on the needle.
		// Let's see if it can make it strong enough to break the block.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();



		// [H5] Mission: Hit the sky block with the charmed needle
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);

		// While sky blocks has not been split
		this.GetSkyBlockBroken().Observe();
		while (!this.GetSkyBlockBroken ().IsTriggered ()) {
			yield return null;
		}

		// Success: Hit the sky block with the charmed needle
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();


		// [D6] I can't believe that worked.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();

		// [H6] Mission: Use the sky fragments to fix the boxes and stabilize the room
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);
		while (!this.isStable) {
			yield return null;
		}

		// Success: [H6] Use the sky fragments to fix the boxes and stabilize the room
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni> ()) {
			player = other.gameObject.GetComponent<PlayerYuni> ();
			playerAttack = player.GetPlayerAttack ();
			playerController = player.GetPlayerMovement ();
			PlayScenes ();
		}
	}
//	public TriggerBoxFix GetBoxFixTrigger() {
//		if (this.triggerBoxFix == null) {
//			this.triggerBoxFix = GetComponentInChildren<TriggerBoxFix> ();
//		}
//		return this.triggerBoxFix;
//	}
	public TriggerSkyBlockHit GetSkyBlockHit() {
		if (this.triggerSkyBlockHit == null) {
			this.triggerSkyBlockHit = GetComponentInChildren<TriggerSkyBlockHit> ();
		}
		return this.triggerSkyBlockHit;
	}
	public TriggerSkyBlockBroken GetSkyBlockBroken() {
		if (this.triggerSkyBlockBroken == null) {
			this.triggerSkyBlockBroken = GetComponentInChildren<TriggerSkyBlockBroken> ();
		}
		return this.triggerSkyBlockBroken;
	}
	public TutorialHandlerPowerCharm GetTriggerPowerCharm() {
		if (this.triggerPowerCharm == null) {
			this.triggerPowerCharm = GetComponentInChildren<TutorialHandlerPowerCharm> ();
		}
		return this.triggerPowerCharm;
	}

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
			playerAttack.canAttack (false);
			StartCoroutine (SkyBlockTutorial ());
		}
	}
	public SlidingDoor GetSlidingDoor() {
		if(this.slidingDoor == null) {
			this.slidingDoor = GetComponentInChildren<SlidingDoor> ();
		}
		return this.slidingDoor;
	}
	public TriggerSkyBlockSpotted GetTriggerSkyBlockSpotted() {
		if(this.triggerSkyBlockSpotted == null) {
			this.triggerSkyBlockSpotted = GetComponentInChildren<TriggerSkyBlockSpotted> ();
		}
		return this.triggerSkyBlockSpotted;
	}

	public TriggerCharmSpotted GetTriggerCharmSpotted() {
		if(this.triggerCharmSpotted == null) {
			this.triggerCharmSpotted = GetComponentInChildren<TriggerCharmSpotted> ();
		}
		return this.triggerCharmSpotted;
	}
}
