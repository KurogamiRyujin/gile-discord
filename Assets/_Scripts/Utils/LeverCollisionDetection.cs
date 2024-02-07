using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverCollisionDetection : MonoBehaviour {

	ElevatorAnimatable elevator;
	bool hit = false;

	void Start() {
		elevator = GameObject.FindGameObjectWithTag ("Elevator").GetComponent<ElevatorAnimatable> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Hammer Child")) {
			elevator.FadeRoof ();
			hit = true;
		}
	}
}
