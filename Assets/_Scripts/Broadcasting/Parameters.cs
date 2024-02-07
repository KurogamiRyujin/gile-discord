using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Data model that holds parameters to be passed alongside observer functions
/// 
/// Structured similary to Android's Intent class.
/// Created By: NeilDG
/// </summary>
public class Parameters {

    /// <summary>
    /// Basic supported parcelable types
    /// </summary>
    private Dictionary<string, char> charData;
	private Dictionary<string, int> intData;
	private Dictionary<string, bool> boolData;
	private Dictionary<string, float> floatData;
	private Dictionary<string, double> doubleData;
	private Dictionary<string, short> shortData;
	private Dictionary<string, long> longData;
	private Dictionary<string, string> stringData;

	//reference type parcelable
	private Dictionary<string, ArrayList> arrayListData;
	private Dictionary<string, object> objectListData;

    /// <summary>
    /// Added Reference types.
    /// </summary>
    private Dictionary<string, DetonatingPlatform.Detonation_Mode> platformDetonationModeData;
    private Dictionary<string, EnemyHealth> enemyHealthData;
    private Dictionary<string, HollowBlock> hollowBlockData;

    public Parameters() {
		this.charData = new Dictionary<string, char>();
		this.intData = new Dictionary<string, int>();
		this.boolData = new Dictionary<string, bool>();
		this.floatData = new Dictionary<string, float>();
		this.doubleData = new Dictionary<string, double>();
		this.shortData = new Dictionary<string, short>();
		this.longData = new Dictionary<string, long>();
		this.stringData = new Dictionary<string, string>();
		this.arrayListData = new Dictionary<string, ArrayList>();
		this.objectListData = new Dictionary<string, object>();
		this.platformDetonationModeData = new Dictionary<string, DetonatingPlatform.Detonation_Mode> ();
		this.enemyHealthData = new Dictionary<string, EnemyHealth> ();
		this.hollowBlockData = new Dictionary<string, HollowBlock> ();
	}

	public void PutExtra(string paramName, bool value) {
		this.boolData.Add(paramName,value);
	}

	public void PutExtra(string paramName, int value) {
		this.intData.Add(paramName, value);
	}

	public void PutExtra(string paramName, char value) {
		this.charData.Add(paramName, value);
	}
	
	public void PutExtra(string paramName, float value) {
		this.floatData.Add(paramName,value);
	}

	public void PutExtra(string paramName, double value) {
		this.doubleData.Add(paramName, value);
	}

	public void PutExtra(string paramName, short value) {
		this.shortData.Add(paramName,value);
	}

	public void PutExtra(string paramName, long value) {
		this.longData.Add(paramName, value);
	}

	public void PutExtra(string paramName, string value) {
		this.stringData.Add(paramName, value);
	}

	public void PutExtra(string paramName, DetonatingPlatform.Detonation_Mode detonationMode) {
		this.platformDetonationModeData.Add (paramName, detonationMode);
	}

	public void PutExtra(string paramName, EnemyHealth enemyHealth) {
		this.enemyHealthData.Add (paramName, enemyHealth);
	}

	public void PutExtra(string paramName, HollowBlock hollowBlock) {
		this.hollowBlockData.Add (paramName, hollowBlock);
	}

	public void PutExtra(string paramName, ArrayList arrayList) {
		this.arrayListData.Add(paramName, arrayList);
	}

	public void PutExtra(string paramName, object[] objectArray) {
		ArrayList arrayList = new ArrayList();
		arrayList.AddRange(objectArray);
		this.PutExtra(paramName,arrayList);
	}

	public void PutObjectExtra(string paramName, object value) {
		this.objectListData.Add(paramName, value);
	}

	public int GetIntExtra(string paramName, int defaultValue) {
		if(this.intData.ContainsKey(paramName)) {
			return this.intData[paramName];
		}
		else {
			return defaultValue;
		}
	}

	public char GetCharExtra(string paramName, char defaultValue) {
		if(this.charData.ContainsKey(paramName)) {
			return this.charData[paramName];
		}
		else {
			return defaultValue;
		}
	}

	public bool GetBoolExtra(string paramName, bool defaultValue) {
		if(this.boolData.ContainsKey(paramName)) {
			return this.boolData[paramName];
		}
		else {
			return defaultValue;
		}
	}

	public float GetFloatExtra(string paramName, float defaultValue) {
		if(this.floatData.ContainsKey(paramName)) {
			return this.floatData[paramName];
		}
		else {
			return defaultValue;
		}
	}

	public double GetDoubleExtra(string paramName, double defaultValue) {
		if(this.doubleData.ContainsKey(paramName)) {
			return this.doubleData[paramName];
		}
		else {
			return defaultValue;
		}
	}

	public short GetShortExtra(string paramName, short defaultValue) {
		if(this.shortData.ContainsKey(paramName)) {
			return this.shortData[paramName];
		}
		else {
			return defaultValue;
		}
	}

	public long GetLongExtra(string paramName, long defaultValue) {
		if(this.longData.ContainsKey(paramName)) {
			return this.longData[paramName];
		}
		else {
			return defaultValue;
		}
	}

	public string GetStringExtra(string paramName, string defaultValue) {
		if(this.stringData.ContainsKey(paramName)) {
			return this.stringData[paramName];
		}
		else {
			return defaultValue;
		}
	}

	public ArrayList GetArrayListExtra(string paramName) {
		if(this.arrayListData.ContainsKey(paramName)) {
			return this.arrayListData[paramName];
		}
		else {
			return null;
		}
	}

	public object[] GetArrayExtra(string paramName) {
		ArrayList arrayListData = this.GetArrayListExtra(paramName);

		if(arrayListData != null) {
			return arrayListData.ToArray();
		}
		else {
			return null;
		}
	}

	public object GetObjectExtra(string paramName) {
		if(this.objectListData.ContainsKey(paramName)) {
			return this.objectListData[paramName];
		}
		else {
			return null;
		}
	}

	public DetonatingPlatform.Detonation_Mode GetPlatformDetonationModeExtra(string paramName, DetonatingPlatform.Detonation_Mode defaultVal) {
		if(this.platformDetonationModeData.ContainsKey(paramName)) {
			return this.platformDetonationModeData[paramName];
		}
		else {
			return defaultVal;
		}
	}

	public EnemyHealth GetEnemyHealthExtra(string paramName) {
		if(this.enemyHealthData.ContainsKey(paramName)) {
			return this.enemyHealthData[paramName];
		}
		else {
			return null;
		}
	}

	public HollowBlock GetHollowBlockExtra(string paramName) {
		if (this.hollowBlockData.ContainsKey (paramName)) {
			return this.hollowBlockData [paramName];
		} else
			return null;
	}
}
