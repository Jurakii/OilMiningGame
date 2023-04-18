using System;
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
    public TextMeshProUGUI truckText;

    [Header("Refinery")]
    public float refineryInv; //How much is in refinery
    public TextMeshProUGUI refineryText; //Text of how much is in refinery
    public float refinedOil; //Refined oil
    public TextMeshProUGUI refinedText;
    public float refineSpeed;
    public bool refining = false;
    public float refineryCapacity;

    [Header("Player")]
    public TextMeshProUGUI oilText;
    public float oil; //Player Oil count
    public TextMeshProUGUI cashText;
    public float cash; //Player cash

    public Slider slider;
    public TextMeshProUGUI sellPriceText;
    public Button sell;
    public int i = 0;
    public float pollution = 0;
    public TextMeshProUGUI pollutionText;

    [Header("Upgrades")]
    public float multi = 1f;
    public float upgradeCost = 0f; //Upgrades sell price, and speed
    public TextMeshProUGUI upgradeText;
    public Button upgradeButton;


    // Start is called before the first frame update
    void Start()
    {
        pollutionText.text = "Pollution: " + pollution + "%";
        truckText.text = "" + trucks;
        upgradeText.text = "$"+upgradeCost;
        cashText.text = "$"+cash;
        if(cash >= upgradeCost){
            upgradeButton.interactable = true;
        } else {
            upgradeButton.interactable = false;
        }
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
        yield return new WaitForSeconds(5);
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
        oil += (miningSpeed + miningAmount) * Time.deltaTime;
        oilText.text = "Crude Oil: " + Mathf.Round(oil);
            if(!refining) {
                if(refineryInv >= refineryCapacity) {
                    refining = true;
                    StartCoroutine(refine());
                }
            }
    }
    IEnumerator refine() {
        yield return new WaitForSeconds(refineSpeed);
        if(refineryInv >= refineryCapacity) {
            refineryInv -= refineryCapacity;
            refinedOil += refineryCapacity;
        } else {
            refinedOil += refineryCapacity;
            refineryInv = 0;
            
        }
        refineryInv = Mathf.Round(refineryInv);
        refinedOil = Mathf.Round(refinedOil);

        refineryText.text = "Refinery Oil: " + refineryInv;
        
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
            if(cash >= upgradeCost) {
                upgradeButton.interactable = true;
            } else {
                upgradeButton.interactable = false;
            }
        }
    }
    public void buyTruck() {
        if(cash >= truckCost) {
            
            pollution += 1;
            pollutionText.text = "Pollution: " + pollution + "%";
            cash -= truckCost;
            trucks += 1;
            truckText.text = ""+trucks;
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
                        if(cash < upgradeCost) {
                upgradeButton.interactable = false;
            }
        }
    }
    public void upgrade() {
        if(cash >= upgradeCost) {
            cash -= upgradeCost;
            pollution += 1;
            pollutionText.text = "Pollution: " + pollution + "%";
            miningAmount += 0.2f;
            cashText.text = "$"+cash;
            upgradeCost = calculateUpgrade();
            upgradeText.text = "$" + upgradeCost;
            multi += 1f;
            if(truckTravelTime > 0.25f) {
                truckTravelTime -= 0.25f;
            } else {
                truckTravelTime = 0.25f;
            }
            if(refineSpeed > 0.25f) {
                refineSpeed -= 0.25f;
            } else {
                refineSpeed = 0.25f;
            }
            if(loadTime > 0.05f) {
                loadTime -= 0.05f;
            }else {
                loadTime = 0.05f;
            }

            truckCapacity += 2;
            miningSpeed += 1;
            refineryCapacity += 2;
            if(cash < upgradeCost) {
                upgradeButton.interactable = false;
            }
            if(cash >= truckCost) {
                truckButton.interactable = true;   
                truckCostText.text = "$" + truckCost;
            } else {
                truckButton.interactable = false;
                truckCostText.text = "$" + truckCost;
            }
        }
    }
    

    public float calculateUpgrade() {
        return Mathf.Round(upgradeCost*1.1f);
    } 
    public float calculateTruckPrice(){
        return Mathf.Round(truckCost*1.1f);
    }
    public float calculatePrice() {
        return Mathf.Round((slider.value)* multi);
    }
}
