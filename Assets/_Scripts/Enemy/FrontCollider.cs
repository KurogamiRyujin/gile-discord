using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCollider : MonoBehaviour {

	PopcornAttack popcornAttack;
	// Use this for initialization
	void Start () {
		popcornAttack = transform.GetComponentInParent<PopcornAttack> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Jump() {
//		popcornAttack.Jump ();
//		Debug.Log ("Popcorn Jump");
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!other.CompareTag ("Player") && !other.CompareTag ("Enemy")) {
			Jump ();
		}
	}
}
