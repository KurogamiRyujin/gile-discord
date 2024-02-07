using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using BNUtils;

public class DataManagerv3 {

	public static void AddSessions(string sceneName, IEnumerable sessions) {
		if (!PlayerDatav3.Instance.sessions.ContainsKey (sceneName))
			PlayerDatav3.Instance.sessions.Add (sceneName, new List<StabilizationSession> ());

		foreach (StabilizationSession session in sessions)
			PlayerDatav3.Instance.sessions [sceneName].Add (session);
	}

	public static IEnumerable<StabilizationSession> GetSessions(string sceneName) {
		if (PlayerDatav3.Instance.sessions.ContainsKey (sceneName))
			return PlayerDatav3.Instance.sessions [sceneName];
		else {
			Debug.Log ("No such scene name recorded.");
			return null;
		}
	}

	public static void RecordSessionTimes() {
		PlayerDatav3.Instance.RecordSessionTimes ();
	}
}
