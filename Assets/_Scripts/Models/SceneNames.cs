using UnityEngine;


/// <summary>
/// Handles random generation of scenes
/// </summary>
public class SceneNames {

    /// <summary>
    /// Holds the previously generated random number
    /// </summary>
    public static int previousNum = 0;
    /// <summary>
    /// Constant string that refers to the 'Beta Testing Random 1' scene
    /// </summary>
    public const string BETA_TESTING_RANDOM_1 = "Beta Testing Random 1";
    /// <summary>
    /// Constant string that refers to the 'Beta Testing Random 2' scene
    /// </summary>
	public const string BETA_TESTING_RANDOM_2 = "Beta Testing Random 2";
    /// <summary>
    /// Constant string that refers to the 'Beta Testing Random 3' scene
    /// </summary>
	public const string BETA_TESTING_RANDOM_3 = "Beta Testing Random 3";
    /// <summary>
    /// Constant string that refers to the 'Beta Testing Dissimilar Fractions 1' scene
    /// </summary>
    public const string BETA_TESTING_RANDOM_4 = "Beta Testing Dissimilar Fractions 1";
    /// <summary>
    /// Constant string that refers to the 'Beta Testing Dissimilar Fractions 2' scene
    /// </summary>
    public const string BETA_TESTING_RANDOM_5 = "Beta Testing Dissimilar Fractions 2";
    /// <summary>
    /// Constant string that refers to the 'Beta Testing Dissimilar Fractions 3' scene
    /// </summary>
    public const string BETA_TESTING_RANDOM_6 = "Beta Testing Dissimilar Fractions 3";

    /// <summary>
    /// Constant string that refers to the 'Beta Testing Random End' scene
    /// </summary>
    public const string BETA_TESTING_RANDOM_END = "Beta Testing Random End";
    /// <summary>
    /// Constant string that refers to the 'Beta Testing Random Pre Boss' scene
    /// </summary>
	public const string BETA_TESTING_PRE_BOSS = "Beta Testing Pre Boss";


    /// <summary>
    /// Generates a random scene name
    /// <returns>Returns a random scene name</returns>
    /// </summary>
    public static string RandomSceneName() {
		string sceneName = BETA_TESTING_RANDOM_1;

		int num = Random.Range (0, 6);
		if (num == previousNum)
			num = (num + 1) % 6;

		switch (num) {
		case 0:
			sceneName = BETA_TESTING_RANDOM_1;
			break;
		case 1:
			sceneName = BETA_TESTING_RANDOM_2;
			break;
		case 2:
			sceneName = BETA_TESTING_RANDOM_3;
			break;
        case 3:
            sceneName = BETA_TESTING_RANDOM_4;
            break;
        case 4:
            sceneName = BETA_TESTING_RANDOM_5;
            break;
        case 5:
            sceneName = BETA_TESTING_RANDOM_6;
            break;
        }

		previousNum = num;
		return sceneName;
	}
}
