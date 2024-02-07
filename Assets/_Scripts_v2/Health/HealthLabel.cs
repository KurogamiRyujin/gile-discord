using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthLabel : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI textMesh;
	void Awake () {
		this.textMesh = GetComponent<TextMeshProUGUI> ();
		EventBroadcaster.Instance.AddObserver (EventNames.ON_HEALTH_UPDATE, this.UpdateLabel);
	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_HEALTH_UPDATE);
	}

	public TextMeshProUGUI GetTextMesh() {
		if (this.textMesh == null) {
			this.textMesh = GetComponent<TextMeshProUGUI> ();
		}
		return this.textMesh;
	}
	public void UpdateLabel(Parameters parameters) {
		float currentHP = parameters.GetFloatExtra ("currentHP", 0f);

		Debug.Log ("CURR HP IS "+currentHP);
		GetTextMesh().text = ""+currentHP;
//		float maxHP = parameters.GetFloatExtra ("maxHP", 0f);
	}
}
