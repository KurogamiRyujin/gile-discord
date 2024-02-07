using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LCDInterface_v3 : MonoBehaviour {

	[SerializeField] private LCDBlockHolder_v2 lcdBlockHolderPrefab;

	[SerializeField] private Transform yarnballBlockPos;
	[SerializeField] private Transform enemyValueBlockPos;
	[SerializeField] private LCDEffectHammer hammerEffect;
	[SerializeField] private Text textSteps;
	[SerializeField] private LCDNumberlineDemo lcdNumberLine;
	[SerializeField] private float lineLength;

	private LCDBlockHolder_v2 yarnballBlock;
	private LCDBlockHolder_v2 enemyValueBlock;

	private EnemyHealth enemyHealthReference;
	private HammerObject hammerReference;

	private int maxSteps = 0;
	private int lcd = 0;

	void Awake() {
		this.Initialize ();
	}
	public void Initialize () {
		GameController_v7.Instance.GetLCDManager ().SubscribeLCDInterface (this);
		this.yarnballBlock = Instantiate<LCDBlockHolder_v2> (lcdBlockHolderPrefab, this.yarnballBlockPos);
		this.enemyValueBlock = Instantiate<LCDBlockHolder_v2> (lcdBlockHolderPrefab, this.enemyValueBlockPos);
		Deactivate ();
	}
//	void Start() {
//		this.Deactivate ();
//	}
	void OnEnable() {
		this.lineLength = lcdNumberLine.GetLineRenderer ().GetPosition (1).x;
		this.lcdNumberLine.SetLineLength (lineLength);
		this.lcdNumberLine.Deactivate ();
	}

	void OnDisable() {
		Deactivate ();
	}

	void OnDestroy() {
		GameController_v7.Instance.GetLCDManager ().UnsubscribeLCDInterface (this);
	}


	private void Activate() {
		for (int i = 0; i < this.transform.childCount; i++) {
			this.transform.GetChild (i).gameObject.SetActive (true);
		}
		this.lcdNumberLine.Deactivate ();
	}

	private void Deactivate () {
		yarnballBlock.PurgeBlocks ();
		enemyValueBlock.PurgeBlocks ();
		enemyHealthReference = null;
		hammerReference = null;
		for (int i = 0; i < this.transform.childCount; i++) {
			this.transform.GetChild (i).gameObject.SetActive (false);
		}
	}

	public void InitiateLCD(int yarnballDenom, EnemyHealth enemyHealth, HammerObject hammer) {
		GameController_v7.Instance.GetPauseController ().Pause ();
		Activate ();
		this.enemyHealthReference = enemyHealth;
		//NOTE: commented out due to error after EnemyHealth was updated
//		this.lcdNumberLine.Segment (this.enemyHealthReference.GetDenominator ());
//		this.lcdNumberLine.TargetSetter (this.enemyHealthReference.GetTargetNumerator (), this.enemyHealthReference.GetDenominator());
//		this.lcdNumberLine.InitialPointerPointTo (this.enemyHealthReference.GetInitialPointerNumerator (), this.enemyHealthReference.GetDenominator());
//		this.hammerReference = hammer;
//		InitiateBlocks (yarnballDenom, enemyHealthReference.GetDenominator ());
	}

	private void InitiateBlocks(int yarnballVal, int enemyVal) {
		int lcd = LCD (yarnballVal, enemyVal);
		int yarnballLCDStepCount = lcd / yarnballVal;
		int enemyLCDStepCount = lcd / enemyVal;
		int maxOverallStepCounts = yarnballLCDStepCount + enemyLCDStepCount;
		this.maxSteps = maxOverallStepCounts-2;
		int multiplier = 2;

		this.GetStepText ().text = this.maxSteps+"";
//		if (yarnballLCDStepCount > enemyLCDStepCount)
//			multiplier = (maxOverallStepCounts / enemyLCDStepCount) + 1;
//		else
//			multiplier = (maxOverallStepCounts / yarnballLCDStepCount) + 1;
//		int maxYarnballLCDStepCount = yarnballLCDStepCount * multiplier;
//		int maxEnemyLCDStepCount = enemyLCDStepCount * multiplier;

		if (yarnballLCDStepCount > enemyLCDStepCount)
			multiplier = (maxSteps / enemyLCDStepCount)+1;
		else
			multiplier = (maxSteps / yarnballLCDStepCount)+1;
		
		int maxYarnballLCDStepCount = (yarnballLCDStepCount * multiplier);
		int maxEnemyLCDStepCount = (enemyLCDStepCount * multiplier);


		Debug.Log ("<color=red>maxYarnballLCDStepCount "+maxYarnballLCDStepCount+"</color>");
		Debug.Log ("<color=red>maxEnemyLCDStepCount "+maxEnemyLCDStepCount+"</color>");

		this.yarnballBlock.SetInitialNumber (yarnballVal);
		this.yarnballBlock.SetBlockCount (maxYarnballLCDStepCount, yarnballBlockPos);


		this.enemyValueBlock.SetInitialNumber (enemyVal);
		this.enemyValueBlock.SetBlockCount (maxEnemyLCDStepCount, enemyValueBlockPos);
	}

	private Text GetStepText() {
		if (this.textSteps == null) {
			this.textSteps = GetComponentInChildren<LCDMaxSteps> ().GetComponent<Text> ();
		}
		return this.textSteps;
	}

	private int LCD(int denominator, int targetValDenominator) {
		int x = denominator, y = targetValDenominator, initialX, initialY;

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

		this.lcd = (initialX * initialY) / y;

		return this.lcd;
	}

	public void Answer() {
		int yarnballBlockNumber = this.yarnballBlock.GetLatestDenominator ();
		int enemyValueBlockNumber = this.enemyValueBlock.GetLatestDenominator ();

		bool correct = false;

		if (yarnballBlockNumber == enemyValueBlockNumber)
			correct = true;

		if (correct) {
			//NOTE: commented out due to error after EnemyHealth was updated
//			enemyHealthReference.LCDUpdate (this);
			hammerReference.prevDenominator = hammerReference.denominator;
			hammerReference.isLCD = true;
			hammerReference.UpdateFromLCD (this);
		}

		Deactivate ();
		if (correct) {
			this.lcdNumberLine.Activate ();
			this.lcdNumberLine.ApplyLCD (this.lcd);
		} else
			GameController_v7.Instance.GetPauseController ().Continue ();

		ReturnCamera ();
//		Parameters data = new Parameters ();
//		data.PutExtra ("shouldZoomIn", false);
//		EventBroadcaster.Instance.PostEvent (EventNames.SHOULD_CAMERA_ZOOM_IN, data);
	}
	public void ReturnCamera() {
		Parameters data = new Parameters ();
		data.PutExtra ("shouldZoomIn", false);

		EventBroadcaster.Instance.PostEvent (EventNames.SHOULD_CAMERA_ZOOM_IN, data);
		EventBroadcaster.Instance.PostEvent (EventNames.ENABLE_CAMERA);
	}
	public void IncreaseCount(int i) {
		if (this.hammerEffect == null) {
			this.hammerEffect = GameObject.FindObjectOfType<LCDEffectHammer> ();
		}
//		this.hammerEffect.GetParticleEffectIncrease ().Play ();

		SoundManager.Instance.Play (AudibleNames.LCDInterface.INCREASE, false);
		switch (i) {
		case 0:
			if (yarnballBlock.GetParticleEffectDecrease () == null) {
				LCDEffectYarnball effectYarnball = GetComponentInChildren<LCDEffectYarnball> ();
				this.yarnballBlock.SetParticleEffectDecrease (effectYarnball.GetParticleEffectDecrease());
			}
			this.maxSteps = yarnballBlock.IncreaseBlock (this.maxSteps);
			this.yarnballBlock.GetParticleEffectDecrease ().Play ();
			break;
		case 1:
			if (enemyValueBlock.GetParticleEffectDecrease () == null) {
				LCDEffectEnemy effectEnemy = GetComponentInChildren<LCDEffectEnemy> ();
				this.enemyValueBlock.SetParticleEffectDecrease (effectEnemy.GetParticleEffectDecrease ());

			}
			this.maxSteps = enemyValueBlock.IncreaseBlock (this.maxSteps);
			this.enemyValueBlock.GetParticleEffectDecrease ().Stop ();
			this.enemyValueBlock.GetParticleEffectDecrease ().Play ();
			break;
		}

		this.GetStepText ().text = this.maxSteps+"";
	}

	public void DecraeseCount(int i) {
		SoundManager.Instance.Play (AudibleNames.LCDInterface.DECREASE, false);
		switch (i) {
		case 0:
//			if (yarnballBlock.GetParticleEffectDecrease () == null) {
//				LCDEffectYarnball effectYarnball = GetComponentInChildren<LCDEffectYarnball> ();
//				this.yarnballBlock.SetParticleEffectDecrease (effectYarnball.GetParticleEffectDecrease());
//			}
			this.maxSteps = yarnballBlock.DecreaseBlock (this.maxSteps);
			break;
		case 1:
//			if (enemyValueBlock.GetParticleEffectDecrease () == null) {
//				LCDEffectEnemy effectEnemy = GetComponentInChildren<LCDEffectEnemy> ();
//				this.enemyValueBlock.SetParticleEffectDecrease (effectEnemy.GetParticleEffectDecrease());
//
//			}
			this.maxSteps = enemyValueBlock.DecreaseBlock (this.maxSteps);
			break;
		}
		this.hammerEffect.GetParticleEffectIncrease ().Play ();
		this.GetStepText ().text = this.maxSteps+"";
	}

	public int GetLCD() {
		return this.lcd;
	}
}
