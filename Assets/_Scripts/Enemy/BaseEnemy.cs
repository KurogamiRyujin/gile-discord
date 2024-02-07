using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

	protected GameObject player;
	protected Rigidbody2D rb;
	protected AIController aiController;
//	protected SpriteRenderer partitionedObjectSprite;

//	public GameObject partitionedObject;
	public int attackPower = 1;

	// Use this for initialization
	void Start () {
		Init ();
	}

	protected virtual void Init() {
//		player = GameObject.FindGameObjectWithTag ("Player");
//		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
//		partitionedObject.transform.localScale = new Vector2 (sprite.bounds.size.x, sprite.bounds.size.y);
		rb = GetComponent<Rigidbody2D> ();
//		partitionedObjectSprite = partitionedObject.GetComponent<SpriteRenderer> ();
//		partitionedObjectSprite.enabled = false;

		aiController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<AIController> ();
	}

//	void Update() {
//		if (player == null) {
//			player = GameObject.FindGameObjectWithTag ("Player");
//		}
//	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag ("Needle")) {
//			partitionedObjectSprite.enabled = true;
//			NeedleController needle = other.gameObject.GetComponent<NeedleController> ();
//			needle.hasHit = needle.onlyHitOnce;
//			partitionedObject.GetComponent<PartitionableObject> ().clone (other.gameObject.GetComponent<NeedleController> ().GetSliceCount ());
		} else if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<PlayerHealth> ().Damage (attackPower);
		}
	}

//	void OnCollisionExit2D(Collision2D other) {
//		if (other.gameObject.CompareTag ("Needle")) {
//			partitionedObjectSprite.enabled = false;
//			partitionedObject.GetComponent<PartitionableObject> ().ClearPartitions ();
//		}
//
//		partitionedObject.GetComponent<PartitionableObject> ().DisableFractionLabel ();
//	}
}
