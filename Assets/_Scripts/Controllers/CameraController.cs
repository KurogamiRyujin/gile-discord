using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameObject player;
	private GameObject target;
	// Maintain the difference of the camera y and the player y
	public float yOffset;


	void Start () {
		Debug.Log ("Camera");
//		player = GameObject.FindGameObjectWithTag ("NeedleObject");
//		player = GameObject.FindGameObjectWithTag ("Needle");
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void FixedUpdate () {
//		player = GameObject.FindGameObjectWithTag ("NeedleObject");

//		player = GameObject.FindGameObjectWithTag ("Needle");
//		if (!player.GetCompon	ent<PlayerManager>().hasNeedle) {
//			NeedleController needleController = player.GetComponent<PlayerManager> ().needleBullet.GetComponent<NeedleController>();
//			transform.position = new Vector3 (needleController.gameObject.transform.position.x,
//				needleController.gameObject.transform.position.y + yOffset,
//				transform.position.z);
//		}
//		else {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}

		if (target == null) {
			if (player != null)
				target = player;
		}
//		else {
//			transform.position = new Vector3 (player.transform.position.x,
//				player.transform.position.y + yOffset,
//				transform.position.z);
//		}
//		if (player != null) {


		if(target != null) {

			transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + yOffset, transform.position.z);

//			transform.position = new Vector3 (target.transform.position.x,
//				target.transform.position.y + yOffset,
//				transform.position.z);
//		}
//		}
		}
	}

	public void SetTarget(GameObject target) {
		this.target = target;
	}
}
