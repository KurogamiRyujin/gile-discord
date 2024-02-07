using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the behaviour of a ghost block.
/// 
/// Ghost blocks are intangible objects that can be filled with sky fragmens to become tangible.
/// 
/// Once this is filled, its fraction value will contribute to the room's stability.
/// </summary>
public class HollowBlock : MonoBehaviour {
    /// <summary>
    /// Reference to the ghost block parent game object.
    /// </summary>
	[SerializeField] private GameObject parent;
    /// <summary>
    /// Material used when the ghost block is tangible.
    /// </summary>
	[SerializeField] private Material tangibleMaterial;
    /// <summary>
    /// Material used when the ghost block is focused.
    /// </summary>
	[SerializeField] private Material focusMaterial;
    /// <summary>
    /// Material used when the ghost block is intangible.
    /// </summary>
	[SerializeField] private Material intangibleMaterial;
    /// <summary>
    /// Material used when the ghost block is unliftable.
    /// </summary>
	[SerializeField] private Material unliftableMaterial;
    /// <summary>
    /// Highlight color used to associate this ghost block with a highlighted portion on the stability number line.
    /// </summary>
	[SerializeField] private Color highlightColor;

    /// <summary>
    /// Flag if the fragments used to fill this ghost block will return to their parent sky block once this is broken.
    /// </summary>
	[SerializeField] private bool piecesReturnToSkyBlock;

	// Tangible outline colors
	[SerializeField] private Color outlineStartColor;
	[SerializeField] private Color outlineEndColor;

	[SerializeField] private BrokenFractionLabel fractionLabel;
	[SerializeField] private string DEFAULT_LAYER = "Default";
	[SerializeField] private string BREAKABLE_LAYER = "Breakable";

    /// <summary>
    /// Flag if the ghost block is solved.
    /// </summary>
	[SerializeField] private bool isSolved;
    /// <summary>
    /// Flag to check if break has been called and prevent cascading calls from children
    /// </summary>
	[SerializeField] protected bool isBroken;

    /// <summary>
    /// Pop up appearing above the ghost block which indicates the current filled amount of the ghost block.
    /// </summary>
	[SerializeField] private HintBubbleHollowStability stabilityLabel;
	[SerializeField] private bool isPurelyKinematic; // Main checker if interactable by needle

    /// <summary>
    /// Numerator of its fraction value.
    /// </summary>
	[SerializeField] private float numerator;
    /// <summary>
    /// Denominator of its fraction value.
    /// </summary>
	[SerializeField] private float denominator;
    /// <summary>
    /// Flag if the ghost block can be lifted once it's tangible.
    /// </summary>
	[SerializeField] private bool isLiftable = true;

//	[SerializeField] private float cloneDenominator;

    //Probably no need to document...
	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private Rigidbody2D rigidBody2D;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private LineRenderer lineRenderer;
	[SerializeField] private BoxCollider2D boxCollider;

    //	[SerializeField] private BrokenPiece brokenPiece; // The tangible piece

    /// <summary>
    /// Width of a 1 whole object.
    /// </summary>
    [SerializeField] private float widthWhole;
    /// <summary>
    /// Width of a piece with size numerator/denominator.
    /// </summary>
	[SerializeField] private float widthPiece;
    /// <summary>
    /// Width of a piece with size 1/denominator.
    /// </summary>
	[SerializeField] private float widthSingle;

    /// <summary>
    /// Height of object.
    /// </summary>
	[SerializeField] private float height;

	[SerializeField] private bool isIntangible; // Main checker if interactable by needle

	[SerializeField] private ResultsUI resultsUI;
	//	[SerializeField] private HintBubbleManager hintBubble;
    /// <summary>
    /// Flag to check if this was filled from a prefill process.
    /// </summary>
	[SerializeField] private bool isSolvedFromPrefill;

    /// <summary>
    /// Reference to the sky fragments container.
    /// </summary>
	[SerializeField] private HollowBlockSkyPieceContainer skyPieceContainer;

	// TODO: Remove when skins are implemented
	[SerializeField] private Color tangibleColor = new Color(0.212f, 0.635f, 0.655f, 1f);
	[SerializeField] private Color tangibleOutlineColor = new Color (1f, 1f, 1f, 1f);

	[SerializeField] private Color observedTangibleColor = new Color(0.74f, 0.62f, 0.25f, 1f);
	[SerializeField] private Color observedTangibleOutlineColor = new Color (0.46f, 0.35f, 0.05f, 1f);

	private Color intangibleColor = new Color(1, 1, 1, 0);
	private Color intangibleOutlineColor = new Color (1, 1, 1, 1);

	private Color unliftableColor = new Color(1, 1, 1, 0);
	private Color unliftableOutlineColor = new Color (0, 0, 0, 1);

	private int sceneObjectId;

	[SerializeField] private Vector3 respawnPosition;

