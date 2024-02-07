using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Listener behaviour for the stability number line's max point label. Handles the display for the label on the number line's far right.
/// </summary>
public class StabilityMaxPointLabelListener : MonoBehaviour {
	/// <summary>
	/// Constant string for the key to retrieve data from a Parameter object.
	/// </summary>
	public const string NEW_MAX = "newMax";
	/// <summary>
	/// Displayed text on the label.
	/// </summary>
	[SerializeField] private TextMeshProUGUI text;
	/// <summary>
	/// Current max value the stability number line is set to.
	/// </summary>
	public float maxValue;

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		maxValue = 1;
		EventBroadcaster.Instance.AddObserver (EventNames.STABILITY_MAX_CHANGE, MaxChange);
		EventBroadcaster.Instance.AddObserver (EventNames.TARGET_MARKER_CHANGED, TargetChange);
		EventBroadcaster.Instance.AddObserver (EventNames.STABILITY_MARKER_CHANGED, StabilityChange);
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.STABILITY_MAX_CHANGE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.TARGET_MARKER_CHANGED);
		EventBroadcaster.Instance.RemoveObserver (EventNames.STABILITY_MARKER_CHANGED);
	}

	/// <summary>
	/// Called when the target marker was updated. Checks and sets if the label should be visible or not.
	/// </summary>
	/// <param name="parameters">Parameter object containing data passed to this function.</param>
	public void TargetChange(Parameters parameters) {
		float numerator = parameters.GetFloatExtra (TargetMarker.NEW_NUMERATOR, 0f);
		float denominator = parameters.GetFloatExtra (TargetMarker.NEW_DENOMINATOR, 1f);

		gameObject.SetActive (true);
		if (numerator / denominator == maxValue) {
			gameObject.SetActive (false);
		}
	}

	/// <summary>
	/// Called when the stability pointer moves. Checks and sets if the label should be visible or not.
	/// </summary>
	/// <param name="parameters">Parameter object containing data passed to this function.</param>
	public void StabilityChange(Parameters parameters) {
		float numerator = parameters.GetFloatExtra (StabilityPointer.NEW_NUMERATOR, 0f);
		float denominator = parameters.GetFloatExtra (StabilityPointer.NEW_DENOMINATOR, 1f);
		int value = (int)(numerator / denominator);
		Debug.Log ("<color=red>ENTERED STABILITY CHANGE</color>"+numerator+"  "+denominator);
		gameObject.SetActive (true);
		if (value == (int) maxValue) {
			gameObject.SetActive (false);
		}
	}
	/// <summary>
	/// Updates the label for the max point marker.
	/// </summary>
	/// <param name="parameters">Parameter object containing data passed to this function.</param>
	public void MaxChange(Parameters parameters) {
		this.maxValue = parameters.GetIntExtra (NEW_MAX, 1);
		this.GetText ().text = "" + this.maxValue;
	}

	/// <summary>
	/// Returns the text component which displays the max point marker.
	/// </summary>
	/// <returns>The text component.</returns>
	public TextMeshProUGUI GetText() {
		if (this.text == null) {
			this.text = GetComponent<TextMeshProUGUI> ();
		}
		return this.text;
	}
}
