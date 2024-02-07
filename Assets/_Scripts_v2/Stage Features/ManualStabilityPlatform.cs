using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour for manually movable platforms when the room is stabilized.
/// </summary>
public class ManualStabilityPlatform : Interactable {

    /// <summary>
    /// Speed which the platform moves.
    /// </summary>
	[SerializeField] private float speed = 1.0f;
    /// <summary>
    /// Position where the platform will move to.
    /// </summary>
	[SerializeField] private Transform targetPoint;
    /// <summary>
    /// Flag if the platform is at the target position.
    /// </summary>
	[SerializeField] private bool isTarget; //at Target position
    /// <summary>
    /// Flag if the platform is moving towards the target position.
    /// </summary>
	[SerializeField] private bool isMoving; //at Target position
    /// <summary>
    /// Flag if the platform can be used.
    /// </summary>
	[SerializeField] private bool isInteractable; //toggled when area is Stable
    /// <summary>
    /// Direction where the platform is currently headed.
    /// </summary>
	[SerializeField] private Vector2 currentDestination;
    /// <summary>
    /// Dialogue presented when the player avatar cannot move the platform yet.
    /// </summary>
	[SerializeField] private Dialogue hintDialogue;

    /// <summary>
    /// Flag if the hint dialogue is playing.
    /// </summary>
	[SerializeField] private bool isPlayingHint;
    /// <summary>
    /// Sprite for the platform.
    /// </summary>
	[SerializeField] private ManualPlatformImage manualPlatformImage;

