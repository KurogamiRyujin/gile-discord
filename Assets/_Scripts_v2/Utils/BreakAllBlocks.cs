using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour to break all filled blocks in a room.
/// </summary>
public class BreakAllBlocks : MonoBehaviour {
    /// <summary>
    /// The list of blocks to break.
    /// </summary>
	private List<HollowBlock> blocks;

    /// <summary>
    /// Standard Unity function.
    /// </summary>
	void Awake() {
		blocks = new List<HollowBlock> ();
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_DEATH, this.ResetBlocks);
	}

    /// <summary>
    /// Unity Standard Function. Starts up the MonoBehaviour. Called once throughout its life.
    /// </summary>
	void Start() {
		GameObject[] temps = GameObject.FindGameObjectsWithTag ("HollowBlock");

		foreach (GameObject block in temps) {
			blocks.Add (block.GetComponent<HollowBlock> ());
		}
	}
    /// <summary>
    /// Unity Function called when the MonoBehaviour is destroyed. Serves as a clean up function.
    /// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.ON_PLAYER_DEATH, this.ResetBlocks);
	}

    /// <summary>
    /// Breaks all filled blocks in the room.
    /// </summary>
	private void ResetBlocks() {
		foreach (HollowBlock block in blocks) {
			block.Break ();
		}
	}
}
