using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftAbility : MonoBehaviour {
	public const string HOLLOW_BLOCK_LAYER = "Breakable";

	[SerializeField] private PlayerYuni player;
	[SerializeField] private HollowBlock overlappingPiece;



//	void OnTriggerEnter2D(Collider2D other) {
//		if (other.gameObject.layer == LayerMask.NameToLayer (HOLLOW_BLOCK_LAYER) &&
//			other.GetComponent<HollowBlock>() != null){
//			Debug.Log ("Lift ENTER");
//			this.SetOverlappingPiece (other.GetComponent<HollowBlock> ());
//		}
//	}
	public void SetPlayer(PlayerYuni playerYuni) {
		this.player = playerYuni;
	}

	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GetComponentInParent<PlayerYuni> ();
		}
		return this.player;
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer (HOLLOW_BLOCK_LAYER) &&
			other.GetComponent<HollowBlock>() != null){
//			this.SetOverlappingPiece (other.GetComponent<HollowBlock> ());

			if (this.overlappingPiece != null) {
				// If other distance is less than current overlapping piece, equip other
				if (Physics2D.Distance (this.GetComponent<Collider2D> (), other).distance <
					Physics2D.Distance (this.GetComponent<Collider2D> (),
						overlappingPiece.gameObject.GetComponent<Collider2D> ()).distance) {
					this.SetOverlappingPiece (other.GetComponent<HollowBlock> ());
				}
			} else {
				this.SetOverlappingPiece (other.GetComponent<HollowBlock> ());
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		this.SetOverlappingPiece(null);
	}

	public float GetOverlapDistance() {
		if (this.overlappingPiece != null) {
			return Physics2D.Distance (this.GetComponent<Collider2D> (),
				overlappingPiece.gameObject.GetComponent<Collider2D> ()).distance;
		} else {
			return Mathf.Infinity;
		}
	}

	public void SetOverlappingPiece(HollowBlock hollowBlock) {
		if (this.overlappingPiece != null) {
			this.overlappingPiece.Unobserve ();
		}

		// If lift piece distance is less than sky piece distance
		if (hollowBlock != null &&
			Physics2D.Distance (this.GetComponent<Collider2D> (),
			hollowBlock.GetComponent<Collider2D> ()).distance <
		    this.GetPlayer ().GetGemOverlapDistance ()) {
			this.GetPlayer ().UnobserveGem ();
			this.overlappingPiece = hollowBlock;
//			if (this.overlappingPiece != null) {
			this.overlappingPiece.Observe ();
			EventBroadcaster.Instance.PostEvent (EventNames.SHOW_PLAYER_CARRY);
//			}
		} else {
			if (hollowBlock != null) {
				hollowBlock.Unobserve ();
			}
			this.overlappingPiece = null;
//			EventBroadcaster.Instance.PostEvent (EventNames.HIDE_PLAYER_CARRY);
		}
//		this.GetPlayer ().CheckOverlap ();
		this.BroadcastOverlappingPiece ();
	}
	void BroadcastOverlappingPiece() {
//		Parameters parameters = new Parameters ();
//		parameters.PutObjectExtra (HintBubbleSkyPiece.OVERLAPPING_PIECE, this.overlappingPiece);
//		EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAYER_LIFT_ENTER, parameters);
	}

	public HollowBlock GetOverlappingPiece() {
		return this.overlappingPiece;
	}
}
