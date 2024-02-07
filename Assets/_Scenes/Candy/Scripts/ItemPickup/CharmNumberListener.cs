using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharmNumberListener : MonoBehaviour {
	private PlayerYuni player;
	[SerializeField] private Text numberText;

	void Awake() {
		this.numberText = GetComponent<Text> ();
		EventBroadcaster.Instance.AddObserver (EventNames.CHARM_PICKUP, UpdateValue);
	}
	void OnDestroy() {

		EventBroadcaster.Instance.RemoveObserver (EventNames.CHARM_PICKUP);
	}

	public void UpdateValue() {
		if (this.GetPlayer () != null) {
			this.numberText.text = ""+this.GetPlayer ().GetPlayerAttack ().getEquippedDenominator ();
		}
	}

	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}
}
