using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	[SerializeField] protected EnemyHealth enemyHealth;
	[SerializeField] protected EnemyAttack enemyAttack;
	[SerializeField] protected EnemyMovement enemyMovement;

	[SerializeField] protected PlayerYuni player;

	protected bool isIdle = false;

	protected virtual void Start() {
		Debug.Log ("INIT");
		player = GameObject.FindObjectOfType<PlayerYuni>();

		this.enemyHealth = GetComponent<EnemyHealth> ();
		this.enemyAttack = GetComponent<EnemyAttack> ();
		this.enemyMovement = GetComponent<EnemyMovement> ();
		Debug.Log ("<color=red>INIT"+this.enemyHealth+"</color>");
	}

	public bool IsFacingRight() {
		return this.enemyMovement.IsFacingRight ();
	}

	public bool IsIdle() {
		return this.isIdle;
	}

	public void SetIdle(bool idle) {
		this.isIdle = idle;
	}

	public bool IsActive() {
		if (enemyHealth != null) {
			return (!isIdle && enemyHealth.isAlive);
		}
		else {
			return false;
		}
	}

	public PlayerYuni GetPlayer() {
		return this.player;
	}
}
