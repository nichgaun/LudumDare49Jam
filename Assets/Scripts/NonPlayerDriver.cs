using UnityEngine;

public class NonPlayerDriver : Driver
{
    private int _vMoving;
    [SerializeField] private DangerCollider _leftCollider;
    [SerializeField] private DangerCollider _centerCollider;
    [SerializeField] private DangerCollider _rightCollider;
    [SerializeField] private bool _tailgating;
    private Car _car;
    private Car _player;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(TagName.Player).GetComponent<Car>();
    }

    public override void Claim(Car car)
    {
        _car = car;
        _leftCollider.Claim(car);
        _centerCollider.Claim(car);
        _rightCollider.Claim(car);
    }

    public override void ControlCar(out int hMove, out int vMove, out bool sprint)
    {
        if (_tailgating)
        {
            var diff = _player.transform.position + Vector3.left * 3f - _car.transform.position;
            _car.DefaultSpeed = _player.DefaultSpeed;
            hMove = (int)Mathf.Sign(diff.x - 0.25f * _car.HSpeed);
            vMove = (int)Mathf.Sign(diff.z);
            sprint = false;
        }
        else
        {
            hMove = 0;
            sprint = false;
            if (_centerCollider.StillDanger)
            {
                if (_leftCollider.StillDanger && !_rightCollider.StillDanger)
                {
                    _vMoving = -1;
                }
                else if (!_leftCollider.StillDanger && _rightCollider.StillDanger)
                {
                    _vMoving = 1;
                }
                else if (_leftCollider.StillDanger && _rightCollider.StillDanger)
                {
                    hMove = -1;
                }
                if (_vMoving == 0)
                {
                    if (_leftCollider.OutOfBounds && !_rightCollider.OutOfBounds)
                    {
                        _vMoving = -1;
                    }
                    else if (!_leftCollider.OutOfBounds && _rightCollider.OutOfBounds)
                    {
                        _vMoving = 1;
                    }
                    else
                    {
                        _vMoving = Random.Range(0, 2) * 2 - 1;
                    }
                }
                if (_car.DirectionMultiplier * transform.position.z > 0)
                {
                    _vMoving = -(int)_car.DirectionMultiplier;
                }
            }
            else
            {
                _vMoving = 0;
            }
            vMove = _vMoving;
        }
        _leftCollider.ControlCar();
        _centerCollider.ControlCar();
        _rightCollider.ControlCar();
    }
}
