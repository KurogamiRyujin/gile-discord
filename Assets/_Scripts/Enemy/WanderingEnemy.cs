using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemy : BaseEnemy, Puppet {

	public float movementSpeed = 5.0f;
	public float wanderRange = 5.0f;
	public float wanderLength = 5.0f;
	public int idleLength = 1;

	private Vector2 startingPos;
	private bool isWandering;

	protected override void Init ()
	{
		base.Init ();

		startingPos = transform.position;
		aiController.ReceiveNewPuppet (this);
	}

	private IEnumerator Wander() {
		Vector2 target = new Vector2 (startingPos.x-wanderRange, transform.position.y);

		while (transform.position.x > target.x) {
			Vector2 moveTowards = Vector2.MoveTowards(rb.position, target, movementSpeed * Time.deltaTime);

			rb.MovePosition (moveTowards);
//			transform.Translate (target);
			yield return null;
		}

		target = new Vector2 (startingPos.x + wanderRange, transform.position.y);

		while (transform.position.x < target.x) {
			Vector2 moveTowards = Vector2.MoveTowards(rb.position, target, movementSpeed * Time.deltaTime);

			rb.MovePosition (moveTowards);
//			transform.Translate(target);
			yield return null;
		}

//		for (int i = 0; i < idleLength; i++) {
//			yield return new WaitForSeconds (1.0f);
//		}

		this.isWandering = false;
	}

	public void Obey() {
		if (!isWandering) {
			this.isWandering = true;
			StartCoroutine (Wander ());
		}
	}
}
