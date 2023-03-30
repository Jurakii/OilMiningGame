using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [Header("Oil Mining")]
    public float miningSpeed;
    
    [Header("Oil Transport")]
    public float trucks;
    public float truckSpeed;
    public float truckCapacity; // How much each truck can carry
    public float stationTimes; // How log it takes to load/unload the trucks
    

    [Header("Refinery")]
    public float refineryInv;
    public Text refineryText;

    [Header("")]
    public Text oilText;
    private float oil;
    void Start()
    {
        oil = 0f;
        
        StartCoroutine(Return());
    }

    void Update()
    {
        // passively generate oil over time
        oil += miningSpeed * Time.deltaTime;
        oilText.text = "" + Mathf.Round(oil);
    }

    public void newTruck() {

    }

IEnumerator Return() {
    Debug.Log("Truck is leaving Refinery");
    yield return new WaitForSeconds(truckSpeed/trucks);
    Debug.Log("Arrived at Mining");
    yield return new WaitForSeconds(stationTimes);

    StartCoroutine(Travel());
}


IEnumerator Travel() {
    float oilInventory;

    if (oil >= truckCapacity) {
        oilInventory = truckCapacity;
        
    } else {
        oilInventory = Mathf.Round(oil);
    }
        oil -= oilInventory;

    Debug.Log("Truck is leaving Mining with " + oilInventory + " units of oil");

    yield return new WaitForSeconds(truckSpeed);
    Debug.Log("Arrived at Refinery with " + oilInventory + " units of oil");
    yield return new WaitForSeconds(stationTimes);
    refineryInv += oilInventory;
    refineryText.text = ""+refineryInv;
    oilInventory = 0;
    Debug.Log("Unloaded");
    StartCoroutine(Return());
}


}
