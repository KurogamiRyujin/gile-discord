using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateManager {

	private const string PARTITIONABLE_OBJECT = "PartitionableObject";
	private const string PLAYER = "Player";
	private const string CUTSCENE = "Cutscene";

	public string currentPlayerName = "Test";

	private List<GameObject> objects;
	private GameObject player;
	private PlayerJSONParams playerStats;

	public ObjectStateManager() {
		FindPlayer ();
		playerStats = new PlayerJSONParams ();
	}

	public void FindPlayer() {
		player = GameObject.FindGameObjectWithTag (PLAYER);
	}

	public void RecordState(GameObject obj, int objCount, string scene) {
		if (obj.CompareTag (PARTITIONABLE_OBJECT)) {
			PartitionableObjectJSONParams partitionable = new PartitionableObjectJSONParams ();
			partitionable.scene = scene;
			partitionable.count = objCount;
			partitionable.posX = obj.transform.position.x;
			partitionable.posY = obj.transform.position.y;
			partitionable.isTangible = obj.GetComponent<PartitionableObject> ().IsTangible ();

			string jsonPartitionable = JsonUtility.ToJson (partitionable);
			PlayerPrefs.SetString (scene + "_" + PARTITIONABLE_OBJECT + "_" + objCount, jsonPartitionable);
		} else if (obj.CompareTag (CUTSCENE)) {
			CutsceneJSONParams cutscene = new CutsceneJSONParams ();
			cutscene.name = obj.GetComponent<Cutscene> ().GetCutsceneName ();
			cutscene.scene = scene;
			cutscene.isTriggered = obj.GetComponent<Cutscene> ().IsTriggered ();

			string jsonCutscene = JsonUtility.ToJson (cutscene);
			PlayerPrefs.SetString (scene + "_" + cutscene.name, jsonCutscene);
		}
	}

	public void UpdatePlayerName(string name) {
		playerStats.playerName = name;
	}

	public void UpdatePlayerMaxHP(int maxHP) {
		playerStats.maxHP = maxHP;
	}

	public void UpdatePlayerCurrentHP(int currentHP) {
		playerStats.currentHP = currentHP;
	}

	public void UpdatePlayerHasNeedle(bool flag) {
		playerStats.hasNeedle = flag;
	}

	public void UpdatePlayerHasHammer(bool flag) {
		playerStats.hasHammer = flag;
	}

	public void UpdatePlayerHasThread (bool flag) {
		playerStats.hasThread = flag;
	}
	public void UpdatePlayerIsAlive(bool flag) {
		playerStats.isAlive = flag;
	}

	//Records the current player stats to the PlayerPrefs
	public void RecordPlayerStats() {
		if (player == null) {
			FindPlayer ();
		}
		PlayerAttack attack = player.GetComponent<PlayerAttack> ();
		playerStats.playerName = currentPlayerName;//Just in case it is prompted
		playerStats.hasNeedle = attack.HasNeedle ();
		playerStats.hasHammer = attack.HasHammer ();
		playerStats.hasThread = attack.HasThread ();
		playerStats.equppedDenominator = attack.getEquippedDenominator ();
		PlayerHealth health = player.GetComponent<PlayerHealth> ();
		playerStats.maxHP = (int) health.GetMaxHp ();
		playerStats.currentHP = (int) health.CurrentHealth ();
		playerStats.isAlive = health.isAlive;

		string jsonPlayer = JsonUtility.ToJson (playerStats);
		PlayerPrefs.SetString (PLAYER + "_" + playerStats.playerName, jsonPlayer);
	}

	//TODO: will have to call this from a button
	public void ClearPlayerData() {
		PlayerPrefs.DeleteKey (PLAYER + "_" + currentPlayerName);
		//TODO: also delete player progress
	}

	//TODO: will have to call this from a button
	public void ClearAllData() {
		PlayerPrefs.DeleteAll ();
	}

	void OnApplicationQuit() {
		//NOTE: THIS IS ONLY TEMPORARY!!!
		ClearAllData ();
	}

	public GameObject GetPlayer() {
		return this.player;
	}
}
