using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLionSpawn : MonoBehaviour {

	[SerializeField] private LionPhantom lionPrefab;

	private int damage;

	public void SendLionTo(Vector3 pos) {
		LionPhantom lion = Instantiate (lionPrefab, gameObject.transform.position, Quaternion.identity);

		lion.SetDamage (this.damage);
		lion.SetTarget (pos);
	}

	public void SetDamage(int damage) {
		this.damage = damage;
	}
}
