using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLock : MonoBehaviour {
	// Must be Ground
//	[SerializeField] private Collider2D collider2D;
//	[SerializeField] private PlayerYuni player;

//	void Awake() {
//		this.collider2D = GetComponent<Collider2D> ();
//	}

	void OnCollisionStay2D(Collision2D other) {
//		Debug.Log ("<color=white>LOCK COLLISION</color> "+other.collider.gameObject.name);
		if (other.collider.gameObject.GetComponent<SkyFragmentPiece> () != null) {
			SkyFragmentPiece skyPiece = other.gameObject.GetComponent<SkyFragmentPiece> ();
			if(!skyPiece.IsCarried() && !skyPiece.IsAssigned()) {
				skyPiece.LockPiece (this);
			}
			if(skyPiece.IsCarried()) {
				skyPiece.UnlockPiece (this);
			}
		}


		if (other.collider.gameObject.GetComponent<HollowBlock> () != null) {
			HollowBlock hollowBlock = other.gameObject.GetComponent<HollowBlock> ();
			if(!hollowBlock.IsCarried() &&
				hollowBlock.gameObject.transform.parent.GetComponent<PlayerYuni>() == null &&
				hollowBlock.GetRigidBody2D().velocity == Vector2.zero) {
				hollowBlock.LockPiece (this);
			}
			else {
				hollowBlock.UnlockPiece (this);
			}
		}
	}
	void OnCollisionExit2D(Collision2D other) {
//		Debug.Log ("<color=white>LOCK COLLISION EXIT</color>");
		if (other.collider.gameObject.GetComponent<SkyFragmentPiece> () != null) {
			SkyFragmentPiece skyPiece = other.gameObject.GetComponent<SkyFragmentPiece> ();
			skyPiece.UnlockPiece (this);
		}
		if (other.gameObject.GetComponent<HollowBlock> () != null) {
			HollowBlock hollowBlock = other.gameObject.GetComponent<HollowBlock> ();
			hollowBlock.UnlockPiece (this);
		}
	}

//	void OnTriggerStay2D(Collider2D other) {
//		if (other.gameObject.GetComponent<PlayerYuni> () != null) {
//			if (this.player == null) {
//				this.SetPlayer (other.gameObject.GetComponent<PlayerYuni> ());
//			}
//			this.player.LockPlatform (this);
//		}
//	}
//	void OnTriggerExit2D(Collider2D other) {
//		if (other.gameObject.GetComponent<PlayerYuni> () != null) {
//			if (this.player == null) {
//				this.SetPlayer (other.gameObject.GetComponent<PlayerYuni> ());
//			}
//			this.player.UnlockPlatform (this);
//		}
//	}




//	public void SetPlayer(PlayerYuni playerYuni) {
//		this.player = playerYuni;
//	}

//	public Rigidbody2D GetRigidBody2D() {
//		if (this.rigidBody2D == null) {
//			this.rigidBody2D = GetComponent<Rigidbody2D> ();
//		}
//		return this.rigidBody2D;
//	}

//	public Collider2D GetCollider2D() {
//		if (this.collider2D == null) {
//			this.collider2D = GetComponent<Collider2D> ();
//		}
//		return this.collider2D;
//	}
}
