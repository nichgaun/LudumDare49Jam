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

    Dictionary<string, float> rageMultipliers;

    System.Action<int> onRageChange;

    // Start is called before the first frame update
    void Start()
    {
        currentRage = 0;
        rageText.text = "Rage: " + currentRage + "/" + rageMax;
        rageMultipliers = new Dictionary<string, float>();
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

            float totalMultiplier = 1;
            foreach (var multiplier in rageMultipliers)
            {
                totalMultiplier *= multiplier.Value;
            }

            UpdateRage(rageIntervalIncrease * totalMultiplier);
            rageTime = 0;
        }

    }

    public void SetMultiplier(string key, float multiplier)
    {
        rageMultipliers[key] = multiplier;
    }

    public void RemoveMultiplier(string key)
    {
        rageMultipliers.Remove(key);
    }
}
