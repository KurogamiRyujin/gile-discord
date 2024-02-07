using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {
	[SerializeField]
	Sprite[] hearts;
	[SerializeField]
	Image heartUI;

	PlayerPlatformerController player;

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerPlatformerController> ();
	}

	void Update() {
		
	}
}
