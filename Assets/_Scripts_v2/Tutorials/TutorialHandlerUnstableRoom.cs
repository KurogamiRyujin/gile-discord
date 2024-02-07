using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandlerUnstableRoom : TutorialHandler {
	void Awake() {
		base.Initialize ();
		EventBroadcaster.Instance.AddObserver (EventNames.CLOSE_TUTORIAL_UNSTABLE_ROOM, TutorialClosed);

	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.CLOSE_TUTORIAL_UNSTABLE_ROOM);
	}

	public override void Action() {
		Debug.Log ("Action by TutorialHandlerUnstableRoom");
		EventBroadcaster.Instance.PostEvent (EventNames.SHOW_TUTORIAL_UNSTABLE_ROOMS);
	}
}
