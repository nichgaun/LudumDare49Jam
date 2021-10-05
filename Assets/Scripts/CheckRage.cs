using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRage : MonoBehaviour
{
    [SerializeField] int rageDelta;

    private void OnTriggerEnter(Collider other)
    {
        Rage rage = other.gameObject.GetComponent<Rage>();
        if (rage)
        {
            rage.UpdateRage(rageDelta);
        }
    }
}
