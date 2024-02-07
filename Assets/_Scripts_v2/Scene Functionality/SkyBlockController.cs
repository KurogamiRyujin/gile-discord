using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns and handles sky blocks in a room.
/// </summary>
public class SkyBlockController : MonoBehaviour {

    /// <summary>
    /// Reference to the base sky block asset. Used as a basis for instances of sky blocks in the scene.
    /// </summary>
	[SerializeField] private SkyBlock skyblockPrefab;

    /// <summary>
    /// List of transforms where sky blocks will be spawned.
    /// 
    /// Always arbitrarily set in the scene.
    /// </summary>
	[SerializeField] private List<Transform> placements = new List<Transform> ();
    /// <summary>
    /// List of sky blocks instantiated in the room.
    /// </summary>
	private List<SkyBlock> skyBlocks = new List<SkyBlock> ();

    /*
	public void SpawnSkyBlocks(int blockCount) {
		PurgeSkyBlocks ();

		if (blockCount > 0) {
			if (blockCount > placements.Count)
				blockCount = placements.Count;

			for (int i = 0; i < blockCount; i++) {
				SkyBlock block = Instantiate<SkyBlock> (this.skyblockPrefab);

				block.transform.position = placements [i].position;
				block.transform.SetParent (placements [i]);
				block.transform.localScale = new Vector2 (1f, 1f);
				this.skyBlocks.Add (block);
			}
		}
	}
    */

    /// <summary>
    /// Coroutine to spawn n number of sky blocks.
    /// 
    /// Sky blocks spawned cannot exceed the number of placements in the room.
    /// </summary>
    /// <param name="blockCount">Requested number of sky blocks to spawn</param>
    /// <returns>None</returns>
    public IEnumerator SpawnSkyBlocks(int blockCount) {
        PurgeSkyBlocks();

        if (blockCount > 0) {
            if (blockCount > placements.Count)
                blockCount = placements.Count;

            for (int i = 0; i < blockCount; i++) {
                SkyBlock block = Instantiate<SkyBlock>(this.skyblockPrefab);

                block.transform.position = placements[i].position;
                block.transform.SetParent(placements[i]);
                block.transform.localScale = new Vector2(1f, 1f);
                this.skyBlocks.Add(block);
                yield return null;
            }
        }
        yield return null;
    }

    /// <summary>
    /// Destroys all instantiated sky blocks in the room, if any.
    /// </summary>
    public void PurgeSkyBlocks() {
		if (this.skyBlocks.Count > 0) {
			foreach (SkyBlock block in this.skyBlocks) {
				if (block != null)
					Destroy (block.gameObject);
			}

			this.skyBlocks.Clear ();
		}
	}

    /// <summary>
    /// Returns all the instantiated sky blocks in the room, if any.
    /// </summary>
    /// <returns></returns>
	public SkyBlock[] GetSkyBlocks() {
		return this.skyBlocks.ToArray ();
	}
}
