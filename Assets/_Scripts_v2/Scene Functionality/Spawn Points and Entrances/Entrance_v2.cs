using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Entrance_v2 : MonoBehaviour {
	[SerializeField] private DoorStateManager doorState;
	[SerializeField] LoadingScreen loadingScreenPrefab;
	public string goingTo;
	public int doorNumber;
	public bool isTangible;

	private SpawnPointManager spawnPointManager;
	private bool playerInVicinity = false;
	MobileUI mobile;

	void Update() {
		if (playerInVicinity && doorState.IsAccessible()) {
			#if UNITY_STANDALONE
			if (Input.GetButtonDown ("Jump"))
				this.Enter ();
			#elif UNITY_ANDROID
			if (mobile.interactPressed || mobile.jumpPressed) {
			mobile.interactPressed = false;
			mobile.jumpPressed = false;
			this.Enter ();
			}
			#endif

		}
	}

	// Use this for initialization
	void Start () {
		GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");
		spawnPointManager = gameController.GetComponent<SpawnPointManager> ();
		#if UNITY_ANDROID
		mobile = GameObject.Find("Mobile UI").GetComponent<MobileUI> ();
		#endif
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(isTangible && other.gameObject.CompareTag("Player")) {
			Debug.Log ("Player at Entrance");
			EventManager.DisplaySceneTitleOnTrigger ();
			playerInVicinity = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			Debug.Log ("Player left Entrance...");
			//			EventManager.RemoveSceneTitleOnTrigger ();
			EventBroadcaster.Instance.PostEvent(EventNames.REMOVE_SCENE_TITLE);
			playerInVicinity = false;
		}
	}

	public void Enter() {
		Debug.Log ("Player Went In " + goingTo + " " + doorNumber);

		GameController_v7.Instance.UpdatePreviousScene (SceneManager.GetActiveScene ().name, this.doorNumber);
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
		else {
			SceneManager.LoadScene (goingTo);
//			Instantiate (loadingScreenPrefab);
		}
	}
}
