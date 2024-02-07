using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilityDependentPlatform : MonoBehaviour {
    
	[SerializeField] private List<Triggerable> platforms;
    
	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, this.Stabilize);
		EventBroadcaster.Instance.AddObserver (EventNames.UNSTABLE_AREA, this.Destabilize);
	}
    
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.STABLE_AREA, this.Stabilize);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.UNSTABLE_AREA, this.Destabilize);
	}
    
	private void Stabilize() {
		foreach (Triggerable platform in platforms) {
			platform.Trigger ();
		}
	}
    
	private void Destabilize() {
		foreach (Triggerable platform in platforms) {
			platform.Disarm ();
		}
	}
}
