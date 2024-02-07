using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Main behaviour of the player avatar.
/// </summary>
public class PlayerYuni : MonoBehaviour {
    /// <summary>
    /// Script Name
    /// </summary>
	public const string scriptName = "PlayerYuni";

    /// <summary>
    /// Reference to the Player Input. Handles the player inputs.
    /// </summary>
	PlayerInput playerInput;
    /// <summary>
    /// Reference to the Player Movement. Handles how the player avatar moves in the scene.
    /// </summary>
	PlayerMovement playerMovement;

    /// <summary>
    /// Reference to the Player Health. Manages the player avatar's health status if its still alive or not.
    /// Handles what happens if its health drops to 0.
    /// </summary>
	[SerializeField] private PlayerHealth playerHealth;
    /// <summary>
    /// Reference to the Player Attack. Manages the player avatar's attack capabilities.
    /// Handles weapon management and other functionalities related to the slice, filling, and breaking mechanic.
    /// </summary>
	[SerializeField] private PlayerAttack playerAttack;
    /// <summary>
    /// Reference to the Player Gem. Allows the player avatar to interact with sky fragments.
    /// </summary>
	[SerializeField] private PlayerGem playerGem;
    /// <summary>
    /// Reference to the Player Lift. Allows the player avatar to lift objects.
    /// </summary>
	[SerializeField] private PlayerLift playerLift;

    /// <summary>
    /// Checker if the player avatar is grounded.
    /// </summary>
	[SerializeField] private Transform groundCheck;
    /// <summary>
    /// Reference to the player avatar's collider.
    /// </summary>
	[SerializeField] private Collider2D collider2d;

    /// <summary>
    /// Position where the lifted sky fragment is on the player avatar.
    /// </summary>
	[SerializeField] private Transform liftedPiecePosition;
    /// <summary>
    /// Position where the lifted sky fragment is dropped.
    /// </summary>
	[SerializeField] private Transform dropPiecePosition;
//	[SerializeField] private bool isCarryingItem = false;

    /// <summary>
    /// Flag if the player avatar is carrying a filled block.
    /// </summary>
	[SerializeField] private bool isCarryingLift = false;
    /// <summary>
    /// Flag if the player avatar is carrying a sky fragment.
    /// </summary>
	[SerializeField] private bool isCarryingGem = false;
    /// <summary>
    /// Flag if the player avatar is overlapping with a charm (formerly yarnball).
    /// </summary>
	[SerializeField] private bool isOverlappingYarnball = false;
    /// <summary>
    /// Flag if the player avatar is on a manual platform.
    /// </summary>
	[SerializeField] private bool isOverlappingManualPlatform = false;
    /// <summary>
    /// Flag if the player avatar is on a manual stability platform.
    /// </summary>
	[SerializeField] private bool isOverlappingManualStabilityPlatform = false;

    /// <summary>
    /// Flag if the player avatar was prompted to carry.
    /// </summary>
	[SerializeField] private bool hasCalledCarry = false;

    /// <summary>
    /// Reference to the overlapped charm (formerly yarnball).
    /// </summary>
	[SerializeField] private Yarnball overlapYarnball;
    /// <summary>
    /// Reference to the picked up object.
    /// </summary>
	[SerializeField] private PickupObject pickupObject;
    /// <summary>
    /// Reference to the manual platform the player avatar is on.
    /// </summary>
	[SerializeField] private ManualPlatform manualPlatform;
    /// <summary>
    /// Reference to the manual stability platform the player avatar is on.
    /// </summary>
	[SerializeField] private ManualStabilityPlatform manualStabilityPlatform;
    /// <summary>
    /// Refernece to the Platform Lock where the player avatar is prevented from acting if the platform is moving.
    /// </summary>
	[SerializeField] private PlatformLock platformLock;
    /// <summary>
    /// Reference to the portal the player avatar is on.
    /// </summary>
	[SerializeField] private Portal portalOverlap;
    /// <summary>
    /// Reference to the Interable Tag found on an interactable object.
    /// </summary>
	[SerializeField] private InteractableTag interactableTag;


	// Strictly for carry hierarchy only
    /// <summary>
    /// Flag if the gem was prompted to carry a sky fragment.
    /// </summary>
	[SerializeField] private bool justInteractedGem = false;
    /// <summary>
    /// Flag if the player lift was prompted to lift a block.
    /// </summary>
	[SerializeField] private bool justInteractedLift = false;

    /// <summary>
    /// Reference to the player avatar's animator
    /// </summary>
	Animator playerAnimator;
    /// <summary>
    /// Reference to the player avatar's rigidbody.
    /// </summary>
	Rigidbody2D rigidBody2d;

