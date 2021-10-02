using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private enum GearShiftState
    {
        UPSHIFTED,
        DOWNSHIFTING,
        DOWNSHIFTED,
    }

    private Collider _collider;
    [SerializeField] private Driver _driver;
    [SerializeField] private float _defaultSpeed;
    [SerializeField] private float _brakeMinSpeed;
    [SerializeField] private float _brakeDeceleration;
    [SerializeField] private float _walkMaxSpeed;
    [SerializeField] private float _walkAcceleration;
    [SerializeField] private float _sprintMaxSpeed;
    [SerializeField] private float _sprintMaxAcceleration;
    [SerializeField] private float _decelerationDuringShift;
    [SerializeField] private float _vSpeed;
    [SerializeField] private float _downshiftDelay;
    private float _hSpeed;
    private GearShiftState _gearShiftState = GearShiftState.UPSHIFTED;
    public float DefaultSpeed { get { return _defaultSpeed; } }
    private Coroutine _downshiftCoroutine;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _hSpeed = _defaultSpeed;
    }

    void FixedUpdate()
    {
        // Get car input from player or AI controller
        _driver.ControlCar(out int hMove, out int vMove, out bool sprint);

        // Handle shifting using a simplified system with only two gears, one that is "too high" and unable to provide optimal acceleration and one that is lower and has more acceleration
        // The lower gear is your sprint state, but takes a second to shift to, giving a lurching feeling
        if (sprint) {
            if (_downshiftCoroutine == null)
            {
                _downshiftCoroutine = StartCoroutine(Downshift());
            }
        } else {
            if (_downshiftCoroutine != null)
            {
                StopCoroutine(_downshiftCoroutine);
                _downshiftCoroutine = null;
            }
            _gearShiftState = GearShiftState.UPSHIFTED;
        }

        // Alter speed
        if (hMove == 0)
        {
            // Tend toward cruising speed if no input given
            if (_hSpeed < _defaultSpeed)
            {
                hMove = 1;
            }
            else if (_hSpeed > _defaultSpeed)
            {
                hMove = -1;
            }
        }
        if (hMove == 1)
        {
            // Accelerate
            if (_gearShiftState == GearShiftState.UPSHIFTED)
            {
                _hSpeed = Mathf.Min(_hSpeed + _walkAcceleration * Time.fixedDeltaTime, _walkMaxSpeed);
            }
            else if (_gearShiftState == GearShiftState.DOWNSHIFTED)
            {
                _hSpeed = Mathf.Min(_hSpeed + Mathf.Max(_defaultSpeed, _hSpeed) / _sprintMaxSpeed * _sprintMaxAcceleration * Time.fixedDeltaTime, _sprintMaxSpeed);
            }
        }
        else if (hMove == -1)
        {
            // Brake
            _hSpeed = Mathf.Max(_hSpeed - _brakeDeceleration * Time.fixedDeltaTime, _brakeMinSpeed);
        }

        // Additionally decelerate while shifting to add to the lurch effect
        if (_gearShiftState == GearShiftState.DOWNSHIFTING)
        {
            _hSpeed = Mathf.Max(_hSpeed - _decelerationDuringShift * Time.fixedDeltaTime, _brakeMinSpeed);
        }

        transform.position = new Vector3(transform.position.x + _hSpeed * Time.fixedDeltaTime, transform.position.y, transform.position.z + vMove * _vSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator<WaitForSeconds> Downshift()
    {
        _gearShiftState = GearShiftState.DOWNSHIFTING;
        yield return new WaitForSeconds(_downshiftDelay);
        _gearShiftState = GearShiftState.DOWNSHIFTED;
    }


    void OnTriggerEnterOrStay(Collider other)
    {
        Physics.ComputePenetration(_collider, _collider.transform.position, _collider.transform.rotation, other, other.transform.position, other.transform.rotation, out Vector3 direction, out float distance);
        transform.position += new Vector3(direction.x * distance, 0, direction.z * distance);
    }

    void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterOrStay(other);
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnterOrStay(other);
    }
}
