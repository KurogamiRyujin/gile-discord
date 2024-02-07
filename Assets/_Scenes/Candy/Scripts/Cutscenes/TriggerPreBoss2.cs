using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPreBoss2 : SingleTriggerObserver {

    void Awake() {
        base.OnAwake();
    }

    public override void Action() {
        Debug.Log("No Action Taken");
    }
}
