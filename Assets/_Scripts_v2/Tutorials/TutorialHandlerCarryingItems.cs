using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandlerCarryingItems : TutorialHandler {
	void Awake() {
		base.Initialize ();
		EventBroadcaster.Instance.AddObserver (EventNames.CLOSE_TUTORIAL_CARRY_ITEM, TutorialClosed);

	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.CLOSE_TUTORIAL_CARRY_ITEM);
	}

	public override void Action() {
		Debug.Log ("Action by TutorialHandlerCarryingItems");
		EventBroadcaster.Instance.PostEvent (EventNames.SHOW_TUTORIAL_CARRY_ITEM);
	}
}
