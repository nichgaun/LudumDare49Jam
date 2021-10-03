using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHeal : MonoBehaviour
{
    [SerializeField] int healing;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HEAL");
        Health health = other.gameObject.GetComponent<Health>();
        if (health){
            Debug.Log("Healing");
            health.Heal(healing);
        }
    }
}
