using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPointManager : MonoBehaviour {

	private List<SpawnPoint> spawnPoints = new List<SpawnPoint> ();//All Spawn Points in the Current Scene where player will come from
	public string previousScene;//NOTE: set this to private upon actual implementation
	public int previousDoorNumber;//NOTE: set this to private upon actual implementation
	private bool deathFlag;
	// Use this for initialization
	void Start () {
		this.deathFlag = false;
		//NOTE: uncomment this when spawning in Theatre (AKA initial stage)
		//previousScene = "None";
		//previousDoorNumber = 0;//0 denotes not coming from a door or an error
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		Debug.Log (scene.name);
		Debug.Log (mode.ToString());
		this.SearchSpawnPoints ();
		this.SpawnPlayer ();
	}

	private void SearchSpawnPoints() {
		this.spawnPoints.Clear ();//Remove Spawn Point List data of previous scene.

		//Seach the Scene for all Spawn Points
		foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("SpawnPoint")) {
			//then add them to Spawn Point List
			spawnPoints.Add (spawnPoint.GetComponent<SpawnPoint>());
		}
	}

	private void SpawnPlayer() {
		Debug.Log ("Player Spawned");
		bool spawnPointFound = false;

		Debug.Log ("Previous Scene: " + this.previousScene);
		Debug.Log ("Spawn Number: " + this.previousDoorNumber);

		foreach (SpawnPoint point in this.spawnPoints) {
			Debug.Log (point.comingFrom);
			Debug.Log (point.spawnNumber);

			if (this.previousScene == "None") {
				if (point.isDefault) {
					point.SpawnPlayer ();
					spawnPointFound = true;
				}
			} else if (point.comingFrom == this.previousScene) {
				if (point.spawnNumber == this.previousDoorNumber) {
					point.SpawnPlayer ();
					spawnPointFound = true;
				}
			}
		}

		if (!spawnPointFound) {
//			Debug.LogError ("Cannot Find Spawn Point!!!");
//			Debug.Log ("Closing App...");
//			Application.Quit ();

			if (deathFlag) {
				Destroy (gameObject);
			}

			deathFlag = true;
		}
	}

	public void RespawnPlayer() {
		this.SpawnPlayer ();
	}

	public void UpdatePreviousScene(string sceneBeforeEnteringNew, int doorNumber) {
		this.previousScene = sceneBeforeEnteringNew;
		this.previousDoorNumber = doorNumber;
		Debug.Log ("Coming From " + this.previousScene);
	}
}
