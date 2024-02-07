using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Gear_MainMenu : MonoBehaviour {

	public bool isOn = true;
	public float gearSpeed = (float)0.05;
	public bool isRight = true;

	private int direction;

	void Start() {
		// Set the direction of the rotation
		this.setDirection ();
	}

	void Update () {
		// If user allows this to be active
		if (this.isOn) {
			// Rotate the game object owner of this script
			gameObject.transform.Rotate (0, 0, this.direction*this.gearSpeed);
		}
	}

	void setDirection() {
		if (this.isRight)
			this.direction = 1;
		else
			this.direction = -1;
	}
}
