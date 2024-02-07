using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Data for a sky block's state. Parsed as a JSON to be saved as a text file.
/// 
/// Shows the state the sky block was at upon leaving the room.
/// </summary>
[Serializable]
public class SkyBlockData {
    /// <summary>
    /// Name of the sky block
    /// </summary>
	public string name;
    /// <summary>
    /// Unique id of the sky block
    /// </summary>
	public int id;
    /// <summary>
    /// X position the sky block was.
    /// </summary>
	public float xPos;
    /// <summary>
    /// Y position the sky block was.
    /// </summary>
	public float yPos;
    /// <summary>
    /// Sky fragment data for the attached piece.
    /// </summary>
	public SkyFragmentData attachedPiece;
    /// <summary>
    /// List of sky fragment data for all detached pieces.
    /// </summary>
	public List<SkyFragmentData> detachedPieces;
}
