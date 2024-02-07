using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Version 7 of the overall handler for the game's managers. Has all non-monobehaviour managers as property.
/// 
/// Singleton instance. Ensures there is only one instance for all managers and prevents overpopulating the project with singletons.
/// </summary>
public class GameController_v7 {

    /// <summary>
    /// Shared instance of the singleton.
    /// </summary>
    private static GameController_v7 sharedInstance = null;

    //Properties; these are the modules the game controller has control over
    /// <summary>
    /// Pause controller reference.
    /// </summary>
    private PauseController_v2 pauseController;
    /// <summary>
    /// Spawn point manager reference.
    /// </summary>
    private SpawnPointManager_v2 spawnPointManager;
    /// <summary>
    /// Object state manager reference.
    /// </summary>
    private ObjectStateManager objectStateManager;
    /// <summary>
    /// Dialogue manager reference.
    /// </summary>
    private DialogueManager_v2 dialogueManager;
    /// <summary>
    /// UNUSED
    /// 
    /// LCD manager reference.
    /// </summary>
    private LCDManager lcdManager;
    /// <summary>
    /// Block manager reference.
    /// </summary>
    private BlockManager blockManager;
    /// <summary>
    /// Tutorial glow manager reference.
    /// </summary>
    private TutorialGlowManager tutorialGlowManager;
    /// <summary>
    /// Mobile UI manager reference.
    /// </summary>
    private MobileUIManager mobileUIManager;
    /// <summary>
    /// Image manager reference.
    /// </summary>
    private ImageManager imageManager;

    /// <summary>
    /// Max rooms to visit before the player is directed to a checkpoint when playing Stage 2 (dynamic rooms).
    /// </summary>
    private int maxRoomsToVisit;
    /// <summary>
    /// Number of rooms visited.
    /// </summary>
    private int roomsVisited;
    /// <summary>
    /// Reference to the player avatar's skin which affects how it looks.
    /// </summary>
    private PlayerSkin playerSkin;

    /// <summary>
    /// Enum for the player avatar's available skins.
    /// </summary>
    public enum PlayerSkin {
        DEFAULT,
        DEFAULT_MALE,
        DEFAULT_FEMALE
    }

    /// <summary>
    /// Static reference to the game controller singleton instance.
    /// </summary>
	public static GameController_v7 Instance {
		get {
			if (sharedInstance == null) {
				sharedInstance = new GameController_v7 ();
			}

			return sharedInstance;
		}
	}

    /// <summary>
    /// Gets the current skin used by the player avatar.
    /// </summary>
    /// <returns></returns>
    public PlayerSkin GetPlayerSkin() {
        return this.playerSkin;
    }

    /// <summary>
    /// Sets the skin to be used by the player avatar.
    /// </summary>
    /// <param name="skin">Player Avatar Skin</param>
    public void SetPlayerSkin(PlayerSkin skin) {
        Debug.LogError("Set player skin");
        this.playerSkin = skin;
    }

    /// <summary>
    /// Constructor
    /// </summary>
	private GameController_v7() {
		pauseController = new PauseController_v2 ();
		spawnPointManager = new SpawnPointManager_v2 ();
		objectStateManager = new ObjectStateManager ();
		dialogueManager = new DialogueManager_v2 ();
		lcdManager = new LCDManager ();

//		eventManager = new EventManager ();
		mobileUIManager = new MobileUIManager ();
		imageManager = new ImageManager ();
		blockManager = new BlockManager ();
		tutorialGlowManager = new TutorialGlowManager ();
		SceneManager.sceneLoaded += OnLevelFinishedLoading;

		roomsVisited = 0;
		maxRoomsToVisit = 5;
        this.playerSkin = PlayerSkin.DEFAULT;
	}

    /// <summary>
    /// Returns the image manager.
    /// </summary>
    /// <returns>Image Manager</returns>
	public ImageManager GetImageManager() {
		return this.imageManager;
	}
    /// <summary>
    /// Returns the mobile UI manager.
    /// </summary>
    /// <returns>Mobile UI Manager</returns>
	public MobileUIManager GetMobileUIManager() {
		return this.mobileUIManager;
	}
    /// <summary>
    /// Returns the pause controller.
    /// </summary>
    /// <returns>Pause Controller</returns>
	public PauseController_v2 GetPauseController() {
		return this.pauseController;
	}

	//What will the game controller do when a new scene is loaded?
    /// <summary>
    /// Called when the menu/a room is done loading.
    /// 
    /// Prompts spawn point manager to handle reference to previous scenes and spawn points to spawn the player.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		//Refresh SpawnPoints list and spawn player
		if (scene.name == "Discord_Main") {
			Debug.Log ("Reset previous scene");
			this.spawnPointManager.ResetPreviousScene ();
		}
		this.spawnPointManager.SearchSpawnPoints ();
		this.spawnPointManager.SpawnPlayer ();
	}

    /// <summary>
    /// Prompts spawn point manager to respawn the player in the room.
    /// </summary>
	public void RespawnPlayer() {
		this.spawnPointManager.RespawnPlayer ();
	}

    /// <summary>
    /// Prompts spawn point manager to update its reference to the previous scene.
    /// </summary>
    /// <param name="sceneBeforeEnteringNew">Scene name</param>
    /// <param name="doorNumber">Door number entered</param>
	public void UpdatePreviousScene(string sceneBeforeEnteringNew, int doorNumber) {
		this.spawnPointManager.UpdatePreviousScene (sceneBeforeEnteringNew, doorNumber);
	}

    /// <summary>
    /// Returns the object state manager.
    /// </summary>
    /// <returns>Object state manager</returns>
	public ObjectStateManager GetObjectStateManager() {
		return this.objectStateManager;
	}

//	public EventManager GetEventManager() {
//		return this.eventManager;
//	}

    /// <summary>
    /// Returns the dialogue manager.
    /// </summary>
    /// <returns>Dialogue manager</returns>
	public DialogueManager_v2 GetDialogueManager() {
		return this.dialogueManager;
	}

    /// <summary>
    /// UNUSED
    /// 
    /// Returns the LCD manager.
    /// </summary>
    /// <returns>LCD manager</returns>
	public LCDManager GetLCDManager() {
		return this.lcdManager;
	}

    /// <summary>
    /// Returns the block manager.
    /// </summary>
    /// <returns>Block manager</returns>
	public BlockManager GetBlockManager() {
		return this.blockManager;
	}
    /// <summary>
    /// Returns the tutorial glow manager.
    /// </summary>
    /// <returns>Tutorial Glow Manager</returns>
	public TutorialGlowManager GetTutorialGlowManager() {
		return this.tutorialGlowManager;
	}

    /// <summary>
    /// Checks the number of rooms visited.
    /// 
    /// If the number of rooms visited has already reached the max number to visit, direct player to the checkpoint room.
    /// Otherwise, increment rooms visited count.
    /// </summary>
    /// <returns></returns>
	public bool RoomCountCheck() {
		if (roomsVisited == maxRoomsToVisit) {
			roomsVisited = 0;
			maxRoomsToVisit += 5;
			return true;
		} else {
			roomsVisited++;
			return false;
		}
	}
}
