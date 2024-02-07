using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathManager : MonoBehaviour {

	public int respawnTimer = 3;

	private SpawnPointManager spawnPointManager;
	private CameraController cameraController;

	// Use this for initialization
	void Start () {
		spawnPointManager = GetComponent<SpawnPointManager> ();
		cameraController = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController> ();
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		cameraController = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController> ();
	}
	
	public void PlayerDeath() {
		//Do any fancy stuff here before coroutine
		if(cameraController != null)
			cameraController.enabled = false;
		StartCoroutine (RespawnPlayerIn (respawnTimer));
	}

	private IEnumerator RespawnPlayerIn(int seconds) {
		//handle how to notify player
		for (int i = 0; i < seconds; i++) {
			yield return new WaitForSeconds (1.0f);
		}

		spawnPointManager.RespawnPlayer ();
		cameraController.enabled = true;
	}
}
