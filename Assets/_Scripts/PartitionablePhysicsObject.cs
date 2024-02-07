using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartitionablePhysicsObject : PhysicsObject {
	[SerializeField] bool isPurelyKinematic = false;

	private PartitionableObject partitionablObject;

	// Use this for initialization
	void Start () {
		partitionablObject = gameObject.GetComponent<PartitionableObject> ();

	}


	protected override void FixedUpdate() {
//		if (partitionablObject.IsTangible () && !isPurelyKinematic) {
		if (!isPurelyKinematic) {
			base.FixedUpdate ();
		}
//		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