    /// <summary>
    /// Flag if the ghost block is set to be prefilled.
    /// </summary>
	[SerializeField] private bool isPreFilled;
    /// <summary>
    /// Make the sky block invisible.
    /// </summary>
	[SerializeField] private bool hideSkyBlock;
    /// <summary>
    /// Of the split pieces, how many will fill this.
    /// </summary>
	[SerializeField] private int fillCount;
    /// <summary>
    /// Slice based on the denominator.
    /// </summary>
	[SerializeField] private int sliceCount;
    /// <summary>
    /// How many pieces you want to remain detached after it is dropped from the referenced sky block.
    /// </summary>
	[SerializeField] private int detachCount;

    /// <summary>
    /// Referenced sky block used for prefilling.
    /// </summary>
	[SerializeField] private SkyBlock skyBlock;
    /// <summary>
    /// List of all positions the detached fragments, if any, will be placed.
    /// </summary>
	[SerializeField] private List<Transform> detachedPositions;
    /// <summary>
    /// What object the hollow block came from when it was dropped.
    /// </summary>
	[SerializeField] protected Transform previousParent;
	[SerializeField] protected PlatformLock platformLock;

    /// <summary>
    /// Flag if the player avatar is carrying this block.
    /// </summary>
	[SerializeField] private bool isCarried;
	[SerializeField] private PlayerYuni player;
    /// <summary>
    /// Flag if the fraction value is arbitrarily set by the developers.
    /// </summary>
	[SerializeField] private bool isArbitrary = false;
    /// <summary>
    /// Flag if this block cannot be carried.
    /// </summary>
	[SerializeField] private bool cantBeCarried;

    /// <summary>
    /// Flag if the block is performing other processes such as animations.
    /// </summary>
    private bool isBlockProcess;
    private bool isBeingProcessed;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
//		if (!this.isArbitrary) {
//			FractionData fraction = PedagogicalComponent_v2.Instance.RequestFraction ();
//			this.numerator = fraction.numerator;
//			this.denominator = fraction.denominator;
//		}
		if (this.CantCarry()) {
			this.isPurelyKinematic = true;
		}
//		this.clone = GetComponentInChildren<BrokenClone> ();
		this.ResetBreak();
		this.fractionLabel = GetComponentInChildren<BrokenFractionLabel> ();

		this.rectTransform = GetComponent<RectTransform> ();
		this.rigidBody2D = GetComponent<Rigidbody2D> ();
		this.boxCollider = GetComponent<BoxCollider2D> ();

//		this.brokenPiece = GetComponentInChildren<BrokenPiece> ();
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
		this.lineRenderer = GetComponent<LineRenderer> ();

