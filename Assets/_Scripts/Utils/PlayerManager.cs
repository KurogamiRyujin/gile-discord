using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Obsolete]
public class PlayerManager : MonoBehaviour {


	KeyCode LEFT = KeyCode.A;
	KeyCode RIGHT = KeyCode.D;
	KeyCode UP = KeyCode.W;
	KeyCode FIRE = KeyCode.F;

	KeyCode NEEDLE_KEY = KeyCode.Space;
	KeyCode NEEDLE_MOUSE = KeyCode.Mouse0;

	public static KeyCode DASH = KeyCode.LeftShift;
	int stateIdle = 0;
	int stateFalling = 1;
	int stateRunning = 2;
	int stateJumping = 3;
	int stateAttacking = 4;

	public Transform needlePosition;
	private Entrance entranceStandingIn;//Entrance instance player is currently standing in, null if not standing in any
	private Exit exitStandingIn;//Exit
	public bool hasNeedle = true;
	public float speedX;
	public float jumpSpeedY;
	public float delayBeforeDoubleJump;
	public AudioClip sfxFire;

	public GameObject needlePrefab;
	public GameObject needleWeapon;
//	public NeedleController needleController;
	private NeedleThrowing needleThrowing;

	bool isFacingRight;
	bool isJumping;
	bool isGrounded;
	bool canDoubleJump;
	bool isPaused;

	float speed;
	Animator anim;
	Rigidbody2D rb2d;
	MobileUI mobile;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		isFacingRight = true;
		isGrounded = true;
		this.entranceStandingIn = null;//Set to null at initialization because we assume player is not at any entrance at the start
		needleWeapon = Instantiate(needlePrefab, transform.position, Quaternion.identity);
//		needleController = needleWeapon.GetComponent<NeedleController> ();
		needleThrowing = needleWeapon.GetComponent<NeedleThrowing>();
		#if UNITY_ANDROID
		mobile = GameObject.Find("Mobile UI").GetComponent<MobileUI> ();
		#endif

