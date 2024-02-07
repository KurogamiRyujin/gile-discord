using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Breakable object behaviour which lets certain blocks be broken by the hammer.
/// </summary>
public class BreakableObject : MonoBehaviour {

    /// <summary>
    /// Flag if he block is broken.
    /// </summary>
	private bool isBroken;
    /// <summary>
    /// List of its fragments that split apart when broken.
    /// </summary>
	private List<GameObject> pieceList;
    /// <summary>
    /// Reference to he game object's rigidbody.
    /// </summary>
	private Rigidbody2D rigidBody2D;

    /// <summary>
    /// Standard Unity Function. Starts up the MonoBehaviour. Called once throughout its life.
    /// </summary>
	void Start() {
		this.isBroken = false;
		this.TogglePiece (false);
	}

    /// <summary>
    /// Toggles a broken piece to be visible or not.
    /// </summary>
    /// <param name="value">Flag</param>
	public void TogglePiece (bool value) {
		foreach (GameObject pieceObject in this.GetPieceList()) {
			pieceObject.SetActive (false);
		}
	}

    /// <summary>
    /// Retrieves the list of broken pieces.
    /// </summary>
    /// <returns></returns>
	public List<GameObject> GetPieceList() {
		if (this.pieceList == null) {
			this.pieceList = new List<GameObject> ();
			foreach (BreakablePiece piece in GetComponentsInChildren<BreakablePiece> ()) {
				this.pieceList.Add (piece.gameObject);
			}
		}
		return this.pieceList;
	}

    /// <summary>
    /// Breaks off the pieces from the main object.
    /// </summary>
	public void ActivatePieces() {
		float speed = 100f;
		foreach (GameObject pieceObject in GetPieceList()) {
			Debug.Log ("<color=green>ACT</color>");

			pieceObject.transform.SetParent (null);
//			pieceObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

			pieceObject.GetComponent<Rigidbody2D> ().AddRelativeForce (Random.onUnitSphere*speed);
			pieceObject.GetComponent<BreakablePiece> ().Activate ();
//			pieceObject.GetComponent<Rigidbody2D> ().AddForce (Vector2.up);
		}
		Destroy (gameObject);
	}

    /// <summary>
    /// Breaks the main object.
    /// </summary>
	public void Break() {
		this.isBroken = true;
		SoundManager.Instance.Play (AudibleNames.BreakableBox.BREAK, false);

		this.ActivatePieces ();
//		this.GetComponent<Rigidbody2D>().velocity = transform.forward*100f;
//		this.GetComponent<Rigidbody2D> ().AddTorque (10f);
		Debug.Log ("<color=green>BREAK</color>");
		EventBroadcaster.Instance.PostEvent (EventNames.BREAKABLE_BROKEN);
	}

    /// <summary>
    /// Retrieves the rigidbody.
    /// </summary>
    /// <returns>RigidBody</returns>
	public Rigidbody2D GetRigidBody2D() {
		if (this.rigidBody2D == null) {
			this.rigidBody2D = GetComponent<Rigidbody2D> ();
		}
		return this.rigidBody2D;
	}

    /// <summary>
    /// Unity Function. Collision checker if the breakable object was hit by the hammer.
    /// </summary>
    /// <param name="collision">Collision</param>
	void OnCollisionEnter2D(Collision2D collision) {
		if (!isBroken) {
			if (collision.collider.gameObject.layer == LayerMask.NameToLayer ("HammerBreaker")) { // Hammer
				HammerObject hammer = collision.collider.gameObject.GetComponentInParent<HammerObject> ();
				if (hammer.IsAttacking ()) {
					this.Break ();
				}
			}
			else if(collision.collider.gameObject.GetComponent<NeedleController>() != null) { // Needle
				Debug.Log ("NEEDLE HAS HIT");
				NeedleThrowing needleThrowing = collision.collider.gameObject.GetComponent<NeedleThrowing> ();
				NeedleController needle = collision.collider.gameObject.GetComponent<NeedleController> ();
				needleThrowing.setPullTowards (false);
				needleThrowing.setHookPullTowards (false);
				needle.hasHit = true;
//				NeedleController needle = collision.collider.gameObject.GetComponent<NeedleController>();
//				Break ();
			}
		}
	}

    /// <summary>
    /// Checker if the breakable object has been broken.
    /// </summary>
    /// <returns></returns>
	public bool IsBroken() {
		return this.isBroken;
	}
}
