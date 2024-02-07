using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

/// <summary>
/// Handles loading and saving of data on scene switch
/// </summary>
public class SceneObjectsController : MonoBehaviour {
	//private const double MAX_MINUTES = 5;
    /// <summary>
    /// Data path of the folder where saved files of the current user is located
    /// </summary>
	string folderDataPath;
    /// <summary>
    /// Data path of the JSON file for that scene
    /// </summary>
    string sceneDataPath;
    /// <summary>
    /// Data path of the JSON file for Yuni's stats
    /// </summary>
    string playerDataPath;
    /// <summary>
    /// Data path of the JSON file for the current user's data
    /// </summary>
    string userDataPath;

    /// <summary>
    /// Instance of the SceneData class used for saving and loading data
    /// </summary>
    SceneData sceneData;
    /// <summary>
    /// Instance of the YuniData class used for saving and loading data
    /// </summary>
	YuniData yuniData;
    /// <summary>
    /// Instance of the UserData class used for saving and loading data
    /// </summary>
	UserData userData;

    /// <summary>
    /// Indicated the topic assigned to a scene
    /// </summary>
    SceneTag sceneTag;

    //	PartitionableObject_v2[] poBoxes;
    /// <summary>
    /// Array that holds all instances of hollow blocks in the room
    /// </summary>
    HollowBlock[] hollowBlocks;
    /// <summary>
    /// Array that holds all instances of skyblocks in the room
    /// </summary>
	SkyBlock[] skyBlocks;
    /// <summary>
    /// List used to attach sky fragment pieces to its parent hollow block based on saved data
    /// </summary>
	List<SkyFragmentPiece> skyFragmentPieces;
    /// <summary>
    /// Counter for the number of blocks in the room
    /// </summary>
	int currentBlockID;
    /// <summary>
    /// Counter for the number of pieces in a block
    /// </summary>
	int currentPieceID;

    /// <summary>
    /// Reference to the PlayerHealth script attached to Yuni
    /// </summary>
    PlayerHealth playerHealth;
    /// <summary>
    /// Reference to the PlayerAttack script attached to Yuni
    /// </summary>
	PlayerAttack playerAttack;
    //	SkyFragmentBlock skyFragmentBlock;


    /// <summary>
    /// Value is true if the player has revisited a scene
    /// </summary>
    bool doesSceneExist;

    /// <summary>
    /// Stores the current level of the player in the Addition of Similar Fractions topic
    /// </summary>
    int currentASLevel;
    /// <summary>
    /// Stores the current level of the player in the Subtraction of Similar Fractions topic
    /// </summary>
    int currentSSLevel;
    /// <summary>
    /// Stores the current level of the player in the Dissimilar Fractions topic
    /// </summary>
    int currentDLevel;

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    static SceneObjectsController instance;

    /// <summary>
    /// Access to the singleton instance
    /// </summary>
    public static SceneObjectsController Instance {
		get { return instance; }
	}

    /// <summary>
    /// Unity Function. Called once upon creation of the object 
    /// </summary>
    void Awake() {
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		Debug.Log ("<color=cyan>PRIORITY LOW</color>");
		instance = this;
//		skyFragmentBlock = 
		EventBroadcaster.Instance.AddObserver (EventNames.LOAD_DATA, this.LoadData);
		EventBroadcaster.Instance.AddObserver (EventNames.SAVE_DATA, this.SaveData);
	}

