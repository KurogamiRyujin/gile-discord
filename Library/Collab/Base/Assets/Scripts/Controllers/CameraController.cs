using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform player;
	// Maintain the difference of the camera y and the player y
	public float yOffset;


	void Start () {
		
	}

	void Update () {
		transform.position = new Vector3(player.position.x,
										player.position.y+yOffset,
										transform.position.z);
	}
}
