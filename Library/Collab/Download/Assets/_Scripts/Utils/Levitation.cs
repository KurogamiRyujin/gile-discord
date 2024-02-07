using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : MonoBehaviour {

	public float bobDistance = 0.09f; //distance of the bobbing

	private Vector2 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = new Vector2 (this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y);
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.localPosition = new Vector2 (startPosition.x, startPosition.y + (Mathf.Sin (Time.time) * bobDistance));
		this.gameObject.transform.eulerAngles = new Vector3 (0, 0, 42.923f);

	}
}
