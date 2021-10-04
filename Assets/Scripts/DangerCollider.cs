using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DangerCollider : MonoBehaviour
{
    public bool StillDanger { get; private set; }
    public bool OutOfBounds { get; private set; }
    private Car _car;
    private Collider _collider;
    [SerializeField] private float _dangerTimeThreshold;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Claim(Car car)
    {
        _car = car;
    }

    public void ControlCar()
    {
        StillDanger = _car.DirectionMultiplier * (transform.TransformPoint(_collider.bounds.center).z + _collider.bounds.size.z / 2) > 0;
        OutOfBounds = StillDanger;
    }

    private void OnTriggerEnterOrStay(Collider other)
    {
        var otherCar = other.gameObject.GetComponent<Car>();

        if (otherCar == _car)
            return;

        float otherSpeed = otherCar ? otherCar.HSpeed : 0;
        float diff = otherSpeed - _car.HSpeed;
        if ((other.transform.position.x - _car.transform.position.x) / Mathf.Max(1e-3f, _car.DefaultSpeed - otherSpeed) < _dangerTimeThreshold)
        {
            StillDanger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterOrStay(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnterOrStay(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnterOrStay(collision.collider);
    }

    private void OnCollisionStay(Collision collision)
    {
        OnTriggerEnterOrStay(collision.collider);
    }
}
