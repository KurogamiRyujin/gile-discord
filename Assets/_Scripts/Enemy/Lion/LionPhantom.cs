using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionPhantom : MonoBehaviour {

	[SerializeField] private float speed = 10.0f;
	[SerializeField] private int damage = 0;
	[SerializeField] private float cooldown = 3.0f;

	private Vector2 target;
	private bool isTargetSet = false;

	private int counter = 0;
	private float originalX;
	private float originalY;
	private int direction = 1;
	private bool isCooldown = false;

	void Start() {
		originalX = transform.position.x;
		originalY = transform.position.y;
		direction *= (int) (transform.localScale.x / Mathf.Abs (transform.localScale.x));
	}

	// Update is called once per frame
	void Update () {
		direction *= (int)(transform.localScale.x / Mathf.Abs (transform.localScale.x));
		if (isTargetSet) {
			ChargeToward ();
		} else {
			ChargeForward ();
		}
		counter++;
	}

	public void FaceOtherDirection() {
		Vector2 scale = new Vector2 (-transform.localScale.x, transform.localScale.y);
		transform.localScale = scale;
	}

	private void ChargeForward() {
		this.transform.position = new Vector2 (originalX + counter * Time.deltaTime * direction, originalY + (Mathf.Sin (counter * Time.deltaTime)));
	}

	private void ChargeToward() {
		float step = speed * Time.deltaTime;
		this.transform.position = Vector2.MoveTowards (this.transform.position, target, step);

		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		if (pos == target)
			Death ();
	}

	private void Death() {
		Destroy (this.gameObject);
	}

	public void SetTarget(Vector2 target) {
		if (this.transform.position.x < target.x && this.transform.localScale.x > 0)
			FaceOtherDirection ();

		this.target = target;

		this.isTargetSet = true;
	}

	public void SetDamage(int damage) {
		this.damage = damage;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			PlayerHealth health = other.gameObject.GetComponent<PlayerHealth> ();

			if(!isCooldown)
				health.Damage (this.damage);
		}
	}

	private IEnumerator Cooldown() {
		isCooldown = true;
		yield return new WaitForSeconds (cooldown);
		isCooldown = false;
	}
}
