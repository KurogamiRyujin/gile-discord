using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfController : MonoBehaviour {
	private const float CHARGE_DELAY = 3.0f;
	private const float NORMAL_SPEED = 4.0f;
	private const float FAST_SPEED = 8.0f;
	private const float FLOAT_ROTATE_SPEED = 3f;
	private const float FLOAT_RADIUS = 0.1f;
	private const float DEFEND_ROTATE_SPEED = 3f;
	private const float DEFEND_RADIUS = 1f;
	private const float WAIT_TIME = 8f;

	[SerializeField] Transform leftCollider;
	[SerializeField] Transform rightCollider;
	[SerializeField] Transform images;
	[SerializeField] Transform RRH;

	float counter;

	Rigidbody2D wolfRB;
	float speed;
	bool facingRight;
	bool shouldTurn;
	bool charged = false;
	float angle;
	Vector2 rrhCenter;
	bool moveBackDown = false;

	private Animator anim;

	// Use this for initialization
	void Start () {
		facingRight = false;
		speed = NORMAL_SPEED;
		wolfRB = GetComponent<Rigidbody2D> ();
		shouldTurn = false;
		anim = GetComponent<Animator> ();
		rrhCenter = new Vector2(RRH.position.x, RRH.position.y);
//		StartCoroutine (chargeContinuous (3));
	}
	
	// Update is called once per frame
	void Update () {

		Float();
//		if (!charged) { 
//			ProtectRRH ();
		//		}
//		transform.RotateAround (rrh.position, new Vector3(0,0,1) , 20 * Time.deltaTime);
//		if (!moveBackDown) {
//			StartCoroutine (MoveDown ());
//		}


//		if(!moveBackDown)
//			DefendRRH();



//		if (!moveBackDown) {
//			print ("moving");
//			StartCoroutine (MoveUp ()); 
////			StartCoroutine (MoveDown ());
//		}
	}

	void ProtectRRH() {
		//todo: add rrh life
		StartCoroutine (chargePause (CHARGE_DELAY));
		//if(rrhcurrlife < rrhprevlife) {
		// chargeTowards(position);
		//}
		//else
		//baseAI();
	}

	void charge(Vector3 target) {
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target, step);
	}

	IEnumerator chargePause(float delay) {
		//call charge
		//charge(CHARGE_DELAY, getEndOfScreen());
		charged = true;

		while (transform.position != GetEndOfScreen ()) {
			charge (GetEndOfScreen());
			yield return null;
		}
		Stop ();
		Turn ();
		yield return new WaitForSeconds (delay);

		charged = false;
	}

	IEnumerator chargeContinuous(int duration) {
		bool onEdge = false;
//		speed = FAST_SPEED;
		for (int i = 0; i < duration; i++) {
			print (onEdge + " " + shouldTurn);
			if (!onEdge) {
				while (!onEdge) {
					Move ();
					if (shouldTurn)
						onEdge = true;
					yield return null;
				}
			}

			if (onEdge) {
				while (onEdge) {
					Stop ();
					Turn ();
					shouldTurn = false;
					Move ();
					onEdge = false;
				}
			}
		}

//		speed = NORMAL_SPEED;
		Stop ();
	}

	void Turn() {
		facingRight = !facingRight;

		Vector3 theScale = images.transform.localScale;
		theScale.x *= -1;
		images.transform.localScale = theScale;
	}

	void OnTriggerEnter2D(Collider2D other) {
		print (other.gameObject.tag);
		if(other.gameObject.CompareTag("Collider")) {
			shouldTurn = true;
		}
	}

	private Vector3 GetEndOfScreen() {
		//get where BBW is facing and return approp. point
		if (facingRight)
			return rightCollider.position - Vector3.right;
		else
			return leftCollider.position + Vector3.right;
	}

	private Vector2 GetDirection() {
		if (facingRight)
			return Vector2.right;
		return Vector2.left;
	}

	void Move() {
		Vector2 myVel = wolfRB.velocity;
		myVel.x = GetDirection().x * speed;
		wolfRB.velocity = myVel;
	}

	void Stop() {
//		anim.SetFloat ("Speed", 0f);
		wolfRB.velocity = Vector2.zero;
	}

	void Float() {
		angle += FLOAT_ROTATE_SPEED * Time.deltaTime;
		Vector2 offset = new Vector2 (12.0f, Mathf.Cos (angle)) * FLOAT_RADIUS;
		transform.position = rrhCenter + offset;
	}

	void DefendRRH() {
//		angle += DEFEND_ROTATE_SPEED * Time.deltaTime;
//		Vector2 offset = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle)) * DEFEND_RADIUS;
//		transform.position = rrhCenter + offset;

//		angle += DEFEND_ROTATE_SPEED * Time.deltaTime;
//		Vector2 offset = new Vector2 (1, Mathf.Sin (angle)) * DEFEND_RADIUS;
//		transform.position = rrhCenter + offset;
		Vector3 targetPosition = RRH.position - new Vector3(2, 0,0);
		float height = 0.5f;
//
		float time = Time.time * 0.75f;
		Vector3 currentPos = Vector3.Lerp (transform.position, targetPosition, time);
		currentPos.y += height * Mathf.Sin (Mathf.Clamp01 (time) * Mathf.PI);
		transform.position = currentPos;
		print (currentPos);
//
//		StartCoroutine (Pause ());
//		print ("after 8 secs");

		Invoke ("Down", 5.0f);
//
//		targetPosition = RRH.position + new Vector3(2, 0,0);
//		currentPos = Vector3.Lerp (transform.position, targetPosition, time);
//		currentPos.y -= height * Mathf.Sin (Mathf.Clamp01 (time) * Mathf.PI);
//		transform.position = currentPos;
	}

	void Down() {
		Vector3 targetPosition = RRH.position + new Vector3(2, 0,0);
		float height = 0.5f;
		//
		float time = Time.time * 0.75f;
		Vector3 currentPos = Vector3.Lerp (transform.position, targetPosition, time);
		currentPos.y -= height * Mathf.Sin (Mathf.Clamp01 (time) * Mathf.PI);
		transform.position = currentPos;

		moveBackDown = true;

	}

	IEnumerator MoveUp() {
		Vector3 targetPosition = RRH.position - new Vector3(2, 0,0);
		float height = 0.5f;
		float time = Time.time * 0.75f;
		bool up = false;
		bool down = false;

		print (targetPosition);
		while (transform.position != targetPosition && !up) {
			time = Time.time * 0.75f;
			Vector3 currentPos = Vector3.Lerp (transform.position, targetPosition, time);
			currentPos.y += height * Mathf.Sin (Mathf.Clamp01 (time) * Mathf.PI);
			transform.position = currentPos;
//			print (time);
//			print (currentPos);
			print (targetPosition);
			print (up);
			yield return null;
		}
		if (transform.position == targetPosition) {
			up = true;
		}
		print (up);
		print ("exit while");
//
		Vector3 targetPosition2 = RRH.position + new Vector3(2, 0,0);
		float time2 = Time.time * 0.75f;
		while (transform.position != targetPosition2 && !down) {
			print ("inside while");
			time2 = Time.time * 0.75f;
			Vector3 currentPos = Vector3.Lerp (transform.position, targetPosition2, time2);
			currentPos.y -= height * Mathf.Sin (Mathf.Clamp01 (time2) * Mathf.PI);
			transform.position = currentPos;
			print (targetPosition2);
			print (time2);
			yield return null;
		}

		if (transform.position == targetPosition2) {
			down = true;
		}
//
//		moveBackDown = true;
	}

	IEnumerator MoveDown() {
		Vector3 targetPosition = RRH.position + new Vector3(2, 0,0);
		float height = 0.5f;
		float time = Time.time * 0.75f;

		while (time <= 2.5f) {
			time = Time.time * 0.75f;
			Vector3 currentPos = Vector3.Lerp (transform.position, targetPosition, time);
			currentPos.y -= height * Mathf.Sin (Mathf.Clamp01 (time) * Mathf.PI);
			transform.position = currentPos;
			print (time);
			print (currentPos);
			yield return null;
		}

		yield return null;

		moveBackDown = true;
	}

	IEnumerator Pause() {
		yield return new WaitForSeconds (WAIT_TIME);
	}

}
