using System;
using System.Collections.Generic;

/// <summary>
/// Data for a room. Parsed as a JSON to be saved as a text file.
/// </summary>
[Serializable]
public class SceneData {
    /// <summary>
    /// Time the room was stabilized. Timestamp saved as a string.
    /// </summary>
	public string timeSolved;
//	public List<PartitionableObjectData> boxes = new List<PartitionableObjectData>();
    /// <summary>
    /// List of data of all ghost blocks in the room.
    /// </summary>
	public List<HollowBlockData> hollowBlocks = new List<HollowBlockData>();
    /// <summary>
    /// List of data of all sky blocks in the room.
    /// </summary>
	public List<SkyBlockData> skyBlocks = new List<SkyBlockData>();
}