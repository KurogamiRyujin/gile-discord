using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Broken piece that splits off from the breakable object upon breaking.
/// </summary>
public class BreakablePiece : MonoBehaviour {

    /// <summary>
    /// Minimum time piece stays on scene.
    /// </summary>
	private float LIFE_TIME_MIN = 1.0f;
    /// <summary>
    /// Maximum time pieces stays on scene.
    /// </summary>
	private float LIFE_TIME_MAX = 3.6f;
    /// <summary>
    /// Time piece stays on scene.
    /// </summary>
	private float LIFE_TIME = 3.0f;
    /// <summary>
    /// Time piece broke off.
    /// </summary>
	private float timeStart = 0.0f;

    /// <summary>
    /// Standard Unity Function. Called once every frame.
    /// </summary>
	void Update() {
		if ((Time.time - this.timeStart) > LIFE_TIME) {
			this.Expire ();	
		}
	}

    /// <summary>
    /// Destroys this MonoBehaviour's game object.
    /// </summary>
	private void Expire() {
		Destroy (this.gameObject);
	}
    
    /// <summary>
    /// Breaks off from the breakable object.
    /// </summary>
	public void Activate() {
		this.LIFE_TIME = Random.Range (LIFE_TIME_MIN, LIFE_TIME_MAX);

		gameObject.SetActive (true);
		this.timeStart = Time.time;
	}
}
