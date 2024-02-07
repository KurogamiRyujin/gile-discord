using System;
using System.Collections.Generic;

/// <summary>
/// Data for the learner model. Parsed as a JSON to be saved as a text file.
/// </summary>
[Serializable]
public class LearnerModelData {
    /// <summary>
    /// Probability for the observed node, Addition of Similar Fractions.
    /// </summary>
	public double additionSimilar;
    /// <summary>
    /// Probability for the observed node, Subtraction of Similar Fractions.
    /// </summary>
	public double subtractionSimilar;
    /// <summary>
    /// Probability for the unobserved node, Operations on Similar Fractions.
    /// </summary>
	public double similar;

    /// <summary>
    /// Probability for the observed node, Addition of Equivalent Fractions.
    /// </summary>
	public double additionEquivalent;
    /// <summary>
    /// Probability for the observed node, Addition of Equivalent Fractions.
    /// </summary>
	public double subtractionEquivalent;
    /// <summary>
    /// Probability for the unobserved node, Operations on Equivalent Fractions.
    /// </summary>
	public double equivalent;

    /// <summary>
    /// Probability for the observed node, Addition of Dissimilar Fractions.
    /// </summary>
	public double additionDissimilar;
    /// <summary>
    /// Probability for the observed node, Addition of Dissimilar Fractions.
    /// </summary>
	public double subtractionDissimilar;
    /// <summary>
    /// Probability for the unobserved node, Operations on Dissimilar Fractions.
    /// </summary>
	public double dissimilar;

    /// <summary>
    /// Denominator Mastery Probabilities for denominators present in the game.
    /// </summary>
	public List<DenominatorMasteryData> denominatorMasteries = new List<DenominatorMasteryData> ();
}
