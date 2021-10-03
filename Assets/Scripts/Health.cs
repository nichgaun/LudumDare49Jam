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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthMax;
        healthText.text = "Health: " + currentHealth + "/" + healthMax;
    }

    public void Damage(int damageAmt){
        currentHealth -= damageAmt;
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

}
