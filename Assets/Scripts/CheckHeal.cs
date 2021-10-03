using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHeal : MonoBehaviour
{
    [SerializeField] int healing;

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        if (health){
            health.Heal(healing);
        }
    }
}
