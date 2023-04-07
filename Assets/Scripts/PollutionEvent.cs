using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class PollutionEvent : MonoBehaviour {
    [System.Serializable]
    public class PollutionEventItem {
        public float pollutionLevel;
        public UnityEvent onPollutionLevelReached;
    }

    public PollutionEventItem[] pollutionEvents;

    private float currentPollutionLevel = 0f;

    // Reference to the script containing the pollution level variable
    public GameController gameController;
    
    public Camera mainCamera;
    public Color sky1;


    void Update() {
        // Access the pollution level variable through the reference to the other script
        currentPollutionLevel = gameController.pollution;

        foreach (PollutionEventItem item in pollutionEvents) {
            if (currentPollutionLevel >= item.pollutionLevel) {
                item.onPollutionLevelReached.Invoke();
            }
        }
    }


    public void swapSky1() {
        mainCamera.backgroundColor = sky1;
    }
}
