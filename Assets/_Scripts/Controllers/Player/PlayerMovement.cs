using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;
public class PlayerMovement {
	private float HIGH_JUMP_MULTIPLIER = 5.0f;
//    [SerializeField]
    private float m_MaxSpeed = 8f;                    // The fastest the player can travel in the x axis.
//    [SerializeField]
    private float m_JumpForce = 360f;                  // Amount of force added when the player jumps.
//    [SerializeField]
    private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
//    [SerializeField]
    private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    public Transform m_GroundCheck;                   // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f;                // Radius of the overlap circle to determine if grounded
//    private bool m_Grounded;                           // Whether or not the player is grounded.
    private Animator m_Anim;                           // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    public bool m_FacingRight;                  // For determining which way the player is currently facing.

//	[SerializeField]
	private PlayerAttack playerAttack;


	// Increases gravity when falling so that the player falls faster
	public float fallMultiplier = 2.5f; // 2.5f
	// For the slightly lower gravity when the player holds the jump button
	public float lowJumpMultiplier = 0.8f; // 2

	// Added @Candy
	public bool canWalk = true;
	public bool canJump = true;



	private PlayerYuni player;
//    void Awake () {
//		gameObject.tag = "Player";
        // Setting up references.
//        m_Anim = GetComponent<Animator>();
//        m_Rigidbody2D = GetComponent<Rigidbody2D>();
//		this.playerAttack = GetComponent<PlayerAttack> ();
//    }

	public PlayerMovement(PlayerYuni player) {
		this.player = player;
		this.playerAttack = player.GetPlayerAttack (); 
		this.m_Anim = player.GetPlayerAnimator ();
		this.m_Rigidbody2D = player.GetRigidBody2d ();

		this.m_GroundCheck = player.GetGroundCheck ();
		m_WhatIsGround = LayerMask.NameToLayer ("Ground");

		this.m_AirControl = true;
		this.m_FacingRight = true;
//		this.m_Anim = playerAnimator;
//		this.m_Rigidbody2D = rigidBody2d;
//		this.playerCollider = playerCollider;
//		this.playerAttack = playerAttack;
	}

//	void Start() {
//		Debug.Log ("shouldFlip "+this.shouldFlip);
//		if (this.shouldFlip) {
//			Flip ();
//		}
//	}
	// Update is called once per frame
	public void FixedUpdate (Rigidbody2D rigidBody2d, float h, bool isGrounded) {
		if (player.GetRigidBody2d().velocity.y < 0) {
			player.GetRigidBody2d().velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

		}
		// Else if jumping and jump button is not held
		else if (player.GetRigidBody2d().velocity.y > 0 &&
								!CrossPlatformInputManager.GetButton("Jump")) {
			player.GetRigidBody2d().velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

		}

		m_Anim.SetBool("Ground", isGrounded);
        //Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }

	public void Move(float _move, bool _jump) {
		Move(_move, _jump, false, HIGH_JUMP_MULTIPLIER);
	}

	public void Move(float _move, bool _jump, bool _highJump, float highJumpMultiplier) {
        // Only control the player if grounded or airControl is turned on
		if ((player.IsGrounded() || player.IsAirControl())
//			&& (canMove() || canWalk)) {
			&& (canMove())) {

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(_move));

            // Move the character

			player.GetRigidBody2d().velocity = new Vector2(_move * m_MaxSpeed, player.GetRigidBody2d().velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (_move > 0 && !m_FacingRight) {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (_move < 0 && m_FacingRight) {
                // ... flip the player.
                Flip();
            }
        }
//		else if (this.canMove ()) {
//			this.isInDialogue ();
//		}
        // If the player should jump...
		if (player.IsGrounded() && _jump && m_Anim.GetBool("Ground")
			&& (canMove() || canJump)) {

            // Add a vertical force to the player.
			player.SetGrounded(false);
            m_Anim.SetBool("Ground", false);
			Vector3 vel = player.GetRigidBody2d().velocity;
			vel.y = 0;
			player.GetRigidBody2d().velocity = vel;

			if (_highJump) {
				player.GetRigidBody2d().AddForce(new Vector2(0f, 1200f));
			}
			else {

				player.GetRigidBody2d().AddForce(new Vector2(0f, m_JumpForce));


			}
        }
    }

	// TODO: Move to PlayerYuni
    public void Flip() {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
		this.playerAttack.HandleTextMeshFlip (); // TODO

        // Multiply the player's x local scale by -1.
        Vector3 theScale = player.transform.localScale;
        theScale.x *= -1;
		player.transform.localScale = theScale;
		this.player.FlipUpdate (theScale.x / Mathf.Abs (theScale.x));
    }

	// Added Functions @Candy
	public void canMove(bool flag) {
		canWalk = flag;
		canJump = flag;
	}

	public bool canMove() {
		if (canWalk && canJump) {
			return true;
		}
		return false;
	}
	public void StopVelocity() {
		Move (0.0f, false);
		m_Anim.SetFloat ("Speed", 0.0f);
	}

	public void isInDialogue() {
		Move (0.0f, false);
		m_Anim.SetFloat ("Speed", 0.0f);
		m_Anim.SetTrigger ("Dialogue");
//		m_Anim.SetBool ("Dialogue", true);
	}

	public PlayerYuni GetPlayer() {
		return player;
	}
}
