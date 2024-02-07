using System;
using System.Collections.Generic;

/// <summary>
/// Data for the game. Parsed as a JSON to be saved as a text file.
/// </summary>
[Serializable]
public class GameData {
    /// <summary>
    /// Number of registered users in the game.
    /// </summary>
	public int numberOfUsers;
    /// <summary>
    /// List of all usernames of all the players.
    /// </summary>
	public List<string> usernames;
}