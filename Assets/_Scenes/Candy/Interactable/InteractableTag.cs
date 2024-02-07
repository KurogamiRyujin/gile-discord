using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component to tag a game object as interactable.
/// </summary>
public class InteractableTag : Interactable {
	[SerializeField] private bool isAwake;
	[SerializeField] private Collider2D boxCollider;

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		this.Sleep ();
	}

	/// <summary>
	/// Notifies the object that it is being observed. Sets flag isAwake to true.
	/// </summary>
	public virtual void Observe() {
		this.isAwake = true;
	}

	/// <summary>
	/// Notifies the object that it is not being observed. Sets flag isAwake to false.
	/// </summary>
	public virtual void Sleep() {
		this.isAwake = false;
	}

	/// <summary>
	/// Destroys collider after single interact.
	/// </summary>
	public override void Interact() {
		Debug.Log ("<color=yellow>INTERACT<color>");
		if (this.GetCollider () != null) {
			Destroy (this.GetCollider ());
		}
	}

	/// <summary>
	/// Gets the collider.
	/// </summary>
	/// <returns>The collider.</returns>
	public Collider2D GetCollider() {
		if (this.boxCollider == null) {
			this.boxCollider = GetComponent<BoxCollider2D> ();
		}
		return this.boxCollider;
	}

	/// <summary>
	/// Determines whether this game object is awake.
	/// </summary>
	/// <returns><c>true</c> if this game object is awake; otherwise, <c>false</c>.</returns>
	public bool IsAwake() {
		return this.isAwake;
	}

	/// <summary>
	/// Raises trigger event while an object stays inside this game object's collider.
	/// </summary>
	/// <param name="other">Another collider of a game object coming in contact with this game object's collider.</param>
	void OnTriggerStay2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni> () != null) {
			if (this.IsAwake ()) {
				this.SetPlayer (other.GetComponent<PlayerYuni> ());
				this.GetPlayerYuni ().AssignInteractable (this);
			}
		}
	}

	/// <summary>
	/// Raises trigger event when an object exits game object's collider.
	/// </summary>
	/// <param name="other">Another collider of a game object coming in contact with this game object's collider.</param>
	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni> () != null) {
			this.GetPlayerYuni ().LeaveInteractable (this);
		}
	}
}
