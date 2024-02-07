using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private bool isStable;

	void Awake() {
		this.isStable = false;
	}


	void Start() {
		StartCoroutine (StartMission ());
		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, Stabilize);

	}
	public void Stabilize() {
		this.isStable = true;
	}


	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.STABLE_AREA);
	}
	IEnumerator StartMission() {
		// Choose which block to break inorder to stabilize the room
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

		while(!this.isStable) {
			yield return null;
		}
		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();
	}
}
