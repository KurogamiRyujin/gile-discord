using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller that handles the background music 
/// </summary>
public class BGMController : MonoBehaviour {
    /// <summary>
    /// Reference to the slider in the Options Popup 
    /// </summary>
	[SerializeField] Slider slider;
    /// <summary>
    /// Reference to the AudioManager script
    /// </summary>
    AudioManager audioManager;

    /// <summary>
    /// Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled 
    /// </summary>
    // Use this for initialization
    void Start () {
        audioManager = FindObjectOfType<AudioManager>();
        Debug.LogError("BGM: "+audioManager.BGM.volume);
        UpdateSliderValue(audioManager.BGM.volume);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// Called on slider value change
    /// </summary>
    public void OnSliderValueChanged() {
        UpdateSliderValue(slider.value);
    }

    /// <summary>
    /// Updates the value of the slider
    /// </summary>
    /// <param name="sliderValue">Value to be assigned to the slider</param>
    void UpdateSliderValue(float sliderValue) {
        Debug.LogError("UPDATED BGM " + sliderValue);
        slider.value = sliderValue;
        audioManager.BGM.volume = sliderValue;

    }
}