        //EventBroadcaster.Instance.AddObserver(EventNames.SPAWN_BLOCKS_PROCESS_ON, BlockProcessOn);
        EventBroadcaster.Instance.AddObserver(EventNames.SPAWN_BLOCKS_PROCESS_OFF, BlockProcessOff);

    }

    /// <summary>
    /// Prompt that the block is processing.
    /// </summary>
    public void ProcessBlock() {
        this.isBeingProcessed = true;
    }

    /// <summary>
    /// Prompt that the block is done processing.
    /// </summary>
    public void ProcessingDone() {
        this.isBeingProcessed = false;
    }

    /// <summary>
    /// Checker if the block is being processed.
    /// </summary>
    /// <returns>If the ghost block is processing. Otherwise, false.</returns>
    public bool IsBeingProcessed() {
        return this.isBeingProcessed;
    }

    public void BlockProcessOn() {
        this.isBlockProcess = true;
    }

    public void BlockProcessOff() {
        this.isBlockProcess = false;
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy() {
		Parameters data = new Parameters ();
		data.PutExtra ("hollowBlock", this);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_BLOCK_DESTROY, data);

        // TODO Check if this is right
        //EventBroadcaster.Instance.RemoveObserver(EventNames.SPAWN_BLOCKS_PROCESS_ON);
        EventBroadcaster.Instance.RemoveObserver(EventNames.SPAWN_BLOCKS_PROCESS_OFF);
        //		this.Break ();
    }

    /// <summary>
	/// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
	/// </summary>
	void Start () {
		if (this.isArbitrary) {
			Init ();
			RegisterBlock ();
		}
	}

    /// <summary>
    /// Sets the fraction value of the ghost block.
    /// 
    /// Its value will be added to the stability number line when this is solved.
    /// </summary>
    /// <param name="fraction">Fraction</param>
    /// <param name="block">Referenced Sky Block, if necessary.</param>
	public void SetFraction(FractionData fraction, SkyBlock block) {
		this.numerator = fraction.numerator;
		this.denominator = fraction.denominator;

		if (block != null) {
			this.SolvedFromPrefill ();
			this.fillCount = (int)this.numerator;
			this.sliceCount = (int)this.denominator;
			this.skyBlock = block;
            this.SetPreFilled(true);
            this.SolvedFromPrefill();
            this.BlockProcessOn();
		} else {
            this.SetPreFilled(false); 
        }
	}

    /// <summary>
    /// Initializes the ghost block's properties.
    /// 
    /// If the ghost block is supposed to be prefilled, prefilling is done as the last process of this function.
    /// </summary>
	public void Init() {
		//originally from Awake
		this.widthWhole = this.rectTransform.rect.width;
		this.widthSingle = this.widthWhole/this.GetDenominator();
		this.widthPiece = this.widthSingle * this.GetNumerator();
		this.height = this.rectTransform.rect.height;

		this.fractionLabel.SetFraction ((int)this.GetNumerator(), (int)this.GetDenominator());
		this.GetStabilityLabel().UpdateLabel (0, (int)this.GetDenominator ());
		this.isSolved = false;

		//originally from Start
		this.InitializeBlockSkins ();
		this.CreatePiece ();
		this.MakeIntangible ();
		this.respawnPosition = gameObject.transform.localPosition;

        /*
		if (SceneObjectsController.Instance != null && SceneObjectsController.Instance.DoesSceneFileExist ()) {
			this.isPreFilled = false;
			if (this.hideSkyBlock && this.skyBlock != null) {
				this.skyBlock.Hide ();
			} else {
				if(this.skyBlock != null)
					this.skyBlock.Show ();
			}
			this.skyBlock = null;
		}
        */
		this.Prefill ();
	}

    /// <summary>
    /// Registers block to the data recorder to be used when the room's stability is being determined.
    /// </summary>
	public void RegisterBlock() {
		Parameters data = new Parameters ();
		data.PutExtra ("hollowBlock", this);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_BLOCK_SPAWN, data);
	}

	public void LockPiece(PlatformLock platform) {
		this.platformLock = platform;
		this.previousParent = gameObject.transform.parent;
		gameObject.transform.SetParent (platform.gameObject.transform);
	}

    /// <summary>
    /// Checker if the sky block should be hidden after it has been used for prefill.
    /// </summary>
    /// <returns></returns>
	public bool IsHiddenSkyBlock() {
		return this.hideSkyBlock;
	}

	public void UnlockPiece(PlatformLock platform) {
		if (this.platformLock == platform) {
			gameObject.transform.SetParent (this.previousParent);
		}
	}

    /// <summary>
    /// Initialize the ghost block's materials/skins.
    /// </summary>
	public void InitializeBlockSkins() {
		SkinPackage skinPackage = GameController_v7.Instance.GetBlockManager ().TakeHollowBlockSkin ();
		this.tangibleMaterial = skinPackage.GetTangibleSkin ();
		this.intangibleMaterial = skinPackage.GetIntangibleSkin ();

		// On purpose
		this.unliftableMaterial = skinPackage.GetTangibleSkin (); 
		this.unliftableOutlineColor = skinPackage.GetOutlineStartColor ();
		//this.unliftableMaterial = skinPackage.GetUnliftableSkin (); 

		this.focusMaterial = skinPackage.GetFocusSkin ();
		this.highlightColor = skinPackage.GetHighlightColor ();
		this.outlineStartColor = skinPackage.GetOutlineStartColor ();
		this.outlineEndColor = skinPackage.GetOutlineEndColor ();
	}

    /// <summary>
    /// Checker if the ghost block is liftable when it is tangible.
    /// </summary>
    /// <returns>If the ghost block is liftable when tangible. Otherwise, false.</returns>
	public bool IsLiftable() {
		return this.isLiftable;
	}

    /// <summary>
    /// Set if the block can be lifted.
    /// </summary>
    /// <param name="canLift"></param>
	public void SetLiftable(bool canLift) {
		this.isLiftable = canLift;
	}


    /// <summary>
    /// Called when filling the box on player death. Lets the skyblock absorb the current pieces before prefilling.
    /// </summary>
    public void DeathPrefill() {
//		this.Break ();
		// Fill only when broken
		if (!IsSolved ()) {
			this.Prefill ();
		}
	}

    /// <summary>
    /// Prefills the ghost block using a referenced sky block.
    /// </summary>
	public void Prefill() {
		if (this.isPreFilled && this.skyBlock != null) {
			if (this.hideSkyBlock) {
				this.skyBlock.Hide ();
			} else {
				this.skyBlock.Show ();
			}

			// Greater fill than slice is not allowed
			if (this.fillCount > this.sliceCount) {
				Debug.Log ("Fill greater than slice");
				this.fillCount = this.sliceCount;
			}
			// Greater detached pieces than slice is not allowed
			if (this.detachCount > this.sliceCount) {
				this.detachCount = this.sliceCount - this.fillCount;
			}

			Debug.Log ("<color=green>DROP</color>");
			this.skyBlock.Drop (this);
		} else if(this.skyBlock != null){
			// Hide SkyBlock if not prefilled
			this.skyBlock.Hide ();
		}
	}
    /// <summary>
    /// Indicate that this ghost block was filled through prefill, excluding it from data tallying.
    /// </summary>
	public void SolvedFromPrefill() {
		this.isSolvedFromPrefill = true;
	}

    /// <summary>
    /// Refills the ghost block with sky fragments from a referenced sky block.
    /// As such, a referenced sky block is needed.
    /// </summary>
	public void Refill() {
		if (this.skyBlock != null && !this.isSolved) {
			foreach (SkyFragmentPiece piece in this.skyBlock.GetDetachedManager().GetComponentsInChildren<SkyFragmentPiece>()) {
				piece.Break ();
			}

			// Greater fill than slice is not allowed
			if (this.fillCount > this.sliceCount) {
				this.fillCount = this.sliceCount;
			}
			// Greater detached pieces than slice is not allowed
			if (this.detachCount > this.sliceCount) {
				this.detachCount = this.sliceCount - this.fillCount;
			}

			Debug.Log ("<color=green>DROP</color>");
			this.skyBlock.Drop (this);
		}
	}
    
	public Vector3 GetDetachedPosition() {
		Vector3 detachedPosition;
		if (this.detachedPositions.Count > 0) {
			detachedPosition = this.detachedPositions[0].position;
			this.detachedPositions.RemoveAt (0);
		} else {
			Debug.Log ("<color=green>DETACH EMPTY</color>");
			detachedPosition = gameObject.transform.position;
		}
		return detachedPosition;
	}

	public int GetFillCount() {
		return this.fillCount;
	}

	public int GetDetachCount() {
		return this.detachCount;
	}

	public int GetSliceCount() {
		return this.sliceCount;
	}

	public Color GetTangibleColor() {
		return this.tangibleColor;
	}
	public Color GetTangibleOutlineColor() {
		return this.tangibleOutlineColor;
	}
	public SpriteRenderer GetSpriteRenderer() {
		return this.spriteRenderer;
	}
	public GameObject GetParent() {
		if (this.parent == null) {
			this.parent = GetComponentInParent<HollowBlockParent> ().gameObject;
			if (this.parent == null) {
				this.parent = GetComponentInParent<Transform> ().gameObject;
			}
		}
		return this.parent;
	}

    /// <summary>
	/// Standard Unity Function. Called once every frame.
	/// </summary>
	void Update() {
		this.UpdateStabilityLabel ();
		this.CarryCheck ();
	}
	public void CarryCheck() {
		if (this.IsCarried () && this.platformLock != null) {
			this.UnlockPiece (this.platformLock);
			this.gameObject.transform.SetParent (this.GetPlayer ().gameObject.transform);
		}
	}

	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GameObject.FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}

	public Rigidbody2D GetRigidBody2D() {
		if(this.rigidBody2D == null) {
			this.rigidBody2D = this.GetComponent<Rigidbody2D>();
		}
		return this.rigidBody2D;
	}

	public HintBubbleHollowStability GetStabilityLabel() {
		if (this.stabilityLabel == null) {
			this.stabilityLabel = GetComponentInChildren<HintBubbleHollowStability> ();
		}
		return this.stabilityLabel;
	}


	public HollowBlockSkyPieceContainer GetSkyPieceContainer() {
		if (this.skyPieceContainer == null) {
			this.skyPieceContainer = GetComponentInChildren<HollowBlockSkyPieceContainer> ();
		}
		return this.skyPieceContainer;
	}

	public float GetStabilityNumerator() {
		return this.GetSkyPieceContainer ().GetStabilityNumerator ();
	}

	public float GetStabilityDenominator() {
		return this.GetSkyPieceContainer ().GetStabilityDenominator ();
	}

    /// <summary>
    /// Absorb a sky fragment. Fills this ghost block.
    /// 
    /// Once the sky fragments add up to the ghost block's value, the ghost block will be filled.
    /// </summary>
    /// <param name="skyPiece">Sky Fragment</param>
	public void Absorb(SkyFragmentPiece skyPiece) {
		if (!isSolved) {
			this.GetSkyPieceContainer ().Absorb (skyPiece);
			this.UpdateStabilityLabel ();
			this.CheckStability ();
		}
	}

    /// <summary>
    /// Checks the sky fragments filling the ghost block.
    /// 
    /// If they add up to the value for tangibility, this ghost block will be solved.
    /// </summary>
	public void CheckStability() {
		this.GetSkyPieceContainer ().UpdateSkyPieceList ();
		this.GetSkyPieceContainer ().UpdateCurrentStability ();

		float[] simplifiedTargetValue = General.SimplifyFraction (this.GetNumerator (), this.GetDenominator ());
//		float[] simplifiedStabilityValue = General.SimplifyFraction (this.GetStabilityNumerator (), this.GetStabilityDenominator ());
		float[] simplifiedStabilityValue = General.SimplifyFraction (this.GetSkyPieceContainer().GetStabilityNumerator(), this.GetSkyPieceContainer().GetStabilityDenominator());

		Debug.Log ("Comparing "+simplifiedTargetValue[0]+" "+simplifiedTargetValue[1]);
		Debug.Log ("With "+simplifiedStabilityValue[0]+" "+simplifiedStabilityValue[1]);

		if (!IsBroken () && simplifiedTargetValue [0] == simplifiedStabilityValue [0] &&
		    simplifiedTargetValue [1] == simplifiedStabilityValue [1]) {
			Debug.Log ("<color=red>IF Not Solved</color>");
			if (!this.isSolved) {
				Debug.Log ("<color=red>Solved</color>");
                if (!isBlockProcess)
                    this.PostHollowBlockEvent(true);
                else {
                    this.PostHollowBlockEventImmediate(true);
                }
				if (!this.isSolvedFromPrefill && !isBlockProcess) {
					this.GetResultsUI ().PlayCraft ();
					Parameters data = new Parameters ();
					data.PutExtra ("hollowBlock", this);
					data.PutExtra ("solved", true);
					EventBroadcaster.Instance.PostEvent (EventNames.RECORD_HOLLOW_BLOCK, data);
				}
			}
			this.SolvedNoPrompt ();
//			this.Solved ();
		} else {
			Debug.Log ("<color=red>ELSE Not Solved</color>");
			if (this.isSolved) {
                if (!isBlockProcess)
                    this.PostHollowBlockEvent(false);
                else {
                    this.PostHollowBlockEventImmediate(false);
                }
                Debug.Log ("<color=red>Not Solved</color>");
				Parameters data = new Parameters ();
				data.PutExtra ("hollowBlock", this);
				data.PutExtra ("solved", false);
				EventBroadcaster.Instance.PostEvent (EventNames.RECORD_HOLLOW_BLOCK, data);
			}
			this.isSolved = false;
		}
		// Immediately set to false after 1 call
		this.isSolvedFromPrefill = false;

//		// Check only if not yet solved
//		if (!isSolved) {
//			float[] simplifiedTargetValue = General.SimplifyFraction (this.GetNumerator (), this.GetDenominator ());
//			float[] simplifiedStabilityValue = General.SimplifyFraction (this.GetStabilityNumerator (), this.GetStabilityDenominator ());
//
//			// Solved
//			if (simplifiedTargetValue [0] == simplifiedStabilityValue [0] &&
//				simplifiedTargetValue [1] ==  simplifiedStabilityValue [1]) {
//				this.Solved ();
//				this.PostHollowBlockEvent (true);
//			}
//		}
//		// If Solved
//		else {
//			float[] simplifiedTargetValue = General.SimplifyFraction (this.GetNumerator (), this.GetDenominator ());
//			float[] simplifiedStabilityValue = General.SimplifyFraction (this.GetStabilityNumerator (), this.GetStabilityDenominator ());
//
//			if (simplifiedTargetValue [0] != simplifiedStabilityValue [0] ||
//				simplifiedTargetValue [1] !=  simplifiedStabilityValue [1]) {
//				//SoundManager.Instance.Play (AudibleNames.Results.MISTAKE, false); TODO Play Sound (Breaking)
//				this.isSolved = false;
//				this.PostHollowBlockEvent (false);
//			}
//		}
	}

    /// <summary>
    /// Solved event but does not broadcast to make the stability number line perform an animation.
    /// </summary>
	public void SolvedNoPrompt() {
		this.isSolved = true;
		this.MakeTangible ();
	}

    /// <summary>
    /// Broadcasts ghost block related events.
    /// </summary>
    /// <param name="isAdd">If its change was an addition operation.</param>
    public void PostHollowBlockEventImmediate(bool isAdd) {
        //this.PostHollowBlockEvent(isAdd);
        Debug.Log("<color=cyan>POST HOLLOW EVENT IMMEDIATE " + isAdd + "</color>");
        Parameters parameters = new Parameters();
        parameters.PutExtra(StabilityNumberLine.NUMERATOR, this.GetNumerator());
        parameters.PutExtra(StabilityNumberLine.DENOMINATOR, this.GetDenominator());
        parameters.PutObjectExtra(StabilityNumberLine.COLOR, this.GetHighlightColor());
        parameters.PutExtra(StabilityNumberLine.IS_ADD, isAdd);
        parameters.PutObjectExtra(StabilityNumberLine.HOLLOW_BLOCK, this);
        EventBroadcaster.Instance.PostEvent(EventNames.ON_HOLLOW_STABILITY_UPDATE_INSTANT, parameters);
    }

    /// <summary>
    /// Broadcasts ghost block related events.
    /// </summary>
    /// <param name="isAdd">If its change was an addition operation.</param>
    public void PostHollowBlockEvent(bool isAdd) {
		Debug.Log ("<color=cyan>POST HOLLOW EVENT "+isAdd+"</color>");
		Parameters parameters = new Parameters ();
		parameters.PutExtra (StabilityNumberLine.NUMERATOR, this.GetNumerator ());
		parameters.PutExtra (StabilityNumberLine.DENOMINATOR, this.GetDenominator ());
		parameters.PutObjectExtra (StabilityNumberLine.COLOR, this.GetHighlightColor ());
		parameters.PutExtra (StabilityNumberLine.IS_ADD, isAdd);
        parameters.PutExtra(StabilityNumberLine.HOLLOW_BLOCK, this);
        EventBroadcaster.Instance.PostEvent (EventNames.ON_HOLLOW_STABILITY_UPDATE, parameters);
	}

	public Color GetHighlightColor() {
		return this.highlightColor;
	}

    /// <summary>
    /// Updates the current filled amount of this ghost block.
    /// </summary>
	public void UpdateStabilityLabel() {
		this.GetSkyPieceContainer ().UpdateSkyPieceList ();
		this.GetSkyPieceContainer().UpdateCurrentStability ();
		this.stabilityLabel.UpdateLabel (
			(int) GetSkyPieceContainer().GetStabilityNumerator(),
			(int) GetSkyPieceContainer().GetStabilityDenominator());
//		this.CheckStability ();
	}
    
    /// <summary>
    /// Create the original piece according to specified numerator and denominator size and then make it intangible.
    /// </summary>
    public void CreatePiece() {
//		this.brokenPiece.SetSize (this.widthPiece, this.height);
		this.spriteRenderer.size = new Vector2 (this.widthPiece, this.height);
		this.boxCollider.size = new Vector2 (this.widthPiece, this.height);
		General.GenerateBoxOutline (this.lineRenderer, this.widthPiece, this.height);
	}

    /// <summary>
    /// Breaks the ghost block and makes it intangible.
    /// </summary>
	public void MakeIntangible() {
		Debug.Log ("<color=red>Make Intangible</color>");
		this.isIntangible = true;
		this.isCarried = false;

		this.rigidBody2D.bodyType = RigidbodyType2D.Kinematic;
		this.rigidBody2D.velocity = Vector2.zero;
		this.rigidBody2D.angularVelocity = 0f;
//		this.boxCollider.enabled = true;
		this.boxCollider.isTrigger = true;
		this.GetSkyPieceContainer ().Show ();


//		this.ChangeColor (this.intangibleColor, this.intangibleOutlineColor);
		this.WearIntangibleSkin();

		gameObject.layer = LayerMask.NameToLayer (DEFAULT_LAYER);
		this.ResetBreak ();

		this.GetStabilityLabel ().WakeUp ();
//		this.GetHintBubble ().Activate ();

//		this.clone.Deactivate ();
	}

	// TODO: Replace with material
	public void ChangeColor(Color fill, Color outline) {
//		this.spriteRenderer.color = fill;
//		this.lineRenderer.startColor = outline;
//		this.lineRenderer.endColor = outline;
	}

	// TODO: Do a null check + contingency
	public Color GetOutlineStartColor() {
		return this.outlineStartColor;
	}

	public Color GetOutlineEndColor() {
		return this.outlineEndColor;
	}

	public Material GetIntangibleMaterial() {
		return this.intangibleMaterial;
	}

	public Material GetTangibleMaterial() {
		return this.tangibleMaterial;
	}
	public Material GetFocusMaterial() {
		return this.focusMaterial;
	}
	public Material GetUnliftableMaterial() {
		return this.unliftableMaterial;
	}

    /// <summary>
    /// Become tangible when solved.
    /// </summary>
	public void MakeTangible() {
		Debug.Log ("<color=red>Make Tangible</color>");
		this.isIntangible = false;
//		this.boxCollider.isTrigger = false;
		this.GetBoxCollider().isTrigger = false;

		this.GetSkyPieceContainer ().Hide ();


//		this.ChangeColor (this.tangibleColor, this.tangibleOutlineColor);
		this.WearTangibleSkin();


		gameObject.layer = LayerMask.NameToLayer (BREAKABLE_LAYER);
//		this.GetHintBubble ().Deactivate ();

		if (!isPurelyKinematic) {
			this.SetDynamicRigidBody ();
		}
		this.GetStabilityLabel ().Sleep ();
	}

    /// <summary>
    /// Set the rigidbody to dynamic.
    /// </summary>
	public void SetDynamicRigidBody() {
		this.rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
		this.rigidBody2D.angularDrag = 0.05f;
		this.rigidBody2D.drag = 80f;
		this.rigidBody2D.gravityScale = 75f;
		this.rigidBody2D.mass = 1f;
	}
	public void DectivateHint() {
//		this.stabilityLabel.GetComponent<BoxCollider2D> ().enabled = false;
//		this.GetStabilityLabel().gameObject.SetActive(false);
//		this.stabilityLabel.gameObject.SetActive (false);
//		this.hintBubble.gameObject.SetActive (false);
	}
	public void ActivateHint() {
//		this.stabilityLabel.HardClose ();
//		this.stabilityLabel.gameObject.SetActive (true);
//		this.hintBubble.gameObject.SetActive (true);
	}

    /// <summary>
    /// Return this block to its respawn position.
    /// </summary>
	public void Respawn() {
//		this.Drop ();
		this.gameObject.transform.localPosition = this.respawnPosition;
	}
    
	public bool CantCarry() {
		return this.cantBeCarried;
	}

    /// <summary>
    /// Player avatar carries this block.
    /// </summary>
    /// <param name="parent">Object carrying the block.</param>
    /// <param name="position">Place where the block is held.</param>
	public void Carry(GameObject parent, Vector3 position) {
		if (this.IsSolved ()) {
			
			if (parent.GetComponent<PlayerYuni> () != null) {
				this.player = parent.GetComponent<PlayerYuni> ();
			}

			this.isCarried = true;
			//			this.respawnPosition = gameObject.transform;
			this.Unobserve ();
			//			this.SetCarried (true);
			//			HideLabel ();
			//			this.hintBubble.HideHint ();
			this.ChangeColor (this.tangibleColor, this.tangibleOutlineColor);
			this.gameObject.layer = LayerMask.NameToLayer (DEFAULT_LAYER);
			//			this.GetStabilityLabel ().Sleep ();
			this.GetRigidBody2D ().bodyType = RigidbodyType2D.Kinematic;
			this.GetRigidBody2D ().velocity = Vector2.zero;
			this.GetRigidBody2D ().angularVelocity = 0f;
			gameObject.transform.SetParent (parent.transform);
			gameObject.transform.localPosition = position;
			gameObject.transform.eulerAngles = new Vector3 (0f, 0f, -45f);
			this.boxCollider.isTrigger = true;

		}
	}

    /// <summary>
    /// Released from being carried.
    /// </summary>
	public void Drop() {
		//		this.SetCarried (false);
		this.isCarried = false;
		this.gameObject.layer = LayerMask.NameToLayer(BREAKABLE_LAYER);
		this.GetRigidBody2D ().bodyType = RigidbodyType2D.Dynamic;
		gameObject.transform.SetParent (this.GetParent().transform);
		gameObject.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
		gameObject.transform.localScale = 
			new Vector3 (Mathf.Abs(gameObject.transform.localScale.x),
				gameObject.transform.localScale.y,
				gameObject.transform.localScale.z);
		this.boxCollider.isTrigger = false;
//		StartCoroutine (DropRoutine());
	}
    
	public bool IsCarried() {
		return this.isCarried;
	}
