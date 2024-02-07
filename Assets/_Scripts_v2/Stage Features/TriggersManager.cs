using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles triggers assigned to this manager.
/// </summary>
public class TriggersManager : MonoBehaviour {

    /// <summary>
    /// Referece to any triggerables assigned to the manager.
    /// </summary>
	[SerializeField] private List<Triggerable> triggerables;
    /// <summary>
    /// Reference to ghost blocks that will be triggered in a certain way depending on the scenario.
    /// </summary>
	[SerializeField] private List<HollowBlock> triggers;

    /// <summary>
    /// Flag if the manager has been triggered.
    /// </summary>
	private bool triggered = false;
    /// <summary>
    /// Previous state of the manager if it was in a triggered or disarmed state.
    /// </summary>
	private bool previousState;

    /// <summary>
	/// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
	/// </summary>
	void Start() {
		previousState = triggered;
		if (triggered) {
			foreach (Triggerable triggerable in triggerables) {
				triggerable.Trigger ();
			}
		} else {
			foreach (Triggerable triggerable in triggerables) {
				triggerable.Disarm ();
			}
		}
	}

    /// <summary>
    /// Standard Unity Function. Called once every frame.
    /// </summary>
	void Update() {
		CheckTriggers ();
	}

    /// <summary>
    /// Checks if any block is unsolved. All triggerables are triggered if all are solved. Disarms them otherwise.
    /// </summary>
    private void CheckTriggers() {
		foreach (HollowBlock block in triggers) {
			if (!block.IsSolved ()) {
				triggered = false;
				break;
			} else
				triggered = true;
		}

		if (triggered != previousState) {
			//Trigger once
			previousState = triggered;

			if (triggered) {
				foreach (Triggerable triggerable in triggerables) {
					triggerable.Trigger ();
				}
			} else {
				foreach (Triggerable triggerable in triggerables) {
					triggerable.Disarm ();
				}
			}
		}
	}
}
