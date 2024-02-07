using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Transform player;
	// Maintain the difference of the camera y and the player y
	public float yOffset;


	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player").transform;
		} else {
			transform.position = new Vector3(player.position.x,
				player.position.y+yOffset,
				transform.position.z);
		}
	}
}
