using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Single trigger observer. Behaviour that lets a game object anticipate another game object to trigger this game object's collider.
/// </summary>
public abstract class SingleTriggerObserver : MonoBehaviour {
	/// <summary>
	/// The box collider.
	/// </summary>
	[SerializeField] protected Collider2D boxCollider;
	/// <summary>
	/// FLag if this has been triggered.
	/// </summary>
	[SerializeField] protected bool isTriggered;

	/// <summary>
	/// Action to do when this has been triggered.
	/// </summary>
	public abstract void Action();

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		OnAwake ();
	}

	/// <summary>
	/// Functionality once the instance has been awaken.
	/// </summary>
	public virtual void OnAwake() {
		this.isTriggered = false;
		this.boxCollider = GetComponent<Collider2D> ();
	}

	/// <summary>
	/// Stop observing.
	/// </summary>
	public virtual void Ignore() {
		if (this.boxCollider != null) {
			this.boxCollider.enabled = false;
		}
	}

	/// <summary>
	/// Begin observing.
	/// </summary>
	public virtual void Observe() {
		if (this.boxCollider != null) {
			this.boxCollider.enabled = true;
		}
	}

	/// <summary>
	/// Unity Function. Action done when another object with a collider enters the game object's collider.
	/// </summary>
	/// <param name="other">Other object's collider.</param>
	void OnTriggerEnter2D(Collider2D other) {
		this.TriggerEnterCondition (other);
	}

	/// <summary>
	/// Action done when an object enters the observer's collider.
	/// </summary>
	/// <param name="other">Other object's collider.</param>
	protected virtual void TriggerEnterCondition(Collider2D other) {
		if (other.GetComponent<PlayerYuni> () != null) {
			Trigger ();
		}
	}

	/// <summary>
	/// Trigger the observer. Set triggered flag to true and destroy collider.
	/// </summary>
	public void Trigger() {
		this.isTriggered = true;
		Destroy (this.boxCollider);
	}

	/// <summary>
	/// Checker if the observer has been triggered.
	/// </summary>
	/// <returns><c>true</c> if this instance is triggered; otherwise, <c>false</c>.</returns>
	public bool IsTriggered() {
		return this.isTriggered;
	}
}
