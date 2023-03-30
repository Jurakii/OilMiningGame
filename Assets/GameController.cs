using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Mining")]
    public float miningSpeed; //How fast you can mine
    public float miningAmount; //Amount per cycle

    [Header("Transport")]
    public float trucks; //Number of trucks
    public float truckTravelTime; //Truck speed
    public float truckCapacity; //Truck capacity
    public float loadTime; //Truck load/offload speed

    [Header("Refinery")]
    public float refineryInv; //How much is in refinery
    public TextMeshProUGUI refineryText; //Text of how much is in refinery
    public float refinedOil; //Refined oil
    public TextMeshProUGUI refinedText;
    public float refineSpeed;
    public bool refining = false;

    [Header("Player")]
    public TextMeshProUGUI oilText;
    public float oil; //Player Oil count
    public TextMeshProUGUI cashText;
    public int cash; //Player cash

    public Slider slider;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameStart());
    }
    IEnumerator GameStart() {
        for(int i = 0; i < trucks; i++) {
            StartCoroutine(Truck(i + 1));
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator Truck(int num) {
        Debug.Log("Truck " + num + " is LOADING");
        
        yield return new WaitForSeconds(loadTime);

        float oilInventory;

        if (oil >= truckCapacity) {
        oilInventory = truckCapacity;
        
        } else {
            oilInventory = Mathf.Round(oil);
        }

        oil -= oilInventory;
        Debug.Log("Truck " + num + " is LEAVING, Oil on truck: " + oilInventory);
        yield return new WaitForSeconds(truckTravelTime);
        StartCoroutine(Arrival(num, oilInventory));
    }
    IEnumerator Arrival(int num, float inv) {
        Debug.Log("Truck " + num + " has arrived at the Refinery, Unloading");
        yield return new WaitForSeconds(loadTime);
        refineryInv += inv;
        refineryText.text = "Refinery Oil: " + refineryInv;
        Debug.Log("Fully unloaded, Truck " + num + " is returning");
        StartCoroutine(Return(num));
    }
    IEnumerator Return(int num) {
        yield return new WaitForSeconds(truckTravelTime);
        Debug.Log("Truck " + num + " has arrived at Mining");
        StartCoroutine(Truck(num));
    }
    //The Petroleum Refineries Sector is the second highest ranked sector in terms of GHG emissions per facility, with an average of 1.22 million metric tons of carbon dioxide equivalent (MMT CO2e), behind only the Power Plant Sector.
    // Update is called once per frame
    void Update()
    {
        oil += miningSpeed * Time.deltaTime;
        oilText.text = "Crude Oil: " + Mathf.Round(oil);
        if(!refining) {
            if(refineryInv > 0) {
                refining = true;
                StartCoroutine(refine());
            }
        }
    }
    IEnumerator refine() {
        yield return new WaitForSeconds(refineSpeed);
        refineryInv -= 1;
        refineryText.text = "Refinery Oil: " + refineryInv;
        refinedOil += 1;
        refinedText.text = "Refined Oil: " + refinedOil;
        refining = false;
        slider.maxValue = refinedOil;
    }
}
