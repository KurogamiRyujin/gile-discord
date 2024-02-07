using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLeftCollider : MonoBehaviour {

	ElevatorAnimatable elevator;
	bool opened = false;

	void Start() {
		elevator = GameObject.FindGameObjectWithTag ("Elevator").GetComponent<ElevatorAnimatable> ();
	}

	void Update() {
//		Debug.Log ("OPENED: " + opened);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag("Player")) {
			if (!opened) {
				elevator.Open ();
				opened = true;
			} else {
				elevator.Close ();
				opened = false;

			}
		}
	}

}
