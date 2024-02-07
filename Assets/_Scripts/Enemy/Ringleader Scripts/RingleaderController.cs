using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingleaderController : MonoBehaviour {
    [SerializeField] private RingleaderMovement ringleaderMovement;
    [SerializeField] private RingleaderAttack ringleaderAttack;
    [SerializeField] private RingleaderAttack_v2 ringleaderAttackV2;
    [SerializeField] private RingleaderHealth ringleaderHealth;
    [SerializeField] private Teleport ringleaderTeleport;


    [SerializeField] private bool callTalkPause;
    [SerializeField] private bool callResume;
    [SerializeField] private Dialogue dialogueHit1;
    [SerializeField] private Dialogue dialogueRetry1;
    [SerializeField] private Dialogue dialogueHit2;
    [SerializeField] private Dialogue dialogueRetry2;
    [SerializeField] private Dialogue dialogueHit3;
    [SerializeField] private Dialogue dialogueRetry3;
    private PlayerYuni player;
    private int hitCount;
    void Awake() {
        this.hitCount = 0;
        this.AwakeFunctions();
    }
    void Start() {
        this.player = FindObjectOfType<PlayerYuni>();
    }
    public void AwakeFunctions() {
        this.TalkPause();
    }
    void Update() {
        if (this.callTalkPause) {
            this.callTalkPause = false;
            this.callResume = false;
            this.TalkPause();
        }
        else if (this.callResume) {
            this.callTalkPause = false;
            this.callResume = false;
            this.Resume();
        }
    }

    protected void disablePlayerControls() {
        player.GetPlayerMovement().canMove(false);
        player.GetPlayerAttack().canAttack(false);
    }

    void enableDialogue(Dialogue dialogues) {
        player.GetPlayerMovement().isInDialogue();
        GameController_v7.Instance.GetDialogueManager().DisplayMessage(DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
    }

    public void DialogueStart(Dialogue dialogues) {
        enableDialogue(dialogues);
        GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
        this.disablePlayerControls();
    }

    public void DialogueEnd() {
        GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
        player.GetPlayerMovement().canMove(true);
        player.GetPlayerAttack().canAttack(true);
    }


    public IEnumerator HitRoutine() {
        this.hitCount++;
        this.TalkPause();
        switch (hitCount) {
            case 1:
                // [D1]: R: ..!
                this.DialogueStart(dialogueHit1);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D2]: Y: Bet that hurt didn't it?
                this.DialogueStart(dialogueHit1);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D3]: R: Fool! You think you can stand against me?
                this.DialogueStart(dialogueHit1);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D4]: Y: Sure do.
                this.DialogueStart(dialogueHit1);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }
                this.DialogueEnd();


                break;
            case 2:
                // [D1]: Y: How'zzat?!
                this.DialogueStart(dialogueHit2);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D2]: R: WEAK.
                this.DialogueStart(dialogueHit2);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                break;
            case 3:
                // [D1]: Y: Is that all you've got?
                this.DialogueStart(dialogueHit3);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D2]: R: ...
                // Again...
                // ...
                this.DialogueStart(dialogueHit3);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D3]: Y: What?
                // (Don't worry, there's no way he can turn back time at his current state.)
                // ...
                // (I think?)
                this.DialogueStart(dialogueHit3);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D4]: R: ... AGAIN!
                this.DialogueStart(dialogueHit3);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D5]: Y: You can't possibly-
                // (Prepare yourself! He's up to something.)
                this.DialogueStart(dialogueHit3);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }
                this.DialogueEnd();
                break;
            default:
                yield return null;
                break;
        }
        this.Resume();
        yield return null;
    }


    public IEnumerator RetryRoutine() {
        this.TalkPause();
        switch (hitCount) {
            case 1:
                // [D1]: Y: What the...
                // (Did he just turn back time?)
                this.DialogueStart(dialogueRetry1);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D2]: R: You don't know who you're up against.
                this.DialogueStart(dialogueRetry1);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D3]: Y: Don't be too sure about that.
                // (Listen, I bet that trick cost him a lot.)
                // (I doubt he can keep doing it for long.)
                // (Just keep going at it. Remember, stabilize then smash.)
                this.DialogueStart(dialogueRetry1);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();


                break;
            case 2:
                // [D1]: R: Give up now. Resistance is futile.
                this.DialogueStart(dialogueRetry2);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D2]: Y: Yeah, resistance is futile!!
                // (One more go, he's too weak to do that again.)
                this.DialogueStart(dialogueRetry2);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                // [D1]: R: ...
                this.DialogueStart(dialogueRetry2);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();


                break;
            case 3:
                // [D1]: Y: Or not.
                // What's up with that, he just disappeared.
                // How boring. He probably ran out to find some body to possess.
                // ... Possess ...
                // Wha- Now that I mention it... I feel weird... It's as if..
                // ... AAAAAAAAAAAAAAAAAH.
                // Just kidding. v( > u < )v
                // Like that would ever happen.
                // Besides, phantoms can't possess their own kind...
                this.DialogueStart(dialogueRetry3);
                while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
                    yield return null;
                }

                this.DialogueEnd();
                break;
            default:
                yield return null;
                break;
        }
        this.Resume();
        yield return null;
    }


    public void Resume() {
        if (this.GetRingleaderMovement() != null) {
            this.ringleaderMovement.Resume();
        }
        if (this.GetRingleaderAttack() != null) {
            this.ringleaderAttack.Resume();
        }
        if (this.GetRingleaderAttackV2() != null) {
            this.ringleaderAttackV2.Resume();
        }
    }
    public void TalkPause() {
        if (this.GetRingleaderMovement() != null) {
            this.ringleaderMovement.TalkPause();
        }
        if (this.GetRingleaderAttack() != null) {
            this.ringleaderAttack.TalkPause();
        }
        if (this.GetRingleaderAttackV2() != null) {
            this.ringleaderAttackV2.TalkPause();
        }
    }

    public RingleaderMovement GetRingleaderMovement() {
        if (this.ringleaderMovement == null) {
            this.ringleaderMovement = GetComponent<RingleaderMovement>();
        }
        return this.ringleaderMovement;
    }
    public RingleaderAttack GetRingleaderAttack() {
        if (this.ringleaderAttack == null) {
            this.ringleaderAttack = GetComponent<RingleaderAttack>();
        }
        return this.ringleaderAttack;
    }
    public RingleaderAttack_v2 GetRingleaderAttackV2() {
        if (this.ringleaderAttackV2 == null) {
            this.ringleaderAttackV2 = GetComponent<RingleaderAttack_v2>();
        }
        return this.ringleaderAttackV2;
    }
    public RingleaderHealth GetRingleaderHealth() {
        if (this.ringleaderHealth == null) {
            this.ringleaderHealth = GetComponent<RingleaderHealth>();
        }
        return this.ringleaderHealth;
    }
    public Teleport GetRingleaderTeleport() {
        if (this.ringleaderTeleport == null) {
            this.ringleaderTeleport = GetComponent<Teleport>();
        }
        return this.ringleaderTeleport;
    }
}
