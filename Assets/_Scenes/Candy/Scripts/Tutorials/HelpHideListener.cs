using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpHideListener : MonoBehaviour {

	// To be used in backstage tutorial only
	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.HIDE_HELP_BUTTON, HideHelp);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_HELP_BUTTON, ShowHelp);

	}
	public void ShowHelp() {
		gameObject.SetActive (true);
	}
	public void HideHelp() {
		gameObject.SetActive (false);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.HIDE_HELP_BUTTON);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_HELP_BUTTON);

	}
}
