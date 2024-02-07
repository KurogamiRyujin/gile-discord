using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedNumberLine : MonoBehaviour {
	[SerializeField] private LineRenderer lineRenderer;
	[SerializeField] private StableAreaAnimatable stableAreaAnimatable;
    //private bool isBlockProcess;
	void Awake() {
		this.stableAreaAnimatable = GetComponent<StableAreaAnimatable> ();
		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, StableArea);
		EventBroadcaster.Instance.AddObserver (EventNames.UNSTABLE_AREA, UnstableArea);

        //EventBroadcaster.Instance.AddObserver(EventNames.SPAWN_BLOCKS_PROCESS_ON, BlockProcessOn);
        //EventBroadcaster.Instance.AddObserver(EventNames.SPAWN_BLOCKS_PROCESS_OFF, BlockProcessOff);
    }

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.STABLE_AREA);
		EventBroadcaster.Instance.RemoveObserver (EventNames.UNSTABLE_AREA);

        EventBroadcaster.Instance.RemoveObserver(EventNames.SPAWN_BLOCKS_PROCESS_ON);
        EventBroadcaster.Instance.RemoveObserver(EventNames.SPAWN_BLOCKS_PROCESS_OFF);
    }

    //public void BlockProcessOn() {
    //    this.isBlockProcess = true;
    //}
    //public void BlockProcessOff() {
    //    this.isBlockProcess = false;
    //}
    public void StableArea() {
        //if (!isBlockProcess) {
            //		this.lineRenderer.enabled = true;
            this.stableAreaAnimatable.Stabilize();
        //}
		Debug.Log ("<color=green>STAB</color>");
	}

	public void UnstableArea() {
        //if (!isBlockProcess) {
            //		this.lineRenderer.enabled = false;
            this.stableAreaAnimatable.Destabilize();
        //}
		Debug.Log ("<color=green>DES</color>");
	}


	public LineRenderer GetLineRenderer() {
		if (this.lineRenderer == null) {
			this.lineRenderer = GetComponentInChildren<FixedLineRenderer> ().gameObject.GetComponent<LineRenderer> ();
		}
		return this.lineRenderer;
	}
}
