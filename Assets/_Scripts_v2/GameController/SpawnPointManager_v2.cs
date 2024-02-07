using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages which spawn point the player avatar spawns.
/// </summary>
public class SpawnPointManager_v2 {

    /// <summary>
    /// List of spawn points found in the room.
    /// </summary>
	private List<SpawnPoint> spawnPoints = new List<SpawnPoint> ();//All Spawn Points in the Current Scene where player will come from
    /// <summary>
    /// Previous room the player came from.
    /// </summary>
    private string previousScene;//NOTE: set this to private upon actual implementation
    /// <summary>
    /// Door number the player went in from the previous room to get to the current room.
    /// </summary>
    private int previousDoorNumber;//NOTE: set this to private upon actual implementation

    /// <summary>
    /// Constructor
    /// </summary>
	public SpawnPointManager_v2() {
		spawnPoints = new List<SpawnPoint> ();
		this.previousScene = "None";
		this.previousDoorNumber = 0;

		EventBroadcaster.Instance.AddObserver (EventNames.RETRY, this.SpawnPlayer);
	}
    
    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy() {
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.RETRY, this.SpawnPlayer);
    }

    /// <summary>
    /// Refreshes and updates the spawn point list. Called whenever a room is loaded.
    /// </summary>
    public void SearchSpawnPoints() {
		this.spawnPoints.Clear ();//Remove Spawn Point List data of previous scene.

		//Seach the Scene for all Spawn Points
		foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("SpawnPoint")) {
			//then add them to Spawn Point List
			spawnPoints.Add (spawnPoint.GetComponent<SpawnPoint>());
		}
	}

    /// <summary>
    /// Prompts a spawn point to spawn the player on its location.
    /// </summary>
	public void SpawnPlayer() {
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
			} else {
				if (point.isDefault) {
					point.SpawnPlayer ();
					spawnPointFound = true;
				}
			}
		}
	}

    /// <summary>
    /// If applicable, calls the SpawnPlayer function when the player avatar has been eliminated either through a depleted health or other reasons.
    /// </summary>
	public void RespawnPlayer() {
		this.SpawnPlayer ();
	}

    /// <summary>
    /// When the player leaves a room, the manager takes note of the room as a reference to where the player came from.
    /// </summary>
    /// <param name="sceneBeforeEnteringNew">Room the player was prior to entering a door.</param>
    /// <param name="doorNumber">Door number in the room.</param>
	public void UpdatePreviousScene(string sceneBeforeEnteringNew, int doorNumber) {
		this.previousScene = sceneBeforeEnteringNew;
		this.previousDoorNumber = doorNumber;
		Debug.Log ("Coming From " + this.previousScene);
	}

    /// <summary>
    /// Resets the reference to the previous room and door number.
    /// </summary>
	public void ResetPreviousScene() {
		this.previousScene = "None";
		this.previousDoorNumber = 0;
	}
}
