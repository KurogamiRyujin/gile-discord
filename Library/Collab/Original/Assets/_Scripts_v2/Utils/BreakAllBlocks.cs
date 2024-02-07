using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour to break all filled blocks in a room.
/// </summary>
public class BreakAllBlocks : MonoBehaviour {

    /// <summary>
    /// Reference to all ghost blocks in the room.
    /// </summary>
	private List<HollowBlock> blocks;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		blocks = new List<HollowBlock> ();
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_DEATH, this.ResetBlocks);
	}

    /// <summary>
	/// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
	/// </summary>
	void Start() {
		GameObject[] temps = GameObject.FindGameObjectsWithTag ("HollowBlock");

		foreach (GameObject block in temps) {
			blocks.Add (block.GetComponent<HollowBlock> ());
		}
	}

    /// <summary>
    /// Raises the destroy event.
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
