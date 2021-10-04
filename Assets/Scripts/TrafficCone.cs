using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficCone : MonoBehaviour
{
    [SerializeField] float collidingTumbleSpeed; // set in editor
    private CollidableObstacle _ourCollidableSelf;

    [SerializeField] int coneDamage;// set in editor

    void Start() {
        _ourCollidableSelf = GetComponent<CollidableObstacle>();
    }

    void Update() {}

    private void OnTriggerEnter(Collider other)
    {
        var impactingCar = other.GetComponent<Car>();
        if (!impactingCar)
        {
            return;
        }

        GetComponent<CollidableObstacle>().dead = true;

        // determine relative positioning
        int _flyawayDirection;
        if (other.gameObject.transform.position.z < gameObject.transform.position.z) {
            _flyawayDirection = -1;
        } else {
            _flyawayDirection = 1;
        }
        var relativeSpeed = impactingCar.HSpeed;
        _ourCollidableSelf.SetVerticalSpeed(relativeSpeed);
        _ourCollidableSelf.SetHorizontalSpeed(relativeSpeed * _flyawayDirection);
        _ourCollidableSelf.SetTumbleDirectionAndIntensity(new Vector3(0, 1, 1), collidingTumbleSpeed);
        SoundPlayer.Play("hitCone");
    }
}
