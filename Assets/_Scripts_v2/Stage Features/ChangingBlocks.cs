using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SessionTags;

public class ChangingBlocks : MonoBehaviour {

	[SerializeField] private HollowBlockParent hollowBlockPrefab;

	[SerializeField] private SessionTopic topic = SessionTopic.SIMILAR;
	[SerializeField] private int blockCount;
	[SerializeField] private List<HollowBlockParent> blocks;
	[SerializeField] private List<Transform> blockPlacements;

	// Use this for initialization
	void Start () {
		if (blockCount > blockPlacements.Count)
			blockCount = blockPlacements.Count;

		for (int i = 0; i < blockCount; i++) {
			HollowBlockParent temp = Instantiate<HollowBlockParent> (this.hollowBlockPrefab);

			temp.transform.position = blockPlacements [i].position;
			blocks.Add (temp);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
