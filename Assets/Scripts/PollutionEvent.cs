using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PollutionEvent : MonoBehaviour {
    [System.Serializable]
    public class PollutionEventItem {
        public float pollutionLevel;
        public UnityEvent onPollutionLevelReached;
    }

    public PollutionEventItem[] pollutionEvents;

    private float currentPollutionLevel = 0f;

    void Update() {
        foreach (PollutionEventItem item in pollutionEvents) {
            if (currentPollutionLevel >= item.pollutionLevel) {
                item.onPollutionLevelReached.Invoke();
            }
        }
    }
}
