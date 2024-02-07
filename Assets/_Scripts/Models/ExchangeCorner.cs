using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeCorner : MonoBehaviour {

	private Camera playerCam;

	public GameObject machineCloseUp;
	public GameObject machineInput;

	// Use this for initialization
	void Start () {
		playerCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics2D.Raycast (new Vector2 (playerCam.ScreenToWorldPoint (Input.mousePosition).x, playerCam.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero)) {
			if (Input.GetMouseButtonDown (0)) {//Left Click
				Debug.Log ("Hit");
				machineCloseUp.SetActive (true);
				GameObject clone = Instantiate (machineInput);
			}
		}
	}
}
