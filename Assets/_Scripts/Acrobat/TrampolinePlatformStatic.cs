using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolinePlatformStatic : MonoBehaviour {
	[SerializeField] private bool isActivated = true;
	[SerializeField] private TrampolinePlatformAnimatable trampolineAnimatable;
	[SerializeField] private LampManager lampManager;

	void Start () {
		NullCheck ();
		this.isActivated = true;	
	}
	void NullCheck() {
		if(this.trampolineAnimatable == null) {
			trampolineAnimatable = gameObject.GetComponentInChildren<TrampolinePlatformAnimatable> ();
		}
		if (this.lampManager != null) {
			lampManager = gameObject.GetComponentInParent<LampManager> ();
		}
	}

	public void Activate() {
		this.isActivated = true;
		this.trampolineAnimatable.Activate ();
	}
	public void Deactivate() {

		this.isActivated = false;

		this.trampolineAnimatable.Deactivate ();
	}

	void PlayerJump(PlayerInput playerInput) {
		playerInput.isHighJump = true;
		playerInput.isJumping = true;

		trampolineAnimatable.Bounce ();
		SoundManager.Instance.Play (AudibleNames.Trampoline.BOUNCE, false);
		if(lampManager != null)
			lampManager.Refresh ();
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (this.isActivated) {
			if (other.gameObject.GetComponent<PlayerYuni>() != null) {
				Debug.Log ("Entered Trampoline");
				PlayerYuni player= other.gameObject.GetComponent<PlayerYuni> ();
				PlayerInput playerInput = player.GetPlayerInput ();
				PlayerJump (playerInput);

			}
		}
	}
	void OnTriggerStay2D(Collider2D other) {
		if (this.isActivated) {
			if (other.gameObject.GetComponent<PlayerYuni>() != null) {
				Debug.Log ("Entered Trampoline");				
				PlayerYuni player= other.gameObject.GetComponent<PlayerYuni> ();
				PlayerInput playerInput = player.GetPlayerInput ();
				PlayerJump (playerInput);

			}

		}
	}

	public void SetLampManager(LampManager manager) {
		this.lampManager = manager;
	}
}
