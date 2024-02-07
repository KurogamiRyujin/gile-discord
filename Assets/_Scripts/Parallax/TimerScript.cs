using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {
	public Text timerLabel;
	private float time;

	private bool isRecording = true;

	public void stop() {
		this.isRecording = false;
	}


	public string getTimeString() {
		return this.timerLabel.text;
	}
	public float getTimeFloat() {
		return this.time;
	}
	public void updateLabel() {
		var minutes = time / 60; //Divide the guiTime by sixty to get the minutes.
		var seconds = time % 60;//Use the euclidean division for the seconds.
		var fraction = (time * 100) % 100;

		//update the label value
		this.timerLabel.text = string.Format ("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
	}

	void Update() {
		if (isRecording) {

			time += Time.deltaTime;

			var minutes = time / 60; //Divide the guiTime by sixty to get the minutes.
			var seconds = time % 60;//Use the euclidean division for the seconds.
			var fraction = (time * 100) % 100;

			//update the label value
			timerLabel.text = string.Format ("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
		}
	}
}