using UnityEngine;

public class NonPlayerDriver : Driver
{
    private int _vMoving;
    [SerializeField] private DangerCollider _leftCollider;
    [SerializeField] private DangerCollider _centerCollider;
    [SerializeField] private DangerCollider _rightCollider;
    private Car _car;

    public override void Claim(Car car)
    {
        _car = car;
        _leftCollider.Claim(car);
        _centerCollider.Claim(car);
        _rightCollider.Claim(car);
    }

    public override void ControlCar(out int hMove, out int vMove, out bool sprint)
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
                _vMoving = Random.Range(0, 2) * 2 - 1;
            }
        }
        else
        {
            _vMoving = 0;
        }
        vMove = _vMoving;
        _leftCollider.ControlCar();
        _centerCollider.ControlCar();
        _rightCollider.ControlCar();
    }
}
