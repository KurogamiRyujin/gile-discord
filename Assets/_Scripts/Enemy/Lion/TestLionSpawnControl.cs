using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLionSpawnControl : MonoBehaviour {

	[SerializeField] private FireballLionSpawn fireball;
	[SerializeField] private Vector2 target;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P))
			fireball.SendLionTo (target);
	}
}
