using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardScript : MonoBehaviour {

	private TimerScript timerScript;

	// Use this for initialization
	void Start () {
		timerScript = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TimerScript> ();
		timerScript.stop ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
