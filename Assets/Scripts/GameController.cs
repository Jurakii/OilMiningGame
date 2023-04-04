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
    public float truckCost; //Cost of truck
    public TextMeshProUGUI truckCostText;
    public Button truckButton; //Button for truck

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
    public float cash; //Player cash

    public Slider slider;
    public TextMeshProUGUI sellPriceText;
    public Button sell;
    public int i = 0;


    // Start is called before the first frame update
    void Start()
    {
        cashText.text = "$"+cash;
        if(cash >= truckCost) {
            truckButton.interactable = true;
            truckCostText.text = "$" + truckCost;
        } else {
            truckButton.interactable = false;
            truckCostText.text = "$" + truckCost;
        }
        StartCoroutine(GameStart());
         slider.onValueChanged.AddListener(delegate {sliderUpdate(); });
    }
    IEnumerator GameStart() {
        for(i = 0; i < trucks; i++) {
            StartCoroutine(Truck(i + 1));
            yield return new WaitForSeconds(1);
            Debug.Log(i);
        }
        Debug.Log(i);
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
    public void sliderUpdate(){
        sellPriceText.text = slider.value + "→$" + calculatePrice();
    }
    public void buttonSold() {
        //Debug.Log("Sold");
        if(slider.value > 0){
            cash += calculatePrice();
            cashText.text = "$"+cash;
            refinedOil -= slider.value;
            slider.value = 0;
            slider.maxValue = refinedOil;
            refinedText.text = "Refined Oil: " + refinedOil;
            sellPriceText.text = slider.value + "→$" + calculatePrice();
            if(cash >= truckCost) {
                truckButton.interactable = true;
            } else {
                truckButton.interactable = false;
            }
        }
    }
    public void buyTruck() {
        if(cash >= truckCost) {
            cash -= truckCost;
            trucks += 1;
            i += 1;
            StartCoroutine(Truck(i));
            truckCost = calculateTruckPrice();
            cashText.text = "$"+cash;
            if(cash >= truckCost) {
                truckButton.interactable = true;   
                truckCostText.text = "$" + truckCost;
            } else {
                truckButton.interactable = false;
                truckCostText.text = "$" + truckCost;
            }
        }
    }

    //public void 
    public float calculateTruckPrice(){
        return Mathf.Round(truckCost*1.25f);
    }
    public float calculatePrice() {
        return Mathf.Round((slider.value * 2 + (slider.value / 2) )* 1);
    }
}
