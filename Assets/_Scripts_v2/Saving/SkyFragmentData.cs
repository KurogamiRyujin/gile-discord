using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SkyFragmentData {

    /// <summary>
    /// Unique id of sky fragment
    /// </summary>
    public int id;
    /// <summary>
    /// Numerator value of the sky fragment piece
    /// </summary>
	public float numerator;
    /// <summary>
    /// Denominator value of the sky fragment piece
    /// </summary>
	public float denominator;
    /// <summary>
    /// Reference to the sky fragment piece's parent sky block
    /// </summary>
	public int skyBlockParentID;
}
