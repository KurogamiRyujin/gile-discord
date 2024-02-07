using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {
	[HideInInspector] public string name;

	public string[] sentences;
	public TextAsset[] textFiles;
	public TextAsset[] mobileTextFiles;
	public int currentTextIndex = -1;

	private TextAsset textFile;

	public TextAsset getTextFile() {
		currentTextIndex += 1;
		if (currentTextIndex < textFiles.Length) {
			#if UNITY_ANDROID
			return mobileTextFiles[currentTextIndex];
//			return textFiles [currentTextIndex]; // TODO HOTFIX || Change to proper array (mobileTextFiles)
			#else
			return textFiles [currentTextIndex];
			#endif
		}
		else
			return null;
	}
}
