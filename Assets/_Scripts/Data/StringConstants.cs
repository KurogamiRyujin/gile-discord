using System.Collections;
using UnityEngine;

public class StringConstants {

	public class ColumnNames {
		//for parent class 'Entry'
		public const string NAME = "NAME";
		public const string TIME_WEAPON_INTERACTED = "TIME_WEAPON_INTERACTED";
		public const string TIME_WEAPON_REMOVED = "TIME_WEAPON_REMOVED";
		public const string ACTUAL_ANSWER = "ACTUAL_ANSWER";
		public const string ATTEMPTED_ANSWERS = "ATTEMPTED_ANSWERS";
		public const string TIME_SOLVED = "TIME_SOLVED";
		public const string INTERACTION_COUNT = "INTERACTION_COUNT";
		public const string NUMBER_OF_ATTEMPTS = "NUMBER_OF_ATTEMPTS";

		//for class 'EnemyEntry'
		public const string INITIAL_VALUE = "INITIAL_VALUE";
		public const string TARGET_VALUE = "TARGET_VALUE";
		public const string IS_SIMILAR = "IS_SIMILAR";
		public const string IS_PROPER = "IS_PROPER";
		public const string IS_DEAD_THROUGH_LCD = "IS_DEAD_THROUGH_LCD";
	
		//for class 'LCDEntry'
		public const string YARNBALL_VALUE = "YARNBALL_VALUE";
		public const string INITIAL_NUMERATOR = "INITIAL_NUMERATOR";
		public const string INITIAL_DENOMINATOR = "INITIAL_DENOMINATOR";
		public const string CONVERTED_DENOMINATOR = "CONVERTED_DENOMINATOR";
		public const string ACTUAL_NUMERATOR = "ACTUAL_NUMERATOR";
		public const string ATTEMPTED_NUMERATORS = "ATTEMPTED_NUMERATORS";

		//for class 'POEntry'
		public const string OBJECT_GIVEN = "OBJECT_GIVEN";
	}

	public class TableNames {
		public const string ENEMY = "ENEMY";
		public const string LCD = "LCD";
		public const string PARTITIONABLE_OBJECT = "PARTITIONABLE_OBJECT";
		public const string ALL = "ALL";

	}
}
