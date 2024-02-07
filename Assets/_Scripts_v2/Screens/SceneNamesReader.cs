using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Reads list of scene names from text files to display on screen
/// </summary>
public class SceneNamesReader : MonoBehaviour {

    /// <summary>
    /// Array of TextAsset files for each topic
    /// </summary>
	[SerializeField] private TextAsset[] textFiles;
    /// <summary>
    /// Array of string that holds scene names under each topic
    /// </summary>
	private string[] sceneNames;

    /// <summary>
    /// Unity Function. Called once upon creation of the object 
    /// </summary>
    // Use this for initialization
    void Awake () {
//		if (textFiles != null) {
//			foreach (TextAsset textFile in textFiles) {
//				if (textFile.text.Length > 0) {
//					this.sceneNames = textFile.text.Split ('\n');
//					TrimText ();
//				} 
//			}
//		}
	}

    /// <summary>
    /// Parses the text file and stores each string to the sceneNames variable
    /// </summary>
    /// <param name="index">Determines which text file to parse</param>
	void ParseData(int index) {
		TextAsset textFile = textFiles [index];
		if (textFile.text.Length > 0) {
			this.sceneNames = textFile.text.Split ('\n');
			TrimText ();
		} 
	}

    /// <summary>
    /// Trims all scene names
    /// </summary>
	public void TrimText() {
		if (this.sceneNames != null) {
			for (int i = 0; i < this.sceneNames.Length; i++) {
				this.sceneNames [i] = this.sceneNames[i].ToString().Trim ();
			}
		}
	}

    /// <summary>
    /// Gets the array of scene names under the Addition of Similar Fractions topic
    /// </summary>
    /// <returns>Returns array of scene names</returns>
	public string[] GetAddSceneNames() {
		ParseData (0);
		return this.sceneNames;
	}

    /// <summary>
    /// Gets the array of scene names under the Subtraction of Similar Fractions topic
    /// </summary>
    /// <returns>Returns array of scene names</returns>
    public string[] GetSubSceneNames() {
		ParseData (1);
		return this.sceneNames;
	}

    /// <summary>
    /// Gets the array of scene names under the Addition and Subtraction of Dissimilar Fractions topic
    /// </summary>
    /// <returns>Returns array of scene names</returns>
    public string[] GetDisSceneNames() {
		ParseData (2);
		return this.sceneNames;
	}


}
