using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDCrosshairListener : MonoBehaviour {

	[SerializeField] private Animator crosshairAnimator;
	void Awake() {

		EventBroadcaster.Instance.AddObserver (EventNames.ON_LCD_PAUSE, this.ActivateCrosshair);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_LCD_UNPAUSE, this.DeactivateCrosshair);
	}

	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ON_LCD_PAUSE, this.ActivateCrosshair);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_LCD_UNPAUSE, this.DeactivateCrosshair);
	}


	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_LCD_PAUSE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_LCD_UNPAUSE);
	}


	public void ActivateCrosshair() {
		Debug.Log ("Activate");
		if (crosshairAnimator != null)
			
		this.crosshairAnimator.SetTrigger ("enter");
	}

	public void DeactivateCrosshair() {
		Debug.Log ("Deactivate");
		if (crosshairAnimator != null)
			
		this.crosshairAnimator.SetTrigger ("exit");
	}
}
