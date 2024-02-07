using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandlerFixingBoxes : TutorialHandler {
	void Awake() {
		base.Initialize ();
		EventBroadcaster.Instance.AddObserver (EventNames.CLOSE_TUTORIAL_FIXING_BLOCKS, TutorialClosed);

	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.CLOSE_TUTORIAL_FIXING_BLOCKS);
	}

	public override void Action() {
		Debug.Log ("Action by TutorialHandlerFixingBoxes");
		EventBroadcaster.Instance.PostEvent (EventNames.SHOW_TUTORIAL_FIXING_BLOCKS);
	}
}
