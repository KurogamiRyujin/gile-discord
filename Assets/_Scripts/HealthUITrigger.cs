using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUITrigger : MonoBehaviour {

	public Animator animator;
	bool isFlipped;

	void Start() {
		isFlipped = false;
	}

	public void Flip() {
		if (isFlipped) {
			animator.SetBool ("isFlipped", false);
			isFlipped = false;
		} else {
			animator.SetBool ("isFlipped", true);
			isFlipped = true;
		}
	}
}
