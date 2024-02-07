using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene controller handles the features in a room.
/// 
/// If the features are not set to arbitrary, this class prompts them to initialize based on the values given by the pedagogical component.
/// </summary>
public class SceneController : MonoBehaviour {

	/// <summary>
	/// Current topic of the puzzle given.
	/// </summary>
	[SerializeField] private SceneTopic topic = SceneTopic.SIMILAR_ADD;
	/// <summary>
	/// Flag if the scene controller should initialize on Start.
	/// </summary>
	[SerializeField] private bool initOnAwake = true;

	/// <summary>
	/// The hollow block controller.
	/// </summary>
	private HollowBlockController hollowBlockController;
	/// <summary>
	/// The yarnball control.
	/// </summary>
	private YarnballControl yarnballControl;
	/// <summary>
	/// The skyblock controller.
	/// </summary>
    private SkyBlockController skyblockController;

	/// <summary>
	/// The stability number line.
	/// </summary>
    private StabilityNumberLine stabilityNumberLine;

	/// <summary>
	/// Flag if the scene controller was prompted to retry the room.
	/// </summary>
    private bool isRetry;

	/// <summary>
	/// Start this instance.
	/// </summary>
    void Start() {
		if (initOnAwake)
			Init ();

		EventBroadcaster.Instance.AddObserver (EventNames.RETRY, this.Retry);
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.RETRY, this.Retry);
	}

	/// <summary>
	/// Exits the game.
	/// </summary>
	public void ExitGame () {
		Debug.Log ("Closing game");
		Application.Quit ();
	}

	/// <summary>
	/// Retry the puzzle in the room.
	/// </summary>
	private void Retry() {
        Debug.LogError("Entered Retry");
        this.isRetry = true;
        StopAllCoroutines();
        //this.DeactivateBlocksAndPedestals ();
        StartCoroutine(InitRoutine());
        //this.Init();
        //		EventBroadcaster.Instance.PostEvent (EventNames.DESTABILIZE);
        //		EventBroadcaster.Instance.PostEvent (EventNames.REQUEST_UPDATE_SESSION);
    }

	/// <summary>
	/// Coroutine started when the Retry function is called.
	/// </summary>
	/// <returns>None</returns>
    public IEnumerator InitRoutine() {
        
        Debug.LogError("Entered Retry INIT COROUTINE");
        EventBroadcaster.Instance.PostEvent(EventNames.SPAWN_BLOCKS_PROCESS_ON);

        LookForControllers();
        yield return StartCoroutine(this.DeactivateBlocksAndPedestals());
        this.stabilityNumberLine.ResetPointers();
        Debug.LogError("Finished Retry INIT COROUTINE");
        //stabilityNumberLine.Reset();
        //yield return StartCoroutine(stabilityNumberLine.ResetRoutine());
        //EventBroadcaster.Instance.PostEvent(EventNames.SPAWN_BLOCKS_PROCESS_OFF);

        this.Init();
        yield return null;
    }

	/// <summary>
	/// Initializes the puzzle for the room.
	/// </summary>
	public void Init() {
		PedagogicalComponent_v2.Instance.ClearFractionsQueue ();

		this.topic = PedagogicalComponent_v2.Instance.CurrentTopic ();

		LookForControllers ();

        StartCoroutine(InitCoroutine());

        /*
		if (this.hollowBlockController != null && this.yarnballControl != null && this.skyblockController != null) {
			if (!this.hollowBlockController.Emergency ()) {
				this.skyblockController.SpawnSkyBlocks (this.hollowBlockController.DetermineCount ());
				this.hollowBlockController.SpawnBlocks (this.topic, this.skyblockController.GetSkyBlocks ());

				if (this.yarnballControl.GivenDenoms () == YarnballControl.GivenDenominators.DYNAMIC)
					this.yarnballControl.InitiateYarnballPedestal ();
			}
		}
        */
	}

	/// <summary>
	/// Coroutine started when the puzzle is initialized. Ensures the instantiation of the features do not abruptly update the stability number line all at once.
	/// </summary>
	/// <returns>None</returns>
    IEnumerator InitCoroutine() {
        //if (isRetry) {

        //    Debug.LogError("Entered Retry INIT COROUTINE");
        //    EventBroadcaster.Instance.PostEvent(EventNames.SPAWN_BLOCKS_PROCESS_ON);
        //    yield return StartCoroutine(this.DeactivateBlocksAndPedestals());

        //    //stabilityNumberLine.Reset();
        //    //yield return StartCoroutine(stabilityNumberLine.ResetRoutine());
        //    //EventBroadcaster.Instance.PostEvent(EventNames.SPAWN_BLOCKS_PROCESS_OFF);
        //}

        
        if (this.hollowBlockController != null && this.yarnballControl != null && this.skyblockController != null) {
            if (!this.hollowBlockController.Emergency()) {

                yield return StartCoroutine(this.skyblockController.SpawnSkyBlocks(this.hollowBlockController.DetermineCount()));
                yield return StartCoroutine(this.hollowBlockController.SpawnBlocks(this.topic, this.skyblockController.GetSkyBlocks(), this.stabilityNumberLine));

                if (this.yarnballControl.GivenDenoms() == YarnballControl.GivenDenominators.DYNAMIC)
                    this.yarnballControl.InitiateYarnballPedestal();
            }
        }
        EventBroadcaster.Instance.PostEvent(EventNames.SPAWN_BLOCKS_PROCESS_OFF);
        if (isRetry) {
            //EventBroadcaster.Instance.PostEvent(EventNames.REQUEST_UPDATE_SESSION);
            this.isRetry = false;
        }
        yield return null;
    }

	/// <summary>
	/// Coroutine that deactivates the blocks and pedestals.
	/// </summary>
	/// <returns>None</returns>
	public IEnumerator DeactivateBlocksAndPedestals() {
		LookForControllers ();
		if (this.hollowBlockController != null && this.yarnballControl != null) {
            yield return StartCoroutine(this.hollowBlockController.PurgeBlocks());

			this.skyblockController.PurgeSkyBlocks ();
			this.yarnballControl.DeactivateYarnballPedestals ();
		}
		PedagogicalComponent_v2.Instance.ClearFractionsQueue ();
        yield return null;
	}

	/// <summary>
	/// Looks for controllers.
	/// </summary>
	private void LookForControllers() {
		if (this.hollowBlockController == null)
			this.hollowBlockController = FindObjectOfType<HollowBlockController> ();
		if (this.yarnballControl == null)
			this.yarnballControl = FindObjectOfType<YarnballControl> ();
		if (this.skyblockController == null)
			this.skyblockController = FindObjectOfType<SkyBlockController> ();
        // ADDED
        if (this.stabilityNumberLine == null)
            this.stabilityNumberLine = FindObjectOfType<StabilityNumberLine>();
    }

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update() {
		PedagogicalComponent_v2.Instance.RandomizeDenoms ();
	}
}
