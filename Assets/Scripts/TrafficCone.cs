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
    ConeState _state = ConeState.IDLE;

    void Start() {} // do nothing

    void Update()
    {
        // spin
        switch (_state) {
        case ConeState.IDLE:
            break;
        case ConeState.COLLIDING:
            transform.Rotate(Time.deltaTime * new Vector3(collidingTumbleSpeed, collidingTumbleSpeed, collidingTumbleSpeed));
            transform.position += (Vector3.up + Vector3.back * 2) * collectingVerticalSpeed * Time.deltaTime;
            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            _state = ConeState.COLLIDING;
    }
}
