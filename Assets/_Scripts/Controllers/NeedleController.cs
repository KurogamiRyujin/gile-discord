using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleController : MonoBehaviour {
	
	public float throwSpeed = 50.0f;
	public float returnDelay = 0.7f;
	public bool hasHit;
	public bool onlyHitOnce = true;
	public bool isPulled;

	private float x;
	private float y;
	private float throwStartTime;
	private Vector3 mousePosition;
	private Vector3 direction;
	private GameObject player;

	private NeedleThrowing needleThrowing;
	float startTime;
	float journeyLength;
	float fracJourney;
	float returnSpeed;
	float angleSpeed;
	float localDirection;
	float targetX;

//	PlayerManager playerManager;
	GameObject needleObject;
//	public Transform thrownPosition;
	ColliderDistance2D colliderDistance;

	//Set to public for testing purposes. Later, will be set to private and will be controlled by Thread objects the player picks up.
	//Still, make sure to use GetWireSliceCount() to get this value.
	private int wireSliceCount = 3;

	void Start() {
		gameObject.tag = "Needle";
	}

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player");
//		playerManager = player.GetComponent<PlayerManager> ();
		needleThrowing = gameObject.GetComponent<NeedleThrowing>();


//		x = transform.position.x;
//		y = transform.position.y;
//		returnSpeed = 50.0f;
		angleSpeed = 1.0f;
		onlyHitOnce = true;
//		gameObject.SetActive(false);
	}

	void OnEnable() {
		this.isPulled = false;
		this.hasHit = false;
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0.0f;
		direction = (mousePosition - transform.position).normalized;
		rotate();


//		thrownPosition.position = mousePosition; 
		this.throwStartTime = Time.time;
	}

//	void Update() {
//		if (!PauseController.isPaused) {
//			colliderDistance = gameObject.GetComponent<Collider2D>().Distance(player.GetComponent<Collider2D>());
////			Debug.Log ("collDist: "+colliderDistance);
//			if(isPulled &&
//				colliderDistance.distance < 0) {
//				disable ();
//			}
//
//			//if needle travel time is greater than returnDelay, pull needle back
//			if ((Time.time - throwStartTime) >= returnDelay) {
//				isPulled = true;
//			}
//
//			//if needle being pulled, return needle
//			if (isPulled) {
//				direction = (playerManager.needlePosition.position - transform.position).normalized;
////				transform.position += direction * throwSpeed * Time.deltaTime;
//				gameObject.GetComponent<Rigidbody2D>().MovePosition(direction);
//			}//else, throw needle
//			else {
////				transform.position += direction * throwSpeed * Time.deltaTime;
//				gameObject.GetComponent<Rigidbody2D>().MovePosition(direction);
//			}
//		}
//	}

	void rotate() {
		//Get the Screen positions of the object
		Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
		//Get the Screen position of the mouse
		Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
		//Get the angle between the points
		float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);


		// Rotate
		transform.rotation =  Quaternion.Euler (new Vector3(0f, 0f, angle));
		//transform.position = new Vector3(transform.position.x+0.5f, y, transform.position.z);
	}

	void rotateTowardsPlayer() {
		//Get the Screen positions of the object
		Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (gameObject.transform.Find("needlePosition").position);
		//Get the Screen position of the mouse
		Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(player.transform.Find("needlePosition").position);
		//Get the angle between the points
		float angle = AngleBetweenTwoPoints(mouseOnScreen, positionOnScreen);

		if (gameObject.transform.eulerAngles.z > angle) {
			angle = gameObject.transform.eulerAngles.z + angleSpeed;
		}
		else if (gameObject.transform.eulerAngles.z < angle) {
			angle = gameObject.transform.eulerAngles.z - angleSpeed;
		}

		// Rotate
		transform.rotation =  Quaternion.Euler (new Vector3(0f, 0f, angle));
		//transform.position = new Vector3(transform.position.x+0.5f, y, transform.position.z);
	}

	float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}

	void pullNeedle() {
		if (!isPulled) {
			this.isPulled = true;
			//rotateTowardsPlayer();
		} 		
	}

	void disable() {
		Debug.Log ("DISABLED");
//		playerManager.hasNeedle = true;
		isPulled = false;
		gameObject.SetActive (false);
	}
	void keepNeedle() {
//		playerManager.hasNeedle = true;
	}

	void OnDisable() {
		keepNeedle ();
	}

	void OnCollisionEnter2D(Collision2D other) {

		Debug.Log ("OTHER TAG IS "+other.gameObject.tag);
//		if (isPulled && other.gameObject.CompareTag ("Player")) {
//			Debug.Log ("Collision");
//			disable ();
//		}
		if (/*!isPulled &&*/ other.gameObject.CompareTag ("Player")) {
//			Physics2D.IgnoreCollision (other.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
//			Debug.Log ("Ignored");
		} else {
//			this.hasHit = true;
		}
//		if(other.gameObject.CompareTag ("Hook Point")) {
//			Debug.Log ("ENTERED");
//			needleThrowing.setPullTowards (true);
//			this.hasHit = true;
//
//		}
//		if(other.gameObject.CompareTag ("Ground")) {
//			this.hasHit = true;
//		}
	}

	public void SyncWireSliceCountWithEquippedDenominator(int denominator) {
		this.wireSliceCount = denominator;
		this.PostDenominatorEvent (this.wireSliceCount);
	}
	public void PostDenominatorEvent(float denominator) {
		Parameters parameters = new Parameters ();
		parameters.PutExtra (YarnballUI.YARNBALL_VALUE, denominator);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_DENOMINATOR_CHANGE, parameters);
	}
	public int GetSliceCount() {
		return this.wireSliceCount;
	}
}

