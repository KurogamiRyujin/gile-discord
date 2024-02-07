using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// Behaviour for the doors. Used to move from one room to another.
/// </summary>
public class Entrance : MonoBehaviour {

    /// <summary>
    /// Reference to the door state manager.
    /// </summary>
	[SerializeField] private DoorStateManager doorState;
    /// <summary>
    /// Reference to a loading screen overlay. Can either be a reference to the asset or an instance in the scene.
    /// </summary>
	[SerializeField] LoadingScreen loadingScreenPrefab;
    /// <summary>
    /// Flag if the door will lead to a random room.
    /// 
    /// Set true in the dynamic stage.
    /// </summary>
	[SerializeField] private bool isRandomDestination = false;
    /// <summary>
    /// Room where the door will lead to.
    /// </summary>
	public string goingTo;
    /// <summary>
    /// Number assigned to this door.
    /// </summary>
	public int doorNumber;
    /// <summary>
    /// Flag if this door is tangible and useable.
    /// </summary>
	public bool isTangible;

    /// <summary>
    /// UNUSED
    /// 
    /// Reference to the spawn point manager.
    /// </summary>
	private SpawnPointManager spawnPointManager;
    /// <summary>
    /// Flag if the player is close to the door.
    /// </summary>
	private bool playerInVicinity = false;
    //	MobileUI mobile;

    /// <summary>
    /// Standard Unity Function. Called once every frame.
    /// </summary>
    void Update() {
		if (playerInVicinity && doorState.IsAccessible()) {
			#if UNITY_STANDALONE
			if (Input.GetButtonDown ("Jump"))
				this.Enter ();
			#elif UNITY_ANDROID
			if (GameController_v7.Instance.GetMobileUIManager().IsInteracting() ||
				GameController_v7.Instance.GetMobileUIManager().IsJumping()) {
				GameController_v7.Instance.GetMobileUIManager().SetInteracting(false);
				GameController_v7.Instance.GetMobileUIManager().SetJumping(false);
				this.Enter ();
			}
//			if (mobile.interactPressed || mobile.jumpPressed) {
//				mobile.interactPressed = false;
//				mobile.jumpPressed = false;
//				this.Enter ();
//			}
			#endif
				
		}
	}

    /// <summary>
    /// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
    /// </summary>
    void Start () {
		GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");
		spawnPointManager = gameController.GetComponent<SpawnPointManager> ();
//		#if UNITY_ANDROID
//			mobile = GameObject.Find("Mobile UI").GetComponent<MobileUI> ();
//		#endif
	}
	
    /// <summary>
    /// Unity Function. Called if another game object's collider enters this game object's collider.
    /// 
    /// If it was the player, raises the flag that the player is in vicinity, allowing the player the choice to enter the door.
    /// </summary>
    /// <param name="other"></param>
	void OnTriggerEnter2D(Collider2D other) {
		if(isTangible && other.gameObject.GetComponent<PlayerYuni>() != null) {//.CompareTag("Player")) {
//			other.gameObject.GetComponent<PlayerManager> ().SetEntranceStandingIn (this);
			Debug.Log ("Player at Entrance");
//			EventManager.DisplaySceneTitleOnTrigger ();
			EventBroadcaster.Instance.PostEvent(EventNames.DISPLAY_SCENE_TITLE);
			playerInVicinity = true;
		}
	}

    /// <summary>
    /// Unity Function. Called when another game object's collider leaves this game object's collider.
    /// 
    /// If it was the player avatar, the player avatar is now not in vicinity.
    /// </summary>
    /// <param name="other"></param>
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni>() != null) {//other.gameObject.CompareTag ("Player")) {
//			other.gameObject.GetComponent<PlayerManager> ().SetEntranceStandingIn (null);
			Debug.Log ("Player left Entrance...");
//			EventManager.RemoveSceneTitleOnTrigger ();
			EventBroadcaster.Instance.PostEvent(EventNames.REMOVE_SCENE_TITLE);
			playerInVicinity = false;
		}
	}

    /// <summary>
    /// If the player is in vicinity of the door, another room is loaded and the current room is unloaded, bringing the player avatar to a new room.
    /// </summary>
	public void Enter() {
		if (this.isRandomDestination) {
			if (PedagogicalComponent_v2.Instance.CurrentTopic () == SceneTopic.NONE)
				this.goingTo = SceneNames.BETA_TESTING_PRE_BOSS;
			else {
				this.goingTo = SceneNames.RandomSceneName ();

				if (GameController_v7.Instance.RoomCountCheck ())
					this.goingTo = SceneNames.BETA_TESTING_RANDOM_END;
			}
		}

		Debug.Log ("Player Went In " + goingTo + " " + doorNumber);

//		spawnPointManager.UpdatePreviousScene (SceneManager.GetActiveScene ().name, this.doorNumber);
		GameController_v7.Instance.UpdatePreviousScene(SceneManager.GetActiveScene ().name, this.doorNumber);
		GameController_v7.Instance.GetObjectStateManager ().RecordPlayerStats ();
		//		objectStateManager.RecordPlayerStats ();
		Parameters parameters = new Parameters();
		parameters.PutExtra ("IS_HARD_SAVE", false);
		EventBroadcaster.Instance.PostEvent(EventNames.SAVE_DATA, parameters);
//		EventManager.RemoveHintOnTrigger ();
		EventBroadcaster.Instance.PostEvent(EventNames.REMOVE_HINT);
		EventManager.ResetTriggerSceneTitle ();
		#if UNITY_ANDROID
//			EventManager.EnableBothButtons ();
		#endif
		//Load Scene Connected to this entrance.
		LoadingScreen loadingScreen = FindObjectOfType<LoadingScreen>();
		if (loadingScreen != null)
			loadingScreen.LoadScene (goingTo, false);
		else
			Instantiate (loadingScreenPrefab);
//			SceneManager.LoadScene (goingTo);
	}
}
