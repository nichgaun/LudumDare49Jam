using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioHighPassFilter))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class FollowCamera : MonoBehaviour
{
    private Car _player;
    private Vector3 _initialDiff;
    [SerializeField] private float _lerpAmount;
    [SerializeField] private float _rollFactor;
    [SerializeField] private float _rollLerpAmount;
    private float _roll;
    private AudioHighPassFilter _hipass;
    private AudioLowPassFilter _lopass;
    private float _lowTarget;
    private float _landedDelay;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(TagName.Player).GetComponent<Car>();
        _initialDiff = transform.position - _player.transform.position;
        _hipass = GetComponent<AudioHighPassFilter>();
        _lopass = GetComponent<AudioLowPassFilter>();
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

    private void Update()
    {
        _landedDelay = Mathf.Max(0, _landedDelay - Time.deltaTime);
        float target = _landedDelay > 1e-3f ? 10f : 10f + 1500f * (1f - Mathf.Exp(Mathf.Min(0, (5f - _player.transform.position.y)) / 10f));
        if (_player.FallSpeed < 0)
        {
            if (_player.transform.position.y < 5f)
            {
                _lowTarget = 22000f;
                _landedDelay = 2f;
            }
            else
            {
                _lowTarget = _landedDelay > 1e-3f ? 22000f : Mathf.Min(_lowTarget, 130f + 21970f * Mathf.Exp(-target));
            }
        }
        else
        {
            _lowTarget = 22000f;
        }
        float lopassSpeed;
        if (_lopass.cutoffFrequency > 200f)
        {
            lopassSpeed = 30000f;
        }
        else
        {
            lopassSpeed = 1600f;
        }
        if (_lopass.cutoffFrequency < _lowTarget)
        {
            _lopass.cutoffFrequency = Mathf.Min(_lowTarget, _lopass.cutoffFrequency + lopassSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            _lopass.cutoffFrequency = Mathf.Max(_lowTarget, _lopass.cutoffFrequency - lopassSpeed * Time.unscaledDeltaTime);
        }
        float hipassSpeed;
        if (_hipass.cutoffFrequency < 70f)
        {
            hipassSpeed = 200f;
        }
        else if (_hipass.cutoffFrequency < 100f)
        {
            hipassSpeed = 150f;
        }
        else
        {
            hipassSpeed = 1500f;
        }
        if (_hipass.cutoffFrequency < target)
        {
            _hipass.cutoffFrequency = Mathf.Min(target, _hipass.cutoffFrequency + hipassSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            _hipass.cutoffFrequency = Mathf.Max(target, _hipass.cutoffFrequency - hipassSpeed * Time.unscaledDeltaTime);
        }
    }
}
