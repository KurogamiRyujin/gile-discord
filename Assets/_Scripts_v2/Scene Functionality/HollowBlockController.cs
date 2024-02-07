using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the instantiation and destruction of a room's ghost blocks (formerly called hollow blocks).
/// </summary>
public class HollowBlockController : MonoBehaviour {

	/// <summary>
	/// The hollow block prefab.
	/// </summary>
	[SerializeField] private HollowBlockParent hollowBlockPrefab;

	/// <summary>
	/// Number of hollow blocks to spawn
	/// </summary>
	[SerializeField] private int count;
	/// <summary>
	/// The hollow blocks.
	/// </summary>
	[SerializeField] private List<HollowBlockParent> hollowBlocks = new List<HollowBlockParent> ();
	/// <summary>
	/// The block placements.
	/// 
	/// Block placements must always be preset.
	/// </summary>
	[SerializeField] private List<Transform> blockPlacements = new List<Transform> ();
//	[SerializeField] private bool isPreset;

	/// <summary>
	/// The number line.
	/// </summary>
	private StabilityNumberLine numberLine;
	/// <summary>
	/// Flag if blocks are being spawned.
	/// </summary>
	private bool isSpawning = false;

	/// <summary>
	/// Flag to raise if a fatal error somehow happened.
	/// </summary>
	private bool emergency = false;
	/// <summary>
	/// Max point to be given to the stability number line.
	/// </summary>
    private int maxPoint;

	/// <summary>
	/// Standard Unity function. Initializes the instance upon scene loading.
	/// </summary>
	void Awake() {
		if ((/*!isPreset || */this.hollowBlocks.Count == 0) && this.blockPlacements.Count > 0) {
//			this.isPreset = false;
			hollowBlocks = new List<HollowBlockParent> ();
		} else if (/*!isPreset && */this.blockPlacements.Count <= 0) {
			Debug.LogError ("BLOCK PLACEMENTS NEED TO BE SET!");
			this.emergency = true;
		}
	}

	/// <summary>
	/// Standard Unity Function. Used to initialize the instance if it is enabled. Only called once in its life.
	/// </summary>
	void Start() {
		this.numberLine = FindObjectOfType<StabilityNumberLine> ();
	}

	/// <summary>
	/// Determines the number of the blocks to be spawned in the room.
	/// </summary>
	/// <returns>The count.</returns>
	public int DetermineCount() {
//		if (!this.isPreset)
			this.count = Random.Range (2, this.blockPlacements.Count + 1);

		return this.count;
	}

    //spawn blocks according to count
    /*
	public void SpawnBlocks(SceneTopic topic, SkyBlock[] skyBlocks) {
//		if (this.isPreset) {
//			for (int i = 0; i < this.hollowBlocks.Count; i++) {
//				if (topic == SceneTopic.SIMILAR_SUB || topic == SceneTopic.EQUIVALENT_SUB || topic == SceneTopic.DISSIMILAR_SUB) {
//					this.hollowBlocks[i].GetHollowBlock ().SetPreFilled (true);
//					this.hollowBlocks[i].GetHollowBlock ().SetFraction (PedagogicalComponent_v2.Instance.RequestFraction (), skyBlocks [i]);
//				} else {
//					this.hollowBlocks[i].GetHollowBlock ().SetPreFilled (false);
//					this.hollowBlocks[i].GetHollowBlock ().SetFraction (PedagogicalComponent_v2.Instance.RequestFraction (), null);
//				}
//				this.hollowBlocks[i].GetHollowBlock ().Init ();
//				this.hollowBlocks[i].GetHollowBlock ().RegisterBlock ();
//			}
//		} else {
			if (!this.IsDoneSpawning ()) {
				if (this.count > blockPlacements.Count)
					this.count = blockPlacements.Count;

				int spawnCount = this.count - hollowBlocks.Count;

				for (int i = 0; i < spawnCount; i++) {
					HollowBlockParent temp = Instantiate<HollowBlockParent> (this.hollowBlockPrefab);
					if (topic == SceneTopic.SIMILAR_SUB || topic == SceneTopic.EQUIVALENT_SUB || topic == SceneTopic.DISSIMILAR_SUB) {
						temp.GetHollowBlock ().SetPreFilled (true);
						temp.GetHollowBlock ().SetFraction (PedagogicalComponent_v2.Instance.RequestFraction (), skyBlocks [i]);
					} else {
						temp.GetHollowBlock ().SetPreFilled (false);
						temp.GetHollowBlock ().SetFraction (PedagogicalComponent_v2.Instance.RequestFraction (), null);
					}
					temp.GetHollowBlock ().Init ();
					temp.GetHollowBlock ().RegisterBlock ();

					temp.transform.position = blockPlacements [i].position;
					hollowBlocks.Add (temp);
				}
			}
//		}
	}
    */
    //public IEnumerator SpawnBlocks(SceneTopic topic, SkyBlock[] skyBlocks) {
    //    if (!this.IsDoneSpawning()) {
    //        if (this.count > blockPlacements.Count)
    //            this.count = blockPlacements.Count;

