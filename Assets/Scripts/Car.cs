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
    [SerializeField] private float _strafeSpeed;
    [SerializeField] private float _strafeStabilization;
    [SerializeField] private float _downshiftDelay;
    [SerializeField] private float _vImpactMultiplier;
    [SerializeField] private float _knockAsideThreshold;
    [SerializeField] private float _knockAsideForce;
    [SerializeField] private float _gravity;
    [SerializeField] private GameObject _modelObject;
    [SerializeField] private float _flipSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _bounceThreshold;
    [SerializeField] private float _bounceProportion;
    [SerializeField] private float _bouncePitchSpeed;
    [SerializeField] private float _breakMagnitude;
    [SerializeField] private bool _breakable;
    private float _hSpeed;
    private float _vSpeed;
    private float _fallSpeed;
    private float _vUncontrolledSpeed;
    private GearShiftState _gearShiftState = GearShiftState.UPSHIFTED;
    private Coroutine _downshiftCoroutine;
    public float VisualAcceleration { get; private set; }
    public float HSpeed { get { return _hSpeed; } }
    public float VSpeed { get { return _vSpeed; } }
    public float FallSpeed { get { return _fallSpeed; } }
    public float DefaultSpeed { get { return _defaultSpeed; } set { _defaultSpeed = value; } }
    public float SprintMaxSpeed { get { return _sprintMaxSpeed; } }
    public float WalkMaxSpeed { get { return _walkMaxSpeed; } }
    private int directionMultiplier = 1;
    private float _pitch;
    private float _pitchSpeed;
    private float _roll;
    private float _rollSpeed;
    private float _yaw;
    private float _yawSpeed;
    private float _rampY;
    private bool _wasRamping;
    private bool _stillRamping;
    [SerializeField] private float _yawLerpAmount;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _hSpeed = _defaultSpeed;
        _driver.Claim(this);
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

        // Apply acceleration
        VisualAcceleration = 0;
        var cruising = hMove == 0;
        if (cruising)
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
                var acc = _walkAcceleration;
                VisualAcceleration += acc;
                _hSpeed = Mathf.Min(_hSpeed + acc * Time.fixedDeltaTime, _walkMaxSpeed);
            }
            else if (_gearShiftState == GearShiftState.DOWNSHIFTED)
            {
                var acc = Mathf.Max(_defaultSpeed, _hSpeed) / _sprintMaxSpeed * _sprintMaxAcceleration;
                VisualAcceleration += acc;
                _hSpeed = Mathf.Min(_hSpeed + acc * Time.fixedDeltaTime, _sprintMaxSpeed);
            }
        }
        else if (hMove == -1)
        {
            // Brake
            var acc = -_brakeDeceleration;
            VisualAcceleration += acc;
            _hSpeed = Mathf.Max(_hSpeed + acc * Time.fixedDeltaTime, _brakeMinSpeed);
        }
        var groundHeight = _rampY;
        if (transform.position.y > groundHeight)
        {
            _fallSpeed -= _gravity * Time.fixedDeltaTime;
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                Emit();
                _pitchSpeed = -_flipSpeed;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
            if (_fallSpeed < -_bounceThreshold)
            {
                _fallSpeed = _fallSpeed * -_bounceProportion;
                _pitchSpeed = Random.Range(-_bouncePitchSpeed, _bouncePitchSpeed);
                _yawSpeed = Mathf.Clamp(_yawSpeed * -0.9f, -_bouncePitchSpeed, _bouncePitchSpeed);
                _rollSpeed = Random.Range(-_bouncePitchSpeed, _bouncePitchSpeed);
                _yaw = 0;
            }
            else
            {
                _fallSpeed = Mathf.Max(_fallSpeed, 0);
                _pitchSpeed = 0;
                _yawSpeed = 0;
                _rollSpeed = 0;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                _fallSpeed = _jumpSpeed;
            }
            _pitch = 0;
            _roll = 0;
        }

        // Strafing
        _vUncontrolledSpeed = Mathf.Lerp(0, _vUncontrolledSpeed, Mathf.Pow(1 - _strafeStabilization, Time.fixedDeltaTime));
        if (Mathf.Abs(_vUncontrolledSpeed) < 1e-2)
        {
            _vUncontrolledSpeed = 0;
        }
        _vSpeed = vMove * _strafeSpeed + _vUncontrolledSpeed;

        // Additionally decelerate while shifting to add to the lurch effect
        if (_gearShiftState == GearShiftState.DOWNSHIFTING)
        {
            var acc = -_decelerationDuringShift;
            VisualAcceleration += acc;
            _hSpeed = Mathf.Max(_hSpeed + acc * Time.fixedDeltaTime, _brakeMinSpeed);
        }

        // Going off ramps
        if (_wasRamping)
        {
            if (_stillRamping)
            {
                _pitch = -13.6f;
                _pitchSpeed = 0;
            }
            else
            {
                _fallSpeed = 0.5f * HSpeed;
                if (HSpeed > 0.8f * _sprintMaxSpeed)
                {
                    if (Mathf.Abs(VSpeed) < 1e-3)
                    {
                        _pitchSpeed = -_flipSpeed;
                    }
                    else
                    {
                        _rollSpeed = Mathf.Sign(VSpeed) * _flipSpeed * 1.5f;
                        _yawSpeed = -Mathf.Sign(VSpeed) * _flipSpeed * 0.75f;
                    }
                }
            }
        }

        // Don't show acceleration when not providing any input to avoid shaking near default speed
        if (cruising)
        {
            VisualAcceleration = 0;
        }

        // Apply velocity
        transform.position = new Vector3(transform.position.x + _hSpeed * Time.fixedDeltaTime * directionMultiplier, transform.position.y + _fallSpeed * Time.fixedDeltaTime, transform.position.z + _vSpeed * Time.fixedDeltaTime * directionMultiplier);
        _pitch = ShiftMod(_pitch + _pitchSpeed * Time.fixedDeltaTime, 360);
        _yaw = ShiftMod(_yaw + _yawSpeed * Time.fixedDeltaTime, 360);
        _roll = ShiftMod(_roll + _rollSpeed * Time.fixedDeltaTime, 360);
        if (_modelObject)
        {
            if (Mathf.Abs(_yawSpeed) < 1e-3)
            {
                _yaw = Mathf.Lerp(_yaw, Mathf.Rad2Deg * (float)Mathf.Atan2(-_vSpeed, _hSpeed), _yawLerpAmount);
            }
            _modelObject.transform.eulerAngles = new Vector3(_pitch, 90 * directionMultiplier + _yaw, _roll);
        }
        _rampY = 0;
        _wasRamping = _stillRamping;
        _stillRamping = false;
    }

    private float ShiftMod(float a, float b)
    {
        return ((a + b / 2) % b + b) % b - b / 2;
    }

    private void Emit()
    {
        var ps = GameObject.FindGameObjectWithTag(TagName.ParticleSystem).GetComponent<ParticleSystem>();
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = transform.position;
        emitParams.applyShapeToPosition = true;
        ps.Emit(emitParams, 20);
    }

    public void ReverseDir(){
        directionMultiplier *= -1;
    }
    public int GetDirection(){
        return directionMultiplier;
    }

    private IEnumerator<WaitForSeconds> Downshift()
    {
        _gearShiftState = GearShiftState.DOWNSHIFTING;
        yield return new WaitForSeconds(_downshiftDelay);
        _gearShiftState = GearShiftState.DOWNSHIFTED;
    }

    public void AddCollisionForce(Vector3 force)
    {
        _hSpeed += force.x;
        _vUncontrolledSpeed += force.z;
        if (_breakable && force.sqrMagnitude > _breakMagnitude)
        {
            Emit();
            Destroy(gameObject);
        }
    }


    void OnTriggerEnterOrStay(Collider other)
    {
        var otherCar = other.gameObject.GetComponent<Car>();

        if (otherCar)
        {
            Physics.ComputePenetration(_collider, _collider.transform.position, _collider.transform.rotation, other, other.transform.position, other.transform.rotation, out Vector3 direction, out float distance);
            transform.position += new Vector3(direction.x * distance, 0, direction.z * distance);
            var impactForce = new Vector3(otherCar.HSpeed - HSpeed, 0, _vImpactMultiplier * (otherCar.VSpeed - VSpeed));
            if (Vector3.Dot(impactForce, direction) > 0)
            {
                var zDiff = transform.position.z - other.transform.position.z;
                AddCollisionForce(DetermineImpactForce(impactForce, zDiff));
                otherCar.AddCollisionForce(otherCar.DetermineImpactForce(-impactForce, -zDiff));
            }
        }
        else
        {
            var collidableObject = other.gameObject.GetComponent<CollidableObstacle>();
            if (collidableObject && !collidableObject.dead)
            {
                var zDiff = transform.position.z - other.transform.position.z;
                var forceDirection = _hSpeed > 0 ? Vector3.left : Vector3.right;
                var impactForce = forceDirection * collidableObject.Inertia;
                AddCollisionForce(DetermineImpactForce(impactForce, 0));
            }
            else
            {
                var ramp = other.gameObject.GetComponent<Ramp>();
                if (ramp)
                {
                    float prog = 0.5f + (transform.position.x - other.transform.position.x) / 5f;
                    _rampY = Mathf.Clamp(prog, 0f, 1f) * 2.5f;
                    _stillRamping = true;
                }
            }
        }
    }

    public Vector3 DetermineImpactForce(Vector3 baseImpactForce, float xDiff)
    {
        if (Mathf.Abs(baseImpactForce.x) > _knockAsideThreshold && Mathf.Abs(baseImpactForce.z) < _knockAsideForce)
        {
            var sign = Mathf.Sign(xDiff);
            if (sign == 0)
            {
                sign = Random.Range(0, 2) * 2 - 1;
            }
            return new Vector3(baseImpactForce.x, baseImpactForce.y, sign * _knockAsideForce);
        }
        return baseImpactForce;
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
