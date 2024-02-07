using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Data for the the player avatar's state. Parsed as a JSON to be saved as a text file.
/// 
/// Shows the state the player avatar was at upon saving.
/// </summary>
[Serializable]
public class YuniData {
    /// <summary>
    /// Flag if the player avatar can attack.
    /// </summary>
	public bool couldAttack;
    /// <summary>
    /// Flag if the player avatar has the needle.
    /// </summary>
	public bool hasNeedle;
    /// <summary>
    /// Flag if the player avatar has the hammer.
    /// </summary>
	public bool hasHammer;
    /// <summary>
    /// Flag if the player avatar has the thread.
    /// </summary>
	public bool hasThread;
    /// <summary>
    /// Flag if the player avatar has the needle set as the equipped weapon.
    /// </summary>
	public bool usingNeedle;
    /// <summary>
    /// Flag if the player avatar has the needle set as the equipped weapon.
    /// </summary>
	public bool usingHammer;
    /// <summary>
    /// Player avatar's HP.
    /// </summary>
	public float hp;
    /// <summary>
    /// Player avatar's max HP.
    /// </summary>
	public float maxHp;
    /// <summary>
    /// Player avatar's equipped denominator.
    /// </summary>
	public int equippedDenom;
    /// <summary>
    /// Denominator used by the needle.
    /// </summary>
	public int needleDenom;
    /// <summary>
    /// UNUSED
    /// 
    /// Denominator used by the hammer.
    /// </summary>
	public int hammerDenom;
    /// <summary>
    /// UNUSED
    /// 
    /// Previous denominator used by the hammer.
    /// </summary>
	public int hammerPrevDenom;
    /// <summary>
    /// UNUSED
    /// 
    /// Hammer's current charge.
    /// </summary>
	public float hammerCharge;
}