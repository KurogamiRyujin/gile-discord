using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Partitionable {
	void Partition(int count);
	void ClearPartitions();
}
