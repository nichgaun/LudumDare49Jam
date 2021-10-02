using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficCone : MonoBehaviour
{
    private enum ConeState
    {
        IDLE,
        COLLIDING,
    }

    [SerializeField] float collidingTumbleSpeed; // set in editor
    [SerializeField] float percentageToVary; // set in editor
    ConeState _state = ConeState.IDLE;
    CollidableObstacle _ourCollidableSelf;

    void Start() {
        _ourCollidableSelf = GetComponent<CollidableObstacle>();
    }

    void Update() {}

    private void OnTriggerEnter(Collider other)
    {
        // determine relative positioning
        int _flyawayDirection;
        if (other.gameObject.transform.position.z < gameObject.transform.position.z) {
            _flyawayDirection = -1;
        } else {
            _flyawayDirection = 1;
        }
        var relativeSpeed = other.GetComponent<Car>().HSpeed;
        _state = ConeState.COLLIDING;
        _ourCollidableSelf.SetVerticalSpeed(relativeSpeed);
        _ourCollidableSelf.SetHorizontalSpeed(relativeSpeed * _flyawayDirection);
        _ourCollidableSelf.SetTumbleDirectionAndIntensity(new Vector3(1, 1, 1), collidingTumbleSpeed);
    }
}
