using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hook point allowing the player avatar to scale towards its position when it is hit by the needle.
/// </summary>
public class HookPoint : MonoBehaviour {
	[SerializeField] float hookPullThreshold = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Needle")) {
			Debug.Log ("Exit :: HOOK NEEDLE HIT "+other.tag);
		}
	}
	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.GetComponent<NeedleThrowing>()) {
			Debug.Log ("HOOK NEEDLE HIT");
			NeedleController needleController = other.gameObject.GetComponent<NeedleController> ();
			NeedleThrowing needleThrowing = other.gameObject.GetComponent<NeedleThrowing> ();

			if (needleThrowing.getPlayerAttack ().HasThread()) {
				needleThrowing.setExtendedNeedleTransform (gameObject.transform.position);
				needleThrowing.setPullTowards (true);
				needleThrowing.setHookPullTowards (true);

				//			needleThrowing.setExtendedNeedleTransform (GameObject.FindGameObjectWithTag("NeedleExtendedTransform").transform.position);

//				Physics2D.IgnoreCollision (needleThrowing.getPlayerMovement().gameObject.GetComponent<Collider2D>(), gameObject.GetComponent <Collider2D> ());
				needleController.hasHit = true;
			}

		
		} 
//		else if (other.gameObject.CompareTag ("Player")) {
//			PlayerMovement player = other.GetComponent<PlayerMovement> ();
//
//			player.transform.position = Vector3.MoveTowards (player.transform.position, transform.position, 1 * Time.deltaTime);
//		}
	}
}
