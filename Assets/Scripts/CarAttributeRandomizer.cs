using UnityEngine;

public class CarAttributeRandomizer : MonoBehaviour
{
    void Awake()
    {
        var car = GetComponentInParent<Car>();
        car.DefaultSpeed *= Random.Range(0.5f, 1.5f);
    }
}
