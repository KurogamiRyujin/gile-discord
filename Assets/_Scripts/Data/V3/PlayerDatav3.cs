using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PlayerDatav3 {
	
	public Dictionary<string, List<StabilizationSession>> sessions;
	private string username;
	private string folderDataPath;
	private string sessionsDataPath;
	private string sessionsFile;

	private static PlayerDatav3 sharedInstance = null;
	public static PlayerDatav3 Instance {
		get {
			if (sharedInstance == null)
				sharedInstance = new PlayerDatav3 ();

			return sharedInstance;
		}
	}

	public PlayerDatav3() {
		this.sessions = new Dictionary<string, List<StabilizationSession>> ();
		if (PlayerPrefs.HasKey ("Username"))
			this.username = PlayerPrefs.GetString ("Username");
		else
			this.username = "default";

		this.folderDataPath = Application.persistentDataPath + "/Game Data/user_" + this.username;
		this.sessionsDataPath = this.folderDataPath + "/Sessions";
		if (!Directory.Exists (this.sessionsDataPath))
			Directory.CreateDirectory (this.sessionsDataPath);
	}

	public void RecordSessionTimes() {
		foreach (string key in this.sessions.Keys) {
			string file = this.sessionsDataPath + "/" + key + "_Time_Logs.txt";

			string txt = "";
			foreach (StabilizationSession session in this.sessions[key]) {
				txt += session.StartTime () + " - " + session.EndTime () + ": " + session.SessionTime () + "\r\n";
			}

			File.AppendAllText (file, txt);
		}
	}
}
