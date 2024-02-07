using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakAllBlocks : MonoBehaviour {

	private List<HollowBlock> blocks;

	void Awake() {
		blocks = new List<HollowBlock> ();
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_DEATH, this.ResetBlocks);
	}

	void Start() {
		GameObject[] temps = GameObject.FindGameObjectsWithTag ("HollowBlock");

		foreach (GameObject block in temps) {
			blocks.Add (block.GetComponent<HollowBlock> ());
		}
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.ON_PLAYER_DEATH, this.ResetBlocks);
	}

	private void ResetBlocks() {
		foreach (HollowBlock block in blocks) {
			block.Break ();
		}
	}
}
