using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCutscene : CutscenePersistent {

    [SerializeField] private Dialogue dialogues;
    [SerializeField] private Dialogue hintDialogues;
    //	[SerializeField] private StabilityNumberLine numberLine;

    private PlayerYuni player;
    private PlayerAttack playerAttack;
    private PlayerMovement playerController;
    private bool hasCharm;
    private bool isStable;



    [SerializeField] private RingleaderController ringleaderController;
    [SerializeField] private TriggerPreBoss1 triggerPreBoss1;
    [SerializeField] private TriggerPreBoss2 triggerPreBoss2;
    [SerializeField] private TriggerPreBoss3 triggerPreBoss3;

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
        Init();
    }
    public void CharmPickup() {
        this.hasCharm = true;
    }

    protected override void Init() {
        this.name = "Final Boss Door Cutscene";
        base.Init();
    }


    protected override void disablePlayerControls() {
        playerController.canMove(false);
        playerAttack.canAttack(false);
    }

    void enableDialogue() {
        playerController.isInDialogue();
        GameController_v7.Instance.GetDialogueManager().DisplayMessage(DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
    }

    public void DialogueStart() {
        enableDialogue();
        GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
        this.disablePlayerControls();
    }

    public void DialogueEnd() {
        GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
        playerController.canMove(true);
        playerAttack.canAttack(true);
    }

    private IEnumerator SubdivisionTutorial() {
        // Hide Portal
        //this.GetTriggerPortalSpotted().HidePortal();
        //this.GetTriggerPortalSpotted().Ignore();
        //this.GetSkyBlockBroken().Ignore();
        //this.GetSkyBlockBroken2().Ignore();
      
        while (!this.GetTriggerPreBoss1().IsTriggered()) {
            yield return null;
        }

        //if (this.GetRingleaderController() != null) {
        //    this.GetRingleaderController().TalkPause();
        //}

        // [D1]: ...
        // This room... It's really unstable.
        // It must be close...
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }
        this.DialogueEnd();




        while (!this.GetTriggerPreBoss2().IsTriggered()) {
            yield return null;
        }

        // [D2]: !!!
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }
        // [D3]: I guess I should congratulate you...
        // For escaping the loop, that is.
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }
        // [D4]: So that was your doing...
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }
        // [D5]: I doubt you're that surprised.
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }
        // [D6]: ...
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }
        // [D7]: Sadly it was all for nothing.
        // This world will soon disappear
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }

        // [D8]: You don't say.
        // (Hey can you hear me?)
        // (It's a phantom, and powerful one too.)
        // (Not only was it able to manifest itself, but it looks like it can alter space as well.)
        // (It's practically invincible right now.)
        // (I bet if we stabilize the area we can-)
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }

        // [D9]: Enough talk
        // Your journey ends here.
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }

        // [D10]: (Just stabilize the room then hit that thing with the hammer!)
        // Let's go!
        this.DialogueStart();
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }
        this.DialogueEnd();

        if (this.GetRingleaderController() != null) {
            this.GetRingleaderController().Resume();
        }
        //while (!this.GetTriggerPreBoss3().IsTriggered()) {
        //    yield return null;
        //}
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerYuni>()) {
            player = other.gameObject.GetComponent<PlayerYuni>();
            playerAttack = player.GetPlayerAttack();
            playerController = player.GetPlayerMovement();
            PlayScenes();
        }
    }

    public override void PlayScenes() {
        if (!isPlaying && !isTriggered) {
            base.PlayScenes();
            StartCoroutine(SubdivisionTutorial());
        }
    }
    public RingleaderController GetRingleaderController() {
        if (this.ringleaderController == null) {
            this.ringleaderController = GetComponentInChildren<RingleaderController>();
        }
        return this.ringleaderController;
    }
    public TriggerPreBoss1 GetTriggerPreBoss1() {
        if (this.triggerPreBoss1 == null) {
            this.triggerPreBoss1 = GetComponentInChildren<TriggerPreBoss1>();
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
}
