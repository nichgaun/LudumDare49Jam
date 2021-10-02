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

    [SerializeField] float collectingVerticalSpeed; // set in editor
    [SerializeField] float collidingTumbleSpeed; // set in editor
    [SerializeField] float percentageToVary; // set in editor
    ConeState _state = ConeState.IDLE;
    int _flyawayDirection;

    [SerializeField] int coneDamage;// set in editor

    void Start() {
        collidingTumbleSpeed = Random.Range(collidingTumbleSpeed * percentageToVary, collidingTumbleSpeed * (1 + percentageToVary));
        collectingVerticalSpeed = Random.Range(collectingVerticalSpeed * percentageToVary, collectingVerticalSpeed * (1 + percentageToVary));
    }

    void Update()
    {
        // spin
        switch (_state) {
        case ConeState.IDLE:
            break;
        case ConeState.COLLIDING:
            transform.Rotate(Time.deltaTime * new Vector3(collidingTumbleSpeed, collidingTumbleSpeed, collidingTumbleSpeed));
            transform.position += (Vector3.up + Vector3.back * 2 * _flyawayDirection) * collectingVerticalSpeed * Time.deltaTime;
            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // determine relative positioning
        if (other.gameObject.transform.position.z < gameObject.transform.position.z) {
            _flyawayDirection = -1;
        } else {
            _flyawayDirection = 1;
        }
        _state = ConeState.COLLIDING;

        Health health = other.gameObject.GetComponent<Health>();
        if (health){
            health.Damage(coneDamage);
        }
    }
}
