using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBossTutorialCutscene : CutscenePersistent {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private Dialogue hintDialogues;
//	[SerializeField] private StabilityNumberLine numberLine;

	private PlayerYuni player;
	private PlayerAttack playerAttack;
	private PlayerMovement playerController;
	private bool hasCharm;
	private bool isStable;



	[SerializeField] private TriggerPreBoss1 triggerPreBoss1;
    [SerializeField] private TriggerPreBoss2 triggerPreBoss2;
    [SerializeField] private TriggerPreBoss3 triggerPreBoss3;
    [SerializeField] private DoorStateManager bossDoor;

    void Awake() {
		this.hasCharm = false;
		this.isStable = false;
//		EventBroadcaster.Instance.AddObserver (EventNames.PICKUP_ITEM_CLOSE, CharmPickup);
		//EventBroadcaster.Instance.AddObserver (EventNames.CHARM_PICKUP_SWITCH, CharmPickup);
		//EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, Stabilize);
	}	
	void OnDestroy() {
		//EventBroadcaster.Instance.RemoveObserver (EventNames.CHARM_PICKUP_SWITCH);
		//EventBroadcaster.Instance.RemoveObserver (EventNames.STABLE_AREA);
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
		this.name = "Final Boss Door Cutscene";
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

	private IEnumerator SubdivisionTutorial() {
		// Hide Portal
		//this.GetTriggerPortalSpotted().HidePortal();
		//this.GetTriggerPortalSpotted().Ignore();
		//this.GetSkyBlockBroken().Ignore();
		//this.GetSkyBlockBroken2().Ignore();

		while (!this.GetTriggerPreBoss1 ().IsTriggered ()) {
			yield return null;
		}

        if (this.GetBossDoor() != null) {
            this.GetBossDoor().Close();
        }

		// [D1]: We've been stabilizing rooms for quite a while now.
        // I can feel that this place is slowly going back to normal.
        // Good job.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();



        while (!this.GetTriggerPreBoss2().IsTriggered()) {
            yield return null;
        }

        // [D2]: This room seems different...
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }
        this.DialogueEnd();

        while (!this.GetTriggerPreBoss3().IsTriggered()) {
            yield return null;
        }

        // [D3]: ... There's something strange behind this door.
        // I can feel a powerful presence.
        // Be careful.
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }
        this.DialogueEnd();


	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni> ()) {
			player = other.gameObject.GetComponent<PlayerYuni> ();
			playerAttack = player.GetPlayerAttack ();
			playerController = player.GetPlayerMovement ();
			PlayScenes ();
		}
	}

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
			StartCoroutine (SubdivisionTutorial ());
		}
	}
	public TriggerPreBoss1 GetTriggerPreBoss1() {
		if(this.triggerPreBoss1 == null) {
			this.triggerPreBoss1 = GetComponentInChildren<TriggerPreBoss1> ();
		}
		return this.triggerPreBoss1;
	}
    public TriggerPreBoss2 GetTriggerPreBoss2() {
        if (this.triggerPreBoss2 == null) {
            this.triggerPreBoss2 = GetComponentInChildren<TriggerPreBoss2>();
        }
        return this.triggerPreBoss2;
    }
    public TriggerPreBoss3 GetTriggerPreBoss3() {
        if (this.triggerPreBoss3 == null) {
            this.triggerPreBoss3 = GetComponentInChildren<TriggerPreBoss3>();
        }
        return this.triggerPreBoss3;
    }
    public DoorStateManager GetBossDoor() {
        if (this.bossDoor == null) {
            this.bossDoor = FindObjectOfType<BossDoorMarker>().GetComponent<DoorStateManager>();
        }
        return this.bossDoor;
    }
}
