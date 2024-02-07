using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class that handles player interactions with filled blocks.
/// </summary>
public class PlayerLift : MonoBehaviour {
    /// <summary>
    /// Reference to the player.
    /// </summary>
	[SerializeField] private PlayerYuni player;
    /// <summary>
    /// Reference to the players lift ability.
    /// </summary>
	[SerializeField] private LiftAbility liftAbility;
    /// <summary>
    /// Reference to the caried filled block.
    /// </summary>
	[SerializeField] private HollowBlock carriedPiece;


	/// <summary>
	/// Function called on awake.
	/// </summary>
	/// <returns></returns>
	void Awake() {
		this.player = GetComponent<PlayerYuni> ();
	}
	/// <summary>
	/// Getter function that returns the overlap distance of the observed filled block.
	/// </summary>
	/// <returns></returns>
	public float GetOverlapDistance() {
		return this.GetLiftAbility ().GetOverlapDistance ();
	}

	/// <summary>
	/// Checks if the player is overlapping a filled block.
	/// </summary>
	/// <returns></returns>
	public bool HasOverlappingPiece() {
		if (this.liftAbility.GetOverlappingPiece () != null && this.liftAbility.GetOverlappingPiece ().IsLiftable()
			&& !this.liftAbility.GetOverlappingPiece ().IsCarried()) {
			return true;
		}
		else {
			return false;
		}
	}
	/// <summary>
	/// Handles the flipping of the carried object in reference to the parent object's scale.
	/// </summary>
	/// <param name="sign"></param>
	/// <returns></returns>
	public void Flip(float sign) {
		if (this.carriedPiece != null) {
			this.carriedPiece.gameObject.transform.localScale = 
				new Vector3 (sign*Mathf.Abs(this.carriedPiece.gameObject.transform.localScale.x),
					this.carriedPiece.gameObject.transform.localScale.y,
					this.carriedPiece.gameObject.transform.localScale.z);
		}
	}

	/// <summary>
	/// Used to release the carried filled blocks upon player death.
	/// </summary>
	/// <returns></returns>
	public void DropDead() {
		if (this.carriedPiece != null) {
			HollowBlock piece = this.carriedPiece;
			this.Release ();
			piece.Respawn ();
		}
	}
	/// <summary>
	/// Unobserves a filled block.
	/// </summary>
	/// <returns></returns>
	public void Unobserve() {
		this.liftAbility.SetOverlappingPiece (null);
	}
	/// <summary>
	/// Getter function that returns the lift ability.
	/// </summary>
	/// <returns></returns>
	public LiftAbility GetLiftAbility() {
		if (this.liftAbility == null) {
			this.liftAbility = gameObject.GetComponentInChildren<LiftAbility> ();
		}
		return this.liftAbility;
	}

	/// <summary>
	/// Performs a null check and releases the carried filled block.
	/// </summary>
	/// <returns></returns>
	public void UseRelease() {
		if (this.carriedPiece != null) {
			this.Release ();
		}
	}
	/// <summary>
	/// Carries the observed fill block.
	/// </summary>
	/// <returns></returns>
	public void UseCarry() {
		if (!this.player.IsCarryingItem () &&
		    this.carriedPiece == null) {
			this.Carry ();
		}
	}

	/// <summary>
	/// Lifts filled blocks if the player is not carrying any, else it releases the currently carried filled block.
	/// </summary>
	/// <returns></returns>
	public void Use() {
		if (!this.player.IsCarryingItem() &&
			this.carriedPiece == null) {
			this.Carry ();
		} else {
			this.Release ();
		}
	}

	/// <summary>
	/// Carries the filled block and posts a carry event.
	/// </summary>
	/// <returns></returns>
	public void Carry() {
		// Allow Carry only if overlapping HollowBlock IsSolved()
		if (this.GetLiftAbility().GetOverlappingPiece() != null &&
			this.GetLiftAbility().GetOverlappingPiece().IsSolved() /*&&
			!this.GetLiftAbility().GetOverlappingPiece().IsPurelyKinematic()*/) {
			// If not set to "CantCarry"
			if (!this.GetLiftAbility ().GetOverlappingPiece ().CantCarry ()) {
				SoundManager.Instance.Play (AudibleNames.Room.PICKUP_FRAGMENT, false);

				this.player.CarryLiftItem ();
				this.carriedPiece = this.GetLiftAbility ().GetOverlappingPiece ();

				Vector3 liftedOffset = new Vector3 (0f,
					                       player.GetLiftedPiecePosition ().localPosition.y,
					                       0f);
				this.carriedPiece.Carry (gameObject, liftedOffset);
				this.PostCarryEvent (this.carriedPiece);
			} else {
				this.player.SayCantCarry ();
			}
		}
	}
	/// <summary>
	/// Function that handles the posting of a carry event.
	/// </summary>
	/// <param name="carriedPiece"></param>
	/// <returns></returns>
	public void PostCarryEvent(HollowBlock carriedPiece) {
		Parameters parameters = new Parameters ();
		parameters.PutObjectExtra (SkyFragmentPieceUI.HOLLOW_BLOCK, carriedPiece);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAYER_LIFT_CARRY, parameters);
	}
	/// <summary>
	/// Performs a nll check and releases the currently lifted filled block.
	/// </summary>
	/// <returns></returns>
	public void Release() {
		if (this.carriedPiece != null) {
			Debug.Log ("ENTERED RELEASE");
			this.player.DropLiftItem ();
			this.carriedPiece.gameObject.transform.position = this.player.GetDropPiecePosition ().position;
			this.carriedPiece.Drop ();
			this.carriedPiece = null;
			SoundManager.Instance.Play (AudibleNames.Room.PICKUP_FRAGMENT, false);
			EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAYER_LIFT_DROP);
		}
	}
}
