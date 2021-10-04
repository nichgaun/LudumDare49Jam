using UnityEngine;

public class PlayerDriver : Driver
{
    private Car _car;
    private Rage _rage;
    [SerializeField] private float _rageBuildRate;
    [SerializeField] private float _timeToFillNeedForSpeed;
    private GameObject _speedUpText;
    private float _needForSpeed;

    public override void Claim(Car car)
    {
        _car = car;
        _rage = car.GetComponent<Rage>();
        _speedUpText = GameObject.FindGameObjectWithTag(TagName.SpeedUp);
    }

    public override void ControlCar(out int hMove, out int vMove, out bool sprint)
    {
        _needForSpeed = Mathf.Clamp(_needForSpeed - (2 * (_car.HSpeed - _car.DefaultSpeed) / (_car.WalkMaxSpeed - _car.DefaultSpeed) - 1) * Time.fixedDeltaTime / _timeToFillNeedForSpeed, 0f, 1f);
        if (_needForSpeed > 1 - 1e-3f)
        {
            _rage.UpdateRage(_rageBuildRate * Time.fixedDeltaTime);
        }
        if (_speedUpText)
        {
            _speedUpText.SetActive(_needForSpeed > 0.8f);
        }
        if (_needForSpeed > 1 - 1e-3f)
        {
            _rage.UpdateRage(_rageBuildRate * Time.fixedDeltaTime);
        }
        else
        {
            if (_speedUpText)
            {
                _speedUpText.SetActive(false);
            }
        }
        hMove = 0;
        vMove = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            hMove -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            hMove += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            vMove -= 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            vMove += 1;
        }
        sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }
}
