using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rage : MonoBehaviour
{
    [SerializeField] float rageMax; //set in editor
    [SerializeField] Text rageText; // set in editor
    float currentRage;
    System.Action<int> onRageChange;

    // Start is called before the first frame update
    void Start()
    {
        currentRage = 0;
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
}