//	IEnumerator DropRoutine() {
//		Debug.Log ("<color=magenta>DROP ROUTINE ENTER</color>");
//		Collider2D[] colliders;
//		Vector3 prevPosition = Vector3.zero;
//		bool hasAbsorbed = false;
//		// While falling
//		while (!hasAbsorbed && gameObject.transform.position != prevPosition) {
//			//			ContactFilter2D contactFilter = new ContactFilter2D ();
//			//			Physics2D.OverlapBox (this.boxCollider.transform.position, this.boxCollider.size, this.boxCollider.transform.eulerAngles.z, contactFilter, colliders);
//			colliders = Physics2D.OverlapBoxAll (this.boxCollider.transform.position, this.boxCollider.size, this.boxCollider.transform.eulerAngles.z);
//
//			foreach(Collider2D collider in colliders) {
//				if (!hasAbsorbed && collider.gameObject.GetComponent<HollowBlock> () != null) {
//					hasAbsorbed = true;
//					Debug.Log ("<color=blue>HOLLOW BLOCK COLLISION</color>");
//					HollowBlock hollowBlock = collider.gameObject.GetComponent<HollowBlock> ();
//					hollowBlock.Absorb (this);
//				}
//			}
//
//			prevPosition = gameObject.transform.position;
//			yield return null;
//		}
//		Debug.Log ("<color=magenta>DROP ROUTINE EXIT</color>");
//	}

    /// <summary>
    /// Prompts that the ghost block is focused by the player character.
    /// </summary>
	public void Observe() {
		if (IsSolved ()) {
			this.WearFocusSkin ();
		}
	}

    /// <summary>
    /// Prompts that the block is no longer focused by the player character.
    /// </summary>
	public void Unobserve() {
		if (this.IsSolved ()) {
			this.WearTangibleSkin ();
//			this.ChangeColor (this.tangibleColor, this.tangibleOutlineColor);
		} else {
			this.WearIntangibleSkin ();
//			this.ChangeColor (this.intangibleColor, this.intangibleOutlineColor);
		}
	}
	public void WearTangibleSkin() {
		this.GetSpriteRenderer ().material = this.GetTangibleMaterial ();
		this.lineRenderer.startColor = this.GetOutlineStartColor ();
		this.lineRenderer.endColor = this.GetOutlineEndColor ();
	}

	public void WearIntangibleSkin() {
		this.GetSpriteRenderer ().material = this.GetIntangibleMaterial ();
		this.lineRenderer.startColor = this.intangibleOutlineColor;
		this.lineRenderer.endColor = this.intangibleOutlineColor;
	}

	public void WearFocusSkin() {
		if (this.IsPurelyKinematic ()) {
			Debug.Log ("<color=red>Wore Unliftable</color>");
			this.GetSpriteRenderer ().material = this.GetUnliftableMaterial ();
			this.lineRenderer.startColor = this.unliftableOutlineColor;
			this.lineRenderer.endColor = this.unliftableOutlineColor;
		} else {
			Debug.Log ("<color=red>Wore Focus</color>");
			this.GetSpriteRenderer ().material = this.GetFocusMaterial ();
			this.lineRenderer.startColor = this.intangibleOutlineColor;
			this.lineRenderer.endColor = this.intangibleOutlineColor;
		}
	}

	/// <summary>
	/// Break to return fragments to Sky Block,
	/// Release to remove fragments but spawn them in place.
	/// </summary>
	public void Break() {
		this.GetSkyPieceContainer ().Show ();
		if (this.piecesReturnToSkyBlock) {
			Debug.Log ("<color=cyan>Called Break</color>");
			this.GetSkyPieceContainer ().Break ();
		} else {
			Debug.Log ("<color=cyan>Called Rekease</color>");
			this.GetSkyPieceContainer ().Release ();
		}
		this.UpdateStabilityLabel ();
		this.MakeIntangible ();
		this.isBroken = true;
		this.CheckStability ();
		this.ResetBreak ();
//		this.isSolved = false; // Don't change stability before stability check;
	}

    /// <summary>
    /// Set if the fragments will return to their original sky block upon this block breaking.
    /// </summary>
    /// <param name="willReturn"></param>
	public void SetPiecesReturnToSkyBlock(bool willReturn) {
		this.piecesReturnToSkyBlock = willReturn;
	}

    /// <summary>
    /// Resets this ghost block when the player avatar dies.
    /// </summary>
	public void DeathReturn() {
		Debug.Log ("Death Return");
		this.isBroken = true;
		this.GetSkyPieceContainer ().Return ();
		this.UpdateStabilityLabel ();
		this.MakeIntangible ();
		this.CheckStability ();
		this.ResetBreak ();
	}

	public float GetHeight() {
		return this.height;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!this.isSolved) {
			// Only needle can interact on trigger
			if (other.GetComponent<NeedleController> () != null) {
				NeedleController needle = other.GetComponent<NeedleController> ();
				//			this.HitEvents (needle);
				//			this.Interact (needle.GetSliceCount());
			}

		}
	}
	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.gameObject.GetComponent<BreakerCollider> () != null &&
			this.IsSolved()) {

			this.boxCollider.isTrigger = true; // To prevent multiple collisions
			BreakerCollider breaker = other.collider.gameObject.GetComponent<BreakerCollider> ();
			breaker.HitHollowBlock ();
			this.Break ();
			//			this.HitEvents (needle);
			//			this.Interact (needle.GetSliceCount());
		}
	}

    /// <summary>
    /// Use up break flag.
    /// </summary>
	public void ResetBreak() {
		this.isBroken = false;
	}
    
	public bool IsBroken() {
		return this.isBroken;
	}

	public BoxCollider2D GetBoxCollider() {
		if (this.boxCollider == null) {
			this.boxCollider = GetComponent<BoxCollider2D> ();
		}
		return this.boxCollider;
	}

