using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorRightCollider : MonoBehaviour {

	ElevatorAnimatable elevator;
	bool opened = true;

	void Start() {
		elevator = GameObject.FindGameObjectWithTag ("Elevator").GetComponent<ElevatorAnimatable> ();
	}

	void Update() {
//		Debug.Log ("OPENED: " + opened);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag("Player")) {
			if (opened) {
				elevator.Close ();
				opened = false;
			} else {
				elevator.Open ();
				opened = true;

			}
		}
	}

}
