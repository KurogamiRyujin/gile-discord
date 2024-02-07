using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

	public string goingTo;
	public int doorNumber;
	public bool isTangible;

	private SpawnPointManager spawnPointManager;

	// Use this for initialization
	void Start () {
		spawnPointManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SpawnPointManager> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(isTangible && other.gameObject.CompareTag("Player")) {
//			other.gameObject.GetComponent<PlayerManager> ().SetExitStandingIn (this);
			Debug.Log ("Player at Exit");
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
//			other.gameObject.GetComponent<PlayerManager> ().SetExitStandingIn (null);
			Debug.Log ("Player left exit...");
		}
	}

	public void Enter() {
		Debug.Log ("Player Went In " + goingTo + " " + doorNumber);

		spawnPointManager.UpdatePreviousScene (SceneManager.GetActiveScene ().name, this.doorNumber);
		//Load Scene Connected to this entrance.
		SceneManager.LoadScene (goingTo);
	}
}
