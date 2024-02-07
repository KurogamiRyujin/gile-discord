using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillController : MonoBehaviour {

	public void FillTarget(int fillCount, int totalFill, PartitionableObject target) {
		target.Fill (fillCount, totalFill);
	}
}
