using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sliding door behaviour. Controls the animation for the opening and closing of a door.
/// </summary>
public class SlidingDoor : MonoBehaviour {
	/// <summary>
	/// Animatable component which handles the triggering of animation calls.
	/// </summary>
	[SerializeField] private SlidingDoorAnimatable animatable;
	/// <summary>
	/// Flag if the door is unlocked.
	/// </summary>
	private bool isUnlocked;

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		this.isUnlocked = false;
	}

	/// <summary>
	/// Call to open the door.
	/// </summary>
	public void Open() {
		this.GetAnimatable ().Open ();
	} 

	/// <summary>
	/// Call to close the door.
	/// </summary>
	public void Close() {
		this.GetAnimatable ().Close ();
	}

	/// <summary>
	/// Lock the door.
	/// </summary>
	public void Lock() {
		this.isUnlocked = false;
	}

	/// <summary>
	/// Unlock the door.
	/// </summary>
	public void Unlock() {
		this.isUnlocked = true;
	}

	/// <summary>
	/// Checker if the door is unlocked.
	/// </summary>
	/// <returns><c>true</c> if the door is unlocked; otherwise, <c>false</c>.</returns>
	public bool IsUnlocked() {
		return this.isUnlocked;
	}

	/// <summary>
	/// Disables the sound when the door opens/closes.
	/// </summary>
	public void SingleMute() {
		this.GetAnimatable().SingleMute();
	}
	/// <summary>
	/// Gets the animatable.
	/// </summary>
	/// <returns>The animatable.</returns>
	public SlidingDoorAnimatable GetAnimatable() {
		if (this.animatable == null) {
			this.animatable = GetComponent<SlidingDoorAnimatable> ();
		}
		return this.animatable;
	}

}
