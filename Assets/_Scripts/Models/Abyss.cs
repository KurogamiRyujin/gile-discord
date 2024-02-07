using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to destroy/kill any game object/character that comes into contact with this game object.
/// </summary>
public class Abyss : MonoBehaviour {
    /// <summary>
    /// Flag if this boundary will kill of the player avatar.
    /// </summary>
	[SerializeField] private bool killsPlayer = true;
    /// <summary>
    /// Flag if this boundary will kill of an enemy.
    /// </summary>
	[SerializeField] private bool killsEnemy = true;
    /// <summary>
    /// Flag if this boundary will destroy a sky fragment.
    /// </summary>
	[SerializeField] private bool killsSkyPiece = true ;

    /// <summary>
    /// Unity Function. Checks if another game object' collider went in the collider of this game object.
    /// 
    /// Depending on the flags set to true, certain types of objects will be destroyed/killed.
    /// </summary>
    /// <param name="other">Other Game Object's Collider.</param>
	void OnTriggerEnter2D(Collider2D other) {
		if (killsPlayer && other.gameObject.GetComponent<PlayerYuni> () != null) {
			Debug.Log ("Entered Player Death");
			PlayerHealth player = other.gameObject.GetComponent<PlayerHealth> ();
			player.isAlive = false;
		} else if (killsEnemy && other.gameObject.CompareTag (Tags.ENEMY)) {
			EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth> ();
			enemy.Death ();
		}
		else if (killsSkyPiece && other.gameObject.CompareTag (Tags.SKY_FRAGMENT_PIECE)) {
			SkyFragmentPiece piece = other.gameObject.GetComponent<SkyFragmentPiece> ();
			piece.transform.position = piece.GetDetachedManagerParent ().transform.position;
		}
	}

    /// <summary>
    /// Deprecated, trigger should be used.
    /// </summary>
    /// <param name="other">Other Game Object's Collider.</param>
    void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag (Tags.PLAYER)) {
			PlayerHealth player = other.gameObject.GetComponent<PlayerHealth> ();
			player.isAlive = false;
		} else if (other.gameObject.CompareTag (Tags.ENEMY)) {
			EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth> ();
			enemy.Death ();
		}
		else if (other.gameObject.CompareTag (Tags.SKY_FRAGMENT_PIECE)) {
			SkyFragmentPiece piece = other.gameObject.GetComponent<SkyFragmentPiece> ();
			piece.transform.position = piece.GetDetachedManagerParent ().transform.position;
		}
//		else
//			Destroy (other.gameObject);
	}
}
