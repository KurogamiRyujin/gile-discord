using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakUIAnimatable : AnimatableClass {

	public const string OPEN = "open";
	//	public const string DESTABILIZE = "destabilize";

	public const string scriptName = "BreakUIAnimatable";
	[SerializeField] private Animator breakUIAnimator;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerOpen = false;
    //	[SerializeField] private bool triggerDestabilize = false;
    private bool noPrompt;

    void Awake() {
		this.isPlaying = false;

		EventBroadcaster.Instance.AddObserver (EventNames.BREAK_AREA, Open);

        EventBroadcaster.Instance.AddObserver(EventNames.STABLE_AREA_NO_PROMPT, NoPrompt);
        EventBroadcaster.Instance.AddObserver(EventNames.STABLE_AREA_WITH_PROMPT, WithPrompt);
    }
    public void NoPrompt() {
        Debug.Log("NO PROMPT TRUE");
        this.noPrompt = true;
    }
    public void WithPrompt() {
        Debug.Log("NO PROMPT FALSE");
        this.noPrompt = false;
    }
    void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.BREAK_AREA);

        EventBroadcaster.Instance.RemoveObserver(EventNames.STABLE_AREA_NO_PROMPT);
        EventBroadcaster.Instance.RemoveObserver(EventNames.STABLE_AREA_WITH_PROMPT);
    }
	void Start () {
		MAX_DURATION = 0.2f;
		this.breakUIAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (breakUIAnimator, "breakUIAnimator", scriptName);
		this.ResetTriggerVariables ();
	}

	void Update () {
		SwitchTriggers ();
	}

	public override void ResetTriggerVariables() {
		triggerOpen = false;
	}

	// Functions to be called by external scripts
	public void Open() {
        if (!noPrompt) {
            Debug.Log("ENTERED CAUSE PROMPT FALSE");
            this.triggerOpen = true;
        }
	}

	public override void ResetTriggers() {
		breakUIAnimator.ResetTrigger (OPEN);
	}
	public override void SwitchTriggers() {
		ResetTriggers ();

		if (triggerOpen) {
			this.AnimationPlay ();
			SoundManager.Instance.Play (AudibleNames.Results.BREAK, false);
			breakUIAnimator.SetTrigger (OPEN);
		}
		ResetTriggerVariables ();
	}
}
