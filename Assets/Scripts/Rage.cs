using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : MonoBehaviour
{
    [SerializeField] float rageMax; //set in editor
    float currentRage;
    public float CurrentRage { get { return currentRage; } }

    Dictionary<string, float> rageMultipliers;

    // Start is called before the first frame update
    void Start()
    {
        currentRage = 0;
        rageMultipliers = new Dictionary<string, float>();
    }

    public void UpdateRage(float rageUpdateVal)
    {
        float totalMultiplier = 1;
        foreach (var multiplier in rageMultipliers)
        {
            totalMultiplier *= multiplier.Value;
        }
        currentRage += rageUpdateVal * totalMultiplier;
        if (currentRage < 0){
            currentRage = 0;
        }
        else if (currentRage > rageMax){
            currentRage = rageMax;
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
