using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickupNeedle : Interactable {
	public override void Interact() {
		Debug.Log ("Interact");
		this.GetPlayerYuni().AcquireNeedle();
		this.GetPlayerYuni().AcquireThread();
		EventBroadcaster.Instance.PostEvent (EventNames.NEEDLE_PICKUP);
		Destroy(this.gameObject);
	}
}
