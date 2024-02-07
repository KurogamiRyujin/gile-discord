using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DataManager : MonoBehaviour {



	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.ON_NEEDLE_HIT, CreateEmptyEntry);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_NEEDLE_HIT);
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void CreateEmptyEntry (Parameters parameters) {
		print ("TYPE " +parameters.GetStringExtra ("TYPE", "NONE") + " NUMBER" + parameters.GetIntExtra("NUMBER", 0) + " DESCRIPTIVE NAME" 
			+parameters.GetStringExtra ("DESCRIPTIVE_NAME", "NONE") + " TIME INTERACTED" + parameters.GetStringExtra ("TIME_NEEDLE_INTERACTED", "NONE"));
		
		if(parameters.GetStringExtra("TYPE", "").Equals("PartitionableObject")) {
			Tuple<Entry.Type, int> key = new Tuple<Entry.Type, int>(Entry.Type.PartitionableObject, parameters.GetIntExtra("NUMBER", 0));

			PlayerData.POTally.Add (key, null);
		} else if(parameters.GetStringExtra("TYPE", "").Equals("Enemy")) {
			Tuple<Entry.Type, int> key = new Tuple<Entry.Type, int>(Entry.Type.Enemy, parameters.GetIntExtra("NUMBER", 0));

			PlayerData.enemyTally.Add (key, null);
		} else if(parameters.GetStringExtra("TYPE", "").Equals("LCD")) {
			Tuple<Entry.Type, int> key = new Tuple<Entry.Type, int>(Entry.Type.LCD, parameters.GetIntExtra("NUMBER", 0));

			PlayerData.LCDTally.Add (key, null);
		}
	}

