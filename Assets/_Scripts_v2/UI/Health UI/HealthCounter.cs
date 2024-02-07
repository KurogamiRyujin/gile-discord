using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCounter : MonoBehaviour {
	public const string CURRENT_HEALTH = "currentHealth";
	public const string MAX_HEALTH = "maxHealth";

	private Text healthText;
	// Use this for initialization
	void Start () {
		this.healthText = GetComponent<Text> ();
//		EventBroadcaster.Instance.AddObserver (EventNames.ON_HEALTH_UPDATE, UpdateText);
	}

	void OnDestroy() {
//		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_HEALTH_UPDATE);

	}

	void UpdateText (Parameters parameters) {
		int current = parameters.GetIntExtra (CURRENT_HEALTH, 0);
		int max = parameters.GetIntExtra (MAX_HEALTH, 0);

		this.healthText.text = current+"/"+max;
	}
}
