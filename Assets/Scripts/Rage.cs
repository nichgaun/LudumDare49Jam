using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rage : MonoBehaviour
{
    [SerializeField] float rageMax; //set in editor
    [SerializeField] int rageInterval; //set in editor
    [SerializeField] float rageIntervalIncrease; //set in editor
    [SerializeField] Text rageText; // set in editor
    float currentRage;
    float rageTime = 0;

    public float rageMultiplier;



    System.Action<int> onRageChange;

    // Start is called before the first frame update
    void Start()
    {
        currentRage = 0;
        rageMultiplier = 1;
        rageText.text = "Rage: " + currentRage + "/" + rageMax;
    }

    public void UpdateRage(float rageUpdateVal){
        currentRage += rageUpdateVal;
        if (currentRage < 0){
            currentRage = 0;
        }
        else if (currentRage > rageMax){
            currentRage = rageMax;
        }
        rageText.text = "Rage: " + currentRage + "/" + rageMax;
        
    }

    void Update(){
        rageTime += Time.deltaTime;
        if (rageTime > rageInterval){
            UpdateRage(rageIntervalIncrease * rageMultiplier);
            rageTime = 0;
        }

    }


}
