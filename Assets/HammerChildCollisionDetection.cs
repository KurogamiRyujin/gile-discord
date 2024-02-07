using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerChildCollisionDetection : MonoBehaviour {
	[SerializeField] private HammerObject parentHammerObject;



	public void hammerSmashEnd() {
		EventBroadcaster.Instance.PostEvent (EventNames.ON_HAMMER_SMASH_END);
		parentHammerObject.IsPlaying (false);
		Debug.Log ("Posted Hammer Smash End event");
	}


	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("<color=green>Hammer hit layer "+LayerMask.LayerToName(other.gameObject.layer)+"</color>");
		if (other.gameObject.CompareTag ("Enemy")) {
			Debug.Log ("ENTERED Trigger Hammer");
			parentHammerObject.TriggerCollision (other);
		}

		if (other.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			parentHammerObject.SetSoundID (AudibleNames.Hammer.HIT);
		}
	}

	public HammerObject GetParentHammerObject() {
		return this.parentHammerObject;
	}

	public Collider2D GetCollider() {
		return parentHammerObject.getHammerChildCollider();
	}
}
