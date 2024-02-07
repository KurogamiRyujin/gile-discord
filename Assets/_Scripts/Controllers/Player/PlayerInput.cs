using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput {
	public const string scriptName = "PlayerInput";

	[SerializeField] private float HIGH_JUMP_MULTIPLIER = 2.5f;
	private PlayerYuni player;
	private PlayerMovement playerMovement;	//Reference to PlayerMovement script
    public bool isJumping;				//To determine if the player is jumping
	public bool isHighJump;				//Added for TrampolinePlatform
	private bool isAttacking;			//To determine if the player is attacking
	private MobileUI mobile;


	void Awake() {
        //References
//        c_movement = GetComponent<PlayerMovement>();
		#if UNITY_ANDROID
		#endif
	}
	public PlayerInput(PlayerYuni player) {
		this.player = player;
		this.playerMovement = player.GetPlayerMovement ();
	}
	public void Update () {
		#if UNITY_ANDROID
		Move (GameController_v7.Instance.GetMobileUIManager().GetInputValue());
		#elif UNITY_STANDALONE
		// Move according to horizontal axis input
		Move (CrossPlatformInputManager.GetAxis ("Horizontal")); // NEEDED

		#endif

		if (isHighJump) {
			player.SetJumping(true);
		}

        //If he is not jumping...
		if (!player.IsJumping()) {
            //See if button is pressed...
			player.SetJumping(CrossPlatformInputManager.GetButtonDown("Jump"));

			#if UNITY_ANDROID
			player.SetJumping(GameController_v7.Instance.GetMobileUIManager().IsJumping());
			#endif
        }	

		//If not attacking
		if (!isAttacking) {
			//See if button is pressed...
			isAttacking = CrossPlatformInputManager.GetButtonDown("Fire1");
		}
		#if UNITY_ANDROID
//		// If trying to interact with sky piece
//		if(GameController_v7.Instance.GetMobileUIManager().IsCharging()) {
//			GameController_v7.Instance.GetMobileUIManager ().ReleaseChargeButton ();
//			this.player.UseSkyGem ();
//		}
		#else
//		if(Input.GetButtonDown ("Fire2")) {
////			this.player.UseSkyGem ();
//			player.CarryItem();
//		}
		#endif
	}
	public void FixedUpdate(float h) {
		#if UNITY_ANDROID
		Move (h);
		#elif UNITY_STANDALONE
		// Move according to horizontal axis input
		Move (h);
		Debug.Log ("HORIZONTAL "+CrossPlatformInputManager.GetAxis ("Horizontal"));
		#endif
	}

	public void SetHighJump(bool isHigh) {
		this.isHighJump = isHigh;
	}
	public void Move() {
		#if UNITY_ANDROID
		Move (GameController_v7.Instance.GetMobileUIManager().GetInputValue());
		#endif
		Move (CrossPlatformInputManager.GetAxis ("Horizontal")); // NEEDED
	}
	public void Move(float h) {
		if (!player.GetPlayerMovement().canJump)
			player.SetJumping(false);
//		c_movement.Move (h, isJumping);
		player.GetPlayerMovement().Move (h, player.IsJumping(), isHighJump, this.HIGH_JUMP_MULTIPLIER);


		//		playerMovement.Move (h, isJumping, isHighJump, this.HIGH_JUMP_MULTIPLIER);
		isJumping = false;
		isHighJump = false;
	}

	public void SetHighJumpMultiplier(float multiplier) {
		this.HIGH_JUMP_MULTIPLIER = multiplier;
	}

	public bool IsJumping() {
		return this.isJumping;
	}

	public void SetJumping(bool jumping) {
		this.isJumping = jumping;
	}
}	
