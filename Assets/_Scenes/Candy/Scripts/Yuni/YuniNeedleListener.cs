using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuniNeedleListener : MonoBehaviour {
	[SerializeField] private PlayerYuni player;

	void Awake () {
		EventBroadcaster.Instance.AddObserver(EventNames.YUNI_ACQUIRED_NEEDLE, AcquiredNeedle);
		EventBroadcaster.Instance.AddObserver(EventNames.YUNI_THREW_NEEDLE, HideNeedle);

	}

	void Start() {
		CheckForNeedle ();
	}

	public void CheckForNeedle() {
		if (this.GetPlayerYuni ().GetPlayerAttack ().HasNeedle ()) {
			this.AcquiredNeedle ();
		} else {
			this.HideNeedle ();
		}
	}


	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver(EventNames.YUNI_ACQUIRED_NEEDLE);
		EventBroadcaster.Instance.RemoveObserver(EventNames.YUNI_THREW_NEEDLE);

	}

	public void HideNeedle() {
		gameObject.SetActive (false);
	}

	public void AcquiredNeedle() {
		if (this.GetPlayerYuni ().GetPlayerAttack ().HasNeedle () &&
			!this.GetPlayerYuni ().GetPlayerAttack ().UsingHammer ()) {
			gameObject.SetActive (true);
		}
	}

	public PlayerYuni GetPlayerYuni() {
		if (this.player == null) {
			this.player = GetComponentInParent<PlayerYuni> ();
		}
		return this.player;
	}

}
