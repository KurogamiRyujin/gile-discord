using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popcorn : Enemy {

	[SerializeField] protected PopcornAttack popcornAttack;
	[SerializeField] protected PopcornMovement popcornMovement;

	public const float MAX_DISTANCE_APART = 4f;
	public const float DISTANCE_RETREAT = 2f;
	public const float SPEED = 2.5f;
	// Radius of the overlap circle to determine if grounded
	// Reference to the player's animator component. 

	bool isFacingRight = false;
	bool cancelWait = false;

	float lastTime = -1f;

	float shootTime = 2;

	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.ON_HAMMER_DOWN_ZERO, this.RetreatEnemy);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_HAMMER_DOWN_ZERO);
	}

	void Start () {
		// Always call base.Start()
		base.Start ();

		this.popcornMovement = GetComponent<PopcornMovement> ();
		this.popcornAttack = GetComponent<PopcornAttack> ();
	}


	// Update is called once per frame
	void Update () {
		if (this.IsActive()) {
			Approach (this.player.transform.position, SPEED * Time.deltaTime);
		}
	}

	void Approach(Vector3 target, float step) {
		if (Vector2.Distance (transform.position, target) > MAX_DISTANCE_APART) {
			//			Debug.Log ("Moving towards player");
			//			Flip ();
			//			transform.position = Vector2.MoveTowards (transform.position, target, step);
			this.enemyMovement.Move(target, step);
			if(!SoundManager.Instance.GetPopcornSFX().GetAudioSource().isPlaying)
				SoundManager.Instance.Play (AudibleNames.Popcorn.WALK, false);
		} else if (Vector2.Distance (transform.position, target) < MAX_DISTANCE_APART && Vector2.Distance (transform.position, target) > DISTANCE_RETREAT) {

			this.enemyMovement.Flip (target);
//						transform.position = this.transform.position;


		} 
		//		else if (Vector2.Distance (transform.position, target) < DISTANCE_RETREAT) {
		////			Debug.Log ("Retreating");
		//			Retreat (target, step);
		//		} 
	}


	void RetreatEnemy() {
		StartCoroutine(Retreat (this.player.transform.position));
	}


	IEnumerator Retreat(Vector3 target) {
		Debug.Log ("Step " + SPEED * Time.deltaTime);
		//		SoundManager.Instance.Play (AudibleNames.Popcorn.WALK, false);
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

	public bool IfIdle() {
		return isIdle;
	}
}
