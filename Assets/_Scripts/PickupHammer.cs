using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickupHammer : Interactable {

	public override void Interact() {
		Debug.Log ("Interact");
		this.GetPlayerYuni ().AcquireHammer ();
		EventBroadcaster.Instance.PostEvent (EventNames.HAMMER_PICKUP);
		Destroy(this.gameObject);
	}
}
