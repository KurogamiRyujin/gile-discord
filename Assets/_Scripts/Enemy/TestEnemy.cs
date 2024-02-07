using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : BaseEnemy, Puppet {

	public float movementSpeed = 5.0f;

	protected override void Init ()
	{
		base.Init ();
		//movementSpeed = 5.0f;

		aiController.ReceiveNewPuppet (this);
	}

	public void Obey() {
		if (player != null) {
//			rb.transform.Translate (player.transform.position * movementSpeed * Time.deltaTime);
			Vector2 targetPos = new Vector2(player.transform.position.x, rb.position.y);
			Vector2 moveTowards = Vector2.MoveTowards(rb.position, targetPos, movementSpeed * Time.deltaTime);

			rb.MovePosition (moveTowards);
		}
	}

}
