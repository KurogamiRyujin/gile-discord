using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StableAreaAnimatable : AnimatableClass {

	public const string STABILIZE = "stabilize";
	public const string DESTABILIZE = "destabilize";

	public const string scriptName = "StableAreaAnimatable";
	[SerializeField] private Animator stableAreaAnimator;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerStabilize = false;
	[SerializeField] private bool triggerDestabilize = false;

    private bool isBlockProcess;
    private bool noPrompt;
    void Awake() {
		this.isPlaying = false;

        EventBroadcaster.Instance.AddObserver(EventNames.SPAWN_BLOCKS_PROCESS_ON, BlockProcessOn);
        EventBroadcaster.Instance.AddObserver(EventNames.SPAWN_BLOCKS_PROCESS_OFF, BlockProcessOff);
        EventBroadcaster.Instance.AddObserver(EventNames.STABLE_AREA_NO_PROMPT, NoPrompt);
        EventBroadcaster.Instance.AddObserver(EventNames.STABLE_AREA_WITH_PROMPT, WithPrompt);
    }
    private void OnDestroy() {

        EventBroadcaster.Instance.RemoveObserver(EventNames.SPAWN_BLOCKS_PROCESS_ON);
        EventBroadcaster.Instance.RemoveObserver(EventNames.SPAWN_BLOCKS_PROCESS_OFF);
        EventBroadcaster.Instance.RemoveObserver(EventNames.STABLE_AREA_NO_PROMPT);
        EventBroadcaster.Instance.RemoveObserver(EventNames.STABLE_AREA_WITH_PROMPT);
    }
    public void NoPrompt() {
        this.noPrompt = true;
    }
    public void WithPrompt() {
        this.noPrompt = false;
    }
    void Start () {
		MAX_DURATION = 0.2f;
		this.stableAreaAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (stableAreaAnimator, "stableAreaAnimator", scriptName);
		this.ResetTriggerVariables ();
	}
    public void BlockProcessOn() {
        this.isBlockProcess = true;
    }
    public void BlockProcessOff() {
        this.isBlockProcess = false;
    }
    void Update () {
		SwitchTriggers ();
	}

	public override void ResetTriggerVariables() {
		triggerStabilize = false;
		triggerDestabilize = false;
	}

	// Functions to be called by external scripts
	public void Stabilize() {
        if (!noPrompt) {
            Debug.Log("<color=green>STAB ANIM</color>");
            this.triggerStabilize = true;
        }
	}

	public void Destabilize() {
        //if (!isBlockProcess) {
        if (!noPrompt) {
            Debug.Log("<color=green>DES ANIM</color>");
            this.triggerDestabilize = true;
        }
            //		Debug.Log ("<color=green>TRIGGER IS </color>"+this.triggerDestabilize);
        //}
	}

	public override void ResetTriggers() {
		stableAreaAnimator.ResetTrigger (STABILIZE);
		stableAreaAnimator.ResetTrigger (DESTABILIZE);
	}
	public override void SwitchTriggers() {
		ResetTriggers ();

//		Debug.Log ("<color=blue>Trigger ENTER</color>");
		if (triggerStabilize) {
			this.AnimationPlay ();
			stableAreaAnimator.SetTrigger (STABILIZE);
//			Debug.Log ("<color=green>Trigger STAB</color>");
		} else if (triggerDestabilize) {
			this.AnimationPlay ();
			stableAreaAnimator.SetTrigger (DESTABILIZE);
//			Debug.Log ("<color=green>Trigger DES</color>");
		}

		ResetTriggerVariables ();
	}

}