    /// <summary>
    /// Flag if the player avatar is in the air.
    /// </summary>
	private bool isAirControl = true;
    /// <summary>
    /// Flag if the player avatar is grounded.
    /// </summary>
	private bool isGrounded = false;
    /// <summary>
    /// Flag if the player avatar can carry.
    /// </summary>
	private bool dontCarry = false; 
    /// <summary>
    /// Reference to the ground layer mask.
    /// </summary>
	private LayerMask groundLayerMask; 
    /// <summary>
    /// Reference to the horizontal axis for input.
    /// </summary>
	private float hAxis;

    /// <summary>
    /// UNUSED
    /// 
    /// Flag if is LCD enabled.
    /// </summary>
	[SerializeField] private bool isLCDEnabled = true;
    /// <summary>
    /// Reference to dialogues the player avatar can speak.
    /// </summary>
	[SerializeField] private Dialogue dialogues;

    [SerializeField] private Dialogue dialogueRetry3;
    /// <summary>
    /// Unity Function. Called once the MonoBehaviour's game object is instantiated.
    /// </summary>
	void Awake () {
		tag = "Player";

		// Order is important
		this.rigidBody2d = GetComponent<Rigidbody2D> ();
		this.collider2d = GetComponent<Collider2D> ();
		this.playerAnimator = GetComponent<Animator> ();
		this.playerHealth = GetComponent<PlayerHealth> ();
		this.playerAttack = GetComponent<PlayerAttack> ();
		this.playerGem = GetComponent<PlayerGem> ();
		this.playerLift = GetComponent<PlayerLift> ();

		this.groundLayerMask = LayerMask.NameToLayer ("Ground");
		this.playerMovement = new PlayerMovement (this);//GetComponent<PlayerMovement> ();
		this.playerInput = new PlayerInput (this);//GetComponent<PlayerInput> ();
		this.DropGemItem();
		this.DropLiftItem ();


        EventBroadcaster.Instance.AddObserver(EventNames.ACQUIRE_ALL, AcquireAll);
        EventBroadcaster.Instance.AddObserver(EventNames.RETRY, DropDead);
        EventBroadcaster.Instance.AddObserver(EventNames.RINGLEADER_DEATH, LastWords);
        // To set proper buttons
        //		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
    }

