using System;
using System.Collections.Generic;

/// <summary>
/// Data for a user. Parsed as a JSON to be saved as a text file.
/// </summary>
[Serializable]
public class UserData {
    /// <summary>
    /// Entered username of the user
    /// </summary>
    public string username;
    /// <summary>
    /// Chosen gender of the user
    /// </summary>
    public string gender;
    /// <summary>
    /// Name of last scene visited by the user
    /// </summary>
    public string lastSceneVisited;
    /// <summary>
    /// Current level of user in Addition of Similar Fractions topic
    /// </summary>
    public int currentAdditionSimilarFractionsLevel;
    /// <summary>
    /// Current level of user in Subtraction of Similar Fractions topic
    /// </summary>
	public int currentSubtractionSimilarFractionsLevel;
    /// <summary>
    /// Current level of user in Addition and Subtraction of Dissimilar Fractions topic
    /// </summary>
	public int currentDissimilarFractionsLevel;
//	public int currentAdditionDissimilarFractionsLevel;
//	public int currentSubtractionDissimilarFractionsLevel;
//	public int currentAdditionEquivalentFractionsLevel;
//	public int currentSubtractionEquivalentFractionsLevel;
}