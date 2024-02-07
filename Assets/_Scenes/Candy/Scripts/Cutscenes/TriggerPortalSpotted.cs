using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single trigger observer. Observes if the player avatar has seen the specified portal.
/// </summary>
public class TriggerPortalSpotted : SingleTriggerObserver {
	/// <summary>
	/// Specified portal the player avatar has to see.
	/// </summary>
	[SerializeField] private Portal portal;

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		base.OnAwake();
	}

	/// <summary>
	/// Action to do when this has been triggered.
	/// </summary>
	public override void Action() {
		Debug.Log ("No Action Taken");
	}

	/// <summary>
	/// Hides the portal.
	/// </summary>
	public void HidePortal() {
		this.GetPortal ().gameObject.SetActive (false);
	}
	/// <summary>
	/// Shows the portal.
	/// </summary>
	public void ShowPortal() {
		this.GetPortal ().gameObject.SetActive (true);
	}

	/// <summary>
	/// Gets the portal.
	/// </summary>
	/// <returns>The portal.</returns>
	public Portal GetPortal() {
		if (this.portal == null) {
			this.portal = FindObjectOfType<Portal> ();
		}
		return this.portal;
	}
}