    /// <summary>
    /// Unity Function called when the MonoBehaviour is destroyed. Serves as a clean up function.
    /// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver(EventNames.ACQUIRE_ALL);
        EventBroadcaster.Instance.RemoveObserver(EventNames.RETRY);
        EventBroadcaster.Instance.RemoveObserver(EventNames.RINGLEADER_DEATH);
        //EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.RETRY, DropDead);
    }

    /// <summary>
    /// Unity Standard Function. Starts up the MonoBehaviour. Called once throughout its life.
    /// </summary>
	void Start() {
		if (this.GetPlayerAttack ().getEquippedDenominator () > 0) {
			EventBroadcaster.Instance.PostEvent (EventNames.CHARM_PICKUP_SWITCH);
			GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
			GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		}
	}
    /// <summary>
    /// For moving platforms. Sets player avatar's parent to the platform so it will move with it
    /// </summary>
    /// <param name="platform">Platform</param>
    public void LockPlatform(PlatformLock platform) {
		this.platformLock = platform;
		gameObject.transform.SetParent (this.platformLock.gameObject.transform);
	}
    public void LastWords() {
        StartCoroutine(LastWordsRoutine());
    }
    public IEnumerator LastWordsRoutine() {
        // [D1]: Y: Or not.
        // What's up with that, he just disappeared.
        // How boring. He probably ran out to find some body to possess.
        // ... Possess ...
        // Wha- Now that I mention it... I feel weird... It's as if..
        // ... AAAAAAAAAAAAAAAAAH.
        // Just kidding. v( > u < )v
        // Like that would ever happen.
        // Besides, phantoms can't possess their own kind...
        this.DialogueStart(dialogueRetry3);
        while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
            yield return null;
        }

        yield return new WaitForSeconds(0.7f);
        EventBroadcaster.Instance.PostEvent(EventNames.SHOW_CONGRATULATIONS);
        this.DialogueEnd2();

        yield return null;
    }
    void enableDialogue(Dialogue dialogues) {
        GetPlayerMovement().isInDialogue();
        GameController_v7.Instance.GetDialogueManager().DisplayMessage(DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
    }

    protected void disablePlayerControls() {
        GetPlayerMovement().canMove(false);
        GetPlayerAttack().canAttack(false);
    }

    public void DialogueStart(Dialogue dialogues) {
        enableDialogue(dialogues);
        GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
        this.disablePlayerControls();
    }

    public void DialogueEnd2() {
        GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
        GetPlayerMovement().canMove(true);
        GetPlayerAttack().canAttack(true);
    }
    /// <summary>
    /// For moving platforms. Sets player avatar's parent to null so its released from the platform lock.
    /// </summary>
	public void UnlockPlatform() {
//		if (!this.GetPlayerAttack ().getHammerObject ().IsAttacking ()) {
//			Debug.Log ("<color=cyan>UNLOCK PLATFORM</color>");
			gameObject.transform.SetParent (null);
//		}
	}

    /// <summary>
    /// Function call to toggle all the player avatar's equipment to be acquired. Used when going through scenes after the tutorial.
    /// </summary>
	public void AcquireAll() {
		this.AcquireNeedle ();
		this.AcquireThread ();
		this.AcquireHammer ();

		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
        GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
	}

    /// <summary>
    /// Setter for Interactable Tag.
    /// </summary>
    /// <param name="tag">Tag</param>
	public void AssignInteractable(InteractableTag tag) {
		this.interactableTag = tag;
	}

    /// <summary>
    /// Removes Interactable Tag.
    /// </summary>
    /// <param name="tag">Tag</param>
	public void LeaveInteractable(InteractableTag tag) {
		if (this.interactableTag == tag) {
			this.interactableTag = null;
		}
	}

    /// <summary>
    /// Sets the portal the player avatar is on.
    /// </summary>
    /// <param name="portal">Portal</param>
	public void AssignPortal(Portal portal) {
		this.portalOverlap = portal;
	}

    /// <summary>
    /// Removes the reference of the portal after the player avatar leaves it.
    /// </summary>
    /// <param name="portal">Portal</param>
	public void LeavePortal(Portal portal) {
		if (this.portalOverlap == portal) {
			this.portalOverlap = null;
		}
	}

    /// <summary>
    /// Handles the orientation of the objects the player avatar is carrying.
    /// </summary>
    /// <param name="sign">Direction</param>
	public void FlipUpdate(float sign) {
//		if (this.IsCarryingGemItem ()) {
			this.GetPlayerGem ().Flip (sign);
//		}
//		else if(this.IsCarryingLiftItem ()) {
			this.GetPlayerLift ().Flip (sign);
//		}
	}

    /// <summary>
    /// Sets pickup object the player avatar it currently on.
    /// </summary>
    /// <param name="pickup"></param>
	public void SetPickup(PickupObject pickup) {
		this.pickupObject = pickup;
	}

    /// <summary>
    /// Removes reference to the pickup object once the player avatar leaves it.
    /// </summary>
    /// <param name="pickup"></param>
	public void ResetPickup(PickupObject pickup) {
		if (this.pickupObject == pickup) {
			this.pickupObject = null;
		}
	}

    /// <summary>
    /// Checker if the player avatar is on a pickup object.
    /// </summary>
    /// <returns>Flag</returns>
	public bool IsOverlappingPickup() {
		if (this.pickupObject != null && !this.pickupObject.IsTaken ()) {
			return true;
		} else {
			return false;
		}
	}

    /// <summary>
    /// Function call to pickup the pickup object.
    /// </summary>
	public void Pickup() {
		if (this.pickupObject != null) {
			pickupObject.Take ();
			pickupObject = null;
		}
	}

    /// <summary>
    /// Drops the objects the player avatar is carrying upon death.
    /// </summary>
	public void DropDead() {
		this.GetPlayerGem ().DropDead();
		this.GetPlayerLift ().DropDead ();
	}

    /// <summary>
    /// UNUSED
    /// 
    /// Checker if LCD is enabled.
    /// </summary>
    /// <returns>Flag</returns>
	public bool IsLCDEnabled() {
		return this.isLCDEnabled;
	}

    /// <summary>
    /// Sets the movement of the player avatar with respect to the horizontal axis.
    /// </summary>
    /// <param name="h"></param>
	public void Move(float h) {
		this.hAxis = h;
	}

    /// <summary>
    /// Sets the manual platform the player avatar is on.
    /// </summary>
    /// <param name="platform">Platform</param>
	public void OverlapManualPlatform(ManualPlatform platform) {
		this.manualPlatform = platform;
		this.isOverlappingManualPlatform = true;
	}
    /// <summary>
    /// Sets the manual stability platform the player avatar is on.
    /// </summary>
    /// <param name="platform">Platform</param>
	public void OverlapManualStabilityPlatform(ManualStabilityPlatform platform) {
		this.manualStabilityPlatform = platform;
		this.isOverlappingManualStabilityPlatform = true;
	}

    /// <summary>
    /// Removes the reference to the manual platform once the player avatar leaves it.
    /// </summary>
    /// <param name="platform">Platform</param>
	public void LeaveManualPlatform(ManualPlatform platform) {
		if (this.manualPlatform == platform) {
			this.manualPlatform = null;
			this.isOverlappingManualPlatform = false;
		}
	}

    /// <summary>
    /// Removes the reference to the manual stability platform once the player avatar leaves it.
    /// </summary>
    /// <param name="platform">Platform</param>
	public void LeaveManualStabilityPlatform(ManualStabilityPlatform platform) {
		if (this.manualStabilityPlatform == platform) {
			this.manualStabilityPlatform = null;
			this.isOverlappingManualStabilityPlatform = false;
		}
		this.GetPlayerMovement ().canMove (true);
		this.GetPlayerAttack ().canAttack (true);
	}

    /// <summary>
    /// Sets the reference for the charm (formerly yarnball) the player avatar is on.
    /// </summary>
    /// <param name="yarnball">Charm</param>
	public void OverlapYarnball(Yarnball yarnball) {
		this.overlapYarnball = yarnball;
		this.isOverlappingYarnball = true;
	}

    /// <summary>
    /// Removes the reference of the charm (formerly yarnball) once the player avatar leaves it.
    /// </summary>
    /// <param name="yarnball"></param>
	public void LeaveYarnball(Yarnball yarnball) {
		if (this.overlapYarnball == yarnball) {
			this.overlapYarnball = null;
			this.isOverlappingYarnball = false;
		}
	}

    /// <summary>
    /// Function call to enable the needle.
    /// </summary>
	public void AcquireNeedle() {
		if (this.GetPlayerAttack () != null) {
			this.GetPlayerAttack ().AcquireNeedle ();
		}
	}

    /// <summary>
    /// Function call to enable the hammer.
    /// </summary>
	public void AcquireHammer() {
		if (this.GetPlayerAttack () != null) {
			this.GetPlayerAttack ().AcquireHammer ();
		}
	}

    /// <summary>
    /// Function call to enable the thread.
    /// </summary>
	public void AcquireThread() {
		if (this.GetPlayerAttack () != null) {
			this.GetPlayerAttack ().AcquireThread ();
		}
	}

    /// <summary>
    /// Unity Function called a number of times every frame. Usually used if there are physics related calls.
    /// Handles the calls for player movement.
    /// </summary>
	void FixedUpdate() { // Changed from Update to FixedUpdate
	#if UNITY_ANDROID
		this.hAxis = GameController_v7.Instance.GetMobileUIManager().GetInputValue();
		#elif UNITY_STANDALONE
		this.hAxis =  CrossPlatformInputManager.GetAxis ("Horizontal"); // Both this and the one inside playerInput.Update() is needed
		#endif
		playerInput.Update ();
		this.SetGrounded(false);

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(GetGroundCheck().position, 0.01f);

//		this.UnlockPlatform ();
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders [i].gameObject.layer == Constants.GetGroundLayer ()) {
				this.SetGrounded (true);
			} else if (colliders [i].gameObject.layer == LayerMask.NameToLayer ("Breakable")) {
				// Raycast downwards to check if the player is ontop
				RaycastHit2D[] hits = Physics2D.RaycastAll (
					                      new Vector2 (this.groundCheck.position.x, this.groundCheck.position.y),
					                      Vector2.down, 0.2f);
				foreach (RaycastHit2D hit in hits) {
					if (hit.collider == colliders [i]) {
						this.SetGrounded (true);
					}
						
				}
			}

			// For moving/manual platforms (to set player as parent)
//			if (colliders [i].gameObject.GetComponent<PlatformLock> () != null) {
//				this.LockPlatform (colliders [i].gameObject.GetComponent<PlatformLock> ());
//			}
		}

		// A larger circle cast for platforms, used to fix moving platforms
		colliders = Physics2D.OverlapCircleAll(GetGroundCheck().position, 1.0f);
		this.UnlockPlatform ();
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders [i].gameObject.GetComponent<PlatformLock> () != null) {
				this.LockPlatform (colliders [i].gameObject.GetComponent<PlatformLock> ());
			}
		}


		#if UNITY_ANDROID
		playerMovement.FixedUpdate (rigidBody2d, this.hAxis, this.IsGrounded()); // NEEDED
		#elif UNITY_STANDALONE
		// Move according to horizontal axis input
		playerMovement.FixedUpdate (rigidBody2d, this.hAxis, this.IsGrounded()); // NEEDED
		#endif
	}

    /// <summary>
    /// Function call to broadcast that the player has pressed the carry button.
    /// </summary>
	public void PostHandPressEvent() {
		EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAYER_HAND_PRESS);
	}

    /// <summary>
    /// Toggle to disable the player avatar from carrying.
    /// </summary>
	public void DisableCarry() {
		this.dontCarry = true;
	}

    /// <summary>
    /// Toggle to enable the player avatar to carry.
    /// </summary>
	public void EnableCarry() {
		this.dontCarry = false;
	}

    /// <summary>
    /// Special case function. Used for tutorial cutscenes.
    /// </summary>
    /// <returns></returns>
    public bool DontCarry() {
		return this.dontCarry;
	}

	// Place all functions that use Carry here
    /// <summary>
    /// Handles functionality of player avatar's carrying capability.
    /// </summary>
	public void CarryItem() {
		if (!DontCarry ()) {

			this.PostHandPressEvent ();
			// Switches for the proper drop-carry behaviour
//		if (this.IsCarryingGemItem ()) {
////			this.SkyUse ();
////			this.LiftUse ();
//			this.playerGem.UseRelease ();
////			this.playerGem.UseCarry ();
////			this.playerLift.UseCarry ();
//		} else if (this.IsCarryingLiftItem ()) {
//
////			this.LiftUse ();
////			this.SkyUse ();
//			this.playerLift.UseRelease ();
//		} else {
//			this.playerGem.UseCarry ();
//			this.playerLift.UseCarry ();
//		}

			// Carry hierarchy
			this.hasCalledCarry = false;

			// Done to prioritize the lesser order
			if ((this.justInteractedGem || this.justInteractedLift) &&
			    (this.HasPortal () || (this.HasPlatform () && this.GetManualPlatform ().IsInteractable ()) ||
			    (this.HasStabilityPlatform () && this.GetManualStabilityPlatform ().IsInteractable ()))) {
				this.ResetJustInteracted ();
				this.Pickup ();
				this.PickupYarnball ();

				if (this.HasInteractableTag ()) {
					this.UseInteractableTag ();
				} else if (this.HasPortal ()) {
					this.UsePortal ();
				} else if (this.HasPlatform ()) {
					this.OperateManualPlatform ();
				} else if (this.HasStabilityPlatform ()) {
					this.OperateManualStabilityPlatform ();
				}
			}
		// Normal Hierarchy
		else {
				if (this.IsCarryingGemItem ()) {
					this.playerGem.UseRelease ();
					this.justInteractedGem = true;
					this.hasCalledCarry = true;

				} else if (this.IsCarryingLiftItem ()) {
					this.playerLift.UseRelease ();
					this.justInteractedLift = true;
					this.hasCalledCarry = true;

				} else {
					this.playerGem.UseCarry ();
					if (IsCarryingGemItem ()) {
						this.justInteractedGem = true;
						this.hasCalledCarry = true;
					}

					this.playerLift.UseCarry ();
					if (IsCarryingLiftItem ()) {
						this.justInteractedLift = true;
						this.hasCalledCarry = true;
					}
				}

				if (!hasCalledCarry) {
					this.Pickup ();
					this.PickupYarnball ();

					if (this.HasInteractableTag ()) {
						this.UseInteractableTag ();
					} else if (this.HasPortal ()) {
						this.UsePortal ();
					} else if (this.HasPlatform ()) {
						this.OperateManualPlatform ();
					} else if (this.HasStabilityPlatform ()) {
						this.OperateManualStabilityPlatform ();
					}
				}
			}
		} else {
			// Post carry disabled event
			if (IsOverlappingGem ()) {
				PostDisabledCarryEvent();
			}
		}
	}
	
    /// <summary>
    /// Function to broadcast the player avatar is disabled from carrying.
    /// </summary>
	public void PostDisabledCarryEvent() {
		EventBroadcaster.Instance.PostEvent (EventNames.CARRY_OVERRIDE);
	}

    /// <summary>
    /// Resets the flags for carrying interaction.
    /// </summary>
	public void ResetJustInteracted() {
		this.justInteractedGem = false;
		this.justInteractedLift = false;
	}
    /// <summary>
    /// Checker if the player avatar is on an interactable.
    /// </summary>
    /// <returns>Flag</returns>
	public bool HasInteractableTag() {
		if (this.interactableTag != null) {
			return true;
		} else {
			return false;
		}
	}

    /// <summary>
    /// Checker if the player avatar is on a portal.
    /// </summary>
    /// <returns></returns>
	public bool HasPortal() {
		if (this.portalOverlap != null) {
			return true;
		} else {
			return false;
		}
	}

    /// <summary>
    /// Checker if the player avatar is on a platform.
    /// </summary>
    /// <returns></returns>
	public bool HasPlatform() {
		if (this.manualPlatform != null /*&& this.GetManualPlatform ().IsInteractable ()*/) {
			return true;
		} else {
			return false;
		}
	}

    /// <summary>
    /// Checker if the player avatar is on a stability platform.
    /// </summary>
    /// <returns></returns>
	public bool HasStabilityPlatform() {
		if (this.manualStabilityPlatform != null) {
			return true;
		} else {
			return false;
		}
	}
	
		
	/// <summary>
	/// To be used by portal. Portals drop anything YUNI carries.
	/// </summary>
	public void DropItems() {
		if (IsCarryingGemItem ()) {
			this.playerGem.UseRelease ();
		}
		if (IsCarryingLiftItem ()) {
			this.playerLift.UseRelease ();
		}
	}

    /// <summary>
    /// Interacts with the interactable the player avatar is on.
    /// </summary>
	public void UseInteractableTag() {
		if (this.interactableTag != null) {
			this.interactableTag.Interact ();
		}
	}

    /// <summary>
    /// Enters the portal the player avatar is on.
    /// </summary>
	public void UsePortal() {
		if (this.portalOverlap != null) {
			this.portalOverlap.Interact ();
		}
	}

    /// <summary>
    /// Picks up the charm (formerly yarnball) the player avatar is on.
    /// </summary>
	public void PickupYarnball() {
		Debug.Log ("ENTERED PICKUP PLAYERYUNI");
		if (this.overlapYarnball != null) {
			this.overlapYarnball.Interact ();
			this.overlapYarnball = null;
		}
	}

    /// <summary>
    /// Operates the manual platform the player avatar is on.
    /// </summary>
	public void OperateManualPlatform() {
		if (this.manualPlatform != null) {
			this.manualPlatform.Interact ();
		}
	}

    /// <summary>
    /// Operates the stability platform the player avatar is on.
    /// </summary>
	public void OperateManualStabilityPlatform() {
		if (this.manualStabilityPlatform != null) {
			this.manualStabilityPlatform.Interact ();
		}
	}

    /// <summary>
    /// Checker if the player avatar is on a portal.
    /// </summary>
    /// <returns></returns>
	public bool IsOverlappingPortal() {
		if (this.portalOverlap != null) {
			return true;
		} else {
			return false;
		}
	}

    /// <summary>
    /// Checker if the player avatar is on an interactable.
    /// </summary>
    /// <returns></returns>
	public bool IsOverlappingInteractable() {
		if (this.interactableTag != null) {
			return true;
		} else {
			return false;
		}
	}

    /// <summary>
    /// Checker if the player avatar is on a sky fragment.
    /// </summary>
    /// <returns></returns>
	public bool IsOverlappingGem() {
		return this.GetPlayerGem ().HasOverlappingPiece ();
	}

    /// <summary>
    /// Checker if the player avatar is on a block it can lift.
    /// </summary>
    /// <returns></returns>
	public bool IsOverlappingLift() {
		return this.GetPlayerLift ().HasOverlappingPiece ();
	}

    /// <summary>
    /// Checker if the player avatar is on a manual platform.
    /// </summary>
    /// <returns></returns>
	public bool IsOverlappingManualPlatform() {
		if (this.manualPlatform != null) {
//			return this.GetManualPlatform ().IsInteractable ();
			return true;
		} else {
			return false;
		}
	}
    /// <summary>
    /// Checker if the player avatar is on a manual stability platform.
    /// </summary>
    /// <returns></returns>
	public bool IsOverlappingManualStabilityPlatform() {
		return this.isOverlappingManualStabilityPlatform;
//		if (this.manualStabilityPlatform != null) {
//			//			return this.GetManualPlatform ().IsInteractable ();
//			return true;
//		} else {
//			return false;
//		}
	}

    /// <summary>
    /// Getter for the reference to the manual platform.
    /// </summary>
    /// <returns></returns>
	public ManualPlatform GetManualPlatform() {
		return this.manualPlatform;
	}

    /// <summary>
    /// Getter for the reference to the manual stability platform.
    /// </summary>
    /// <returns></returns>
	public ManualStabilityPlatform GetManualStabilityPlatform() {
		return this.manualStabilityPlatform;
	}

    /// <summary>
    /// Broadcast to show what the player avatar is carrying.
    /// </summary>
	void ShowHintCarry() {
		EventBroadcaster.Instance.PostEvent (EventNames.SHOW_PLAYER_CARRY);
	}

    /// <summary>
    /// Broadcast to hide the hint what the player avatar is carrying.
    /// </summary>
	void HideHintCarry() {
//		EventBroadcaster.Instance.PostEvent (EventNames.HIDE_PLAYER_CARRY);
		EventBroadcaster.Instance.PostEvent (EventNames.HIDE_SIMPLE_PLAYER_CARRY);
	}

    /// <summary>
    /// Starts any dialogue the player avatar can speak.
    /// </summary>
	public void DialogueStart() {
		playerMovement.isInDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		playerMovement.canMove (false);
		playerAttack.canAttack (false);
	}

    /// <summary>
    /// Ends the dialogue the player avatar is doing.
    /// </summary>
	public void DialogueEnd() {
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerMovement.canMove (true);
		playerAttack.canAttack (true);
	}

    /// <summary>
    /// Player avatar will say that there's no need to carry the box
    /// </summary>
    public void SayCantCarry() {
		StartCoroutine (RoutineSayCantCarry ());
	}

    /// <summary>
    /// Coroutine for when the player avatar's dialogue about not needing to carry the box.
    /// </summary>
    /// <returns></returns>
	IEnumerator RoutineSayCantCarry() {
		this.dialogues.currentTextIndex = -1;

		this.DialogueStart ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			playerAttack.canAttack (false);
			yield return null;
		}
		this.DialogueEnd ();
	}

    /// <summary>
    /// When player avatar cannot operate a manual platform
    /// </summary>
    public void SayCantOperateManualPlatform() {
		StartCoroutine (RoutineSayCantOperateManualPlatform ());
	}

    /// <summary>
    /// Coroutine where the player avatar says it cannot operate the manual platform.
    /// </summary>
    /// <returns></returns>
	IEnumerator RoutineSayCantOperateManualPlatform() {
		// Current index should be index-1 since dialogue auto increments
		this.dialogues.currentTextIndex = 0;

		this.DialogueStart ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			playerAttack.canAttack (false);
			yield return null;
		}
		this.DialogueEnd ();
	}

    /// <summary>
    /// Checks if the player avatar is on something.
    /// </summary>
	public void CheckOverlap() {
		if (this.IsOverlappingGem () || this.IsOverlappingLift () ||
		    this.IsOverlappingYarnball () || this.IsOverlappingManualPlatform () ||
			this.IsOverlappingManualStabilityPlatform () ||
		    this.IsOverlappingPortal () || this.IsOverlappingPickup () ||
		    this.IsOverlappingInteractable ()) {
			this.ShowHintCarry ();
		} else {
			HideHintCarry ();
		}
	}

    /// <summary>
    /// Getter for the Player Lift functionality.
    /// </summary>
    /// <returns></returns>
	public PlayerLift GetPlayerLift() {
		if (this.playerLift == null) {
			this.playerLift = GetComponentInChildren<PlayerLift> ();
		}
		return this.playerLift;
	}

    /// <summary>
    /// Removes the reference of the sky fragment the player avatar is on in the Player Gem.
    /// </summary>
	public void UnobserveGem() {
		this.GetPlayerGem ().Unobserve ();
	}

    /// <summary>
    /// Removes the reference of the block the player avatar is on in the Player Lift.
    /// </summary>
	public void UnobserveLift() {
		this.GetPlayerLift ().Unobserve ();
	}

    /// <summary>
    /// Retrieves the distance of the player avatar from the sky frament.
    /// </summary>
    /// <returns>Distance</returns>
	public float GetGemOverlapDistance() {
		return this.GetPlayerGem ().GetOverlapDistance ();
	}

    /// <summary>
    /// Retrieves the distance of the player avatar from the filled block.
    /// </summary>
    /// <returns>Distance</returns>
	public float GetLiftOverlapDistance() {
		return this.GetPlayerLift ().GetOverlapDistance ();
	}

    /// <summary>
    /// Checker if the player avatar is carrying anything.
    /// </summary>
    /// <returns>Flag</returns>
	public bool IsCarryingItem() {
		return this.IsCarryingGemItem () || this.IsCarryingLiftItem ();
	}

    /// <summary>
    /// Function to toggle that the player avatar is carrying a filled block.
    /// </summary>
	public void CarryLiftItem() {
		this.isCarryingLift = true;
	}

    /// <summary>
    /// Function to toggle that the player avatar has dropped the filled block.
    /// </summary>
	public void DropLiftItem() {
		this.isCarryingLift = false;
	}

    /// <summary>
    /// Function to toggle that the player avatar is carrying a sky fragment.
    /// </summary>
	public void CarryGemItem() {
		this.isCarryingGem = true;
	}

    /// <summary>
    /// Function to toggle that the player avatar has dropped the sky fragment.
    /// </summary>
	public void DropGemItem() {
		this.isCarryingGem = false;
	}

    /// <summary>
    /// Checker if the player avatar is on a charm (formerly yarnball).
    /// </summary>
    /// <returns></returns>
	public bool IsOverlappingYarnball() {
		return this.isOverlappingYarnball;
	}
    /// <summary>
    /// Checker if the player avatar is carrying a filled block.
    /// </summary>
    /// <returns></returns>
	public bool IsCarryingLiftItem() {
		return this.isCarryingLift;
	}

    /// <summary>
    /// Checker if the player avatar is carrying a sky fragment.
    /// </summary>
    /// <returns>Flag</returns>
	public bool IsCarryingGemItem() {
		return this.isCarryingGem;
	}

    /// <summary>
    /// UNUSED
    /// 
    /// Carry or drop a sky fragment.
    /// </summary>
	public void SkyUse() {
		this.playerGem.Use ();
//		this.playerGem.Release ();
//		this.playerGem.UseCarry ();
	}

    /// <summary>
    /// Carry or drop a filled block.
    /// </summary>
	public void LiftUse () {
		this.playerLift.Use ();
//		this.playerLift.Release ();
//		this.playerLift.UseCarry ();
	}

    /// <summary>
    /// Getter for the player avatar's animator.
    /// </summary>
    /// <returns></returns>
	public Animator GetPlayerAnimator() {
		General.CheckIfNull (playerAnimator, "playerAnimator", scriptName);
		return this.playerAnimator;
	}

    /// <summary>
    /// Getter for the player avatar's rigidbody.
    /// </summary>
    /// <returns></returns>
	public Rigidbody2D GetRigidBody2d() {
		General.CheckIfNull (rigidBody2d, "rigidBody2d", scriptName);
		return this.rigidBody2d;
	}

    /// <summary>
    /// Getter for the player avatar's collider.
    /// </summary>
    /// <returns></returns>
	public Collider2D GetCollider2d() {
		General.CheckIfNull (collider2d, "collider2d", scriptName);
		return this.collider2d;
	}

    /// <summary>
    /// Getter for the position where the player puts the lifted sky fragment/filled block.
    /// </summary>
    /// <returns></returns>
	public Transform GetLiftedPiecePosition() {
		if (this.liftedPiecePosition == null) {
			this.liftedPiecePosition = gameObject.GetComponentInChildren<LiftedPiecePosition> ().gameObject.transform;
		}
		return this.liftedPiecePosition.gameObject.transform;
	}

    /// <summary>
    /// Getter for the position where sky fragments/filled blocks are dropped.
    /// </summary>
    /// <returns></returns>
	public Transform GetDropPiecePosition() {
		if (this.dropPiecePosition == null) {
			this.dropPiecePosition = gameObject.GetComponentInChildren<DropPiecePosition> ().gameObject.transform;
		}
		return this.dropPiecePosition.gameObject.transform;
	}

    /// <summary>
    /// Getter for the player movement.
    /// </summary>
    /// <returns></returns>
	public PlayerMovement GetPlayerMovement() {
		return this.playerMovement;
	}

    /// <summary>
    /// Getter for the Player Gem.
    /// </summary>
    /// <returns></returns>
	public PlayerGem GetPlayerGem() {
		return this.playerGem;
	}

    /// <summary>
    /// Getter for the ground checker.
    /// </summary>
    /// <returns></returns>
	public Transform GetGroundCheck() {
		return this.groundCheck;
	}

    /// <summary>
    /// Checker if the player avatar is grounded.
    /// </summary>
    /// <returns></returns>
	public bool IsGrounded() {
		return this.isGrounded;
	}

    /// <summary>
    /// Setter if the player avatar is grounded.
    /// </summary>
    /// <param name="grounded">Flag</param>
	public void SetGrounded(bool grounded) {
		this.isGrounded = grounded;
	}


    /// <summary>
    /// Checker if the player avatar is in the air.
    /// </summary>
    /// <returns></returns>
	public bool IsAirControl() {
		return this.isAirControl;
	}

    /// <summary>
    /// Checker if the player avatar is jumping.
    /// </summary>
    /// <returns></returns>
	public bool IsJumping() {
		return this.playerInput.IsJumping ();
	}

    /// <summary>
    /// Setter if the player avatar is jumping.
    /// </summary>
    /// <param name="jumping"></param>
	public void SetJumping(bool jumping) {
		this.playerInput.SetJumping(jumping);
	}

    /// <summary>
    /// Getter for the player attack.
    /// </summary>
    /// <returns></returns>
	public PlayerAttack GetPlayerAttack() {
		return this.playerAttack;
	}

    /// <summary>
    /// Getter for the player health.
    /// </summary>
    /// <returns></returns>
	public PlayerHealth GetPlayerHealth() {
		return this.playerHealth;
	}

    /// <summary>
    /// Getter for the player input
    /// </summary>
    /// <returns></returns>
	public PlayerInput GetPlayerInput() {
		return this.playerInput;
	}
}
