using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCloseHideListener : MonoBehaviour {

	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_UNSTABLE_ROOMS, Hide);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_CARRY_ITEM, Hide);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_FIXING_BLOCKS, Hide);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_USING_NEEDLE, Hide);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_USING_HAMMER, Hide);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_POWER_CHARMS, Hide);

		EventBroadcaster.Instance.AddObserver (EventNames.CLOSE_TUTORIAL_UNSTABLE_ROOM, Show);
		EventBroadcaster.Instance.AddObserver (EventNames.CLOSE_TUTORIAL_CARRY_ITEM, Show);
		EventBroadcaster.Instance.AddObserver (EventNames.CLOSE_TUTORIAL_FIXING_BLOCKS, Show);
		EventBroadcaster.Instance.AddObserver (EventNames.CLOSE_TUTORIAL_USING_NEEDLE, Show);
		EventBroadcaster.Instance.AddObserver (EventNames.CLOSE_TUTORIAL_USING_HAMMER, Show);
		EventBroadcaster.Instance.AddObserver (EventNames.CLOSE_TUTORIAL_POWER_CHARMS, Show);
	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_TUTORIAL_UNSTABLE_ROOMS);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_TUTORIAL_CARRY_ITEM);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_TUTORIAL_FIXING_BLOCKS);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_TUTORIAL_USING_NEEDLE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_TUTORIAL_USING_HAMMER);

		EventBroadcaster.Instance.RemoveObserver (EventNames.CLOSE_TUTORIAL_UNSTABLE_ROOM);
		EventBroadcaster.Instance.RemoveObserver (EventNames.CLOSE_TUTORIAL_CARRY_ITEM);
		EventBroadcaster.Instance.RemoveObserver (EventNames.CLOSE_TUTORIAL_FIXING_BLOCKS);
		EventBroadcaster.Instance.RemoveObserver (EventNames.CLOSE_TUTORIAL_USING_NEEDLE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.CLOSE_TUTORIAL_USING_HAMMER);
		EventBroadcaster.Instance.RemoveObserver (EventNames.CLOSE_TUTORIAL_POWER_CHARMS);
	}
	public void Show() {
		gameObject.SetActive (true);
	}
	public void Hide() {
		gameObject.SetActive (false);
	}

}