//	public void HitEvents(NeedleController needle) {
//		// Continued by BrokenClone Deactivat()
//		GameController_v7.Instance.GetPauseController().Pause ();
//		needle.hasHit = needle.onlyHitOnce;
//
//		#if UNITY_ANDROID
//		//		EventManager.DisableJumpButton ();
//		//		EventManager.ToggleSwitchButton (false);
//		//		GameController_v7.Instance.GetEventManager().ToggleLeftRightButtons(false);
//
//		EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_JUMP);
//		Parameters parameters = new Parameters ();
//		parameters.PutExtra ("FLAG", false);
//		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, parameters);
//		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, parameters);
//		#endif
//		// TODO Record Data?
//	}

	// Needle interaction. Can only interact if intangible
//	public void Interact(SkyFragmentPiece skyPiece) {
//		if (this.isIntangible) {
//			// Add to current stability
//		}
//	}

	// Correct if the clone is equal to the value of this object
	public bool IsCorrect(float cloneNumerator, float cloneDenominator) {
//		if (this.numerator / this.denominator == cloneNumerator / cloneDenominator) {

//			return true;
//		} else {
			return false;
//		}
	}

	public ResultsUI GetResultsUI() {
		if (this.resultsUI == null) {
			this.resultsUI = GetComponentInChildren<ResultsUI> ();
		}
		return this.resultsUI;
	}

