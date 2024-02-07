using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Version 2. Handles what happens upon player avatar death before player avatar is respawned.
/// </summary>
public class PlayerDeathManager_v2 : MonoBehaviour {
    /// <summary>
    /// Time in seconds before player avatar is respawned.
    /// </summary>
	public float respawnTimer = 2.0f;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_DEATH, PlayerDeath);
	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.ON_PLAYER_DEATH, PlayerDeath);
	}

    /// <summary>
    /// Called upon player death. Performs events when the player avatar dies.
    /// </summary>
	private void PlayerDeath() {
		StartCoroutine (RespawnTime ());
	}

    /// <summary>
    /// Coroutine handling respawning. Player avatar is respawned after the respawn timer expires.
    /// 
    /// Any other behaviour/event concerning player death is handled here.
    /// </summary>
    /// <returns></returns>
	public IEnumerator RespawnTime () {
		//Do any fancy stuff here before coroutine
		EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_CAMERA);

		yield return new WaitForSeconds (respawnTimer);

		GameController_v7.Instance.RespawnPlayer ();
		EventBroadcaster.Instance.PostEvent (EventNames.ENABLE_CAMERA);
	}
}
