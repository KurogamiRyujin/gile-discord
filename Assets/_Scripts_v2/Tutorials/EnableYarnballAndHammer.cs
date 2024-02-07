using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables the player avatar's weapons.
/// </summary>
public class EnableYarnballAndHammer : MonoBehaviour {
	

	private PlayerYuni player;
	private bool isPlaying;
	private bool isTriggered;

	void Awake() {
		this.isPlaying = false;
		this.isTriggered = false;
	}
	private PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}
	private IEnumerator EnableItems() {
		this.isPlaying = true;
		this.isTriggered = true;

		this.GetPlayer ().AcquireNeedle ();
		this.GetPlayer ().AcquireHammer ();
		EventBroadcaster.Instance.PostEvent (EventNames.OPEN_YARNBALL);

		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);

		yield return null;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni> ()) {
			player = other.gameObject.GetComponent<PlayerYuni> ();
			PlayScenes ();
		}
	}
	public void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			StartCoroutine (EnableItems ());
		}
	}
}
