using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Car _player;
    private Vector3 _initialDiff;
    [SerializeField] private float _lerpAmount;
    [SerializeField] private float _rollFactor;
    [SerializeField] private float _rollLerpAmount;
    private float _roll;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Car>();
        _initialDiff = transform.position - _player.transform.position;
    }

    private void FixedUpdate()
    {
        StartCoroutine(LateFixedUpdate());
    }

    private IEnumerator<WaitForFixedUpdate> LateFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        transform.position = Vector3.Lerp(_player.transform.position + _initialDiff, transform.position + _player.DefaultSpeed * Time.deltaTime * Vector3.right, Mathf.Pow(1 - _lerpAmount, Time.fixedDeltaTime));
        transform.LookAt(_player.transform.position, Vector3.up);
        _roll = Mathf.Lerp(_player.VisualAcceleration * _rollFactor, _roll, Mathf.Pow(1 - _rollLerpAmount, Time.fixedDeltaTime));
        transform.Rotate(transform.InverseTransformDirection(transform.forward), _roll);
    }
}
