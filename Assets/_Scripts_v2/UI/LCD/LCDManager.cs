using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDManager {
	private LCDInterface_v3 lcdInterfaceReference;

	public void SubscribeLCDInterface(LCDInterface_v3 lcdInterface) {
		this.lcdInterfaceReference = lcdInterface;
	}

	public void UnsubscribeLCDInterface(LCDInterface_v3 lcdInterface) {
		//only the interface that subscribed can unsubscribe
		if (lcdInterface == this.lcdInterfaceReference)
			lcdInterfaceReference = null;
	}

	public void InitiateLCD(int yarnballDenominator, EnemyHealth enemyHealth, HammerObject hammer) {
		if (this.lcdInterfaceReference != null)
			this.lcdInterfaceReference.InitiateLCD (yarnballDenominator, enemyHealth, hammer);
	}
}
