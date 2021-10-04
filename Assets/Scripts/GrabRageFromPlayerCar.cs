using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRageFromPlayerCar : MonoBehaviour
{
    Rage playerRage; //set in start
    RectTransform rt; //set in start

    float minAngle = 130f;
    float maxAngle = -130f;
    float maxGaugeSpeed = 100f;

    void Start()
    {
        playerRage = GameObject.FindGameObjectWithTag(TagName.Player).GetComponent<Rage>();
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        rt.localRotation = Quaternion.Euler(0, 0, minAngle + Mathf.Clamp(Mathf.Abs(playerRage.CurrentRage), 0, maxGaugeSpeed) / maxGaugeSpeed * (maxAngle - minAngle));
    }
}
