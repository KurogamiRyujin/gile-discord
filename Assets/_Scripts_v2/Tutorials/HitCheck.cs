using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCheck : MonoBehaviour {

	[SerializeField] private EnemyHealth enemy;
	public bool isHit = false;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<HammerChildCollisionDetection>() != null) {
			this.isHit = true;
		}
	}
}
