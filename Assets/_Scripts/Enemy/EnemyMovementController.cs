using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour {

	public float speed;
	Transform myTrans;
	float enemyWidth, enemyHeight, collWidth;
//	Animator enemyAnimator;

	public GameObject enemyUI;
//	bool canFlip = true;
	bool facingRight = false;
	float flipTime = 5.0f;
	float nextFlipChance = 0f;

	public LayerMask enemyMask;
//	public float chargeTime;
//	float startChargeTime;
//	bool charging;
	Rigidbody2D enemyRB;
	private EnemyHealth enemyHealth;
	[SerializeField] private SpriteRenderer enemySprite;

	// Use this for initialization
	void Start () {
//		enemyAnimator = GetComponent<Animator> ();
		myTrans = this.transform;
		enemyRB = GetComponent<Rigidbody2D> ();
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		enemyWidth = GetComponent<SpriteRenderer> ().bounds.extents.x;
		enemyHeight = GetComponent<SpriteRenderer> ().bounds.extents.y;
		BoxCollider2D coll = GetComponent<BoxCollider2D> ();
		collWidth = coll.size.x;
		print (collWidth);

		this.enemyHealth = GetComponent<EnemyHealth> ();
	}

	void FixedUpdate() {
		Vector2 lineCastPos = new Vector2(myTrans.position.x, myTrans.position.y) - new Vector2(myTrans.right.x, myTrans.right.y) * enemyWidth + Vector2.up * enemyHeight;
		Debug.DrawLine (lineCastPos, lineCastPos + Vector2.down*1.5f, Color.red);
		bool isGrounded = Physics2D.Linecast (lineCastPos, lineCastPos + Vector2.down, enemyMask);
		RaycastHit2D hit = Physics2D.Linecast (lineCastPos, lineCastPos + Vector2.down, enemyMask);
		Debug.DrawLine (lineCastPos, lineCastPos - new Vector2(myTrans.right.x, myTrans.right.y) * 1.5f, Color.red);
//		bool isBlocked = Physics2D.Linecast (lineCastPos, lineCastPos - new Vector2(myTrans.right.x, myTrans.right.y) * 1.5f, enemyMask);
//		RaycastHit2D hit = Physics2D.Linecast (lineCastPos, lineCastPos - new Vector2(myTrans.right.x, myTrans.right.y) * 1.5f, enemyMask);
//	
//		if (hit.collider != null) {
//			print (hit.collider.name + " is on top of " + gameObject.name);
//		}
//				Debug.Log ("Grounded " + isGrounded);
//		if (!isGrounded || isBlocked) {
		if (!isGrounded) {
			FlipFacing ();
		} 
		if (Time.time > nextFlipChance) {
			FlipFacing ();
			nextFlipChance = Time.time + flipTime;
		}

	}
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag("Player")) {
			if (facingRight && other.transform.position.x < myTrans.position.x) {
				FlipFacing ();
			} else if (!facingRight && other.transform.position.x > myTrans.position.x) {
				FlipFacing ();
			}

//			canFlip = false;
//			charging = true;
//			startChargeTime = Time.time + chargeTime;
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
//			Debug.Log ("stay");
			if (facingRight && other.transform.position.x < myTrans.position.x) {
				FlipFacing ();
				Move ();
			} else if (!facingRight && other.transform.position.x > myTrans.position.x) {
				FlipFacing ();
				Move ();
			} else {
				Move ();

			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
//			canFlip = true;
//			charging = false;
			enemyRB.velocity = Vector2.zero;
//			enemyAnimator.SetBool ("isCharging", charging);
		}
	}

	void FlipFacing() {
//		if (!canFlip)
//			return;
//		if (!facingRight) {
//		}

//		GetComponentInChildren<SpriteRenderer> ().flipX = !GetComponentInChildren<SpriteRenderer> ().flipX;
		enemySprite.flipX = !enemySprite.flipX;
		facingRight = !facingRight;

//		Vector2 currRot = myTrans.eulerAngles;
//		currRot.y += 180;
//		myTrans.eulerAngles = currRot;
//		facingRight = !facingRight;
//		this.enemyHealth.HandleNumberLineFlip ();
//	
//		print ("Facing right " + facingRight);
	}

	void Move() {
		Vector2 myVel = enemyRB.velocity;
		myVel.x = -myTrans.right.x * speed;
		enemyRB.velocity = myVel;
	}
}
