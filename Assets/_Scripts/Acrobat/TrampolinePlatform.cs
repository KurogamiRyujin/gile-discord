using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolinePlatform : MonoBehaviour {
	[SerializeField] private bool isActivated = false;
	[SerializeField] private TrampolinePlatformAnimatable trampolineAnimatable;
	[SerializeField] private LampManager lampManager;

	[SerializeField] private Collider2D collider;

	void Awake() {
		lampManager = GetComponentInParent<LampManager> ();
		this.collider = GetComponent<BoxCollider2D> ();
//		Physics2D.IgnoreCollision (this.collider, GameObject.FindObjectOfType<PlayerYuni>().GetComponent<Collider2D>());
//		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Trampoline"), LayerMask.NameToLayer("Player"));
	}
	void Start () {
		NullCheck ();
		this.isActivated = false;
	}
	void NullCheck() {
		if(this.trampolineAnimatable == null) {
			trampolineAnimatable = gameObject.GetComponentInChildren<TrampolinePlatformAnimatable> ();
		}
		if (this.lampManager != null) {
			lampManager = gameObject.GetComponentInParent<LampManager> ();
		}
	}
	public void Show() {
		this.trampolineAnimatable.Activate ();
//		Physics2D.IgnoreCollision (this.collider, GameObject.FindObjectOfType<PlayerYuni>().GetComponent<Collider2D>());
//		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Trampoline"), LayerMask.NameToLayer("Player"));
	}
	public void Activate() {
		this.isActivated = true;
//		this.trampolineAnimatable.Bounce ();
		this.trampolineAnimatable.Activate ();
	}
	public void Deactivate() {
		this.isActivated = false;
		this.trampolineAnimatable.Deactivate ();
	}

	IEnumerator PlayerJump(PlayerInput playerInput) {
		playerInput.isHighJump = true;
		playerInput.isJumping = true;
		this.isActivated = false;
//		playerInput.Move ();
		trampolineAnimatable.Bounce ();
		while (trampolineAnimatable.IsPlaying ()) {
			yield return null;
		}
		Debug.Log ("BOUNCE EXIT");
		if(lampManager != null)
			lampManager.Refresh ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (this.isActivated) {
			if (other.gameObject.GetComponent<PlayerYuni> () != null) {
				Debug.Log ("Entered Trampoline");
				PlayerInput playerInput = other.GetComponent<PlayerYuni> ().GetPlayerInput ();
				StartCoroutine (PlayerJump (playerInput));
			}
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (this.isActivated) {
			if (other.gameObject.GetComponent<PlayerYuni>() != null) {
				Debug.Log ("Entered Trampoline");
				PlayerInput playerInput = other.GetComponent<PlayerYuni> ().GetPlayerInput();
				StartCoroutine (PlayerJump (playerInput));
			}
		}
	}

	public void SetLampManager(LampManager manager) {
		this.lampManager = manager;
	}
}
