using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandlerPowerCharm : TutorialHandler {
	void Awake() {
		base.Initialize ();
		EventBroadcaster.Instance.AddObserver (EventNames.CLOSE_TUTORIAL_POWER_CHARMS, TutorialClosed);

	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.CLOSE_TUTORIAL_POWER_CHARMS);
	}

	public override void Action() {
		Debug.Log ("Action by TutorialHandlerPowerCharm");
		EventBroadcaster.Instance.PostEvent (EventNames.SHOW_TUTORIAL_POWER_CHARMS);
	}
}
