using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartitionController : MonoBehaviour {

	public float MIN_PartitionSize = 0.25f;

	// TODO: Check if object passes for MIN_PartitionSize before partitioning
	// If it is larger than the MIN_PartitionSize, change it accordingly (Error Handling)

	public void Partition(PartitionableObject original, GameObject partitionableClone, int sliceCount) {

		if (original == null)
			Debug.Log ("Original PartitionableObject is null.");
		if (partitionableClone == null)
			Debug.Log ("Cloned PartitionableObject is null.");
		original.Partition ();
		partitionableClone.GetComponent<PartitionedClone> ().Partition (sliceCount, original.partitionPiece, original.gameObject);
	}
}
