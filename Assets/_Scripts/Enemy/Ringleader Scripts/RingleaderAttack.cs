using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingleaderAttack : EnemyAttack {

	public const string scriptName = "RingleaderAttack";

	public enum Gambit {
		ROBIN_HOOD,
		RIDING_HOOD,
		ALICE,
		FINAL
	}
//	[SerializeField] private YarnballControl.OperationType operationType = YarnballControl.OperationType.SIMILAR;
	[SerializeField] private Gambit gambit = Gambit.ROBIN_HOOD;
	[SerializeField] private RingleaderAnimatable ringleaderAnimatable;
	private EnemyHealth enemyHealth;

	private RingleaderMovement ringleaderMovement;
	[SerializeField] private List<EnemySpawner> spawnPoints;
	[SerializeField] private float spawnEnemyInterval = 2.5f;
	[SerializeField] private float STRONG = 100.0f;
	[SerializeField] private float MEDIUM = 50.0f;
	[SerializeField] private float WEAK = 25.0f;
	[SerializeField] private BulletHell bulletHell;
	[SerializeField] private RingleaderNormalAttackHazard attackHazard;
	[SerializeField] private FireballLionSpawn fireballLionSpawn;

	[SerializeField] private List<Transform> positions;
	private Transform playerTransform = null;
	private Vector2 previousLionTarget;

	[SerializeField] private float normalAttackDelay = 1.0f;//might remove when animation is implemented
	[SerializeField] private float chargingTime = 5.0f;
	[SerializeField] private float bulletHellTime = 10.0f;
	[SerializeField] private float detonationTime = 10.0f;//needs to be more than the number of platform pieces
	[SerializeField] private float meleeAttackCooldown = 5.0f;
	[SerializeField] private float fireballCooldown = 5.0f;
	private Vector2 throwForce;
	[SerializeField] private int direction = -1;

	private PlayerYuni player;

	//Attacking Booleans
	private bool canAttack = true;

	private bool isUnleashingHell = false;
	private bool isCharging = false;
	private bool isOnEnemySpawnCooldown = false;
	private bool isDetonatingPlatforms = false;
	private bool isRestoringPlatforms = false;
	private bool isMeleeAttacking = false;
	private bool isMeleeAttackCooldown = false;
	private bool isFireballCooldown = false;
    private bool talkPause;
	void Awake() {
		ringleaderMovement = GetComponent<RingleaderMovement> ();
		previousLionTarget = this.transform.position;
		enemyHealth = GetComponent<EnemyHealth> ();
	}

	void Start() {
		gameObject.tag = "Enemy";
//		Random.InitState (17398);
//		direction *= (int) (transform.localScale.x / Mathf.Abs (transform.localScale.x));
		Debug.Log ("Start Direction: " + direction);
		throwForce = new Vector2 (WEAK * direction, WEAK);
//		Vector2 throwDirection = new Vector2 (throwForce.x * -direction, throwForce.y);
//		throwForce = throwDirection;


		this.ringleaderAnimatable = gameObject.GetComponent<RingleaderAnimatable> ();
		General.CheckIfNull (ringleaderAnimatable, "ringleaderAnimatable", scriptName);

	}

	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GameObject.FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}
    public void TalkPause() {
        this.talkPause = true;
    }
    public void Resume() {
        this.talkPause = false;
    }
    void Update() {
        if (!talkPause) {
//		direction *= (int) (transform.localScale.x / Mathf.Abs (transform.localScale.x));
//		Debug.Log ("Direction: " + direction);
		Random.Range (0, 10);

//		if (!enemyHealth.isAlive) {
//			StopAllCoroutines ();
//			enemyHealth.CallDeath (0);
//		} else {
		if (!enemyHealth.isAlive) {
			// TODO
		} else {
			if (this.GetPlayer() != null) {
				playerTransform = this.GetPlayer().transform;
			} else
				playerTransform = null;

			if (canAttack)
				Attack ();
		}
            //		}

        }
    }

