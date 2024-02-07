using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Spawns the player avatar to this behaviour's game object.
/// </summary>
public class SpawnPoint : MonoBehaviour {
    /// <summary>
    /// Flag if the player avatar should be facing right when spawned.
    /// </summary>
	[SerializeField] private bool spawnFacingRight = true;
    /// <summary>
    /// Reference to the door state manager.
    /// </summary>
	[SerializeField] private DoorStateManager doorStateManager;
    /// <summary>
    /// Reference to the player avatar asset.
    /// </summary>
	public GameObject playerPrefab;

    /// <summary>
    /// Reference to the player avatar's default variant.
    /// </summary>
    public GameObject prefabDefault;
    /// <summary>
    /// Reference to the player avatar's male variant.
    /// </summary>
    public GameObject prefabDefaultMale;
    /// <summary>
    /// Reference to the player avatar's female variant.
    /// </summary>
    public GameObject prefabDefaultFemale;

    /// <summary>
    /// String reference of the room the door is connected to.
    /// </summary>
    public string comingFrom;
    /// <summary>
    /// Door number.
    /// </summary>
	public int spawnNumber;
    /// <summary>
    /// If the spawn point is where the player avatar will spawn if it does not come from anywhere.
    /// </summary>
	public bool isDefault = false;

    /// <summary>
    /// Reference to the player avatar's status.
    /// </summary>
	private PlayerYuni player;
    /// <summary>
    /// Reference to the player avatar's movement behaviour.
    /// </summary>
	private PlayerMovement playerMovement;

    /// <summary>
    /// Reference to the file directory to store the user data.
    /// </summary>
    string userDataPath;
    /// <summary>
    /// User data
    /// </summary>
    UserData userData;

    /// <summary>
	/// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
	/// </summary>
	void Start () {
//		playerCharacter = GameObject.FindGameObjectWithTag ("Player");
		player = GameObject.FindObjectOfType<PlayerYuni>();
		playerMovement = player.GetPlayerMovement ();
    }

    /// <summary>
    /// Loads user data from the specified file path.
    /// If it exists, load the JSON file and apply its status and information to the player avatar.
    /// </summary>
    void LoadUserData() {
        userDataPath = Application.persistentDataPath + "/Game Data/user_" + PlayerPrefs.GetString("Username")+"/UserData.json";
        userData = new UserData();
        if (File.Exists(userDataPath)) {
            string data = File.ReadAllText(userDataPath);
            userData = JsonUtility.FromJson<UserData>(data);
            
            switch(userData.gender) {
                case "MALE":
                    GameController_v7.Instance.SetPlayerSkin(GameController_v7.PlayerSkin.DEFAULT_MALE);
                    break;
                case "FEMALE":
                    GameController_v7.Instance.SetPlayerSkin(GameController_v7.PlayerSkin.DEFAULT_FEMALE);
                    break;
                default:
                    GameController_v7.Instance.SetPlayerSkin(GameController_v7.PlayerSkin.DEFAULT);
                    break;
            }
            //GameController_v7.Instance.SetPlayerSkin(GameController_v7.PlayerSkin.DEFAULT_FEMALE);   
        }
        else {
            //			Debug.LogError ("Unable to read the saved data, file doesn't exist");
            userData = new UserData();
        }
    }

    /// <summary>
    /// Load the player avatar's skin, depending on the selected gender.
    /// </summary>
    public void LoadSkin() {
        switch(GameController_v7.Instance.GetPlayerSkin()) {
            case GameController_v7.PlayerSkin.DEFAULT:
                if(this.prefabDefault != null)
                    this.playerPrefab = this.prefabDefault;
                break;
            case GameController_v7.PlayerSkin.DEFAULT_FEMALE:
                if (this.prefabDefaultFemale != null)
                    this.playerPrefab = this.prefabDefaultFemale;
                break;
            case GameController_v7.PlayerSkin.DEFAULT_MALE:
                if (this.prefabDefaultMale != null)
                    this.playerPrefab = this.prefabDefaultMale;
                break;
        }
    }

    /// <summary>
    /// Spawn the player avatar on this game object's position.
    /// </summary>
	public void SpawnPlayer() {
		Debug.Log ("Spawning...");
		Debug.Log ("Instantiated");
		this.spawnFacingRight = true; // added
		if (player == null) {
            LoadUserData();
            LoadSkin();
            Debug.LogError("Loaded skin" +GameController_v7.Instance.GetPlayerSkin());
			if (spawnFacingRight) {
				GameObject playerObject = Instantiate (playerPrefab, this.gameObject.transform.position, Quaternion.identity);
				this.player = playerObject.GetComponent<PlayerYuni> ();
			}

			else {
				GameObject playerObject = Instantiate (playerPrefab, this.gameObject.transform.position, Quaternion.identity);
				playerObject.transform.localScale = new Vector3 (-playerObject.transform.localScale.x, playerObject.transform.localScale.y, playerObject.transform.localScale.z);
				this.player = playerObject.GetComponent<PlayerYuni> ();
				player.GetPlayerMovement ().Flip ();
			}

		}

		player.tag = "Player";
		player.gameObject.SetActive (true);
		player.transform.SetPositionAndRotation (this.gameObject.transform.position, Quaternion.identity);
		this.playerMovement = player.GetPlayerMovement ();
		
		Debug.Log ("<color=cyan>Spawn Point : Tag is "+player.tag+"</color>");
//		playerMovement.Flip ();

		if (this.doorStateManager != null)
			this.doorStateManager.PlayerSpawnedHere ();

       
    }
}
