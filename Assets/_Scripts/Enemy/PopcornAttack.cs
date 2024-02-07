using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornAttack : EnemyAttack {

	private Popcorn popcorn;
	[SerializeField] Bullet bullet;

	public const float HELL_TIME = 0.25f;
	public const float REG_TIME = 2f;
	public const float IDLE_TIME = 5f;

	private bool attacked = false;
	private float elapsedTime;
	private int regCount;

	void Start() {
		this.popcorn = GetComponent<Popcorn> ();
		elapsedTime = REG_TIME;
		regCount = 0;
	}

	void Update() {
		if (popcorn.IsActive ()) {
			AttackCheck (popcorn.GetPlayer().transform.position);
		}
	}

	void AttackCheck(Vector3 target) {
		if (Vector2.Distance (transform.position, target) < Popcorn.MAX_DISTANCE_APART && Vector2.Distance (transform.position, target) > Popcorn.DISTANCE_RETREAT) {
			if (elapsedTime <= 0) {

				if (!attacked) {
					attacked = true;
					Attack ();
				}

				CheckTime ();
				regCount++;

			} else {
				elapsedTime -= Time.deltaTime;
			}
		}
	}

	public override void Attack() {
		Debug.Log ("<color=red>Attack</color>");
		SoundManager.Instance.Play (AudibleNames.Popcorn.ATTACK, false);
		Bullet bulletClone;
		if (popcorn.IsFacingRight()) {
			bulletClone = Instantiate (bullet, transform.position, Quaternion.identity);
			bulletClone.GetComponent<BulletDamage> ().SetDamage (this.damage);
		} else {
			bulletClone = Instantiate (bullet, transform.position, Quaternion.Euler (0, 0, 180));
			bulletClone.GetComponent<BulletDamage> ().SetDamage (this.damage);
		}
		attacked = false;
	}

	IEnumerator Idle() {
		popcorn.SetIdle (true);
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
		popcorn.SetIdle (false);
	}

	void CheckTime() {

		if (regCount > 3 && regCount < 10) {
			Debug.Log ("HELL TIME");
			elapsedTime = HELL_TIME;
		} else {
			elapsedTime = REG_TIME;
		}

		if (regCount == 10 || regCount == 4) {
			StartCoroutine (Idle ());
		}

		if(regCount == 10) 
			regCount = 0;
	}
}