    //        int spawnCount = this.count - hollowBlocks.Count;


    //        EventBroadcaster.Instance.PostEvent(EventNames.SPAWN_BLOCKS_PROCESS_ON);
    //        for (int i = 0; i < spawnCount; i++) {
    //            HollowBlockParent temp = Instantiate<HollowBlockParent>(this.hollowBlockPrefab);
    //            if (topic == SceneTopic.SIMILAR_SUB || topic == SceneTopic.EQUIVALENT_SUB || topic == SceneTopic.DISSIMILAR_SUB) {

    //                EventBroadcaster.Instance.PostEvent(EventNames.REQUEST_UPDATE_SESSION);
    //                temp.GetHollowBlock().SetPreFilled(true);
    //                temp.GetHollowBlock().SolvedFromPrefill();
    //                temp.GetHollowBlock().BlockProcessOn();
    //                temp.GetHollowBlock().SetFraction(PedagogicalComponent_v2.Instance.RequestFraction(), skyBlocks[i]);

    //            }
    //            else {
    //                temp.GetHollowBlock().SetPreFilled(false);
    //                temp.GetHollowBlock().SetFraction(PedagogicalComponent_v2.Instance.RequestFraction(), null);
    //            }
    //            temp.GetHollowBlock().Init();
    //            temp.GetHollowBlock().RegisterBlock();

    //            temp.transform.position = blockPlacements[i].position;
    //            hollowBlocks.Add(temp);

    //            if (temp.GetHollowBlock().IsPreFilled()) {
    //                while (temp.GetHollowBlock().IsBeingProcessed()) {
    //                    yield return null;
    //                }

    //            }
    //            yield return null;
    //        }
    //    }
    //    EventBroadcaster.Instance.PostEvent(EventNames.REQUEST_UPDATE_SESSION);

    //    //EventBroadcaster.Instance.PostEvent(EventNames.RECONFIGURE_HIGHLIGHTS);
    //    yield return null;
    //}

	/// <summary>
	/// Computes the max point to be given to the stability number line.
	/// </summary>
	/// <returns>The max point.</returns>
	/// <param name="fractionDataList">Fractions</param>
    public IEnumerator ComputeMaxPoint(List<FractionData> fractionDataList) {
        float rawMaxPoint = 0;
        for (int i = 0; i < fractionDataList.Count; i++) {
            rawMaxPoint += (fractionDataList[i].denominator / fractionDataList[i].denominator);
            yield return null;
        }
        Debug.LogError("COMPUTE RAW MAX GAVE "+ rawMaxPoint);
        Debug.LogError("COMPUTE MAX GAVE " + Mathf.CeilToInt(rawMaxPoint));
        this.maxPoint = Mathf.CeilToInt(rawMaxPoint);
        yield return null;
    }

