using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndCutscene : Cutscene {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private Dialogue hintDialogues;
//	[SerializeField] private StabilityNumberLine numberLine;

	private PlayerYuni player;
	private PlayerAttack playerAttack;
	private PlayerMovement playerController;
	private bool hasCharm;
	private bool isStable;



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

	private IEnumerator EndRoomTutorial() {

		// [D1] Congratulations, you finished the tutorial!
		// There are more rooms to explore, but let's take a rest for now.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();


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

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
//			playerAttack.canAttack (false);
			StartCoroutine (EndRoomTutorial ());
		}
	}
//	public SlidingDoor GetSlidingDoor() {
//		if(this.slidingDoor == null) {
//			this.slidingDoor = GetComponentInChildren<SlidingDoor> ();
//		}
//		return this.slidingDoor;
//	}
//	public TriggerSkyBlockSpotted GetTriggerSkyBlockSpotted() {
//		if(this.triggerSkyBlockSpotted == null) {
//			this.triggerSkyBlockSpotted = GetComponentInChildren<TriggerSkyBlockSpotted> ();
//		}
//		return this.triggerSkyBlockSpotted;
//	}
//
//	public TriggerCharmSpotted GetTriggerCharmSpotted() {
//		if(this.triggerCharmSpotted == null) {
//			this.triggerCharmSpotted = GetComponentInChildren<TriggerCharmSpotted> ();
//		}
//		return this.triggerCharmSpotted;
//	}
}
