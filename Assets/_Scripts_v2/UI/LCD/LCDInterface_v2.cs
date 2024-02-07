using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDInterface_v2 : MonoBehaviour {

	[SerializeField] private LCDBlockHolder lcdBlockHolderPrefab;

	[SerializeField] private Transform yarnballBlockPos;
	[SerializeField] private Transform enemyValueBlockPos;

	[SerializeField] private float maxBlockLength = 5.0f;//measurement is in Unity Scale Transform so its quite big

	private LCDBlockHolder yarnballBlock;
	private LCDBlockHolder enemyValueBlock;
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitiateBlocks(int yarnballVal, int enemyVal) {
		int lcd = LCD (yarnballVal, enemyVal);
		int yarnballLCDStepCount = lcd / yarnballVal;
		int enemyLCDStepCount = lcd / enemyVal;
		int maxOverallStepCounts = yarnballLCDStepCount + enemyLCDStepCount;
		int multiplier = 2;
		if (yarnballLCDStepCount > enemyLCDStepCount)
			multiplier = (maxOverallStepCounts / enemyLCDStepCount) + 1;
		else
			multiplier = (maxOverallStepCounts / yarnballLCDStepCount) + 1;
		int maxYarnballLCDStepCount = yarnballLCDStepCount * multiplier;
		int maxEnemyLCDStepCount = enemyLCDStepCount * multiplier;

		this.yarnballBlock = Instantiate<LCDBlockHolder> (lcdBlockHolderPrefab, this.yarnballBlockPos);
		this.yarnballBlock.SetBlockCount (maxYarnballLCDStepCount);

		this.enemyValueBlock = Instantiate<LCDBlockHolder> (lcdBlockHolderPrefab, this.enemyValueBlockPos);
		this.enemyValueBlock.SetBlockCount (maxEnemyLCDStepCount);
	}

	private int LCD(int denominator, int targetValDenominator) {
		int x = denominator, y = targetValDenominator, lcd, initialX, initialY;

		initialX = x;
		initialY = y;

		while (x != y) {
			if (x > y) {
				x = x - y;
			}
			else {
				y = y - x;
			}
		}

		lcd = (initialX * initialY) / y;

		return lcd;
	}
}
