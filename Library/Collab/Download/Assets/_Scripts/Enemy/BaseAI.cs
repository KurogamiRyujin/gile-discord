using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour {

	private const float IDLE_TIME = 5f;
	private const float TIME_BETWEEN_ATTACKS = 2f;
	private const float MAX_DISTANCE_APART = 4f;
	private const float DISTANCE_RETREAT = 2f;
	private const float SPEED = 2.5f;

	bool isIdle = false;
	bool attacked = false;
	bool isFacingRight = false;
	bool cancelWait = false;

	Transform player;
	private float elapsedTime;
	float lastTime = -1f;

	void Awake() {

		EventBroadcaster.Instance.AddObserver (EventNames.ON_HAMMER_DOWN_ZERO, this.RetreatEnemy);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_HAMMER_DOWN_ZERO);
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		elapsedTime = TIME_BETWEEN_ATTACKS;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isIdle) {
			Approach (player.position, SPEED * Time.deltaTime);
		}
	}

	void Approach(Vector3 target, float step) {
		if (Vector2.Distance (transform.position, target) > MAX_DISTANCE_APART) {
			Debug.Log ("Moving towards player");
			Flip ();
			transform.position = Vector2.MoveTowards (transform.position, target, step);
		} else if (Vector2.Distance (transform.position, target) < MAX_DISTANCE_APART && Vector2.Distance (transform.position, target) > DISTANCE_RETREAT) {
			Flip ();
			Debug.Log ("Stop moving");
			transform.position = this.transform.position;
			if (elapsedTime <= 0) {
				Attack ();
			} else {
				elapsedTime -= Time.deltaTime;
			}
		} 
//		else if (Vector2.Distance (transform.position, target) < DISTANCE_RETREAT) {
////			Debug.Log ("Retreating");
//			Retreat (target, step);
//		} 
	}

	void Attack() {
		Debug.Log ("Attack");
		//insert attack here
		StartCoroutine (Idle ());
		cancelWait = false;
		elapsedTime = TIME_BETWEEN_ATTACKS;
	}

	IEnumerator Idle() {
		isIdle = true;
		Debug.Log("Resting");
//		float i = 0;
//		while(i < (int)IDLE_TIME && !cancelWait) {
////			if (Vector2.Distance (transform.position, player.position) < DISTANCE_RETREAT) {
////				Debug.Log ("Retreating in coroutine");
////				cancelWait = true;
////			} 
//			i += Time.deltaTime;
//			yield return null;
//		}

		yield return new WaitForSeconds (IDLE_TIME);
		Debug.Log ("Finished resting");
		isIdle = false;
	}

	void RetreatEnemy() {
		StartCoroutine(Retreat (player.position));
	}

	IEnumerator Retreat(Vector3 target) {
		Debug.Log ("Step " + SPEED * Time.deltaTime);
		float lerp = 0;
		float distance = 1;
		float time = distance / SPEED;

		Vector3 startPos = transform.position;
		Vector3 endPos;
		if(isFacingRight)
			endPos = startPos + new Vector3(-DISTANCE_RETREAT,0,0) * distance;
		else 
			endPos = startPos + new Vector3(DISTANCE_RETREAT,0,0) * distance;
			
//		Vector3 endPos = startPos + Vector3.right * distance;

		while (lerp < 1f) {
			transform.position = Vector2.Lerp (startPos, endPos, lerp);
			Debug.Log ("retreating");
			lerp += Time.deltaTime / time;
			yield return null;
		}
//		transform.position = Vector2.MoveTowards (transform.position, target, -SPEED * Time.deltaTime);
//		yield return null;
	}

	void Flip() {
		if (player.position.x >= transform.position.x) {
			if (!isFacingRight) {
				Vector3 temp = transform.GetChild(1).localScale;
				temp.x *= -1;
				transform.GetChild(1).localScale = temp;
				temp = transform.GetChild (2).localScale;
				temp.x *= -1;
				transform.GetChild(2).localScale = temp;
				isFacingRight = !isFacingRight;
			}
		} else if (player.position.x < transform.position.x) {
			if (isFacingRight) {
				Vector3 temp = transform.GetChild(1).localScale;
				temp.x *= -1;
				transform.GetChild(1).localScale = temp;
				temp = transform.GetChild (2).localScale;
				temp.x *= -1;
				transform.GetChild(2).localScale = temp;
				isFacingRight = !isFacingRight;
			}
		}
	}

	public bool IfIdle() {
		return isIdle;
	}
}
