    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeComponent : MonoBehaviour
{
    [SerializeField] float collidingTumbleSpeed; // set in editor
    private CollidableObstacle _ourCollidableSelf;
    private int _spinDirection;

    // 1 or -1, for left & right spinning differently
    public void setSpinDirection(int dir) {
        _spinDirection = dir;
    }

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
            _flyawayDirection = 1;
        } else {
            _flyawayDirection = -1;
        }

        var relativeSpeed = impactingCar.HSpeed;
        _ourCollidableSelf.SetVerticalSpeed(relativeSpeed);
        _ourCollidableSelf.SetHorizontalSpeed(relativeSpeed * _flyawayDirection);
        _ourCollidableSelf.SetTumbleDirectionAndIntensity(new Vector3(0, _spinDirection, 1), collidingTumbleSpeed);
        SoundPlayer.Play("hitBarricade");
    }

}
