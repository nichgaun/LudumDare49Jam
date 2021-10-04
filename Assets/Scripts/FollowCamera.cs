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
    private Vector3 _targetPoint;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(TagName.Player).GetComponent<Car>();
        _initialDiff = transform.position - _player.transform.position;
        _hipass = GetComponent<AudioHighPassFilter>();
        _lopass = GetComponent<AudioLowPassFilter>();
        _targetPoint = Vector3.zero;
    }

    private void FixedUpdate()
    {
        StartCoroutine(LateFixedUpdate());
    }

    private IEnumerator<WaitForFixedUpdate> LateFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        /*
        if (_player.HSpeed > _player.DefaultSpeed + (_player.SprintMaxSpeed - _player.DefaultSpeed) * 0.5f)
        {
            _targetProg = Mathf.Lerp(1f, _targetProg, Mathf.Pow(1 - _lerpAmount, Time.fixedDeltaTime));
        }
        else if (_player.HSpeed > _player.DefaultSpeed + (_player.WalkMaxSpeed - _player.DefaultSpeed) * 0.5f)
        {
            _targetProg = Mathf.Lerp(0.25f, _targetProg, Mathf.Pow(1 - _lerpAmount, Time.fixedDeltaTime));
        }
        else
        {
            _targetProg = Mathf.Lerp(0f, _targetProg, Mathf.Pow(1 - _lerpAmount, Time.fixedDeltaTime));
        }
        var targetPoint = _player.transform.position + Vector3.right * (1f + 8f * _targetProg) + Vector3.up * 4f * _targetProg;
        var lookAtPoint = _player.transform.position + Vector3.right * (1f + 8f * _targetProg);
        */
        var targetPoint = Vector3.zero;
        if (_player.HSpeed > _player.DefaultSpeed + (_player.SprintMaxSpeed - _player.DefaultSpeed) * 0.9f)
        {
            targetPoint = 4f * Vector3.up;
        }
        _targetPoint = Vector3.Lerp(targetPoint, _targetPoint, Mathf.Pow(1 - 0.75f, Time.fixedDeltaTime));
        transform.position = Vector3.Lerp(_player.transform.position + _targetPoint + _initialDiff, transform.position + _player.DefaultSpeed * Time.deltaTime * Vector3.right, Mathf.Pow(1 - _lerpAmount, Time.fixedDeltaTime));
        transform.LookAt(_player.transform.position, Vector3.up);
        _roll = Mathf.Lerp(_player.VisualAcceleration * _rollFactor, _roll, Mathf.Pow(1 - _rollLerpAmount, Time.fixedDeltaTime));
        transform.Rotate(transform.InverseTransformDirection(transform.forward), _roll);
    }

    private void Update()
    {
        _landedDelay = Mathf.Max(0, _landedDelay - Time.deltaTime);
        float target = _landedDelay > 1e-3f ? 10f : 10f + 1500f * (1f - Mathf.Exp(Mathf.Min(0, (2f - _player.transform.position.y * _player.transform.position.y - Mathf.Max(0, _player.FallSpeed) * Mathf.Max(0, _player.FallSpeed))) / 10f));
        if (_player.FallSpeed < 0)
        {
            if (_player.transform.position.y < 5f)
            {
                _lowTarget = 22000f;
                _landedDelay = 0.5f;
            }
            else
            {
                _lowTarget = _landedDelay > 1e-3f ? 22000f : Mathf.Min(_lowTarget, 200f + 21800f * Mathf.Exp(-target));
                target = Mathf.Max(10f, Mathf.Min(target, _lowTarget - 1000f));
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
            hipassSpeed = 500f;
        }
        else if (_hipass.cutoffFrequency < 100f)
        {
            hipassSpeed = 200f;
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
