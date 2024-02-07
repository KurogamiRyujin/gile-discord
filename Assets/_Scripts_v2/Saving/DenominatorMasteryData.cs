using System;

/// <summary>
/// Data for a denominator mastery entry. Parsed as a JSON to be saved as a text file.
/// </summary>
[Serializable]
public class DenominatorMasteryData {
	public int denominator;
    /// <summary>
    /// Topic that the player has a mastery of the denominator in.
    /// </summary>
	public SceneTopic topic;
    /// <summary>
    /// Mastery probability of the denominator.
    /// </summary>
	public float mastery;
}
