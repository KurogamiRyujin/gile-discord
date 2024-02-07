using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeadzone : MonoBehaviour {

	// Max value of x going to the right
	[SerializeField] Transform character;
	[SerializeField] float speedX = 3;
	[SerializeField] float speedY = 5;

	// Defines how far the character is from the x and y axes
	[SerializeField] float xDifference;
	[SerializeField] float yDifference;
	[SerializeField] float yOffset;

	// How far from the camera the character can get before the camera starts to follow
	[SerializeField] float movementThreshold = 3;
	private Vector3 moveTemp;
	private Camera camera;

	void Start() {

		camera = GameObject.FindObjectOfType<Camera> ();

	
	}

	void Update () {

		// Use this to get a positive number, don't use Math.abs
		if (character.transform.position.x > transform.position.x) {
			xDifference = character.transform.position.x - transform.position.x;
		}
		else {
			xDifference = transform.position.x - character.position.x;
		}
	
		if (character.transform.position.y > transform.position.y) {
			yDifference = character.transform.position.y - transform.position.y;
		}
		else {
			yDifference = transform.position.y - character.position.y;
		}


		if (xDifference >= movementThreshold) {
			moveTemp = character.transform.position;
			moveTemp.y = transform.position.y;

			moveTemp.z = transform.position.z;
			transform.position = Vector3.MoveTowards (transform.position, moveTemp, speedX * Time.deltaTime);

		}

		if (yDifference >= movementThreshold) {
			moveTemp.y = character.transform.position.y + yOffset; // 1

			moveTemp.z = transform.position.z;
			transform.position = Vector3.MoveTowards (transform.position, moveTemp, (speedY) * Time.deltaTime);

		}
	}
}
