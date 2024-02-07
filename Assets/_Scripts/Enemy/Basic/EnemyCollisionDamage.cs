using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionDamage : MonoBehaviour {

	[SerializeField] private float CONSTANT_DAMAGE_INTERVAL = 1.0f;
	[SerializeField] private int CONSTANT_COLLISION_DAMAGE = 1;

	[SerializeField] private Collider2D damageCollider;
	[SerializeField] private float damageTriggerTime = 0.0f;
	private float timer;
	private bool canDamage;
	void Start () {
		this.timer = CONSTANT_DAMAGE_INTERVAL;
		this.canDamage = true;
	}

	void Update() {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			this.canDamage = true;
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth> ();
			this.ConstantDamage (playerHealth);
		}
	}
	public void ConstantDamage(PlayerHealth playerHealth) {
		if (this.canDamage) {
			this.canDamage = false;
			playerHealth.Damage (CONSTANT_COLLISION_DAMAGE);
			Debug.Log ("<color=cyan><b>PLAYER DAMAGED</b></color>");
			this.timer = CONSTANT_DAMAGE_INTERVAL;
		}
	
	}

}
