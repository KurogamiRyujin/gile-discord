using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for UI glowing behaviour.
/// Used to emphasize attention to a UI element when its function is being explained in the tutorial.
/// </summary>
public class TutorialGlow : MonoBehaviour {
    /// <summary>
    /// Reference to the tutorial animatable.
    /// </summary>
	[SerializeField] protected TutorialAnimatable tutorialAnimatable;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		this.Initialize ();
	}

    /// <summary>
    /// Initializes the behaviour.
    /// </summary>
	protected void Initialize() {
		this.tutorialAnimatable = GetComponent<TutorialAnimatable> ();
	}

    /// <summary>
    /// Activate glow.
    /// </summary>
	public void Glow() {
		this.GetTutorialAnimatable ().Glow ();
		Debug.Log ("<color=red>CALLED GLOW 5</color>");
	}

    /// <summary>
    /// Stop glow.
    /// </summary>
	public void Close() {
		this.GetTutorialAnimatable ().Close ();
	}

    /// <summary>
    /// Returns the tutorial animatable.
    /// </summary>
    /// <returns>Tutorial Animatable</returns>
	public TutorialAnimatable GetTutorialAnimatable() {
		if(this.tutorialAnimatable == null) {
			this.tutorialAnimatable = GetComponent<TutorialAnimatable> ();
		}
		return this.tutorialAnimatable;
	}
}
