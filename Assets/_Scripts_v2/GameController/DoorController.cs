using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the state of the doors in a room during stability status changes and enemy presence.
/// </summary>
public class DoorController : MonoBehaviour {

    /// <summary>
    /// Flag indicating if the area is stabilized.
    /// </summary>
	private bool areaStabilized = false;
    /// <summary>
    /// Flag indicating if the area is free of enemies.
    /// </summary>
	private bool enemiesDefeated = false;
    /// <summary>
    /// Flag indicating if the doors are currently unlocked.
    /// </summary>
	private bool doorsUnlocked = false;

    /// <summary>
    /// List of enemies currently in the room.
    /// </summary>
	private List<EnemyHealth> enemies;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake () {
		enemies = new List<EnemyHealth> ();
		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, AreaStable);
		EventBroadcaster.Instance.AddObserver (EventNames.UNSTABLE_AREA, AreaUnstable);
		EventBroadcaster.Instance.AddObserver (EventNames.REGISTER_ENEMY, RegisterEnemy);
		EventBroadcaster.Instance.AddObserver (EventNames.UNREGISTER_ENEMY, UnregisterEnemy);
	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.STABLE_AREA, AreaStable);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.UNSTABLE_AREA, AreaUnstable);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.REGISTER_ENEMY, RegisterEnemy);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.UNREGISTER_ENEMY, UnregisterEnemy);
	}

    /// <summary>
	/// Standard Unity Function. Called once every frame.
	/// </summary>
	void Update () {
		if (this.enemies.Count == 0)
			enemiesDefeated = true;
		else
			enemiesDefeated = false;

		CheckDoors ();
	}

    /// <summary>
    /// Sets the areaStabilized flag to true. Called when the room has been stabilized.
    /// </summary>
	private void AreaStable () {
//		Debug.LogError ("AREA STABILIZED TRUE");
		this.areaStabilized = true;
	}

    /// <summary>
    /// Sets the areaStabilized flag to false. Called when the room has been destabilized.
    /// </summary>
	private void AreaUnstable() {
//		Debug.LogError ("AREA STABILIZED FALSE");
		this.areaStabilized = false;
	}

    /// <summary>
    /// Updates the doors' status based on the flags.
    /// If the room is stabilized, free of enemies, and the doors are locked, broadcast to all doors to unlock themselves and set doorsUnlocked to true.
    /// Otherwise, broadcast to all doors to lock themselves and set doorsUnlocked to false.
    /// </summary>
	private void CheckDoors() {
		if (areaStabilized && enemiesDefeated && !doorsUnlocked) {
			doorsUnlocked = true;
			EventBroadcaster.Instance.PostEvent (EventNames.ON_DOOR_UNLOCK);
		} else if (doorsUnlocked && (!areaStabilized || !enemiesDefeated)) {
			doorsUnlocked = false;
			EventBroadcaster.Instance.PostEvent (EventNames.ON_DOOR_CLOSE);
			EventBroadcaster.Instance.PostEvent (EventNames.ON_DOOR_LOCK);
		}
	}

    /// <summary>
    /// When an enemy is spawned, the enemy registers itself through this function.
    /// 
    /// Adds an enemy instance into the list of enemies present in the room.
    /// </summary>
    /// <param name="data">Parameter object containing data to be passed into the function.</param>
	private void RegisterEnemy(Parameters data) {
		EnemyHealth enemy = data.GetEnemyHealthExtra ("enemyHealth");

		if (!this.enemies.Contains (enemy))
			this.enemies.Add (enemy);
	}

    /// <summary>
    /// When an enemy is pacified, the enemy unregisters itself through this function.
    /// 
    /// Removes an enemy instance from the list of enemies present in the room.
    /// </summary>
    /// <param name="data">Parameter object containing data to be passed into the function.</param>
	private void UnregisterEnemy(Parameters data) {
		EnemyHealth enemy = data.GetEnemyHealthExtra ("enemyHealth");

		if (this.enemies.Contains (enemy))
			this.enemies.Remove (enemy);
	}
}