		GameController_v7.Instance.GetPauseController ().onPauseGame += Pause;
		GameController_v7.Instance.GetPauseController ().onContinueGame += Continue;
	}
	void handleGrounded() {
//		foreach(Collider2D collider in gameObject.GetComponent<BoxCollider2D>().) {
//			if (collider.CompareTag ("Ground") || 
//				(collider.CompareTag ("PartitionableObject") && collider.gameObject.GetComponent<PartitionableObject>().IsTangible())){
//				isJumping = false;
//				isGrounded = true;
//				canDoubleJump = false;
//				anim.SetInteger("State", stateIdle);
//			}
//		}
	}

	public void IsPaused (bool pause) {
		this.isPaused = pause;
	}

	public void Pause() {
		this.isPaused = true;
	}

	public void Continue() {
		this.isPaused = false;
	}


	// Update is called once per frame
	void Update () {
		if (!this.isPaused) {
			movePlayer (speed);
			handleJumpAndFall ();
			flip ();
			handleGrounded ();
			if (!needleWeapon.activeInHierarchy) {
				this.hasNeedle = true;
			}
			#region Key Conditions
//			if(hasNeedle) {
				// If left is pressed
				if (isLeftDown ()) {
					moveLeft ();
				}
				// If left is released but right is held down
				if (isLeftReleased () && isRightDown ()) {
					moveRight ();
				} else if (isLeftReleased ()) {
					idle ();
				}

				// If right is pressed
				if (isRightDown ()) {
					moveRight ();
				}
				// If left is released but right is held down
				if (isRightReleased () && isLeftDown ()) {
					moveLeft ();
				} else if (isRightReleased ()) {
					idle ();
				}

				// If up is pressed
			if (isUpPressed ()) {
					jump ();
				}

//				// If up is while air-borne
//				if (isUpPressed () && canDoubleJump) {
//					jump ();
//				}

				//If up is pressed while standing in entrance

				#if UNITY_STANDALONE
				if (Input.GetButtonDown ("Jump"))
					this.EnterEntrance ();
				#elif UNITY_ANDROID
//				if ((mobile.interactPressed || mobile.jumpPressed) && this.entranceStandingIn != null) {
//					mobile.interactPressed = false;
//					mobile.jumpPressed = false;
//					this.EnterEntrance ();
//				}
				#endif
				if (isUpPressed () && this.entranceStandingIn != null) {
					this.EnterEntrance ();
				}

				if (isNeedlePressed ()) {
					Needle();
				}

//			if (!isLeftPressed() && !isRightPressed() && isGrounded) {
//				idle();
//				idleAnimation();
//			}
//			}
//			else {
//				idle();
//			}

//			if(isJumping) {
//				jumpingAnimation();
//			}
//			if(rb2d.velocity.y < 0) {
//				fallingAnimation();
//			}
			#endregion
		}
	}

	//function called by a gameObject with Entrance component that sets the entrance player is currently standing in
	//gameObject sets player's entranceStandingIn property to null upon exiting gameObject
	public void SetEntranceStandingIn(Entrance entrance) {
		this.entranceStandingIn = entrance;
	}

	//function called by a gameObject with Entrance component that sets the entrance player is currently standing in
	//gameObject sets player's entranceStandingIn property to null upon exiting gameObject
	public void SetExitStandingIn(Exit exit) {
		this.exitStandingIn = exit;
	}


	//Player enters the entrance
	private void EnterEntrance() {
		this.entranceStandingIn.Enter ();
	}


	//Player enters the entrance
	private void EnterExit() {
		this.exitStandingIn.Enter ();
	}

	public void idleAnimation() {
		anim.SetInteger("State", stateIdle);
	}
	public void jumpingAnimation() {
		anim.SetInteger("State", stateJumping);
	}

	public void fallingAnimation() {
		anim.SetInteger("State", stateFalling);
	}


	public void attackingAnimation() {
		anim.SetTrigger (stateAttacking);
		//		anim.SetInteger("State", stateAttacking);
	}

	void movePlayer(float playerSpeed) {
		
		if(playerSpeed < 0 && !isJumping ||
			playerSpeed > 0 && !isJumping) {
			anim.SetInteger("State", stateRunning);
		}
		if(playerSpeed == 0 && !isJumping) {
			anim.SetInteger("State", stateIdle);
		}
		rb2d.velocity = new Vector3(speed, rb2d.velocity.y, 0);
	}

	void handleJumpAndFall() {
//		if(!isJumping) {
			if (rb2d.velocity.y < 0) {
//				anim.SetInteger ("State", stateJumping);
			anim.SetInteger ("State", stateFalling);
			}
//			else {
//				anim.SetInteger ("State", stateFalling);
//			}
//		}
	}


	void manualFlip() {
		isFacingRight = !isFacingRight;
		Vector3 temp = transform.localScale;
		temp.x *= -1;
		transform.localScale = temp;
	}

	void flip() {
		if(speed > 0 && !isFacingRight ||
		   speed < 0 && isFacingRight) {
			manualFlip();
		}
	}

	public void idle() {
		this.idleAnimation ();

		speed = 0;
	}

	public void jump() {

		// Single jump
		if(isGrounded) {
			isJumping = true;
			isGrounded = false;
			// Add force to the vertical direction
			rb2d.AddForce (new Vector2 (rb2d.velocity.x, jumpSpeedY));
			anim.SetInteger ("State", stateJumping);
//			Invoke ("enableDoubleJump", delayBeforeDoubleJump);
		}

		// Double jump
//		if(canDoubleJump) {
//			canDoubleJump = false;
//			rb2d.AddForce (new Vector2 (rb2d.velocity.x, jumpSpeedY));
//			anim.SetInteger ("State", stateJumping);
//		}
	}

//	public bool isMobileButtonClicked() {
//		GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
//		if (selectedGameObject == null)
//			return true;
//		else {
//			if (selectedGameObject.tag != "MobileButton")
//				return true;
//			return false;
//		}
//	}

	public void Needle() {
		AudioSource.PlayClipAtPoint (sfxFire, transform.position);
		attackingAnimation ();
		needleThrowing.Throw (Input.mousePosition);
	}

