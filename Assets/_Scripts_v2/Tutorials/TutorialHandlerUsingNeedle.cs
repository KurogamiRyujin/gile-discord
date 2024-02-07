using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandlerUsingNeedle : TutorialHandler {
	void Awake() {
		base.Initialize ();
		EventBroadcaster.Instance.AddObserver (EventNames.CLOSE_TUTORIAL_USING_NEEDLE, TutorialClosed);

	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.CLOSE_TUTORIAL_USING_NEEDLE);
	}

	public override void Action() {
		Debug.Log ("Action by TutorialHandlerUsingNeedle");
		EventBroadcaster.Instance.PostEvent (EventNames.SHOW_TUTORIAL_USING_NEEDLE);
	}
}
