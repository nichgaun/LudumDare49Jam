using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] int healthMax; //set in editor
    [SerializeField] Text healthText; // set in editor
    int currentHealth;

    System.Action<int> onHealthChange;

    Dictionary<string, float> damageMultipliers;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthMax;
        healthText.text = "Health: " + currentHealth + "/" + healthMax;
        damageMultipliers = new Dictionary<string, float>();
    }

    public void Damage(int damageAmt){
        float totalMultiplier = 1;
        foreach (var multiplier in damageMultipliers)
        {
            totalMultiplier *= multiplier.Value;
        }
        
        currentHealth -= Mathf.RoundToInt(damageAmt * totalMultiplier);
        if (currentHealth < 0){
            currentHealth = 0;
        }
        healthText.text = "Health: " + currentHealth + "/" + healthMax;
        
        if (onHealthChange != null) onHealthChange(currentHealth);
    }

    public void Heal(int healAmt){
        currentHealth += healAmt;
        if (currentHealth > healthMax){
            currentHealth = healthMax;
        }
        healthText.text = "Health: " + currentHealth + "/" + healthMax;
        if (onHealthChange != null) onHealthChange(currentHealth);
    }

    public void SubscribeToHealthChange(System.Action<int> action){
        onHealthChange += action;
    }

    public void UnsubscribeToHealthChange(System.Action<int> action){
        onHealthChange -= action;
    }

    public void SetMultiplier(string key, float multiplier)
    {
        damageMultipliers[key] = multiplier;
    }

    public void RemoveMultiplier(string key)
    {
        damageMultipliers.Remove(key);
    }
}
