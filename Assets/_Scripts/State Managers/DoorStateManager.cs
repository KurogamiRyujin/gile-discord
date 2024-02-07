using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the state of a door: if it is locked, permanently locked, or open.
/// 
/// Door's accessibility is based on the state the door state manager is in.
/// </summary>
public class DoorStateManager : MonoBehaviour {
    /// <summary>
    /// Reference to the door's lock animator.
    /// </summary>
	[SerializeField] private Animator doorLockAnimator;

    /// <summary>
    /// Flag to indicate if the door is permanently inaccessible.
    /// </summary>
    [SerializeField] private bool permaLock = false;

    /// <summary>
    /// Flag if the door is unocked.
    /// </summary>
    [SerializeField] private bool isUnlocked = true; 

	/// <summary>
    /// Flag if the door is open.
    /// </summary>
	[SerializeField] private bool isOpen = true;

    /// <summary>
    /// Flag if this is the door the player came from.
    /// </summary>
	private bool isOriginPoint = false;

    /// <summary>
	/// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
	/// </summary>
	void Start () {
		if (!this.permaLock) {
			EventBroadcaster.Instance.AddObserver (EventNames.ON_DOOR_OPEN, this.Open);
			EventBroadcaster.Instance.AddObserver (EventNames.ON_DOOR_CLOSE, this.Close);
			this.Close ();
			if (!this.isOriginPoint) {
				EventBroadcaster.Instance.AddObserver (EventNames.ON_DOOR_LOCK, this.Lock);
				EventBroadcaster.Instance.AddObserver (EventNames.ON_DOOR_UNLOCK, this.Unlock);
				this.Lock ();
			}
		} else {
			this.Close ();
			this.Lock ();
			this.isUnlocked = false;
			this.isOpen = false;
		}
	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
	void OnDestroy() {
		if (!this.permaLock) {
			if (!this.isOriginPoint) {
				EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.ON_DOOR_LOCK, this.Lock);
				EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.ON_DOOR_UNLOCK, this.Unlock);
			}
			EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.ON_DOOR_OPEN, this.Open);
			EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.ON_DOOR_CLOSE, this.Close);
		}
	}

    /// <summary>
    /// Resets triggers of the animator.
    /// </summary>
	void ResetTriggers() {
		this.doorLockAnimator.ResetTrigger ("on");
		this.doorLockAnimator.ResetTrigger ("off");
	}

    /// <summary>
    /// Checker if the door has a lock.
    /// </summary>
    /// <returns>If the door has a lock. Otherwise, false.</returns>
	bool HasLock() {
		if (this.doorLockAnimator != null) {
			return true;
		}
		else {
			return false;
		}			
	}

    /// <summary>
    /// Unlocks the door.
    /// </summary>
	public void Unlock() {
		this.isUnlocked = true;

		if (this.HasLock ()) {
			this.ResetTriggers ();
			this.doorLockAnimator.SetTrigger ("off");
			SoundManager.Instance.Play (AudibleNames.Door.OPEN, false);
		}
	}

    /// <summary>
    /// Locks the door.
    /// </summary>
	public void Lock() {
		this.isUnlocked = false;
	}
		
    /// <summary>
    /// Opens the door but does not play the SFX.
    /// </summary>
	public void OpenSoundless() {
		this.isOpen = true;

		if (isUnlocked) {
			if (this.HasLock ()) {
				this.ResetTriggers ();
				this.doorLockAnimator.SetTrigger ("off");
			}
		}
	}

    /// <summary>
    /// Opens the door.
    /// </summary>
	public void Open() {
		this.isOpen = true;

		if (isUnlocked) {
			if (this.HasLock ()) {
				this.ResetTriggers ();
				this.doorLockAnimator.SetTrigger ("off");
				SoundManager.Instance.Play (AudibleNames.Door.OPEN, false);
			}
		}
	}

	/// <summary>
    /// Closes the door.
    /// </summary>
	public void Close() {
		this.isOpen = false;

		if (this.HasLock ()) {
			this.ResetTriggers ();
			this.doorLockAnimator.SetTrigger ("on");
			SoundManager.Instance.Play (AudibleNames.Door.CLOSE, false);
		}
	}

    /// <summary>
    /// Opens the door, soundless, when the player is near.
    /// </summary>
	public void OpenAfar() {
		this.PlayerInVicinitySoundless ();		
	}

    /// <summary>
    /// Closes the door when the player is away from the door.
    /// </summary>
	public void CloseAfar() {
		this.PlayerNotInVicinity ();		
	}

    /// <summary>
    /// Unity Function. Checks if another game object's collider entered this game object's collider.
    /// 
    /// If the player comes near the door, it will open if the door is unlocked.
    /// </summary>
    /// <param name="collider">Other Game Object's Collider</param>
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.GetComponent<PlayerYuni> () != null) {
			this.PlayerInVicinity ();		
		}
	}
    
    /// <summary>
    /// Unity Function. Checks if another game object's collider is inside this game object's collider.
    /// 
    /// Keeps the door opened while the player avatar is in the door's vicinity if it is unlocked.
    /// </summary>
    /// <param name="collider">Other Game Object's Collider</param>
	void OnTriggerStay2D(Collider2D collider) {
		if (collider.gameObject.GetComponent<PlayerYuni> () != null) {
			this.PlayerInVicinitySoundless ();		
		}
	}

    /// <summary>
    /// Unity Function. Checks if another game object's collider leaves this game object's collider.
    /// 
    /// If the player is not in vicinity, close door.
    /// </summary>
    /// <param name="collider">Other Game Object's Collider</param>
	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.GetComponent<PlayerYuni> () != null) {
			this.PlayerNotInVicinity ();		
		}
	}

    /// <summary>
    /// If the player is near and the door is unlocked, open the door.
    /// </summary>
	public void PlayerInVicinity() {
		if (isUnlocked) {
//			Debug.Log ("DOOR OPEN");
			this.Open ();
		}
	}

    /// <summary>
    /// If the player is near and door is unlocked, open the door soundless.
    /// </summary>
	public void PlayerInVicinitySoundless() {
		if (isUnlocked) {
//			Debug.Log ("DOOR OPEN Soundless");
			this.OpenSoundless ();
		}
	}

    /// <summary>
    /// If the player is not near and the door is open, close the door.
    /// </summary>
	public void PlayerNotInVicinity() {
		if (isOpen) {
//			Debug.Log ("DOOR CLOSE");
			this.Close ();
		}
	}

    /// <summary>
    /// Checker if the door is open and unlocked.
    /// </summary>
    /// <returns>If the door is open and unlocked. Otherwise, false.</returns>
	public bool IsAccessible() {
		if (this.isOpen && this.isUnlocked) {
			return true;
		}
		else {
			return false;
		}
	}

    /// <summary>
    /// Indicate that the player spawned here.
    /// </summary>
	public void PlayerSpawnedHere() {
		this.isOriginPoint = true;
	}
}
