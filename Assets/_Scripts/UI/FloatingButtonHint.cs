using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingButtonHint : MonoBehaviour {

//	[SerializeField]Sprite jumpSprite;
//	[SerializeField]Sprite interactSprite;
//	GameObject btnJump;
//	GameObject btnInteract;
	public bool isTangible = true;
	public Animator animator;

	private bool playerInVicinity = false;
	private SpriteRenderer sprite;

	GameObject collided;

	void Start() {
		Debug.Log ("Floating Button Hint: Start");
		this.animator = gameObject.GetComponent<Animator> ();
		this.sprite = gameObject.GetComponent<SpriteRenderer> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		collided = this.gameObject;
		if(isTangible && other.gameObject.CompareTag("Player")) {
			other.gameObject.GetComponent<Animator> ().SetBool("isHint", true);
			StartCoroutine (OpenHint ());
			#if UNITY_ANDROID
//				EventManager.DisableJumpButton ();
//			EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_JUMP);
			Parameters parameters = new Parameters();
			parameters.PutExtra("FLAG", false);
			EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_JUMP, parameters);

			parameters = new Parameters();
			parameters.PutExtra("FLAG", true);
			EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_INTERACT, parameters);
			#endif

		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (!isTangible && this.sprite.enabled == true && other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<Animator> ().SetBool("isHint", false);
			StartCoroutine (CloseHint ());
			#if UNITY_ANDROID

//				GameController_v7.Instance.GetEventManager().DisableInteractButton();
//			EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_INTERACT);
//				EventManager.DisableInteractButton ();
			Parameters parameters = new Parameters();
			parameters.PutExtra("FLAG", true);
			EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_JUMP, parameters);

			parameters = new Parameters();
			parameters.PutExtra("FLAG", false);
			EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_INTERACT, parameters);
			#endif
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<Animator> ().SetBool("isHint", false);
			StartCoroutine (CloseHint ());
			#if UNITY_ANDROID
			//				EventManager.DisableInteractButton ();
//			GameController_v7.Instance.GetEventManager().DisableInteractButton();

//			EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_INTERACT);
			Parameters parameters = new Parameters();
			parameters.PutExtra("FLAG", true);
			EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_JUMP, parameters);

			parameters = new Parameters();
			parameters.PutExtra("FLAG", false);
			EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_INTERACT, parameters);

			#endif
		}
	}

	IEnumerator OpenHint() {
		Debug.Log ("Player at Entrance");
		playerInVicinity = true;
		this.sprite.enabled = true;

		animator.SetTrigger ("isOpen");
		while (AnimationIsPlaying ()) {
//			Debug.Log (animator.GetCurrentAnimatorStateInfo(0).length + "      " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

			yield return null;
		}

		Parameters parameters = new Parameters ();
		parameters.PutExtra ("LAYER_VALUE", collided.layer);
		EventBroadcaster.Instance.PostEvent (EventNames.DISPLAY_HINT, parameters);
//		EventManager.DisplayHintOnTrigger (collided);
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

//		EventManager.RemoveHintOnTrigger ();
		EventBroadcaster.Instance.PostEvent(EventNames.REMOVE_HINT);
	}

	bool AnimationIsPlaying() {
		if(isTangible && 1+animator.GetCurrentAnimatorStateInfo(0).length < animator.GetCurrentAnimatorStateInfo(0).normalizedTime) {
			return true;
		}
		return false;
	}
}