//	public static void CreateEmptyEntry (Tuple<Entry.Type, int> key) {
//		PlayerData.tally.Add (key, null);
//	}

	public static void AddPOEntry(POEntry entry) {
		Tuple<Entry.Type, int> last = PlayerData.POTally.Keys.Last ();
		PlayerData.POTally.Add (last, entry);
//		Debug.Log (last);
	}

	public static void AddEnemyEntry(EnemyEntry entry) {
		Tuple<Entry.Type, int> last = PlayerData.enemyTally.Keys.Last ();
		PlayerData.enemyTally.Add (last, entry);
//		Debug.Log (last);
	}

	public static void AddLCDEntry(LCDEntry entry) {
		Tuple<Entry.Type, int> last = PlayerData.LCDTally.Keys.Last ();
		PlayerData.LCDTally.Add (last, entry);
		//		Debug.Log (last);
	}

	public static void RemovePOEntry(Tuple<Entry.Type, int> key) {
		PlayerData.POTally.Remove (key);
	}

	public static void RemoveEnemyEntry(Tuple<Entry.Type, int> key) {
		PlayerData.enemyTally.Remove (key);
	}

	public static void RemoveLCDEntry(Tuple<Entry.Type, int> key) {
		PlayerData.LCDTally.Remove (key);
	}

	public static void RemovePOEntry(POEntry entry) {
		Tuple<Entry.Type, int> key = PlayerData.POTally.FirstOrDefault (x => x.Value == entry).Key;
		PlayerData.POTally.Remove (key);
	}

	public static void RemoveEnemyEntry(EnemyEntry entry) {
		Tuple<Entry.Type, int> key = PlayerData.enemyTally.FirstOrDefault (x => x.Value == entry).Key;
		PlayerData.enemyTally.Remove (key);
	}

	public static void RemoveLCDEntry(LCDEntry entry) {
		Tuple<Entry.Type, int> key = PlayerData.LCDTally.FirstOrDefault (x => x.Value == entry).Key;
		PlayerData.LCDTally.Remove (key);
	}

	public static void UpdatePOEntry(Tuple<Entry.Type, int> key, POEntry entry) {
		if (PlayerData.POTally.ContainsKey (key)) {
			PlayerData.POTally [key] = entry;
		}

		PrintPODictionary ();
	}

	public static void UpdateEnemyEntry(Tuple<Entry.Type, int> key, EnemyEntry entry) {
		if (PlayerData.enemyTally.ContainsKey (key)) {
			PlayerData.enemyTally [key] = entry;
		}
        
		PrintEnemyDictionary ();
	}

	public static void UpdateLCDEntry(Tuple<Entry.Type, int> key, LCDEntry entry) {
		if (PlayerData.LCDTally.ContainsKey (key)) {
			PlayerData.LCDTally [key] = entry;
		}

		PrintLCDDictionary ();
	}

    public static POEntry GetPOEntry(Tuple<Entry.Type, int> key)
    {
        if (PlayerData.POTally.ContainsKey(key))
            return PlayerData.POTally[key];
        return null;
    }

    public static EnemyEntry GetEnemyEntry(Tuple<Entry.Type, int> key)
    {
        if (PlayerData.enemyTally.ContainsKey(key))
            return PlayerData.enemyTally[key];
        return null;
    }

    public static LCDEntry GetLCDEntry(Tuple<Entry.Type, int> key)
    {
        Debug.LogWarning("Get LCD Entry :" +String.Join(" ", key));
        if (PlayerData.LCDTally.ContainsKey(key))
        {
            return PlayerData.LCDTally[key];
        }
        return null;
    }

    public static Tuple<Entry.Type, int> GetPOLastKey() {
		return PlayerData.POTally.Keys.Last ();
	}

	public static Tuple<Entry.Type, int> GetEnemyLastKey() {
		return PlayerData.enemyTally.Keys.Last ();
	}

	public static Tuple<Entry.Type, int> GetLCDLastKey() {
		return PlayerData.LCDTally.Keys.Last ();
	}

	public static bool DoesKeyExist(Tuple<Entry.Type, int> key) {
		print ("Key " + key.Item1 + ", " + key.Item2);
		if (key.Item1.Equals (Entry.Type.PartitionableObject)) {
			if (PlayerData.POTally.ContainsKey (key))
				return true;
			return false;
		} else if (key.Item1.Equals (Entry.Type.Enemy)) {
			if (PlayerData.enemyTally.ContainsKey (key))
				return true;
			return false;
		} else if (key.Item1.Equals (Entry.Type.LCD)) {
			if (PlayerData.LCDTally.ContainsKey (key))
				return true;
			return false;
		}

		return false;
	}

	public static void PrintPODictionary() {
		print ("PO tally count: "+PlayerData.POTally.Keys.Count);
//		foreach (Tuple<Entry.Type, int> key in PlayerData.tally.Keys) {
//			print (key.Item1 + " " + key.Item2);
//		}
		foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> kvp in PlayerData.POTally) {
			print ("KEY: " + kvp.Key.Item1 + ", " + kvp.Key.Item2 + " VALUE: Name: " + kvp.Value.name + ", Time needle interacted: " +
				String.Join (", ", kvp.Value.timeWeaponInteracted) + ", Time Needle Removed: " + String.Join (", ", kvp.Value.timeWeaponInteracted) + ", Fraction Given: " +
			kvp.Value.objectGiven.Item1 + "/" + kvp.Value.objectGiven.Item2 + ", Correct Answer: " + kvp.Value.actualAnswer.Item1 + "/" + kvp.Value.actualAnswer.Item2 +
			", Attempted answers: " + String.Join (", ", kvp.Value.attemptedAnswers.Select (x => x.Item1 + "/" + x.Item2)) + ", Time Solved: " + kvp.Value.timeSolved.ToString () +
				", Needle Interaction Count: "+kvp.Value.interactionCount + ", Number of attempts: " + kvp.Value.numberOfAttempts + ", Topic: "+kvp.Value.topic);
//				kvp.Value.objectGivenNumerator.ToString() + "/" + kvp.Value.objectGivenDenominator.ToString() + ", Correct Answer: " + kvp.Value.actualAnswerNumerator + 
//				"/" + kvp.Value.actualAnswerDenominator + ", Attempted Answer: "+kvp.Value.attemptedAnswerNumerator + "/" +kvp.Value.attemptedAnswerDenominator);
		}
	}

	public static void PrintEnemyDictionary() {
		print ("enemy tally count: "+PlayerData.enemyTally.Keys.Count);
		//		foreach (Tuple<Entry.Type, int> key in PlayerData.tally.Keys) {
		//			print (key.Item1 + " " + key.Item2);
		//		}
		foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> kvp in PlayerData.enemyTally) {
			print ("KEY: " + kvp.Key.Item1 + ", " + kvp.Key.Item2 + " VALUE: Name: " + kvp.Value.name + ", Time hammer interacted: " +
				String.Join (", ", kvp.Value.timeWeaponInteracted) + ", Time Hammer Removed: " + String.Join (", ", kvp.Value.timeWeaponInteracted) + ", Initial Pointer: " +
				kvp.Value.initialValue.Item1 + "/" + kvp.Value.initialValue.Item2 + ", Target Value: " + kvp.Value.targetValue.Item1 + "/" + kvp.Value.targetValue.Item2 +
				", Correct Answer: " + kvp.Value.actualAnswer.Item1 + "/" + kvp.Value.actualAnswer.Item2 +
				", Attempted answers: " + String.Join (", ", kvp.Value.attemptedAnswers.Select (x => x.Item1 + "/" + x.Item2)) + ", Time Solved: " + kvp.Value.timeSolved.ToString () +
				", Needle Interaction Count: "+kvp.Value.interactionCount + ", Number of attempts: " + kvp.Value.numberOfAttempts + ", Is Similar: "+kvp.Value.isSimilar + 
				", Is Proper: "+kvp.Value.isProper + ", Is Dead: "+kvp.Value.isDeadThroughLCD + ", Topic: "+kvp.Value.topic + ", Hammer Value: "+kvp.Value.hammerValue);
			//				kvp.Value.objectGivenNumerator.ToString() + "/" + kvp.Value.objectGivenDenominator.ToString() + ", Correct Answer: " + kvp.Value.actualAnswerNumerator + 
			//				"/" + kvp.Value.actualAnswerDenominator + ", Attempted Answer: "+kvp.Value.attemptedAnswerNumerator + "/" +kvp.Value.attemptedAnswerDenominator);
		}
	}

	public static void PrintLCDDictionary() {
		print ("enemy tally count: "+PlayerData.LCDTally.Keys.Count);
		//		foreach (Tuple<Entry.Type, int> key in PlayerData.tally.Keys) {
		//			print (key.Item1 + " " + key.Item2);
		//		}
		foreach (KeyValuePair<Tuple<Entry.Type, int>, LCDEntry> kvp in PlayerData.LCDTally) {
			print ("KEY: " + kvp.Key.Item1 + ", " + kvp.Key.Item2 + " VALUE: Name: " + kvp.Value.name + ", Yarnball value: " +
			kvp.Value.yarnballValue + ", Initial Numerator: " + kvp.Value.initialNumerator + ", Initial Denominator: " + kvp.Value.initialDenominator +
			", Converted Denominator: " + kvp.Value.convertedDenominator + ", Actual Numerator: " + kvp.Value.actualNumerator +
				", Attempted answers: " + String.Join (", ", kvp.Value.attemptedNumerators)+ ", Topic: "+kvp.Value.topic + ", Hammer Value: "+kvp.Value.hammerValue);
		}
	}
		
	public static List<int> GetIntColumn(string tallyName, string columnName) {
		List<int> intData = new List<int> ();
		switch (tallyName) {
		case StringConstants.TableNames.ENEMY:
			switch(columnName) {
			case StringConstants.ColumnNames.INTERACTION_COUNT:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					intData.Add (e.Value.interactionCount);
				}
				break;
			case StringConstants.ColumnNames.NUMBER_OF_ATTEMPTS:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					intData.Add (e.Value.numberOfAttempts);
				}
				break;
			default:
				intData = null;
				break;
			}
			break;
		case StringConstants.TableNames.LCD:
			switch (columnName) {
			case StringConstants.ColumnNames.YARNBALL_VALUE:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, LCDEntry> e in PlayerData.LCDTally) {
					intData.Add (e.Value.yarnballValue);
				}
				break;
			case StringConstants.ColumnNames.INITIAL_NUMERATOR:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, LCDEntry> e in PlayerData.LCDTally) {
					intData.Add (e.Value.initialNumerator);
				}
				break;
			case StringConstants.ColumnNames.INITIAL_DENOMINATOR:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, LCDEntry> e in PlayerData.LCDTally) {
					intData.Add (e.Value.initialDenominator);
				}
				break;
			case StringConstants.ColumnNames.CONVERTED_DENOMINATOR:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, LCDEntry> e in PlayerData.LCDTally) {
					intData.Add (e.Value.convertedDenominator);
				}
				break;
			case StringConstants.ColumnNames.ACTUAL_NUMERATOR:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, LCDEntry> e in PlayerData.LCDTally) {
					intData.Add (e.Value.actualNumerator);
				}
				break;
			default:
				intData = null;
				break;
			}
			break;
		case StringConstants.TableNames.PARTITIONABLE_OBJECT:
			switch(columnName) {
			case StringConstants.ColumnNames.INTERACTION_COUNT:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> e in PlayerData.POTally) {
					intData.Add (e.Value.interactionCount);
				}
				break;
			case StringConstants.ColumnNames.NUMBER_OF_ATTEMPTS:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> e in PlayerData.POTally) {
					intData.Add (e.Value.numberOfAttempts);
				}
				break;
			default:
				intData = null;
				break;
			}
			break;
		default:
			intData = null;
			break;
		}

		return intData;
	}

	public static List<string> GetStringColumn(string tallyName, string columnName) {
		List<string> stringData = new List<string> ();

		switch (tallyName) {
		case StringConstants.TableNames.ENEMY: 
			switch (columnName) {
			case StringConstants.ColumnNames.NAME:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					stringData.Add (e.Value.name);
				}
				break;
			}
			break;
		case StringConstants.TableNames.PARTITIONABLE_OBJECT:
			switch (columnName) {
			case StringConstants.ColumnNames.NAME:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> e in PlayerData.POTally) {
					stringData.Add (e.Value.name);
				}
				break;
			}
			break;
		case StringConstants.TableNames.LCD:
			switch (columnName) {
			case StringConstants.ColumnNames.NAME:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, LCDEntry> e in PlayerData.LCDTally) {
					stringData.Add (e.Value.name);
				}
				break;
			}
			break;
		default:
			stringData = null;
			break;
		}

		return stringData;
	}

	public static List<DateTime?> GetDateTimeColumn(string tallyName, string columnName) {
		List<DateTime?> timestampData = new List<DateTime?> ();

		switch (tallyName) {
		case StringConstants.TableNames.ENEMY:
			switch (columnName) {
			case StringConstants.ColumnNames.TIME_SOLVED:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					timestampData.Add (e.Value.timeSolved);
				}
				break;
			default:
				timestampData = null;
				break;
			}
			break;
		case StringConstants.TableNames.PARTITIONABLE_OBJECT:
			switch (columnName) {
			case StringConstants.ColumnNames.TIME_SOLVED:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> e in PlayerData.POTally) {
					timestampData.Add (e.Value.timeSolved);
				}
				break;
			default:
				timestampData = null;
				break;
			}
			break;
		default:
			timestampData = null;
			break;

		}
		return timestampData;
	}

    public static List<bool?> GetBoolColumn(string tallyName, string columnName)
    {
        List<bool?> boolData = new List<bool?>();

		switch (tallyName) {
		case StringConstants.TableNames.ENEMY: 
			switch (columnName) {
			case StringConstants.ColumnNames.IS_SIMILAR:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					boolData.Add (e.Value.isSimilar);
				}
				break;
			case StringConstants.ColumnNames.IS_PROPER:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					boolData.Add (e.Value.isProper);
				}
				break;
			case StringConstants.ColumnNames.IS_DEAD_THROUGH_LCD:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					boolData.Add (e.Value.isDeadThroughLCD);
				}
				break;
			default:
				boolData = null;
				break;
			}
			break;
		default:
			boolData = null;
			break;
		}

        return boolData;
	}

	public static List<Tuple<int, int>> GetTupleColumn(string tallyName, string columnName)
	{
		List<Tuple<int, int>> tupleData = new List<Tuple<int,int>>();

		switch (tallyName) {
		case StringConstants.TableNames.ENEMY:
			switch (columnName) {
			case StringConstants.ColumnNames.ACTUAL_ANSWER:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					tupleData.Add (e.Value.actualAnswer);
				}
				break;
			case StringConstants.ColumnNames.INITIAL_VALUE:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					tupleData.Add (e.Value.initialValue);
				}
				break;
			case StringConstants.ColumnNames.TARGET_VALUE:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					tupleData.Add (e.Value.targetValue);
				}
				break;
			default:
				tupleData = null;
				break;
			}
			break;
		case StringConstants.TableNames.PARTITIONABLE_OBJECT:
			switch (columnName) {
			case StringConstants.ColumnNames.ACTUAL_ANSWER:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> e in PlayerData.POTally) {
					tupleData.Add (e.Value.actualAnswer);
				}
				break;
			case StringConstants.ColumnNames.OBJECT_GIVEN:
				foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> e in PlayerData.POTally) {
					tupleData.Add (e.Value.objectGiven);
				}
				break;
			default:
				tupleData = null;
				break;
			}
			break;
		default:
			tupleData = null;
			break;
		}

		return tupleData;
	}

	public static List<List<int>> GetListIntColumn(string tallyName, string columnName)
    {
		List<List<int>> listData = new List<List<int>>();

		switch (tallyName) {
		case StringConstants.TableNames.LCD:
			switch (columnName) {
			case StringConstants.ColumnNames.ATTEMPTED_NUMERATORS: 
				foreach (KeyValuePair<Tuple<Entry.Type, int>, LCDEntry> e in PlayerData.LCDTally) {
					listData.Add (e.Value.attemptedNumerators);
				}
				break;
			default:
				listData = null;
				break;
			}
			break;
		default:
			listData = null;
			break;
		}

        return listData;
    }

	public static List<List<Tuple<int, int>>> GetListTupleColumn(string tallyName, string columnName)
	{
		List<List<Tuple<int, int>>> listData = new List<List<Tuple<int, int>>>();

		switch (tallyName) {
		case StringConstants.TableNames.ENEMY:
			switch (columnName) {
			case StringConstants.ColumnNames.ATTEMPTED_ANSWERS: 
//				if (PlayerData.enemyTally.Count != 0) {
					foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
						listData.Add (e.Value.attemptedAnswers);
					}
//				}
				break;
			default:
				listData = null;
				break;
			}
			break;
		case StringConstants.TableNames.PARTITIONABLE_OBJECT:
			switch (columnName) {
			case StringConstants.ColumnNames.ATTEMPTED_ANSWERS: 
//				if (PlayerData.POTally.Count != 0) {
					foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> e in PlayerData.POTally) {
						listData.Add (e.Value.attemptedAnswers);
					}
//				}
				break;
			default:
				listData = null;
				break;
			}
			break;
		default:
			listData = null;
			break;
		}

		return listData;
	}

	public static List<List<DateTime>> GetListDateTimeColumn(string tallyName, string columnName)
	{
		List<List<DateTime>> listData = new List<List<DateTime>>();
		switch (tallyName) {
		case StringConstants.TableNames.ENEMY:
			switch (columnName) {
			case StringConstants.ColumnNames.TIME_WEAPON_INTERACTED: 
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					listData.Add (e.Value.timeWeaponRemoved);
				}
				break;
			case StringConstants.ColumnNames.TIME_WEAPON_REMOVED: 
				foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
					listData.Add (e.Value.timeWeaponRemoved);
				}
				break;
			default:
				listData = null;
				break;
			}
			break;
		case StringConstants.TableNames.PARTITIONABLE_OBJECT:
			switch (columnName) {
			case StringConstants.ColumnNames.TIME_WEAPON_INTERACTED: 
				foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> e in PlayerData.POTally) {
					listData.Add (e.Value.timeWeaponInteracted);
				}
				break;
			case StringConstants.ColumnNames.TIME_WEAPON_REMOVED: 
				foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> e in PlayerData.POTally) {
					listData.Add (e.Value.timeWeaponRemoved);
				}
				break;
			default:
				listData = null;
				break;
			}
			break;
		default:
			listData = null;
			break;
		}
		return listData;
	}

	public static List<Entry> GetRowsWithTopic (string filter, Entry.Topic topic) {
		List<Entry> entries = new List<Entry> ();

		switch (filter) {
		case StringConstants.TableNames.ENEMY:
			foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
				if(e.Value.topic == topic)
					entries.Add (e.Value);
			}
			break;
		case StringConstants.TableNames.LCD:
			foreach (KeyValuePair<Tuple<Entry.Type, int>, LCDEntry> e in PlayerData.LCDTally) {
				if(e.Value.topic == topic)
					entries.Add (e.Value);
			}
			break;
		case StringConstants.TableNames.PARTITIONABLE_OBJECT:
			foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> e in PlayerData.POTally) {
				if(e.Value.topic == topic)
					entries.Add (e.Value);
			}
			break;
		case StringConstants.TableNames.ALL:
			foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
				if(e.Value.topic == topic)
					entries.Add (e.Value);
			}

			foreach (KeyValuePair<Tuple<Entry.Type, int>, POEntry> e in PlayerData.POTally) {
				if(e.Value.topic == topic)
					entries.Add (e.Value);
			}

			foreach (KeyValuePair<Tuple<Entry.Type, int>, LCDEntry> e in PlayerData.LCDTally) {
				if(e.Value.topic == topic)
					entries.Add (e.Value);
			}

			foreach (KeyValuePair<Tuple<Entry.Type, int>, EnemyEntry> e in PlayerData.enemyTally) {
				if(e.Value.topic == topic)
					entries.Add (e.Value);
			}
			break;
		}

		return entries;
	}

	public static List<int> GetTopNDenominators(string filter, int number, bool withPlayerInput) {
		List<List<Tuple<int, int>>> enemyEntries = new List<List<Tuple<int, int>>> ();
		List<List<Tuple<int, int>>> poboxEntries = new List<List<Tuple<int, int>>> ();
		List<List<int>> lcdattemptsEntries = new List<List<int>> ();
		List<int> lcdEntries = new List<int> ();
		List<int> denoms = new List<int> ();

		Dictionary<int, int> dictionary = new Dictionary<int, int> ();

		int enemyAttemptedAnswersCount;
		int poBoxAttemptedAnswersCount;
		int lcdAttemptedAnswersCount;
//
		switch (filter) {
		case StringConstants.TableNames.ENEMY:
			enemyEntries = GetListTupleColumn (StringConstants.TableNames.ENEMY, StringConstants.ColumnNames.ATTEMPTED_ANSWERS);

			if (withPlayerInput) {
				

			} else {
				for (int i = 0; i < enemyEntries.Count; i++) {
					for (int j = 0; j < enemyEntries [i].Count; j++) {
						denoms.Add (enemyEntries [i] [j].Item2);
					}
				}
			}
				break;
			case StringConstants.TableNames.LCD:
				lcdEntries = GetIntColumn (StringConstants.TableNames.LCD, StringConstants.ColumnNames.INITIAL_DENOMINATOR);
				lcdattemptsEntries = GetListIntColumn (StringConstants.TableNames.LCD, StringConstants.ColumnNames.ATTEMPTED_NUMERATORS);
				for (int i = 0; i < lcdEntries.Count; i++) {
					for (int j = 0; j < lcdattemptsEntries [i].Count; j++) {
						denoms.Add (lcdEntries[i]);
					}
				}
				break;
			case StringConstants.TableNames.PARTITIONABLE_OBJECT:
				poboxEntries = GetListTupleColumn (StringConstants.TableNames.PARTITIONABLE_OBJECT, StringConstants.ColumnNames.ATTEMPTED_ANSWERS);

				for (int i = 0; i < poboxEntries.Count; i++) {
					for (int j = 0; j < poboxEntries [i].Count; j++) {
						denoms.Add (poboxEntries[i][j].Item2);
					}
				}

				break;
		case StringConstants.TableNames.ALL:
			enemyEntries = GetListTupleColumn (StringConstants.TableNames.ENEMY, StringConstants.ColumnNames.ATTEMPTED_ANSWERS);
			poboxEntries = GetListTupleColumn (StringConstants.TableNames.PARTITIONABLE_OBJECT, StringConstants.ColumnNames.ATTEMPTED_ANSWERS);
			lcdEntries = GetIntColumn (StringConstants.TableNames.LCD, StringConstants.ColumnNames.INITIAL_DENOMINATOR);
			lcdattemptsEntries = GetListIntColumn (StringConstants.TableNames.LCD, StringConstants.ColumnNames.ATTEMPTED_NUMERATORS);
				
			if (lcdEntries.Count != 0) {
				for (int i = 0; i < lcdEntries.Count; i++) {
					for (int j = 0; j < lcdattemptsEntries [i].Count; j++) {
						if (!dictionary.ContainsKey (lcdEntries [i])) {
							dictionary.Add (lcdEntries [i], 1);
						} else
							dictionary [lcdEntries [i]]++;
					}
				}
			}

			if (poboxEntries.Count != 0) {
				for (int i = 0; i < poboxEntries.Count; i++) {
					for (int j = 0; j < poboxEntries [i].Count; j++) {
//						denoms.Add (poboxEntries[i][j].Item2);
						if (!dictionary.ContainsKey (poboxEntries [i] [j].Item2)) {
							dictionary.Add (poboxEntries [i] [j].Item2, 1);
						} else
							dictionary [poboxEntries [i] [j].Item2]++;
					}
				}
			}

			if (enemyEntries.Count != 0) {
				for (int i = 0; i < enemyEntries.Count; i++) {
					for (int j = 0; j < enemyEntries [i].Count; j++) {
//						denoms.Add (enemyEntries[i][j].Item2);
						if (!dictionary.ContainsKey (enemyEntries [i] [j].Item2)) {
							dictionary.Add (enemyEntries [i] [j].Item2, 1);
						} else
							dictionary [enemyEntries [i] [j].Item2]++;
					}
				}
			}
//				for (int i = 0; i < lcdEntries.Count; i++) {
//					for (int j = 0; j < lcdattemptsEntries [i].Count; j++) {
//						denoms.Add (lcdEntries[i]);
//					}
//				}
//
//				for (int i = 0; i < poboxEntries.Count; i++) {
//					for (int j = 0; j < poboxEntries [i].Count; j++) {
//						denoms.Add (poboxEntries[i][j].Item2);
//					}
//				}
//
//				for (int i = 0; i < enemyEntries.Count; i++) {
//					for (int j = 0; j < enemyEntries [i].Count; j++) {
//						denoms.Add (enemyEntries[i][j].Item2);
//					}
//				}

				break;
			default:
				Debug.Log ("Invalid filter");
				break;
		}

//		for (int i = 0; i < 100; i++) {
//			denoms.Add (UnityEngine.Random.Range (2, 11));
//		}
//

		foreach (int d in denoms) {
			if (!dictionary.ContainsKey (d)) {
				dictionary.Add (d, 1);
			} else
				dictionary [d]++;
		}

		var result = dictionary.OrderByDescending (x => x.Value);

//		var groups = denoms.GroupBy (v => v).OrderByDescending(x => x.Count());
		Debug.Log ("<color=red>REQUESTED DENOMINATORS</color>");
		foreach (var r in result) {
			Debug.Log(String.Format("Value {0} has {1} occurences", r.Key, r.Value));
		}
		int count = 0;
		if (number <= result.Count()) {
			foreach (var r in result) {
				if (count > number)
					dictionary.Remove(r.Key);
				count++;
			}

			result = dictionary.OrderByDescending (x => x.Value);
			foreach (var r in result) {
				Debug.Log (String.Format ("Value {0} has {1} occurences", r.Key, r.Value));
			}

			denoms = denoms.Distinct ().ToList ();
		}


//		foreach (var group in groups) {
//			Debug.Log(String.Format("Value {0} has {1} occurences", group.Key, group.Count()));
//		}
//
//		int count = 0;
//		if (number <= groups.Count()) {
//			foreach (var group in groups) {
//				if (count > number)
//					denoms.RemoveAll ((x => x.Equals (group.Key)));
//				count++;
//			}
//
//			groups = denoms.GroupBy (v => v).OrderByDescending (x => x.Count ());
//			foreach (var group in groups) {
//				Debug.Log (String.Format ("Value {0} has {1} occurences", group.Key, group.Count ()));
//			}
//
//			denoms = denoms.Distinct ().ToList ();
//		}

		return denoms;
	}

}
