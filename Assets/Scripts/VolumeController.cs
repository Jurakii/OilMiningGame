using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VolumeController : MonoBehaviour
{
    // Reference to the slider UI component
    public Slider volumeSlider;

    // Reference to the Video Player component
    public VideoPlayer videoPlayer;

    // The default value of the volume
    public float defaultVolume = 0.5f;

    // Called when the script is first enabled
    void Start()
    {
        // Set the value of the slider to the default volume
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", defaultVolume);

        // Add a listener to the slider value changed event
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Set the volume to the default value
        SetVolume(PlayerPrefs.GetFloat("Volume", defaultVolume));
    }

    // Called when the slider value is changed
    void SetVolume(float value)
    {
        // Set the volume of the Audio Listener
        AudioListener.volume = value;

        // Save the volume setting to player prefs
        PlayerPrefs.SetFloat("Volume", value);

        // Set the volume of the Video Player
        if (videoPlayer != null)
        {
            if (value == 0f)
            {
                videoPlayer.SetDirectAudioMute(0, true);
            }
            else
            {
                videoPlayer.SetDirectAudioMute(0, false);
                videoPlayer.SetDirectAudioVolume(0, value);
            }
        }
    }
}
