using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Behaviour for the background whenever stability is changed in the room.
/// </summary>
public class StabilityBackground : MonoBehaviour {

	/// <summary>
	/// The children objects that contribute to the look and feel of stability.
	/// </summary>
	[SerializeField] private List<StableElement> childrenObjects;

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		this.childrenObjects = GetComponentsInChildren<StableElement> ().ToList ();
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
	/// Stabilize call to execute functions relating to a stable room.
	/// </summary>
	public void Stabilize() {
		this.EnableChildren ();
	}

	/// <summary>
	/// Destabilize call to execute functions relating to an unstable room.
	/// </summary>
	public void Destabilize() {
		this.DisableChildren ();
	}

	/// <summary>
	/// Disables the children.
	/// </summary>
	public void DisableChildren() {
		foreach (StableElement child in this.GetChildrenObjects()) {
			child.gameObject.SetActive (false);
		}
	}

	/// <summary>
	/// Enables the children.
	/// </summary>
	public void EnableChildren() {
		foreach (StableElement child in this.GetChildrenObjects()) {
			child.gameObject.SetActive (true);
		}
	}

	/// <summary>
	/// Gets the children objects.
	/// </summary>
	/// <returns>The children objects.</returns>
	public List<StableElement> GetChildrenObjects() {
		if (this.childrenObjects == null) {
			this.childrenObjects = new List<StableElement> ();
		}
		return this.childrenObjects;
	}
}
