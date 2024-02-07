using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnballProjectile : MonoBehaviour {

	private float MAX_PROJECTILE_DISTANCE = 20.0f;
	private float MAX_PROJECTILE_LIFE = 10.0f;
	private float speed = 1.5f;

	private float timeStart;

	[SerializeField] private Vector2 target;
	private Vector2 origin;
	private int denominator;
	private Yarnball sourceYarnball;
	private HammerObject hammer;
//	private PauseController pauseController;

	// Use this for initialization
	void Awake () {
		origin = transform.position;
//		pauseController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PauseController> ();
	}
	
	// Update is called once per frame
	void Update () {
//		float step = speed * Time.deltaTime;
//		this.transform.position = Vector2.MoveTowards (this.transform.position, this.target, step);


		if ((Vector3.Distance (this.transform.position, target) >= -0.5f && 
			Vector3.Distance (this.transform.position, target) <= 0.5f) 
			//|| (Vector3.Distance (this.transform.position, origin) >= MAX_PROJECTILE_DISTANCE)
		    || ((Time.time - this.timeStart) > MAX_PROJECTILE_LIFE)) {
			Expire ();
		}
		else {
			Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();
			if (rigidbody != null) {
				rigidbody.AddForce ((new Vector3(target.x, target.y)-this.transform.position).normalized*0.2f*Time.smoothDeltaTime);
			}
		}
	}

	public void Propel() {
		this.timeStart = Time.time;
//		Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();
//		if (rigidbody != null) {
//			rigidbody.AddForce ((new Vector3(target.x, target.y)-this.transform.position).normalized*5000*Time.smoothDeltaTime);
////			transform.LookAt(target);
//		}
	}

	private void Expire() {
		Destroy (this.gameObject);
	}

	public void SetTarget(Vector2 target) {
		this.target = target;
	}

	public void SetDenominator(int denominator) {
		this.denominator = denominator;
	}

	public void SetSource(Yarnball source) {
		this.sourceYarnball = source;
	}

	public void SetHammer(HammerObject hammer) {
		this.hammer = hammer;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.GetComponent<EnemyHealth>() != null) {
			EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth> ();
			Debug.Log ("Yarnball Denominator: " + this.denominator);
//			enemyHealth.LCD (this.denominator, this.sourceYarnball);

			//NOTE: commented out due to error after EnemyHealth was updated
//			enemyHealth.LCD (this.denominator, this.hammer);

			#if UNITY_ANDROID
//			EventManager.DisableJumpButton ();
//			EventManager.ToggleSwitchButton (false);
////			EventManager.Instance.ToggleLeftRightButtons (false);
//			GameController_v7.Instance.GetEventManager().ToggleLeftRightButtons(false);

			Parameters parameters = new Parameters();
			parameters.PutExtra("FLAG", false);
			EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_JUMP, parameters);

			parameters = new Parameters();
			parameters.PutExtra("FLAG", true);
			EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_INTERACT, parameters);

			parameters = new Parameters ();
			parameters.PutExtra ("FLAG", false);
			EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, parameters);
			EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, parameters);
			#endif

			Expire ();
		}
//		if (other.gameObject.CompareTag ("Ground")) {
//			Expire ();
//		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			Expire ();
		}
//		if (!(other.gameObject.CompareTag ("Hammer Child") || other.gameObject.CompareTag ("Player"))) {
//			Expire ();
//		}
	}
}
