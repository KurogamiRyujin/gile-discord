using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single trigger observer. Behaviour when the player touches the screen on a specific location on the screen.
/// </summary>
public class SingleTriggerTouchObserver : SingleTriggerObserver {
	/// <summary>
	/// Flag if the player has interacted with the object to be interacted.
	/// </summary>
	[SerializeField] protected bool hasInteracted;
	/// <summary>
	/// Flag if the player avatar is in range with the object it will interact.
	/// </summary>
	[SerializeField] protected bool isInRange;
	/// <summary>
	/// Flag if the player has pressed the carry button.
	/// </summary>
	[SerializeField] protected bool isCarryPressed;

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		base.OnAwake();
		isCarryPressed = false;
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_HAND_PRESS, CarryPressed);
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_PLAYER_HAND_PRESS);
	}

	/// <summary>
	/// Checker if the player pressed the carry button.
	/// </summary>
	/// <returns><c>true</c> if the player pressed the carry button; otherwise, <c>false</c>.</returns>
	public bool IsCarryPressed() {
		return this.isCarryPressed;
	}

	/// <summary>
	/// Confirms the player has pressed the carry button.
	/// </summary>
	public void CarryPressed() {
		if (this.isInRange) {
			base.Trigger ();
			this.isCarryPressed = true;
		}
	}

	/// <summary>
	/// Action to do when this has been triggered.
	/// </summary>
	public override void Action() {
		Debug.Log ("No Action Taken");
	}

	/// <summary>
	/// Action done when an object enters the observer's collider.
	/// </summary>
	/// <param name="other">Other object's collider.</param>
	protected override void TriggerEnterCondition(Collider2D other) {
		Debug.Log ("<color=green>TRIGGER ENTER</color>");
	}

	/// <summary>
	/// Action done while an object is in the observer's collider.
	/// </summary>
	/// <param name="other">Other object's collider.</param>
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni>() != null) {
			this.isInRange = true;
		}
	}

	/// <summary>
	/// Action done when an object leaves the observer's collider.
	/// </summary>
	/// <param name="other">Other object's collider.</param>
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni>() != null) {
			this.isInRange = false;
		}
	}
}