    /// <summary>
    /// Flag if the player avatar has interacted with the platform.
    /// </summary>
	[SerializeField] private bool hasInteracted;
    /// <summary>
    /// Platform's initial position.
    /// </summary>
	private Vector2 defaultPosition;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		this.isPlayingHint = false;
		this.hasInteracted = false;
		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, Stabilize);
		EventBroadcaster.Instance.AddObserver (EventNames.UNSTABLE_AREA, Destabilize);
		EventBroadcaster.Instance.AddObserver (EventNames.RETRY, Retry);
	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.STABLE_AREA, Stabilize);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.UNSTABLE_AREA, Destabilize);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.RETRY, Retry);
	}

    /// <summary>
    /// Resets the platform to its initial state.
    /// </summary>
	private void Retry() {
		this.ToDefault ();
	}

    /// <summary>
    /// Enables the platform to be used.
    /// </summary>
	public void Stabilize() {
		this.isInteractable = true;
	}

    /// <summary>
    /// Disables the platform form being used.
    /// </summary>
	public void Destabilize() {
		this.isInteractable = false;
	}

    /// <summary>
    /// Checker if the platform is useable.
    /// </summary>
    /// <returns>If the game object is interactable. Otherwise, false.</returns>
	public bool IsInteractable() {
		if (this.isInteractable && !this.isMoving) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
    /// Unity Function. Performs an event when the component is enabled.
    /// 
    /// Sets the initial position to the current position of the platform.
    /// Sets the current destination to the initial position.
    /// </summary>
	void OnEnable () {
		this.defaultPosition = this.transform.position;
		this.currentDestination = this.defaultPosition;
	}

    /// <summary>
    /// Unity Function. Called a set amount of times every frame. Used when physics related calculations are done.
    /// 
    /// Moves the platform.
    /// </summary>
	void FixedUpdate () {
		Move (currentDestination);
	}

    /// <summary>
    /// Function performed when the object is interacted with.
    /// </summary>
    public override void Interact() {
		if (this.IsInteractable ()) { // If interactable AND not moving
			this.hasInteracted = true;
			if (this.isTarget) { // If already at Target
				this.ToDefault ();
			} else {
				this.ToTarget ();
			}
		} else if(!isInteractable) { // If not interactable
			SoundManager.Instance.Play (AudibleNames.Results.MISTAKE, false);
			this.player.SayCantOperateManualPlatform ();
//			HintPlayer ();
		}
	}

//	public void HintPlayer() {
//		if (!isPlayingHint) {
//			this.hintDialogue.currentTextIndex = -1;
//			if (GameController_v7.Instance.GetDialogueManager ().DisplayMinorHint (hintDialogue)) {
//				this.isPlayingHint = true;
//				StartCoroutine (HintCloseRoutine ());
//			}
//		}
//	}
    
    /// <summary>
    /// Coroutine that closes the hint dialogue after a delay.
    /// </summary>
    /// <returns>None</returns>
	IEnumerator HintCloseRoutine() {
		yield return new WaitForSeconds (5.0f);
		GameController_v7.Instance.GetDialogueManager ().CloseMinorHint ();
		this.isPlayingHint = false;
	}

    /// <summary>
    /// Returns the sprite of the platform.
    /// </summary>
    /// <returns>Platform Image</returns>
	public ManualPlatformImage GetManualPlatformImage() {
		if(this.manualPlatformImage == null) {
			this.manualPlatformImage = GetComponentInChildren<ManualPlatformImage> ();
		}
		return this.manualPlatformImage;
	}

    /// <summary>
    /// Sets the current destination of the platform to its target point.
    /// </summary>
	public void ToTarget() {
		if (this.player != null) {
			this.player.GetPlayerMovement ().StopVelocity ();
			this.player.GetPlayerMovement ().canMove (false);
			this.player.GetPlayerAttack ().canAttack (false);
		}
//		Move (targetPoint.position);
		this.isMoving = true;
		this.currentDestination = targetPoint.position;
		this.isTarget = true;
		this.GetManualPlatformImage ().ToTarget ();
	}

    /// <summary>
    /// Reverts the platform to its initial state set during instantiation.
    /// </summary>
	public void ToDefault() {
//		Move (defaultPosition);
		if (this.player != null) {
			this.player.GetPlayerMovement ().StopVelocity ();
			this.player.GetPlayerMovement ().canMove (false);
			this.player.GetPlayerAttack ().canAttack (false);
		}
		this.isMoving = true;
		this.currentDestination = defaultPosition;
		this.isTarget = false;
		this.GetManualPlatformImage ().ToDefault ();
	}

    /// <summary>
    /// Moves the platform to a specified target direction.
    /// </summary>
    /// <param name="target">Target Direction</param>
	private void Move (Vector2 target) {
		Vector2 pos = this.transform.position;

		if (pos != target) {
			float step = speed * Time.deltaTime;
			this.transform.position = Vector2.MoveTowards (this.transform.position, target, step);
		} else {
			if (this.hasInteracted) {
				if (this.player != null) {
					this.player.GetPlayerMovement ().canMove (true);
					this.player.GetPlayerAttack ().canAttack (true);
				}
				this.hasInteracted = false;
			}
			this.isMoving = false;
		}
	}


    /// <summary>
    /// Unity Function. Called when the collider detects another game object's collider.
    /// 
    /// Sets the reference to the player avatar, if it was the player that was detected.
    /// </summary>
    /// <param name="other">Other game object's collider</param>
	void OnTriggerStay2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni>() != null) {
//			Debug.Log ("MANUAL STABILITY PLATFORM OVERLAP");
			this.player = other.GetComponent<PlayerYuni> ();
			this.player.OverlapManualStabilityPlatform (this);

			#if UNITY_STANDALONE
			if (Input.GetButtonDown ("Fire2")) {
				Interact();
			}
			#endif
		}
	}

    /// <summary>
    /// Unity Function. Called when another object's collider leaves the collider.
    /// 
    /// Removes the reference to the player avatar, if it was the player that left.
    /// </summary>
    /// <param name="other">Other game object's collider.</param>
	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni> () != null) {
			this.player = other.GetComponent<PlayerYuni> ();
			this.player.LeaveManualStabilityPlatform (this);
//			this.GetPlayerYuni ().LeaveManualStabilityPlatform (this);
			this.player = null;
		}
	}
}
