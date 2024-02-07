using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Portal which takes the player avatar from one point in the room to another point where a portal is.
/// </summary>
public class Portal : Interactable {

    /// <summary>
    /// This portal's position.
    /// </summary>
	[SerializeField] private Transform source;
    /// <summary>
    /// The target portal's position.
    /// </summary>
	[SerializeField] private Transform destination;

    /// <summary>
    /// Puts the player in the position where the destination portal is.
    /// </summary>
	public override void Interact() {
		SoundManager.Instance.Play (AudibleNames.Trampoline.BOUNCE, false);
//		this.GetPlayerYuni ().DropItems ();
		this.GetPlayerYuni ().gameObject.transform.position = GetDestination ().position;
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni> () != null) {
			this.SetPlayer (other.GetComponent<PlayerYuni> ());
			this.GetPlayerYuni ().AssignPortal (this);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni> () != null) {
			this.GetPlayerYuni ().LeavePortal (this);
		}
	}

    /// <summary>
    /// Returns the transform of the destination this portal leads to.
    /// </summary>
    /// <returns>Destination Transform</returns>
	public Transform GetDestination() {
		return this.destination;
	}

    /// <summary>
    /// Returns this portal's transform.
    /// </summary>
    /// <returns>Portal Transform</returns>
	public Transform GetSource() {
		return this.source;
	}
}
