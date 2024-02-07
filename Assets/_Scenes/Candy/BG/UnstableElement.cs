using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unstable element.
/// </summary>
public class UnstableElement : MonoBehaviour {

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, Stabilize);
		EventBroadcaster.Instance.AddObserver (EventNames.UNSTABLE_AREA, Destabilize);
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.STABLE_AREA);
		EventBroadcaster.Instance.RemoveObserver (EventNames.UNSTABLE_AREA);
	}

	/// <summary>
	/// Stabilize call to execute functions to make the room look and feel stable.
	/// </summary>
	public void Stabilize() {
		gameObject.SetActive (false);
	}

	/// <summary>
	/// Destabilize call to execute functions to make the room look and feel unstable.
	/// </summary>
	public void Destabilize() {
		gameObject.SetActive (true);
		
	}

}
