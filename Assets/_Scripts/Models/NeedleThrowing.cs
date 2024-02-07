using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleThrowing : MonoBehaviour {
	public const string scriptName = "NeedleThrowing";

	[SerializeField] private const int HOOK_PULL_FORCE = 600;
	private const float extendedDestinationThreshold = 2.0f;
	[SerializeField] float needleReturnRange = 10.0f;
	[SerializeField] ParticleSystem effectNeedleThrow;

	private Transform playerTransform;
	private Animator playerAnimator;

	private Transform targetTransform;
	private SpriteRenderer spriteRenderer;
	private SpriteRenderer playerSpriteRenderer;
	private Collider2D needleCollider;
	private LineRenderer threadRenderer;
	private NeedleController needleController;
	private PlayerMovement playerMovement;

	private PlayerAttack playerAttack;
	private Vector3 needleTargetTransform;

	public float throwSpeed = 20.0f;
	public bool flying;

	protected bool attack = false;
	protected bool pullTowards;
	protected bool hookPullTowards = false;
	protected bool inFlightCoroutineRunning = false;
	protected Vector3 needleExtendedTransform;
	[SerializeField] private float hookPullSpeed = 20.0f;

	public bool isPause;


	private PlayerYuni playerYuni;
	// Use this for initialization
	void Awake () {
		playerTransform = GameObject.FindObjectOfType<PlayerYuni> ().gameObject.transform; // GameObject.FindGameObjectWithTag ("Player").transform;
		playerAnimator = playerTransform.gameObject.GetComponent<Animator> ();
		playerSpriteRenderer = playerTransform.gameObject.GetComponent<SpriteRenderer> ();
		this.playerAttack = playerTransform.gameObject.GetComponent<PlayerAttack> ();

		spriteRenderer = GetComponent<SpriteRenderer> ();
		needleCollider = GetComponent<Collider2D> ();
		threadRenderer = GetComponent<LineRenderer> ();
		needleController = GetComponent<NeedleController> ();

		needleExtendedTransform = needleController.gameObject.GetComponentInChildren<Transform> ().position;

		// NEW @Candy
		this.playerYuni = GameObject.FindObjectOfType<PlayerYuni>();
		this.playerMovement = playerYuni.GetPlayerMovement ();

			

		needleCollider.enabled = false;
		flying = false;
		pullTowards = false;

		GameController_v7.Instance.GetPauseController ().onContinueGame += NeedleUpdate;
		GameController_v7.Instance.GetPauseController ().onPauseGame += Pause;
		GameController_v7.Instance.GetPauseController ().onContinueGame += Continue;
	}

	public void Pause() {
		this.isPause = true;
	}

	void OnDestroy() {
		GameController_v7.Instance.GetPauseController ().onContinueGame -= NeedleUpdate;
		GameController_v7.Instance.GetPauseController ().onPauseGame -= Pause;
		GameController_v7.Instance.GetPauseController ().onContinueGame -= Continue;
	}
	public void Continue() {
		this.isPause = false;
	}
	void FixedUpdate() {
		if (!isPause) { // TODO Needle Pause
			if (!flying) {
				transform.position = playerTransform.position;
				spriteRenderer.enabled = false;
			}
		}
	}

	void NeedleUpdate() {
		if (!flying) {
			transform.position = playerTransform.position;
			spriteRenderer.enabled = false;
		}
	}

	// Throw needle is called in attack animator
	public void Throw(Vector3 pos) {
		if (!inFlightCoroutineRunning) {
			this.needleTargetTransform = pos;
			this.needleTargetTransform.z = 0.0f;
			rotate ((Vector2)pos);

			attack = true;
			playerAnimator.SetBool ("attack", attack);
			// Coroutine attack start and seperate attack end
//			playerAnimator.SetTrigger ("throwNeedle");

			// Note: calling InFlight is in ThrowNeedle() function which is called in PlayerAttack's ThrowNeedle() function
		}
	}
	// This is called in PlayerAttack ThrowNeedle which is called in an animation event (see Yuni's Attack_Throw animation in animator)
	public void ThrowNeedle () {
//		if (!flying && attack) {
		if (!inFlightCoroutineRunning) {
			this.effectNeedleThrow.Play ();
			Debug.Log ("IS CALLED");
			gameObject.tag = "Needle";
			StartCoroutine (InFlight (this.needleTargetTransform));
		}
	}
//	public void ThrowNeedle(Vector3 pos) {
//		if (!flying) {
//			Vector3 targetTransform = Camera.main.ScreenToWorldPoint (pos);
//			targetTransform.z = 0.0f;
//			rotate ();
//			StartCoroutine (InFlight (targetTransform));
//
//			attack = true;
//			playerAnimator.SetBool ("attack", attack);
//			playerAnimator.SetTrigger ("throwNeedle");
//
//			FaceThrowDirection (targetTransform);
//			AudioSource.PlayClipAtPoint (sfxFire, transform.position);
//		}
//	}


	public void FaceThrowDirection(Vector3 targetTransform) {
		if (targetTransform.x > playerTransform.transform.position.x && !playerMovement.m_FacingRight) {
			playerMovement.Flip ();
		}
		else if (targetTransform.x < playerTransform.transform.position.x && playerMovement.m_FacingRight) {
			General.CheckIfNull (playerMovement, "playerMovement", scriptName);
			playerMovement.Flip ();
//			playerSpriteRenderer.flipX = true;
		}
	}

	// For old player platformer, to remove @Candy
//	public void FaceThrowDirection(Vector3 targetTransform) {
//		if (targetTransform.x > playerTransform.transform.position.x) {
//			playerSpriteRenderer.flipX = false;
//		}
//		else {
//			playerSpriteRenderer.flipX = true;
//		}
//	}

	void rotate(Vector2 pos) {
		//Get the Screen positions of the object
		Vector2 positionOnScreen = transform.position;
		//Get the Screen position of the mouse
//		Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//Get the angle between the points
		float angle = AngleBetweenTwoPoints(positionOnScreen, pos);


		// Rotate
		transform.rotation =  Quaternion.Euler (new Vector3(0f, 0f, angle));
		//transform.position = new Vector3(transform.position.x+0.5f, y, transform.position.z);
	}

	float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}


	private void enablePlayerColliders(bool isEnabled) {
//		gameObject.GetComponent<Collider2D> ().enabled = isEnabled;
		needleCollider.enabled = isEnabled;
//		playerMovement.gameObject.GetComponent<Collider2D> ().enabled = isEnabled;
//		playerMovement.gameObject.GetComponent<PlayerAttack> ().getHammerChildCollider().enabled = isEnabled;

	}
	private IEnumerator InFlight(Vector3 target) {
		this.inFlightCoroutineRunning = true;
//		this.enablePlayerColliders (false); // TODO
		FaceThrowDirection (this.needleTargetTransform);
		SoundManager.Instance.Play (AudibleNames.Needle.BASIC, false);
		EventBroadcaster.Instance.PostEvent(EventNames.YUNI_THREW_NEEDLE);

		attack = false;
		playerAnimator.SetBool ("attack", attack);

		flying = true;
		spriteRenderer.enabled = true;
		needleCollider.enabled = true;

		threadRenderer.enabled = true;

		threadRenderer.positionCount = 1;
		threadRenderer.sortingLayerName = "Needle";
		threadRenderer.sortingOrder = (-1);
		threadRenderer.SetPosition (0, playerTransform.transform.position);
		threadRenderer.startColor = Color.cyan;
		threadRenderer.endColor = Color.cyan;

		// Change the needle's z to -4 to make it appear above the thread
		target = new Vector3 (target.x, target.y, -4);
		transform.position = new Vector3 (transform.position.x, transform.position.y, -4);

		int count = 1;


		// While needle has not hit
		while (transform.position != target && !needleController.hasHit) {

			FaceThrowDirection (target);
			transform.position = Vector3.MoveTowards (transform.position, target, throwSpeed * Time.deltaTime);
			threadRenderer.positionCount = count+1;

			threadRenderer.SetPosition (0, new Vector3(playerTransform.transform.position.x, 
				playerTransform.transform.position.y, 
				(float)-0.1));
			threadRenderer.SetPosition (count, new Vector3(transform.position.x, transform.position.y, (float)-0.1));

			threadRenderer.startColor = Color.cyan;
			threadRenderer.endColor = Color.cyan;



//			threadRenderer.sortingLayerName = "Needle";
//			threadRenderer.sortingOrder = (-1);
			yield return null;
		}


		this.enablePlayerColliders (true);

		while (this.isPause) {
			FaceThrowDirection (target);
			yield return null;
		}

		needleCollider.enabled = false;
		// Change the needle's z to -4 to make it appear above the thread
		playerTransform.position = new Vector3 (playerTransform.position.x, playerTransform.position.y, -4);
		transform.position = new Vector3 (transform.position.x, transform.position.y, -4);


		bool hasFaced = false;
		// pull Yuni towards needle if it has hit something and pullTowards was set to true
		if (needleController.hasHit && pullTowards) {

			playerTransform.GetComponent<Rigidbody2D> ().velocity = Vector2.zero; // TODO

			while (needleExtendedTransform != playerTransform.position &&
//			while (transform.position != playerTransform.position &&
				Vector3.Distance (playerTransform.position, needleExtendedTransform) > extendedDestinationThreshold) {

				Debug.Log ("Distance "+Vector3.Distance (playerTransform.position, needleExtendedTransform)+" ");
				Debug.Log ("Player pos "+playerTransform.position);
				Debug.Log ("Target pos "+needleExtendedTransform);

				if (!hasFaced) {
					FaceThrowDirection (target);
					hasFaced = true;
				}
				playerTransform.position = Vector3.MoveTowards (playerTransform.position, needleExtendedTransform/*needleExtendedTransform*/, throwSpeed * Time.deltaTime);

				threadRenderer.positionCount = count + 1;

				threadRenderer.SetPosition (0, playerTransform.transform.position);
				threadRenderer.SetPosition (count, transform.position);
				threadRenderer.SetPosition (0, new Vector3 (playerTransform.transform.position.x, 
					playerTransform.transform.position.y, 
					(float)-0.1));
				threadRenderer.SetPosition (count, new Vector3 (transform.position.x, transform.position.y, (float)-0.1));

				threadRenderer.startColor = Color.cyan;
				threadRenderer.endColor = Color.white;

				yield return null;
			}

			if (hookPullTowards) {
				Debug.Log ("ENTERED HOOK TOWARDS");
				playerTransform.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
				playerTransform.GetComponent<Rigidbody2D> ().AddForce ((needleExtendedTransform-playerTransform.position).normalized*HOOK_PULL_FORCE);
				//			playerTransform.gameObject.GetComponent<Rigidbody2D> ().AddForce ((needleExtendedTransform-playerTransform.position).normalized*800*Time.smoothDeltaTime);
				this.hookPullTowards = false;
				//			while(Vector3.Distance (playerTransform.position, needleExtendedTransform) > needleReturnRange) {
				//				Debug.Log ("Entered with distance: " + Vector3.Distance (playerTransform.position, needleExtendedTransform));
				//				playerTransform.position = Vector3.MoveTowards (playerTransform.position, needleExtendedTransform, hookPullSpeed * Time.deltaTime);
				//				yield return null;
				//			}
			}
		}

		// pull needle towards Yuni
		else {
			while (transform.position != playerTransform.position &&
				Vector3.Distance (transform.position, playerTransform.position) > needleReturnRange) {

				transform.position = Vector3.MoveTowards (transform.position, playerTransform.position, throwSpeed * Time.deltaTime);

				threadRenderer.positionCount = count+1;

				threadRenderer.SetPosition (0, playerTransform.transform.position);
				threadRenderer.SetPosition (count, transform.position);
				threadRenderer.SetPosition (0, new Vector3(playerTransform.transform.position.x, 
					playerTransform.transform.position.y, 
					(float)-0.1));
				threadRenderer.SetPosition (count, new Vector3(transform.position.x, transform.position.y, (float)-0.1));

				threadRenderer.startColor = Color.cyan;
				threadRenderer.endColor = Color.white;

				yield return null;
			}
		}


		needleController.hasHit = false;
		flying = false;
		spriteRenderer.enabled = false;
		threadRenderer.enabled = false;

		EventBroadcaster.Instance.PostEvent(EventNames.YUNI_ACQUIRED_NEEDLE);
		this.inFlightCoroutineRunning = false;
//		playerAnimator.SetBool ("attack", false);
	}

	public void setPullTowards(bool isTowards) {
		this.pullTowards = isTowards;
	}
	public void setHookPullTowards(bool isTowards) {
		this.hookPullTowards = isTowards;
	}

	public void setExtendedNeedleTransform(Vector3 extendedTransform) {
		this.needleExtendedTransform = extendedTransform;
	}
	public PlayerMovement getPlayerMovement() {
		return this.playerMovement;
	}
	public PlayerAttack getPlayerAttack() {
		return this.playerAttack;
	}
}
