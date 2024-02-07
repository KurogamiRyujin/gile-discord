using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTitleTrigger : MonoBehaviour {

	private bool triggered = false;

	// Use this for initialization
	void Start () {
//		EventManager.DisplaySceneTitleMethods += Display;
//		EventManager.RemoveSceneTitleMethods += Hide;
//		EventManager.ResetSceneTitleTriggerMethods += ResetTrigger;
	}

	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.DISPLAY_SCENE_TITLE, Display);
		EventBroadcaster.Instance.AddObserver (EventNames.REMOVE_SCENE_TITLE, Hide);
		EventBroadcaster.Instance.AddObserver (EventNames.RESET_SCENE_TITLE_TRIGGER, ResetTrigger);
	}

	void OnDestroy() {
//		EventManager.DisplaySceneTitleMethods -= Display;
//		EventManager.RemoveSceneTitleMethods -= Hide;
//		EventManager.ResetSceneTitleTriggerMethods -= ResetTrigger;

		EventBroadcaster.Instance.RemoveObserver (EventNames.DISPLAY_SCENE_TITLE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.REMOVE_SCENE_TITLE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.RESET_SCENE_TITLE_TRIGGER);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Display() {
		if (!triggered) {
			this.gameObject.SetActive (true);
			triggered = true;
		}
	}

	void Hide() {
		this.gameObject.SetActive (false);
	}

	void ResetTrigger() {
		triggered = false;
	}
}
