using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowObjectController : MonoBehaviour {

	// public GameObject cloneObjectScript;
//	public PauseController pauseController;
	Transform clonePosition;
	public GameObject cloneObject;
	bool isCloned;

	void Start () {
		clonePosition = transform.Find("clonePosition");
		Debug.Log ("Start");

		//this.cloneObject = GameObject.Instantiate(cloneObject, clonePosition.position, Quaternion.identity);
	}

		
	void clone(Collision2D other) {
		Destroy(other.gameObject);

		//if(!this.cloneObject.activeInHierarchy) {
		//	pauseController.PauseGame ();

//		pauseController.PauseGame();

		GameController_v7.Instance.GetPauseController().Pause();
		this.cloneObject.SetActive(true);
		//}
	}

	// Called whenever 2 colliders bump into each other
	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.CompareTag("Player")) {
			Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
		}

		if(other.gameObject.CompareTag ("Needle")) {
			this.clone(other);
		}
	}
}