//	public void needle() {
//		if (hasNeedle) {
//			if (!PauseController.isPaused) {
//				this.hasNeedle = false;
//				Vector3 mousePosition;
//				mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
//				mousePosition.z = 0.0f;
//				Vector3 direction = mousePosition.normalized;
//				needleController.thrownPosition.position = mousePosition;
//
//				Physics2D.IgnoreCollision(needleWeapon.GetComponent<Collider2D>(), GetComponent<Collider2D>());
//
//				if (isFacingRight) {
//					if (mousePosition.x < transform.position.x)
//						manualFlip ();
//					if (!isRightDown ()) {
//						// If not holding right, the player can shoot left or right
////						if (mousePosition.x < transform.position.x)
////							manualFlip ();
//						AudioSource.PlayClipAtPoint (sfxFire, transform.position);
//						activateNeedle ();
////						Instantiate (needleWeapon, needlePosition.position, Quaternion.identity);
//					} else /*if (mousePosition.x > transform.position.x)*/ {
//						// If holding right, the player can only shoot right
//						AudioSource.PlayClipAtPoint (sfxFire, transform.position);
//						activateNeedle ();
////						Instantiate (needleWeapon, needlePosition.position, Quaternion.identity);
//					}
//				} else {
//					if (mousePosition.x > transform.position.x)
//						manualFlip ();
//					if (!isLeftDown ()) {
//						// If not holding left, the player can shoot left or right
////						if (mousePosition.x > transform.position.x)
////							manualFlip ();
//						AudioSource.PlayClipAtPoint (sfxFire, transform.position);
//						activateNeedle ();
//						//Instantiate (needleWeapon, needlePosition.position, Quaternion.identity);
//					} else /* if (mousePosition.x < transform.position.x) */{
//						// If holding left, the player can only shoot left
//						AudioSource.PlayClipAtPoint (sfxFire, transform.position);
//						activateNeedle ();
////						Instantiate (needleWeapon, needlePosition.position, Quaternion.identity);
//					}
//				}
//			}
//		}
//	}


	public void activateNeedle() {
		AudioSource.PlayClipAtPoint (sfxFire, transform.position);
		needleWeapon.transform.position = transform.position;
		needleWeapon.SetActive (true);
	}

	public void enableDoubleJump() {
		// TODO: Uncomment to enable double jump
		// canDoubleJump = true;
	}
	public void moveLeft() {
		speed = -speedX;
	}

	public void moveRight() {
		speed = speedX;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("PartitionableObject")
			/*(other.gameObject.CompareTag("PartitionableObject") && 
			other.gameObject.GetComponent<PartitionableObject> ().IsTangible ())*/) {
			isJumping = false;
			isGrounded = true;
			canDoubleJump = false;
			anim.SetInteger("State", stateIdle);
		}

//		if (!needleController.isPulled && other.gameObject.CompareTag ("Player")) {
//			Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
//		}
	}

	void OnCollisionStay2D(Collision2D other) {
		
		if(other.gameObject.CompareTag("Ground") ||
			(other.gameObject.CompareTag("PartitionableObject") && 
				other.gameObject.GetComponent<PartitionableObject> ().IsTangible ())) {
			isJumping = false;
			isGrounded = true;
			canDoubleJump = false;
			anim.SetInteger("State", stateIdle);
		}
	}

	#region Left
	public bool isLeftPressed() {
		if (Input.GetKeyDown(LEFT))
			return true;
		else
			return false;
	}

	public bool isLeftDown() {
		if (Input.GetKey(LEFT))
			return true;
		else
			return false;
	}

	public bool isLeftReleased() {
		if (Input.GetKeyUp(LEFT))
			return true;
		else
			return false;
	}
	#endregion

	#region Right
	public bool isRightPressed() {
		if (Input.GetKeyDown(RIGHT))
			return true;
		else
			return false;
	}

	public bool isRightDown() {
		if (Input.GetKey(RIGHT))
			return true;
		else
			return false;
	}

	public bool isRightReleased() {
		if (Input.GetKeyUp(RIGHT))
			return true;
		else
			return false;
	}
	#endregion

	#region Up
	public bool isUpPressed() {
		if (Input.GetButtonDown("Jump"))
			return true;
		else
			return false;
	}

	public bool isUpDown() {
		if (Input.GetButton("Jump"))
			return true;
		else
			return false;
	}

	public bool isUpReleased() {
		if (Input.GetButtonUp("Jump"))
			return true;
		else
			return false;
	}
	#endregion

	#region Needle
	public bool isNeedlePressed() {
		if(Input.GetKeyDown(NEEDLE_KEY) ||
			Input.GetKeyDown(NEEDLE_MOUSE)) {
			return true;
		}
		else {
			return false;
		}
	}

	public bool isNeedleDown() {
		if(Input.GetKey(NEEDLE_KEY) ||
			Input.GetKey(NEEDLE_MOUSE)) {
			return true;
		}
		else {
			return false;
		}
	}
	#endregion

	#region Getters
	public KeyCode getKeyLEFT() {
		return this.LEFT;
	}

	public KeyCode getKeyRIGHT() {
		return this.RIGHT;
	}

	#endregion

}
