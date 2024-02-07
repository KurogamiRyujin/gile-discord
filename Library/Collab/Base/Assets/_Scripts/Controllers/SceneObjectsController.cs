using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class SceneObjectsController : MonoBehaviour {
	private const double MAX_MINUTES = 5; 

	bool isNewUser;
	string currentFolderID;
	string folderDataPath;
	string sceneDataPath;
	string playerDataPath;

	SceneData sceneData;
	YuniData yuniData;

	PartitionableObject_v2[] poBoxes;
	int currentID;

	PlayerHealth playerHealth;
	PlayerAttack playerAttack;

	static SceneObjectsController instance;
	public static SceneObjectsController Instance {
		get { return instance; }
	}

	void Awake() {
		instance = this;
		EventBroadcaster.Instance.AddObserver (EventNames.LOAD_DATA, this.LoadData);
		EventBroadcaster.Instance.AddObserver (EventNames.SAVE_DATA, this.SaveData);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.LOAD_DATA);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SAVE_DATA);
	}

	void Start() {
		currentID = 1;
		isNewUser = true;
		folderDataPath = Application.persistentDataPath + "/Game Data/user_"+PlayerPrefs.GetString("Username");
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
		Debug.Log ("Folder data path: " + folderDataPath);
		sceneDataPath = folderDataPath + "/" + SceneManager.GetActiveScene ().name + ".json";
		playerDataPath = folderDataPath + "/YuniData.json";
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		playerAttack = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerAttack> ();
		poBoxes = FindObjectsOfType<PartitionableObject_v2> ();
		foreach (PartitionableObject_v2 box in poBoxes) {
			box.SetSceneObjectId (currentID);
			currentID++;
		}
	}

	void Update() {

	}

	void SaveData(Parameters parameters) {
		bool isHardSave = parameters.GetBoolExtra ("IS_HARD_SAVE", false);
		SaveSceneData (isHardSave);
		SavePlayerData ();
	}

	void LoadData() {
		LoadSceneData ();
		LoadPlayerData ();
	}

	void SaveSceneData(bool isHardSave) {
		if (poBoxes != null) {
			Debug.Log ("Saved");
			if (isHardSave)
				sceneData.timeSolved = "NONE";
			else
				sceneData.timeSolved = System.DateTime.Now.ToString();

			foreach (PartitionableObject_v2 box in poBoxes) {
				if (box != null) {
					PartitionableObjectData boxData = new PartitionableObjectData ();
					boxData.name = box.gameObject.name;
					boxData.id = box.GetSceneObjectId ();
					boxData.xPos = box.gameObject.transform.position.x;
					boxData.yPos = box.gameObject.transform.position.y;
					boxData.isSolved = box.IsTangible ();
					sceneData.boxes.Add (boxData);
				}
			}

			string newData = JsonUtility.ToJson (sceneData, true);
			File.WriteAllText (sceneDataPath, newData);
			sceneData = new SceneData ();
			currentID = 1;
			poBoxes = null;
		}
	}
	 
	void LoadSceneData() {
		if (File.Exists (sceneDataPath)) {
			string data = File.ReadAllText (sceneDataPath);
			sceneData = JsonUtility.FromJson<SceneData> (data);

			if (sceneData.timeSolved != "NONE") {
				Debug.Log ("<color='blue'>TIME SOLVED </color>" + DateTime.Parse(sceneData.timeSolved));
				TimeSpan elapsedTime = System.DateTime.Now - DateTime.Parse(sceneData.timeSolved);
				Debug.Log ("<color='blue'>ELAPSED TIME </color>" + elapsedTime);
				Debug.Log ("<color='blue'>ELAPSED TIME </color>" + elapsedTime.TotalMinutes);
				if (elapsedTime.TotalMinutes < MAX_MINUTES) {
					//DO NOT SAVE DATA
					foreach (PartitionableObject_v2 pobox in poBoxes) {
						Debug.Log (pobox.name + " " + pobox.GetSceneObjectId());
					}

					foreach (PartitionableObjectData box in sceneData.boxes) {
//						Destroy (box.gameObject);
						if (box.isSolved) {
							Debug.Log ("Destroyed " + box.name + " " + box.id);
//							Debug.Log (poBoxes.ToList ().Find (x => x.GetSceneObjectId ().Equals (box.id)).gameObject.name);
//							Debug.Log (poBoxes[box.id-1].gameObject.name);
//							Destroy (poBoxes.ToList ().FirstOrDefault (x => x.GetSceneObjectId ().Equals (box.id)).gameObject);
//							Destroy (poBoxes[box.id - 1].gameObject);
							poBoxes[box.id - 1].Solved();
						}
					}

					Debug.Log ("Disable tutorials");
					EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_TUTORIALS);

				} else {
					Debug.Log ("Reset puzzles");
				}
			}
			Debug.Log ("Loaded scene data");
		} else {
			Debug.LogError ("Unable to read the saved data, file doesn't exist");
			sceneData = new SceneData ();
		}
	}

	void SavePlayerData() {
		yuniData.lastSceneVisited = SceneManager.GetActiveScene ().name;
		yuniData.hasNeedle = playerAttack.HasNeedle ();
		yuniData.hasHammer = playerAttack.HasHammer ();
		yuniData.hasThread = playerAttack.HasThread ();
		yuniData.usingHammer = playerAttack.UsingHammer ();
		yuniData.usingNeedle = playerAttack.UsingNeedle ();
		yuniData.needleDenom = playerAttack.getEquippedDenominator ();
		yuniData.hammerDenom = playerAttack.getEquippedDenominator ();
		yuniData.hammerCharge = playerAttack.getHammerObject ().chargeRate;
		yuniData.hammerPrevDenom = playerAttack.getHammerObject ().prevDenominator;

		string newData = JsonUtility.ToJson (yuniData, true);
		File.WriteAllText (playerDataPath, newData);
		yuniData = new YuniData ();

		Debug.Log ("Saved player data");
	}

	void LoadPlayerData() {
		
		Debug.Log ("File exists" + File.Exists (playerDataPath));
		if (File.Exists (playerDataPath)) {
			string data = File.ReadAllText (playerDataPath);
			yuniData = JsonUtility.FromJson<YuniData> (data);
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

			if (yuniData.usingHammer) {
				Debug.Log ("CHANGED WEAPONS");
				playerAttack.ChangeWeapons ();
			}

			Debug.Log ("<color=red>CHANGED WEAPONS </color> "+yuniData.hammerDenom);
			Debug.Log ("Loaded player data");
		} else {
			Debug.LogError ("Unable to read the saved data, file doesn't exist");
			yuniData = new YuniData ();
		}
	}
}

