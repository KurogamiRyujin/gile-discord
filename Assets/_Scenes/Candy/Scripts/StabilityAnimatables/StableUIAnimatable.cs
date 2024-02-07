using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StableUIAnimatable : AnimatableClass {

	public const string OPEN = "open";
//	public const string DESTABILIZE = "destabilize";

	public const string scriptName = "StableUIAnimatable";
	[SerializeField] private Animator stableUIAnimator;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerOpen = false;
    //	[SerializeField] private bool triggerDestabilize = false;
    private bool noPrompt;

    public void NoPrompt() {
        Debug.Log("NO PROMPT TRUE");
        this.noPrompt = true;
    }
    public void WithPrompt() {
        Debug.Log("NO PROMPT FALSE");
        this.noPrompt = false;
    }
    void Awake() {
		this.isPlaying = false;

		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, Open);
        EventBroadcaster.Instance.AddObserver(EventNames.STABLE_AREA_NO_PROMPT, NoPrompt);
        EventBroadcaster.Instance.AddObserver(EventNames.STABLE_AREA_WITH_PROMPT, WithPrompt);
    }
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.STABLE_AREA);
        EventBroadcaster.Instance.RemoveObserver(EventNames.STABLE_AREA_NO_PROMPT);
        EventBroadcaster.Instance.RemoveObserver(EventNames.STABLE_AREA_WITH_PROMPT);

    }
    void Start () {
		MAX_DURATION = 0.2f;
		this.stableUIAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (stableUIAnimator, "stableUIAnimator", scriptName);
		this.ResetTriggerVariables ();
	}

	void Update () {
		SwitchTriggers ();
	}

	public override void ResetTriggerVariables() {
		triggerOpen = false;
//		triggerDestabilize = false;
	}

	// Functions to be called by external scripts
	public void Open() {
        if (!noPrompt) {
            this.triggerOpen = true;
        }
	}

//	public void Destabilize() {
//		this.triggerDestabilize = true;
//	}

	public override void ResetTriggers() {
		stableUIAnimator.ResetTrigger (OPEN);
//		stableAreaAnimator.ResetTrigger (DESTABILIZE);
	}
	public override void SwitchTriggers() {
		ResetTriggers ();

		if (triggerOpen) {
			this.AnimationPlay ();
			SoundManager.Instance.Play (AudibleNames.Results.SUCCESS, false);
			stableUIAnimator.SetTrigger (OPEN);
		}
//		else if (triggerDestabilize) {
//			this.AnimationPlay ();
//			stableAreaAnimator.SetTrigger (DESTABILIZE);
//		}

		ResetTriggerVariables ();
	}
}
