using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningBlocks : MonoBehaviour {

	private const int MAX_ACTIVE_BLOCKS = 2;

	public GameObject blockPrefab;
	public int respawnTimer = 2;
	public int blockWidth = 1;

	private GameObject block;
	private bool isSpawning;
	private PartitionableObject partitionableObject;
	private PartitionablePhysicsObject physics;

	List<GameObject> blocks;
	// Use this for initialization
	void Start () {
		blocks = new List<GameObject> ();
		this.Spawn ();
	}

	void Update() {
		if (block == null && !isSpawning) {
			isSpawning = true;
			StartCoroutine (SpawnPlatform ());
		} else if (partitionableObject != null && partitionableObject.IsTangible ()) {
			physics.enabled = true;
//			block.transform.parent = null;

//			this.gameObject.transform.DetachChildren ();
			partitionableObject = null;
			physics = null;
			block = null;
		}
	}

	private IEnumerator SpawnPlatform() {
		for (int i = 0; i < respawnTimer; i++) {
			yield return new WaitForSeconds (1.0f);
		}

		isSpawning = false;
		int numberOfActiveBlocks = GameObject.FindGameObjectsWithTag ("PartitionableObject").Length;
		print (numberOfActiveBlocks);
		Spawn ();
	}
	
	private void Spawn() {
		block = Instantiate (blockPrefab, this.gameObject.transform.position, Quaternion.identity);
		block.transform.position = this.gameObject.transform.position;
		block.transform.localScale = new Vector3 (block.transform.localScale.x /* blockWidth*/, block.transform.localScale.y, block.transform.localScale.z);
//		block.transform.SetParent (this.gameObject.transform);

		partitionableObject = block.GetComponent<PartitionableObject> ();
		partitionableObject.blockWidth = this.blockWidth;
		physics = block.GetComponent<PartitionablePhysicsObject> ();
		physics.enabled = false;

		blocks.Add(block);
		print (blocks.Count);
		while(blocks.Count > MAX_ACTIVE_BLOCKS)
		{
			if(blocks[0] != null)
				Destroy(blocks[0].gameObject);
			blocks.RemoveAt(0);
		}

		block.SetActive (true);
	}
}
