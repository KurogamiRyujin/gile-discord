using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Data for a ghost block's state. Parsed as a JSON to be saved as a text file.
/// </summary>
[Serializable]
public class HollowBlockData {
    /// <summary>
    /// Name of the hollow block
    /// </summary>
	public string name;
    /// <summary>
    /// Unique id of the hollow block
    /// </summary>
	public int id;
    /// <summary>
    /// X position of the hollow block
    /// </summary>
	public float xPos;
    /// <summary>
    /// Y position of the hollow block
    /// </summary>
	public float yPos;
    /// <summary>
    /// Flag if the hollow block is already solved
    /// </summary>
	public bool isSolved;
    //	public int skyBlockParentID;
    /// <summary>
    /// List of data of all sky fragment pieces attached to the hollow block
    /// </summary>
    public List<SkyFragmentData> hollowBlockPieces = new List<SkyFragmentData>();
}
