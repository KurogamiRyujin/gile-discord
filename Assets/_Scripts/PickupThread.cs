using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupThread : Interactable {
	public override void Interact() {
		Debug.Log ("Interact");
		this.GetPlayerYuni ().AcquireThread ();
		EventBroadcaster.Instance.PostEvent (EventNames.THREAD_PICKUP);
		Destroy (this.gameObject);
	}
}

