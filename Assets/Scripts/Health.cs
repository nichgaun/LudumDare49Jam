using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int healthMax; //set in editor
    public int MaxHealth { get { return healthMax; } }
    [SerializeField] int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    System.Action<int> onHealthChange;

    Dictionary<string, float> damageMultipliers;

    private void Awake()
    {
        damageMultipliers = new Dictionary<string, float>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthMax;
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
        
        if (onHealthChange != null) onHealthChange(currentHealth);
    }

    public void Heal(int healAmt){
        currentHealth += healAmt;
        if (currentHealth > healthMax){
            currentHealth = healthMax;
        }
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