	/// <summary>
	/// Spawns the blocks.
	/// </summary>
	/// <returns>None</returns>
	/// <param name="topic">Topic</param>
	/// <param name="skyBlocks">Sky blocks</param>
	/// <param name="numberLine">Number line</param>
    public IEnumerator SpawnBlocks(SceneTopic topic, SkyBlock[] skyBlocks, StabilityNumberLine numberLine) {
        if (!this.IsDoneSpawning()) {
            if (this.count > blockPlacements.Count)
                this.count = blockPlacements.Count;

            int spawnCount = this.count - hollowBlocks.Count;
            List<FractionData> fractionDataList = new List<FractionData>();
            for (int i = 0; i < spawnCount; i++) {
                fractionDataList.Add(PedagogicalComponent_v2.Instance.RequestFraction());
                yield return null;
            }
            yield return ComputeMaxPoint(fractionDataList);
            numberLine.SetMaxPoint(this.maxPoint);
            EventBroadcaster.Instance.PostEvent(EventNames.SPAWN_BLOCKS_PROCESS_ON);
            for (int i = 0; i < spawnCount; i++) {
                HollowBlockParent temp = Instantiate<HollowBlockParent>(this.hollowBlockPrefab);
                if (topic == SceneTopic.SIMILAR_SUB || topic == SceneTopic.EQUIVALENT_SUB || topic == SceneTopic.DISSIMILAR_SUB) {

                    PostSetMaxPointEvent();
                    //temp.GetHollowBlock().SetPreFilled(true);
                    //temp.GetHollowBlock().SolvedFromPrefill();
                    //temp.GetHollowBlock().BlockProcessOn();
                    //temp.GetHollowBlock().SetFraction(PedagogicalComponent_v2.Instance.RequestFraction(), skyBlocks[i]);
                    temp.GetHollowBlock().SetFraction(fractionDataList[i], skyBlocks[i]);

                }
                else {
                    temp.GetHollowBlock().SetPreFilled(false);
                    temp.GetHollowBlock().SetFraction(fractionDataList[i], null);
                    //temp.GetHollowBlock().SetFraction(PedagogicalComponent_v2.Instance.RequestFraction(), null);
                }
                temp.GetHollowBlock().Init();
                temp.GetHollowBlock().RegisterBlock();

                temp.transform.position = blockPlacements[i].position;
                hollowBlocks.Add(temp);

                if (temp.GetHollowBlock().IsPreFilled()) {
                    while (temp.GetHollowBlock().IsBeingProcessed()) {
                        yield return null;
                    }

                }
                yield return null;
            }
        }

        //EventBroadcaster.Instance.PostEvent(EventNames.RECONFIGURE_HIGHLIGHTS);
        yield return null;
    }

	/// <summary>
	/// Broadcasts the max point to be used by the room's stability number line.
	/// </summary>
    public void PostSetMaxPointEvent() {
        Debug.LogError("POSTED PARAM with value " + this.maxPoint);
        Parameters parameters = new Parameters();
        parameters.PutExtra(StabilityNumberLine.MAX_POINT, this.maxPoint);
        EventBroadcaster.Instance.PostEvent(EventNames.SET_MAX_POINT, parameters);
    }

	/// <summary>
	/// Purges the blocks.
	/// </summary>
	/// <returns>None</returns>
    public IEnumerator PurgeBlocks() {
        Debug.LogError("hollowblock Count is " + this.hollowBlocks.Count);
        if (this.hollowBlocks != null && this.hollowBlocks.Count > 0) {
            int i = 0;
            Debug.LogError("Enter purge");
            foreach (HollowBlockParent block in this.hollowBlocks) {
                if (block != null && block.GetHollowBlock().IsSolved()) {
                    block.GetHollowBlock().ProcessBlock();
                    block.GetHollowBlock().BlockProcessOn();
                    block.GetHollowBlock().Break();
                    
                    while (block.GetHollowBlock().IsBeingProcessed()) {
                        yield return null;
                    }
                    //block.GetHollowBlock().BlockProcessOff();
                }

                Debug.LogError("Purge " + i);
                i++;
                Destroy(block.gameObject);
                yield return null;
            }

            this.hollowBlocks.Clear();
        }
        yield return null;
    }

    //   public void PurgeBlocks() {
    //	if (this.hollowBlocks != null && this.hollowBlocks.Count > 0) {
    //		foreach (HollowBlockParent block in this.hollowBlocks) {
    //			if (block != null)
    //				block.GetHollowBlock ().Break ();

    //				Destroy (block.gameObject);
    //		}

    //		this.hollowBlocks.Clear ();
    //	}
    //}

	/// <summary>
	/// Determines whether this instance is done spawning the blocks.
	/// </summary>
	/// <returns><c>true</c> if this instance is done spawning; otherwise, <c>false</c>.</returns>
    public bool IsDoneSpawning() {
		return (this.count == this.hollowBlocks.Count) ? true : false;
	}

//	public bool IsPreset() {
//		return this.isPreset;
//	}

	/// <summary>
	/// Checker if the controller encountered a problem.
	/// </summary>
	public bool Emergency() {
		return this.emergency;
	}

	/// <summary>
	/// Number of blocks currently instantiated.
	/// </summary>
	/// <returns>Number of Blocks</returns>
	public int BlockCount() {
		return this.count;
	}
}
