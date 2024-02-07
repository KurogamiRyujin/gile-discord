using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour {

	// Increases gravity when falling so that the player falls faster
	public float fallMultiplier = 2.5f;
	// For the slightly lower gravity when the player holds the jump button
	public float lowJumpMultiplier = 2f;


	Rigidbody2D rb;

	void Awake() {
		rb = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate() {
		// If falling
		if (rb.velocity.y < 0) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

//			GetComponent<PlayerManager> ().fallingAnimation ();
		}
		// Else if jumping and jump button is not held
		#if UNITY_ANDROID
		else if (rb.velocity.y > 0 && !GameController_v7.Instance.GetMobileUIManager().IsJumping()) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
			//			GetComponent<PlayerManager> ().jumpingAnimation ();
		}
		#else
		else if (rb.velocity.y > 0 && !Input.GetButton ("Jump")) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
//			GetComponent<PlayerManager> ().jumpingAnimation ();
		}
		#endif
	} 
}
