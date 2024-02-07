using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	[SerializeField] private EnemyList enemyList;

	void OnEnable() {
		enemyList = GameObject.FindGameObjectWithTag ("GameController").GetComponent<EnemyList> ();
	}

	public GameObject SpawnEnemy(EnemyList.EnemyType name, Vector3 position) {
		GameObject enemy = enemyList.RequestEnemy (name);
		enemy.transform.position = position;

		return enemy;
	}
}
