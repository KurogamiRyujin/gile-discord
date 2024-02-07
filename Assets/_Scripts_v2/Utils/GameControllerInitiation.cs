using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initializes the Game Controller (V7) singleton.
/// </summary>
public class GameControllerInitiation : MonoBehaviour {

    /// <summary>
    /// Unity Function. Called once upon creation of the object.
    /// </summary>
    void Awake () {
		GameController_v7.Instance.GetPauseController ();
	}
}
