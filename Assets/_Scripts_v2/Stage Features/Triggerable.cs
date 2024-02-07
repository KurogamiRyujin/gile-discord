using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for game objects that can be triggered.
/// </summary>
public abstract class Triggerable : MonoBehaviour {
    /// <summary>
    /// Launch the event when this game object is triggered.
    /// </summary>
	public abstract void Trigger();
    /// <summary>
    /// Prevent this game object from triggering again.
    /// </summary>
	public abstract void Disarm();
}
