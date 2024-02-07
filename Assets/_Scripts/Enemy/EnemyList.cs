using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour {

	public enum EnemyType {
		POPCORN,
		LION,
		LION_DOLL,
	}

	[SerializeField] private GameObject popcorn;
	[SerializeField] private GameObject lion;
	[SerializeField] private GameObject lionDoll;

	public GameObject RequestEnemy(EnemyType name) {
		GameObject enemy = null;

		switch (name) {
		case EnemyType.POPCORN:
			enemy = Instantiate (popcorn);
			break;
		case EnemyType.LION:
			enemy = Instantiate (lion);
			break;
		case EnemyType.LION_DOLL:
			enemy = Instantiate (lionDoll);
			break;
		default:
			enemy = Instantiate (popcorn);
			Debug.Log ("<color=red>Error: No such enemy exists within database so have a popcorn!</color>");
			break;
		}

		return enemy;
	}

}
