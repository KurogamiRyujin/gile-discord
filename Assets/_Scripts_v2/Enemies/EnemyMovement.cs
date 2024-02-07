using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour {
	
	protected bool isFacingRight = false;
	protected Body enemyBody;

	protected float enemyWidth;
	protected float enemyHeight;

	protected bool grounded;
	protected float jumpForce = 410f;
	protected Rigidbody2D rigidBody2D;

	protected Transform myTransform;
	protected LayerMask enemyMask;
	[SerializeField] LayerMask wallMask;

	protected virtual void Start() {
		this.enemyBody = GetComponentInChildren<Body> ();
		this.rigidBody2D = GetComponent<Rigidbody2D>();
		this.myTransform = this.transform;

		enemyWidth = GetComponent<Collider2D> ().bounds.extents.x;
		enemyHeight = GetComponent<SpriteRenderer> ().bounds.extents.y;
	}

	public void Move (Vector3 target, float step) {
		Flip (target);
		if (isFacingRight) {
			Vector2 lineCastPos = new Vector2 (myTransform.position.x, myTransform.position.y) - new Vector2 (myTransform.right.x, myTransform.right.y) * enemyWidth + Vector2.up * enemyHeight;
			bool inFront = Physics2D.Linecast (lineCastPos, lineCastPos + Vector2.right, wallMask);
			Debug.DrawLine (lineCastPos, lineCastPos + new Vector2(myTransform.right.x, myTransform.right.y) * 1.5f, Color.red);
			if (!inFront) {
				transform.position = Vector2.MoveTowards (transform.position, new Vector2(target.x, transform.position.y), step);
			}
		} else {
			Vector2 lineCastPos = new Vector2 (myTransform.position.x, myTransform.position.y) - new Vector2 (myTransform.right.x, myTransform.right.y) * enemyWidth + Vector2.up * enemyHeight;
			bool inFront = Physics2D.Linecast (lineCastPos, lineCastPos + Vector2.left, wallMask);
			Debug.DrawLine (lineCastPos, lineCastPos + Vector2.left * 1.5f, Color.red);
			if (!inFront) {
				transform.position = Vector2.MoveTowards (transform.position, new Vector2(target.x, transform.position.y), step);
			}
		}

	}

	public void Flip(Vector3 target) {
		if (target.x >= transform.position.x) {
			if (!isFacingRight) {
				Flip ();
			}
		} else if (target.x < transform.position.x) {
			if (isFacingRight) {
				Flip ();
			}
		}
	}

	protected virtual void FixedUpdate() {
//		Vector2 lineCastPos = new Vector2(myTransform.position.x, myTransform.position.y) - new Vector2(myTransform.right.x, myTransform.right.y) * enemyWidth + Vector2.up * enemyHeight;
//		Debug.DrawLine (lineCastPos, lineCastPos + Vector2.down*1.5f, Color.red);
//		grounded = Physics2D.Linecast (lineCastPos, lineCastPos + Vector2.down, enemyMask);
//		RaycastHit2D hit = Physics2D.Linecast (lineCastPos, lineCastPos + Vector2.down, enemyMask);
//		Debug.DrawLine (lineCastPos, lineCastPos - new Vector2(myTransform.right.x, myTransform.right.y) * 1.5f, Color.red);
	}

	protected void Flip() {
		Vector3 temp = this.enemyBody.gameObject.transform.localScale;
		this.enemyBody.gameObject.transform.localScale = new Vector3 (-temp.x, temp.y, temp.z);
		isFacingRight = !isFacingRight;
	}

	public void Jump() {
		if (grounded) {
			Debug.Log ("Jump");
			grounded = false;
			Vector3 vel = rigidBody2D.velocity;
			vel.y = 0;
			rigidBody2D.velocity = vel;

			rigidBody2D.AddForce (new Vector2 (0f, jumpForce));
			//			SoundManager.Instance.Play (AudibleNames.Popcorn.JUMP, false);
		}

	}

	public bool IsFacingRight() {
		return this.isFacingRight;
	}
}
