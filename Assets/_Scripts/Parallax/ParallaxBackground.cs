using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

	public float backgroundSize;
	public float parallaxSpeed;
	public bool isParallax;
	public bool isScrolling;

	private Transform cameraTransform;
	private Transform[] layers;
	private float viewZone = 10;
	private int leftIndex;
	private int rightIndex;

	private float lastCameraX;

	private void Start() {
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
		layers = new Transform[transform.childCount];

		for(int i = 0; i < transform.childCount; i++)
			layers[i] = transform.GetChild(i);

		leftIndex = 0;
		rightIndex = layers.Length - 1;
		isParallax = true;
		isScrolling = true;
	}

	private void Update() {
		if (isParallax) {
			float deltaX = cameraTransform.position.x - lastCameraX;
			float originalY = transform.position.y;
			transform.position += Vector3.right * (deltaX * parallaxSpeed);
			transform.position = new Vector2(transform.position.x, originalY);
		}

		lastCameraX = cameraTransform.position.x;

		if (isScrolling) {
			if (cameraTransform.position.x < (layers [leftIndex].transform.position.x + viewZone))
				scrollLeft ();
		
			if (cameraTransform.position.x > (layers [rightIndex].transform.position.x - viewZone))
				scrollRight ();
		}
	}

	private void scrollLeft() {
		int lastRight = rightIndex;
		float y = layers [rightIndex].position.y;

		layers [rightIndex].position = Vector3.right * (layers [leftIndex].position.x - backgroundSize);
		layers [rightIndex].position = new Vector2 (layers [rightIndex].position.x, y);

		leftIndex = rightIndex;
		rightIndex--;
		if (rightIndex < 0)
			rightIndex = layers.Length - 1;
	}

	private void scrollRight() {
		int lastRight = leftIndex;

		float y = layers [leftIndex].position.y;

		layers [leftIndex].position = Vector3.right * (layers [rightIndex].position.x + backgroundSize);
		layers [leftIndex].position = new Vector2 (layers [rightIndex].position.x, y);

		rightIndex = leftIndex;
		leftIndex++;
		if (leftIndex == layers.Length)
			leftIndex = 0;
	}

}