    /// <summary>
    /// Unity Function. Raises the destroy event
    /// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.LOAD_DATA);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SAVE_DATA);
	}

    /// <summary>
    /// Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled 
    /// </summary>
	void Start() {
		doesSceneExist = false;
		currentBlockID = 1;
		currentPieceID = 1;
		sceneTag = GameObject.FindGameObjectWithTag ("SceneTag").GetComponent<SceneTag> ();
		if (PlayerPrefs.HasKey ("Username")) {
			folderDataPath = Application.persistentDataPath + "/Game Data/user_" + PlayerPrefs.GetString ("Username");
//			Debug.LogError("Username "+PlayerPrefs.GetString("Username"));
		}
		else
			folderDataPath = Application.persistentDataPath + "/Game Data/user_default";

//		folderDataPath = Application.persistentDataPath + "/Game Data/user_1";
		Debug.Log ("Folder data path: " + folderDataPath);
//		if (isNewUser) {
//			if (!Directory.Exists (folderDataPath)) {
//				currentFolderID = "1";
//			} else {
//				string[] directories = Directory.GetDirectories (folderDataPath);
//				List<int> folderIDs = new List<int> ();
//				foreach (string dir in directories) {
//					folderIDs.Add (int.Parse (dir.Split ('_').Last ()));
//				}
//
//				folderIDs.OrderByDescending (x => x);
//				string directoryName = directories.OrderByDescending (x => x).First ();
//				currentFolderID = (1 + folderIDs.OrderByDescending (x => x).First ()).ToString ();
//			}
//
//			folderDataPath = Application.persistentDataPath + "/Game Data/user_" + currentFolderID;
//			Debug.Log ("Folder data path: " + folderDataPath);
//			if (!Directory.Exists (folderDataPath)) {
//				Directory.CreateDirectory (folderDataPath);
//			}
//		} else {
//			folderDataPath += "/user_1";
//		}

		sceneData = new SceneData ();
		yuniData = new YuniData ();
		userData = new UserData ();
		Debug.Log ("Folder data path: " + folderDataPath);
		sceneDataPath = folderDataPath + "/" + SceneManager.GetActiveScene ().name + ".json";
		playerDataPath = folderDataPath + "/YuniData.json";
		userDataPath = folderDataPath + "/UserData.json";
		playerHealth = GameObject.FindObjectOfType<PlayerHealth> ();
		playerAttack = GameObject.FindObjectOfType<PlayerAttack> ();
		hollowBlocks = FindObjectsOfType<HollowBlock>();
		skyBlocks = FindObjectsOfType<SkyBlock> ();
		foreach (HollowBlock block in hollowBlocks) {
			block.SetSceneObjectId (currentBlockID);
			currentBlockID++;
//			skyFragmentPieces = block.gameObject.GetComponentsInChildren<SkyFragmentPiece> ();
		}

		currentBlockID = 1;
		foreach (SkyBlock block in skyBlocks) {
			block.SetSceneObjectId (currentBlockID);
			currentBlockID++;
		}
	}

	void Update() {

	}

    /// <summary>
    /// Saves all necessary data e.g. user data, scene data, and Yuni's data
    /// </summary>
    /// <param name="parameters">Parameters passed by posted event</param>
    void SaveData(Parameters parameters) {
		bool isHardSave = parameters.GetBoolExtra ("IS_HARD_SAVE", false);
		SaveUserData ();
		SaveSceneData (isHardSave);
		SavePlayerData ();
	}

    /// <summary>
    /// Loads saved data, if it exists
    /// </summary>
    /// <param name="parameters">Parameters passed by posted event</param>
	void LoadData(Parameters parameters) {
		bool willLoadData = parameters.GetBoolExtra ("WILL_LOAD_DATA", false);
		LoadUserData ();
		if(willLoadData)
			LoadSceneData ();
		LoadPlayerData ();
	}

    /// <summary>
    /// Saves the necessary data in a scene
    /// </summary>
    /// <param name="isHardSave">True if the saving is triggered by a save point and not by a scene switch</param>
    void SaveSceneData(bool isHardSave) {
//		if (poBoxes != null) {
		if (hollowBlocks != null) {
			Debug.Log ("Saved");
			if (isHardSave)
				sceneData.timeSolved = "NONE";
			else
				sceneData.timeSolved = System.DateTime.Now.ToString ();

			sceneData.hollowBlocks = new List<HollowBlockData> ();
			foreach (HollowBlock block in hollowBlocks) {
				if (block != null) {
					HollowBlockData blockData = new HollowBlockData ();
					blockData.name = block.gameObject.name;
					blockData.id = block.GetSceneObjectId ();
					blockData.yPos = block.gameObject.transform.position.y;
					blockData.xPos = block.gameObject.transform.position.x;
//					blockData.skyBlockParentID = block.gameObject.
					blockData.isSolved = block.IsSolved ();
					blockData.hollowBlockPieces = new List<SkyFragmentData> ();
					skyFragmentPieces = block.GetSkyPieceContainer ().GetSkyPieces ();
					foreach (SkyFragmentPiece piece in skyFragmentPieces) {
						piece.SetSceneObjectId (currentPieceID);
						currentPieceID++;
						SkyFragmentData skyFragmentData = new SkyFragmentData ();
						skyFragmentData.id = piece.GetSceneObjectId ();
						skyFragmentData.numerator = piece.GetNumerator ();
						skyFragmentData.denominator = piece.GetDenominator ();
						skyFragmentData.skyBlockParentID = piece.GetSkyBlockParent ().GetSceneObjectId ();
						blockData.hollowBlockPieces.Add (skyFragmentData);
					}

					sceneData.hollowBlocks.Add (blockData);
					currentPieceID = 1;
				}
			}

//			foreach (SkyBlock block in skyBlocks) {
//				if (block != null) {
//					SkyBlockData blockData = new SkyBlockData ();
//					blockData.name = block.gameObject.name;
//					blockData.id = block.GetSceneObjectId ();
//					blockData.yPos = block.gameObject.transform.position.y;
//					blockData.xPos = block.gameObject.transform.position.x;
//					blockData.isSolved = block.IsSolved ();
//
//				}
//			}
	

			string newData = JsonUtility.ToJson (sceneData, true);
			File.WriteAllText (sceneDataPath, newData);
			sceneData = new SceneData ();
			currentBlockID = 1;
			currentPieceID = 1;
//			poBoxes = null;
			hollowBlocks = null;
		}
	}

    /// <summary>
    /// Loads saved scene data, if it exists
    /// </summary>
    void LoadSceneData() {
		if (File.Exists (sceneDataPath)) {
			string data = File.ReadAllText (sceneDataPath);
			sceneData = JsonUtility.FromJson<SceneData> (data);

			if (sceneData.timeSolved != "NONE") {
				Debug.Log ("<color='blue'>TIME SOLVED </color>" + DateTime.Parse(sceneData.timeSolved));
				TimeSpan elapsedTime = System.DateTime.Now - DateTime.Parse(sceneData.timeSolved);
				Debug.Log ("<color='blue'>ELAPSED TIME </color>" + elapsedTime);
				Debug.Log ("<color='blue'>ELAPSED TIME </color>" + elapsedTime.TotalMinutes);
//				if (elapsedTime.TotalMinutes < MAX_MINUTES) {
					//DO NOT SAVE DATA
					foreach (HollowBlockData block in sceneData.hollowBlocks) {
						Debug.Log ("Block "+block.name);
						hollowBlocks [block.id - 1].SetPiecesReturnToSkyBlock(true);
						hollowBlocks [block.id - 1].Break ();

						foreach (SkyFragmentData skyFragmentData in block.hollowBlockPieces) {
							SkyFragmentPiece piece = skyBlocks [skyFragmentData.skyBlockParentID - 1].GetComponentInChildren<SkyFragmentBlock> ().CreateSkyFragmentPiece (skyBlocks [skyFragmentData.skyBlockParentID - 1].GetDetachedManager ().gameObject, (int)skyFragmentData.numerator, (int)skyFragmentData.denominator);
							hollowBlocks [block.id - 1].Absorb (piece);
						}
//						if (block.isSolved)
//							hollowBlocks [block.id - 1].Solved ();
				}


					Debug.Log ("Disable tutorials");
					EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_TUTORIALS);

//				} else {
//					Debug.Log ("Reset puzzles");
//				}
			}

			Debug.Log ("Loaded scene data");
		} else {
//			Debug.LogError ("Unable to read the saved data, file doesn't exist");
			sceneData = new SceneData ();
		}
	}

    /// <summary>
    /// Saves Yuni's stats
    /// </summary>
	void SavePlayerData() {
		yuniData.hasNeedle = playerAttack.HasNeedle ();
		yuniData.hasHammer = playerAttack.HasHammer ();
		yuniData.hasThread = playerAttack.HasThread ();
		yuniData.usingHammer = playerAttack.UsingHammer ();
		yuniData.usingNeedle = playerAttack.UsingNeedle ();
		yuniData.hp = playerHealth.CurrentHealth ();
		yuniData.maxHp = playerHealth.GetMaxHp ();
		yuniData.equippedDenom = playerAttack.getEquippedDenominator ();
		yuniData.needleDenom = playerAttack.getEquippedDenominator ();
		yuniData.hammerDenom = playerAttack.getEquippedDenominator ();
		yuniData.hammerCharge = playerAttack.getHammerObject ().chargeRate;
		yuniData.hammerPrevDenom = playerAttack.getHammerObject ().prevDenominator;

		string newData = JsonUtility.ToJson (yuniData, true);
		File.WriteAllText (playerDataPath, newData);
		yuniData = new YuniData ();

		Debug.Log ("Saved player data"+"valu "+ yuniData.needleDenom);
	}

    /// <summary>
    /// Loads saved Yuni's stats, if it exists
    /// </summary>
    void LoadPlayerData() {
		
		Debug.Log ("File exists" + File.Exists (playerDataPath));
		if (File.Exists (playerDataPath)) {
			string data = File.ReadAllText (playerDataPath);
			yuniData = JsonUtility.FromJson<YuniData> (data);
			playerAttack.SetEquippedDenominator (yuniData.equippedDenom);
			playerAttack.SetHasHammer (yuniData.hasHammer);
			playerAttack.SetHasNeedle (yuniData.hasNeedle);
			playerAttack.SetHasThread (yuniData.hasThread);
//			playerAttack.SetUsingHammer (yuniData.usingHammer);
//			playerAttack.SetUsingNeedle (yuniData.usingNeedle);
			playerAttack.getHammerObject ().chargeRate = yuniData.hammerCharge;
			playerAttack.getHammerObject ().SyncWithEquippedDenominator(yuniData.hammerDenom);
			playerAttack.getNeedle().SyncWireSliceCountWithEquippedDenominator(yuniData.needleDenom);
			playerAttack.getHammerObject ().prevDenominator = yuniData.hammerPrevDenom;
			playerHealth.SetCurrentHealth (yuniData.hp);
			playerHealth.SetMaxHp (yuniData.maxHp);

			if (yuniData.usingHammer) {
				Debug.Log ("CHANGED WEAPONS");
				playerAttack.ChangeWeapons ();
			}

			Debug.Log ("<color=red>CHANGED WEAPONS </color> "+yuniData.hammerDenom);
//			Debug.Log ("LOAD"+"valu "+ yuniData.needleDenom);
			Debug.Log ("Loaded player data");
		} else {
//			Debug.LogError ("Unable to read the saved data, file doesn't exist");
			yuniData = new YuniData ();
		}
	}

    /// <summary>
    /// Loads saved user data, if it exists
    /// </summary>
	void LoadUserData() {
		if (File.Exists (sceneDataPath)) {
			doesSceneExist = true;
		} else {
			doesSceneExist = false;
		}

		if (File.Exists (userDataPath)) {
			string data = File.ReadAllText (userDataPath);
			userData = JsonUtility.FromJson<UserData> (data);

            //GameController_v7.Instance.SetPlayerSkin(GameController_v7.PlayerSkin.DEFAULT_FEMALE);
			currentASLevel = userData.currentAdditionSimilarFractionsLevel;
			currentSSLevel = userData.currentSubtractionSimilarFractionsLevel;
			currentDLevel = userData.currentDissimilarFractionsLevel;
		} else {
			//			Debug.LogError ("Unable to read the saved data, file doesn't exist");
			userData = new UserData ();
		}
	}

    /// <summary>
    /// Saves name of last scene visited and current levels for each topic of the player
    /// </summary>
	void SaveUserData() {
		userData.lastSceneVisited = SceneManager.GetActiveScene().name;

		if (!doesSceneExist && sceneTag != null) {
			userData.currentAdditionSimilarFractionsLevel = currentASLevel;
			userData.currentSubtractionSimilarFractionsLevel = currentSSLevel;
			userData.currentDissimilarFractionsLevel = currentDLevel;
			if (sceneTag.GetSceneTopic ().Equals (SceneTopic.SIMILAR_ADD)) {
				Debug.Log ("Inside if add");
				currentASLevel++;
//				userData.currentAdditionSimilarFractionsLevel = currentASLevel++;
			} else if (sceneTag.GetSceneTopic ().Equals (SceneTopic.SIMILAR_SUB)) {
				currentSSLevel++;
//				userData.currentSubtractionSimilarFractionsLevel = currentSSLevel++;
			} else if (sceneTag.GetSceneTopic ().Equals (SceneTopic.DISSIMILAR_ADD) || sceneTag.GetSceneTopic ().Equals (SceneTopic.DISSIMILAR_SUB)
			          || sceneTag.GetSceneTopic ().Equals (SceneTopic.EQUIVALENT_ADD) || sceneTag.GetSceneTopic ().Equals (SceneTopic.EQUIVALENT_SUB)) {
				currentDLevel++;
//				userData.currentDissimilarFractionsLevel = currentDLevel++;
			}
		} 
//		else {
//			userData.currentAdditionSimilarFractionsLevel = currentASLevel;
//			userData.currentSubtractionSimilarFractionsLevel = currentSSLevel;
//			userData.currentDissimilarFractionsLevel = currentDLevel;
//		}
		userData.currentAdditionSimilarFractionsLevel = currentASLevel;
		userData.currentSubtractionSimilarFractionsLevel = currentSSLevel;
		userData.currentDissimilarFractionsLevel = currentDLevel;

		string newData = JsonUtility.ToJson (userData, true);
		File.WriteAllText (userDataPath, newData);
		userData = new UserData ();

		Debug.Log ("Saved user data");
	}

    
	//public bool DoesSceneFileExist() {
	//	return File.Exists(sceneDataPath);
	//}
}

