using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSpeedFromPlayerCar : MonoBehaviour
{
    Car playerCar; //set in start
    RectTransform rt; //set in start

    float minAngle = 130f;
    float maxAngle = -130f;
    float maxGaugeSpeed = 80f;

    void Start()
    {
        playerCar = GameObject.FindGameObjectWithTag(TagName.Player).GetComponent<Car>();
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        float desiredAngle = minAngle + Mathf.Clamp(Mathf.Abs(playerCar.HSpeed), 0, maxGaugeSpeed) / maxGaugeSpeed * (maxAngle - minAngle);

        rt.localRotation = Quaternion.Euler(
            0,
            0,
            desiredAngle);
    }
}
