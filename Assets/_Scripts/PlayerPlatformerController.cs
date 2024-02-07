using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {
	// Increases gravity when falling so that the player falls faster
	public float fallMultiplier = 2.5f;
	// For the slightly lower gravity when the player holds the jump button
	public float lowJumpMultiplier = 2f;

	public float maxSpeed = 3.5f;
	public float jumpTakeOffSpeed = 7f;

	public bool canWalk = true;
	public bool canJump = true;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		canWalk = true;
		canJump = true;
	}

//	void Attack() {
//
//		// Place this in the function which ends the attack
//		attack = false;
//		animator.SetBool ("attack", attack);
//	}
//
//	protected override void PerformAttack() {
//		if (Input.GetButtonDown ("Fire1")) {
//			attack = true;
//			Attack ();
//			animator.SetBool ("attack", attack);
//		}
//	}
	public void canMove(bool flag) {
		canWalk = flag;
		canJump = flag;
	}
	bool canMove() {
		if (canWalk && canJump) {
			return true;
		}
		return false;
	}
	protected override void ComputeVelocity() {
		Vector2 move = Vector2.zero;

		if (canMove() || canWalk) {
			move.x = Input.GetAxis ("Horizontal");
			print ("Moved");
		} else
			move.x = 0.0f;
		
		if (Input.GetButtonDown ("Jump") && (canMove() || canJump) && grounded) {
			velocity.y = jumpTakeOffSpeed;
		}
		// If jump button is released
		else if (Input.GetButtonUp ("Jump") && canMove()) {
			// If jumping and jump released, reduce velocity by half
			if (velocity.y > 0) {
				velocity.y = velocity.y * 0.5f;
			}
		}

		bool flipSprite = (spriteRenderer.flipX ? (move.x > 0f) : (move.x < 0f));
		if (flipSprite && move.x != 0) {
			this.flip ();
		}

		animator.SetBool ("grounded", grounded);
		animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

		#if UNITY_ANDROID
//			animator.SetBool ("isWalking", GameController_v7.Instance.GetMobileUIManager().GetInputValue());
		if(GameController_v7.Instance.GetMobileUIManager().GetInputValue() != 0) {
			animator.SetBool ("isWalking", true);
		}
		#else
			animator.SetBool ("isWalking", Input.GetButton("Horizontal"));
		#endif

		targetVelocity = move * maxSpeed;
	}

	public void flip() {
		spriteRenderer.flipX = !spriteRenderer.flipX;
	}
}