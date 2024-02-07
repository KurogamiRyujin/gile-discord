using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LCDYarnball : MonoBehaviour {

	[SerializeField] private Text yarnballText;
	private LineRenderer yarnThread;
	private ParticleEffect threadEffect;
	private Vector3 threadSource;
	private bool runThreadEffect;
	private float effectSpeed = 15f;
	private float timeout = 0.5f;

	private float startTime = 0f;
	private GameObject thread;

	private Vector3 threadTarget;
	void Awake() {
		yarnThread = GetComponent<LineRenderer> ();
	}

	public void SetLCD(int lcd) {
		this.yarnballText.text = lcd.ToString ();
	}

	public Vector3 GetThreadSource() {
		if (this.threadSource == null) {
			this.threadSource = this.threadSource = GetComponentInChildren<LCDThreadSource>().gameObject.transform.position;
		}
		return this.threadSource;
	}

	public GameObject GetThread() {
		if(this.thread == null) {
			this.thread = GetThreadEffect ().gameObject;
		}
		return this.thread;
	}

	public ParticleEffect GetThreadEffect() {
		if (this.threadEffect == null) {
			this.threadEffect = GetComponentInChildren<ParticleEffect> ();
		}
		return this.threadEffect;
	}

	public void ThreadTowards(Vector3 target) {
		this.threadEffect = GetComponentInChildren<ParticleEffect> ();
		this.thread = GetThreadEffect().gameObject;
		this.threadSource = GetComponentInChildren<LCDThreadSource>().gameObject.transform.position;


		Vector3[] points = new Vector3[2];
		points [0] = this.gameObject.transform.position;
		points [1] = target;

		yarnThread.SetPositions (points);
	}

	public IEnumerator ThreadEffects(Vector3 target) {
//		this.GetThreadEffect().Play ();
		ThreadTowards (target);
		GetThread().transform.position = this.GetThreadSource();
		this.threadTarget = target;
		Vector3 targetDir;
		Vector3 newRotation = Vector3.zero;
		Vector3 prevRotation = newRotation;

		do {
			Physics2D.Simulate (effectSpeed * 2 * Time.unscaledDeltaTime);
			prevRotation = newRotation;
			targetDir = target - GetThread ().transform.position;
			newRotation = Vector3.RotateTowards (GetThread ().transform.forward, targetDir, 2 * effectSpeed * Time.unscaledDeltaTime, 0.0f);

			this.GetThread ().transform.rotation = Quaternion.LookRotation (newRotation);

			Debug.Log ("ROTATION " + newRotation);
			Debug.Log ("OBJ ROTATION " + this.GetThread ().transform.rotation);

			Debug.DrawRay (GetThread ().transform.position, targetDir, Color.red);

			yield return null;
		} while (Mathf.Abs((float)(prevRotation.x - newRotation.x)) > 0.01f);//GetThread ().transform.rotation.x != Quaternion.LookRotation (targetDir).x);
		this.startTime = Time.unscaledDeltaTime;
		while (Vector2.Distance(GetThread().transform.position, target) > 0.01f && !Timeout()) {

			Debug.Log ("In Timout");
			Physics2D.Simulate (effectSpeed * Time.unscaledDeltaTime);
			this.GetThread().transform.position = Vector2.MoveTowards(GetThread().transform.position, target, effectSpeed * Time.unscaledDeltaTime);
			yield return null;
		}
		SoundManager.Instance.Play (AudibleNames.LCDInterface.SYNC, false);
//		this.GetThreadEffect().Stop ();
	}
	public bool Timeout() {
		if (Time.unscaledDeltaTime > this.startTime+timeout) {
			return  true;
		}
		return false;
	}
}