//	public HintBubbleManager GetHintBubble() {
//		if (this.hintBubble == null) {
//			this.hintBubble = GetComponentInChildren<HintBubbleManager> ();
//		}
//		return this.hintBubble;
//	}

	// Called when the crafted clone is correct
	public void Solved() {
		this.GetResultsUI ().PlayCraft ();
		this.SolvedNoPrompt ();
	}

//	public BrokenPiece GetBrokenPiece() {
//		return this.brokenPiece;
//	}

	public float GetNumerator() {
		if (this.numerator<= 0)
			this.numerator = 1;
		return this.numerator;
	}

	public float GetDenominator() {
		if (this.denominator <= 0)
			this.denominator = 1;
		return this.denominator;
	}

    /// <summary>
    /// Returns the fraction value which is the value its filled fragments should add up to solve it.
    /// </summary>
    /// <returns>Fraction Value</returns>
	public FractionData GetFraction() {
		FractionData fraction = new FractionData ();
		fraction.numerator = (int)this.GetNumerator ();
		fraction.denominator = (int)this.GetDenominator ();

		return fraction;
	}

	public float GetWidthPiece() {
		return this.widthPiece;	
	}
	public float GetWidthWhole() {
		return this.widthWhole;	
	}

	public bool IsSolved() {
		return this.isSolved;
	}
		
	public int GetSceneObjectId() {
		return this.sceneObjectId;
	}

	public void SetSceneObjectId(int id) {
		this.sceneObjectId = id;
	}

	public bool IsPurelyKinematic() {
		return this.isPurelyKinematic;
	}

	public bool IsPreFilled() {
		return this.isPreFilled;
	}
	public void SetPreFilled(bool flag) {
		this.isPreFilled = flag;
	}
}
