using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCircle : MonoBehaviour {

	private Image healthMask;
	private RectTransform rectTransform;

	void Awake() {
		this.healthMask = GetComponent<Image> ();
		this.rectTransform = GetComponent<RectTransform> ();
	}

	public Image GetHealthMask() {
		return this.healthMask;
	}

	public void UpdateHealth(float currentHP, float maxHP) {
		float healthPercentage = currentHP/maxHP;
		float y = this.GetImageHeight () * healthPercentage;
		Vector3 newPosition = new Vector3 (0f, y, 0f);
		AlignToLocal (newPosition);

		// Update Text
		Parameters parameters = new Parameters ();
		parameters.PutExtra ("currentHP", currentHP);
		parameters.PutExtra ("maxHP", maxHP);

		EventBroadcaster.Instance.PostEvent (EventNames.ON_HEALTH_UPDATE, parameters);
	}

	private float GetImageHeight() {
		return this.healthMask.rectTransform.rect.height;
	}

	public void AlignToLocal(Vector3 position) {
		this.rectTransform.localPosition = position;
	}
}
