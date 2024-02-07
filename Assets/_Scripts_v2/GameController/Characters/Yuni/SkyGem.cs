using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles player interactions with sky fragments.
/// </summary>
public class SkyGem : MonoBehaviour {
	/// <summary>
    ///  Reference to the string of the sky piece layer.
    /// </summary>
	public const string SKY_PIECE_LAYER = "SkyPiece";
    /// <summary>
    /// Reference to the player.
    /// </summary>
	[SerializeField] private PlayerYuni player;
    /// <summary>
    /// Reference to the overlapping sky fragment piece.
    /// </summary>
	[SerializeField] private SkyFragmentPiece overlappingPiece;
    /// <summary>
    /// Reference to the overlap distance of the overlapping piece.
    /// </summary>
	[SerializeField] private float overlapDistance;

    
	/// <summary>
	/// Function called upon awake.
	/// </summary>
	/// <returns></returns>
	void Awake() {
		this.overlappingPiece = null;
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("SkyGem"));
	}

	/// <summary>
	/// Function called on trigger stay.
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer (SKY_PIECE_LAYER) &&
			other.GetComponent<SkyFragmentPiece>() != null &&
			!other.GetComponent<SkyFragmentPiece>().IsCarried()){


			if (this.overlappingPiece != null) {
				// If other distance is less than current overlapping piece, equip other
				if (Physics2D.Distance (this.GetComponent<Collider2D> (), other).distance <
				      Physics2D.Distance (this.GetComponent<Collider2D> (),
					      overlappingPiece.gameObject.GetComponent<Collider2D> ()).distance) {
					this.SetOverlappingPiece (other.GetComponent<SkyFragmentPiece> ());
				}
			} else {
					this.SetOverlappingPiece (other.GetComponent<SkyFragmentPiece> ());
			}
		}
	}

	/// <summary>
	/// Function called on trigger exit.
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	void OnTriggerExit2D(Collider2D other) {
		// Dereference piece if it entered TriggerExit
		this.SetOverlappingPiece(null);
	}

	/// <summary>
	/// Getter function that returns the overlap distance.
	/// </summary>
	/// <returns></returns>
	public float GetOverlapDistance() {
		if (this.overlappingPiece != null) {
			return Physics2D.Distance (this.GetComponent<Collider2D> (),
				overlappingPiece.gameObject.GetComponent<Collider2D> ()).distance;
		} else {
			return Mathf.Infinity;
		}
	}

	/// <summary>
	/// Function that broadcasts the overlapping piece.
	/// </summary>
	/// <returns></returns>
	void BroadcastOverlappingPiece() {
//		Parameters parameters = new Parameters ();
//		parameters.PutObjectExtra (HintBubbleSkyPiece.OVERLAPPING_PIECE, this.overlappingPiece);
//		EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAYER_GEM_ENTER, parameters);
	}
	/// <summary>
	/// Getter function that returns the player.
	/// </summary>
	/// <returns></returns>
	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GetComponentInParent<PlayerYuni> ();
		}
		return this.player;
	}

	/// <summary>
	/// Setter function that sets the current player.
	/// </summary>
	/// <param name="playerYuni"></param>
	/// <returns></returns>
	public void SetPlayer(PlayerYuni playerYuni) {
		this.player = playerYuni;
	}
	/// <summary>
	/// Setter function that sets the overlappine piece.
	/// </summary>
	/// <param name="skyPiece"></param>
	/// <returns></returns>
	public void SetOverlappingPiece(SkyFragmentPiece skyPiece) {
		if (this.overlappingPiece != null) {
			this.overlappingPiece.Unobserve ();
		}

		// If gem piece distance is less than sky piece distance
		if (skyPiece != null &&
			Physics2D.Distance (this.GetComponent<Collider2D> (),
			skyPiece.GetComponent<Collider2D> ()).distance <
		    this.GetPlayer ().GetLiftOverlapDistance ()) {

			this.GetPlayer ().UnobserveLift ();
			this.overlappingPiece = skyPiece;
//			if (this.overlappingPiece != null) {
			this.overlappingPiece.Observe ();

			EventBroadcaster.Instance.PostEvent (EventNames.SHOW_PLAYER_CARRY);
//			}
		} else {
			if (skyPiece != null) {
				skyPiece.Unobserve ();
			}
			this.overlappingPiece = null;
//			EventBroadcaster.Instance.PostEvent (EventNames.HIDE_PLAYER_CARRY);
		}
//		this.GetPlayer ().CheckOverlap ();
		this.BroadcastOverlappingPiece ();

	}
	public SkyFragmentPiece GetOverlappingPiece() {
		return this.overlappingPiece;
	}
}
