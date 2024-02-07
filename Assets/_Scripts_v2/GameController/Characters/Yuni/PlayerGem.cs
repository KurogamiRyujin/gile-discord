using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class that handles interactions with sky fragments.
/// </summary>
public class PlayerGem : MonoBehaviour {
	public float yPadding = 0.5f;
    /// <summary>
    /// Reference to the player.
    /// </summary>
	[SerializeField] private PlayerYuni player;
    /// <summary>
    /// Reference to the sky gem.
    /// </summary>
	[SerializeField] private SkyGem skyGem;
    /// <summary>
    /// Reference to the player's carried sky fragment piece.
    /// </summary>
	[SerializeField] private SkyFragmentPiece carriedPiece;
    /// <summary>
    /// Reference to the previously carried sky fragment piece.
    /// </summary>
	[SerializeField] private SkyFragmentPiece lastPiece;

    
	/// <summary>
	/// Function called on awake.
	/// </summary>
	/// <returns></returns>
	void Awake() {
		this.player = GetComponent<PlayerYuni> ();
	}
		
	/// <summary>
	/// Getter that returns the overlap distance of the sky gem.
	/// </summary>
	/// <returns></returns>
	public float GetOverlapDistance() {
		return this.GetSkyGem ().GetOverlapDistance ();
	}

	/// <summary>
	/// Flips the gem to be in the proper orientation in reference to the player's scale.
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
	/// Handles the dropping of carried objects upon player death.
	/// </summary>
	/// <returns></returns>
	public void DropDead() {
		if (this.carriedPiece != null) {
			this.Release ();
			this.lastPiece.DeathBreak ();
		}
	}

	/// <summary>
	/// Getter function that returns the sky gem.
	/// </summary>
	/// <returns></returns>
	public SkyGem GetSkyGem() {
		if (this.skyGem == null) {
			this.skyGem = gameObject.GetComponentInChildren<SkyGem> ();
		}
		return this.skyGem;
	}
    

	/// <summary>
	/// Lifts sky pieces the player is not carrying any, else it releases the currently carried sky piece.
	/// </summary>
	/// <returns></returns>
	public void Use() {
		if (!this.player.IsCarryingItem() &&
			this.carriedPiece == null) {
			this.Carry ();
		}
		else {
			this.Release ();
		}
	}
	/// <summary>
	/// Unassigns the sky gems overlapping piece.
	/// </summary>
	/// <returns></returns>
	public void Unobserve() {
		this.skyGem.SetOverlappingPiece (null);
	}

	/// <summary>
	/// Getter function that returns the position of the lifted piece.
	/// </summary>
	/// <returns></returns>
	public Transform GetLiftedPiecePosition() {
		return this.player.GetLiftedPiecePosition();
	}

	/// <summary>
	/// Releases the carried piece.
	/// </summary>
	/// <returns></returns>
	public void UseRelease() {
		if (this.carriedPiece != null) {
			this.Release ();
		}
	}

	/// <summary>
	/// Sets the currently carried piece to null and carries the overlapping piece.
	/// </summary>
	/// <returns></returns>
	public void UseCarry() {
		if (!this.player.IsCarryingItem () &&
			this.carriedPiece == null) {
			Debug.Log ("Player used carry");
			this.Carry ();
		}
	}
    

	/// <summary>
	/// Carries the overlapping piece.
	/// </summary>
	/// <returns></returns>
	public void Carry() {
		if (this.carriedPiece == null ||
		    this.lastPiece != this.GetSkyGem ().GetOverlappingPiece ()) {
			if (/*this.player.GetPlayerAttack().HasNeedle() &&*/
				this.GetSkyGem ().GetOverlappingPiece () != null &&
				!this.GetSkyGem ().GetOverlappingPiece ().IsAssigned ()) {
				//&&
				//			(this.lastPiece != this.GetSkyGem ().GetOverlappingPiece ()) ||
				//			this.lastPiece != null) {

				//			if (this.carriedPiece == null ||
				//				this.lastPiece != this.GetSkyGem ().GetOverlappingPiece ()) {

				SoundManager.Instance.Play (AudibleNames.Room.PICKUP_FRAGMENT, false);
				this.lastPiece = null;
				this.player.CarryGemItem ();
				this.carriedPiece = this.GetSkyGem().GetOverlappingPiece ();

				Vector3 liftedOffset = new Vector3 (0f,
					//				carriedPiece.GetHeight()+(carriedPiece.GetHeight()/2)+yPadding,
					player.GetLiftedPiecePosition().localPosition.y,
					0f);
				this.carriedPiece.Carry (gameObject, liftedOffset);
				this.PostCarryEvent (this.carriedPiece);
				//			}
			}
		}
	}
    /// <summary>
    /// Checks if the player is near a sky fragment.
    /// </summary>
    /// <returns></returns>
	public bool HasOverlappingPiece() {
		if (this.skyGem.GetOverlappingPiece () != null && !this.skyGem.GetOverlappingPiece().IsAssigned()
			&& !this.skyGem.GetOverlappingPiece().IsCarried()) {
			return true;
		}
		else {
			return false;
		}
	}
    
	/// <summary>
	/// Posts a carry event.
	/// </summary>
	/// <param name="carriedPiece"></param>
	/// <returns></returns>
	public void PostCarryEvent(SkyFragmentPiece carriedPiece) {
		Parameters parameters = new Parameters ();
		parameters.PutObjectExtra (SkyFragmentPieceUI.SKY_PIECE, carriedPiece);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAYER_GEM_CARRY, parameters);
	}
    

	/// <summary>
	/// Releases the carried piece and posts a drop event.
	/// </summary>
	/// <returns></returns>
	public void Release() {
		if (this.carriedPiece != null) {
			this.player.DropGemItem ();
			this.lastPiece = this.carriedPiece;
			this.carriedPiece.gameObject.transform.position = this.player.GetDropPiecePosition ().position;
			this.carriedPiece.Drop ();
			this.carriedPiece = null;
			SoundManager.Instance.Play (AudibleNames.Room.PICKUP_FRAGMENT, false);
			EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAYER_GEM_DROP);
		}
	}
}