//	public void FaceOtherDirection() {
//		Vector2 scale = new Vector2 (-transform.localScale.x, transform.localScale.y);
//		transform.localScale = scale;
//		direction *= (int) (transform.localScale.x / Mathf.Abs (transform.localScale.x));
//		attackHazard.transform.localScale = new Vector2 (attackHazard.transform.localScale.x * direction, 1);
////		Vector2 throwDirection = new Vector2 (throwForce.x * -direction, throwForce.y);
////		throwForce = throwDirection;
//	}

	public override void Attack() {
		if (!isMeleeAttacking && !isMeleeAttackCooldown && playerTransform != null && !ringleaderMovement.IsTeleporting ()) {
			ringleaderMovement.canMove = true;
		} else
			ringleaderMovement.canMove = false;

		if (gambit == Gambit.ROBIN_HOOD) {

			ringleaderMovement.MoveTowards (positions [2].position);

			if (!isMeleeAttackCooldown && !isMeleeAttacking && !ringleaderMovement.IsTeleporting() && (Vector3.Distance(transform.position, positions[2].position) < 1.0f)) {
				ringleaderMovement.TeleportToPlayer ();
				StartCoroutine (MeleeAttacking ());
			}

			if (!isMeleeAttacking && isCharging && !isOnEnemySpawnCooldown) {
				StartCoroutine (SpawnEnemyInterval ());

				int spawnIndex = Random.Range (0, spawnPoints.Count);
				int throwStrength = Random.Range (0, 3);

				switch (throwStrength) {
				case 0:
					throwForce = new Vector2 (WEAK * direction, WEAK);
					Debug.Log ("WACK");
					break;
				case 1:
					throwForce = new Vector2 (MEDIUM * direction, MEDIUM);
					Debug.Log ("MEDI");
					break;
				case 2:
					throwForce = new Vector2 (STRONG * direction, STRONG);
					Debug.Log ("STRONK");
					break;
				}

				Debug.Log ("Spawn Index: " + spawnIndex);
				GameObject enemy = spawnPoints [spawnIndex].SpawnEnemy (EnemyList.EnemyType.POPCORN, spawnPoints [spawnIndex].transform.position);
				enemy.transform.localScale = new Vector2 (direction * enemy.transform.localScale.x, enemy.transform.localScale.y);
				enemy.gameObject.GetComponent<Rigidbody2D> ().AddForce (throwForce);
			}

			if (!isDetonatingPlatforms && !isRestoringPlatforms && !isCharging) {
				StartCoroutine (DetonatingPlatforms ());
				int detonationMode = Random.Range (0, 3);
				Parameters parameters = new Parameters ();

				switch (detonationMode) {
				case 0:
					parameters.PutExtra (DetonatingPlatform.DETONATION, DetonatingPlatform.Detonation_Mode.LEFT_TO_RIGHT);
					break;
				case 1:
					parameters.PutExtra (DetonatingPlatform.DETONATION, DetonatingPlatform.Detonation_Mode.RIGHT_TO_LEFT);
					break;
				case 2:
					parameters.PutExtra (DetonatingPlatform.DETONATION, DetonatingPlatform.Detonation_Mode.EITHER_SIDES);
					break;
				}

				EventBroadcaster.Instance.PostEvent (EventNames.INITIATE_PLATFORM_DETONATION, parameters);
			}

		} else if (gambit == Gambit.RIDING_HOOD) {
			Debug.Log ("POS COUNT RIDING HOOD "+positions.Count);
			Vector3 target = positions [Random.Range (0, positions.Count)].position;

			if (!isFireballCooldown) {
				StartCoroutine (FireballCooldown ());

				fireballLionSpawn.SetDamage (this.damage);
				fireballLionSpawn.SendLionTo (target);
				previousLionTarget = target;
			}

			if (!isMeleeAttacking && !isMeleeAttackCooldown && playerTransform != null && !ringleaderMovement.IsTeleporting ()) {
				ringleaderMovement.MoveTowards (playerTransform.position);

			if (!isMeleeAttacking && !isMeleeAttackCooldown && ringleaderMovement.IsNearTarget () && !ringleaderMovement.IsTeleporting ())
				StartCoroutine (MeleeAttacking ());

//				attackHazard.TriggerDamage (this.damage);
			}

		} else if (gambit == Gambit.ALICE) {
//			if (!isUnleashingHell && !isCharging) {
//				StartCoroutine (BulletHellTime ());
//
//				bulletHell.SetDamage (damage);
//				bulletHell.SetTarget (playerTransform);
//				List<int> denominators = new List<int> ();
//
//				switch (operationType) {
//				case YarnballControl.OperationType.DISSIMILAR:
//					List<int> possibleDenominators = FractionsReference.Instance ().RequestDenominators ();
//
//					//NOTE: commented out due to error after EnemyHealth was updated
////					foreach (int denominator in possibleDenominators) {
////						if (denominator != enemyHealth.GetDenominator ())
////							denominators.Add (denominator);
////					}
//					break;
//				case YarnballControl.OperationType.SIMILAR:
//					//NOTE: commented out due to error after EnemyHealth was updated
////					denominators.Add (enemyHealth.GetDenominator ());
//					break;
//				case YarnballControl.OperationType.MIXED:
//					
//					break;
//				}
//
//				bulletHell.SetValidDenominators (denominators);
//
//				int hellStyle = Random.Range (0, 2);
//				int spinDirection = Random.Range (0, 2);
//
//				switch (hellStyle) {
//				case 0:
//					bulletHell.SetStyle (BulletHell.HellStyle.SPIRALING_SHOTS);
//					break;
//				case 1:
//					bulletHell.SetStyle (BulletHell.HellStyle.STRAIGHT_SHOTS);
//					break;
//				}
//
//				switch (spinDirection) {
//				case 0:
//					bulletHell.IsCounterClockwise (true);
//					break;
//				case 1:
//					bulletHell.IsCounterClockwise (false);
//					break;
//				}
//				Debug.Log ("Bullet Helling");
//				bulletHell.BeginHell ();
//			}
		}
	}

	private IEnumerator FireballCooldown() {
		isFireballCooldown = true;
		yield return new WaitForSeconds (fireballCooldown);
		isFireballCooldown = false;
	}

	private IEnumerator MeleeAttacking() {
		isMeleeAttacking = true;
		ringleaderMovement.canMove = false;

//		ringleaderMovement.TeleportToPlayer ();
		while (ringleaderMovement.IsTeleporting ())
			yield return null;

		this.ringleaderAnimatable.MeleeOpen();
		while (ringleaderAnimatable.IsPlaying ()) {
			yield return null;
		}

		attackHazard.TriggerDamage (this.damage);

		isMeleeAttacking = false;

		StartCoroutine (MeleeAttackCooldown ());
	}

	private IEnumerator MeleeAttackCooldown () {
		isMeleeAttackCooldown = true;
		yield return new WaitForSeconds (meleeAttackCooldown);
		isMeleeAttackCooldown = false;
		ringleaderMovement.canMove = true;
		if (gambit == Gambit.RIDING_HOOD)
			ringleaderMovement.TeleportTo (previousLionTarget);
	}

	private IEnumerator BulletHellTime() {
		this.isUnleashingHell = true;
		yield return new WaitForSeconds (bulletHellTime);
		this.isUnleashingHell = false;
		bulletHell.EndHell ();

		StartCoroutine (Charging ());
	}

	private IEnumerator Charging() {
		this.isCharging = true;
		yield return new WaitForSeconds (chargingTime);
		this.isCharging = false;
	}

	private IEnumerator SpawnEnemyInterval() {
		this.isOnEnemySpawnCooldown = true;
		yield return new WaitForSeconds (spawnEnemyInterval);
		this.isOnEnemySpawnCooldown = false;
	}

	private IEnumerator DetonatingPlatforms() {
		isDetonatingPlatforms = true;
		yield return new WaitForSeconds (detonationTime);
		isDetonatingPlatforms = false;

		StartCoroutine (ReplenishingPlatforms ());
	}

	private IEnumerator ReplenishingPlatforms() {
		EventBroadcaster.Instance.PostEvent (EventNames.REPLENISH_DETONATED_PLATFORMS);

		isRestoringPlatforms = true;
		yield return new WaitForSeconds (detonationTime);
		isRestoringPlatforms = false;

		StartCoroutine (Charging ());
	}
}
