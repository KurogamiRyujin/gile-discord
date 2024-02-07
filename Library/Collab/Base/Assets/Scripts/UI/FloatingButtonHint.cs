using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingButtonHint : MonoBehaviour {


	public bool isTangible = true;
	public Animator animator;

	private bool playerInVicinity = false;
	private SpriteRenderer sprite;

	void Start() {
		this.animator = gameObject.GetComponent<Animator> ();
		this.sprite = gameObject.GetComponent<SpriteRenderer> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(isTangible && other.gameObject.CompareTag("Player")) {
			StartCoroutine (OpenHint ());
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (!isTangible && this.sprite.enabled == true && other.gameObject.CompareTag ("Player")) {
			StartCoroutine (CloseHint ());
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			StartCoroutine (CloseHint ());
		}
	}

	IEnumerator OpenHint() {
		Debug.Log ("Player at Entrance");
		playerInVicinity = true;
		this.sprite.enabled = true;

		animator.SetTrigger ("isOpen");
		while (AnimationIsPlaying ()) {
			Debug.Log (animator.GetCurrentAnimatorStateInfo(0).length + "      " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			yield return null;
		}
	}

	IEnumerator CloseHint() {
		animator.SetTrigger ("isClose");
		while (AnimationIsPlaying ()) {
//			Debug.Log (animator.GetCurrentAnimatorStateInfo(0).length + "      " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			yield return null;
		}

		Debug.Log ("Player left Entrance...");
		playerInVicinity = false;
		this.sprite.enabled = false;
	}

	bool AnimationIsPlaying() {
		if(isTangible && 1+animator.GetCurrentAnimatorStateInfo(0).length < animator.GetCurrentAnimatorStateInfo(0).normalizedTime) {
			return true;
		}
		return false;
	}
}
