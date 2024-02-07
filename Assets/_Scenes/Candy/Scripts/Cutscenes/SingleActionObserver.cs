using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Single action observer. Behaviour that lets a game object wait for an event that will cause an event once.
/// </summary>
public abstract class SingleActionObserver : MonoBehaviour {
	/// <summary>
	/// Flag if this observer has been triggered.
	/// </summary>
	[SerializeField] protected bool isTriggered;
	/// <summary>
	/// Flag if this observer is still observing.
	/// </summary>
	[SerializeField] protected bool isObserving;

	/// <summary>
	/// Action to do when this has been triggered.
	/// </summary>
	public abstract void Action();

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		OnAwake ();
		this.Ignore ();
	}

	/// <summary>
	/// Functionality once the instance has been awaken.
	/// </summary>
	public virtual void OnAwake() {
		this.isTriggered = false;
	}


	/// <summary>
	/// Stop observing.
	/// </summary>
	public void Ignore() {
		this.isObserving = false;
	}

	/// <summary>
	/// Begin observing.
	/// </summary>
	public void Observe() {
		this.isObserving = true;
	}

	/// <summary>
	/// Checker if this is triggered.
	/// </summary>
	/// <returns><c>true</c> if this instance is triggered; otherwise, <c>false</c>.</returns>
	public bool IsTriggered() {
		return this.isTriggered;
	}
}
