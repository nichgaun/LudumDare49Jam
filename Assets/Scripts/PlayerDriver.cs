using UnityEngine;

public class PlayerDriver : Driver
{
    private Car _car;
    private Rage _rage;
    [SerializeField] private float _rageBuildRate;
    [SerializeField] private float _timeToFillNeedForSpeed;
    [SerializeField] bool hasRage;
    private Blink _speedUpText;
    private float _needForSpeed;

    public override void Claim(Car car)
    {
        _car = car;
        _rage = car.GetComponent<Rage>();
        _speedUpText = GameObject.FindGameObjectWithTag(TagName.SpeedUp).GetComponent<Blink>();
    }

    public override void ControlCar(out int hMove, out int vMove, out bool sprint)
    {
        _needForSpeed = Mathf.Clamp(_needForSpeed - (2 * (_car.HSpeed - _car.DefaultSpeed) / (_car.WalkMaxSpeed - _car.DefaultSpeed) - 1) * Time.fixedDeltaTime / _timeToFillNeedForSpeed, 0f, 1f);
        if (_speedUpText)
        {
            if (_needForSpeed > 0.95f)
            {
                if (!_speedUpText.isActiveAndEnabled)
                {
                    _speedUpText.BlinkTime = 0;
                }
                _speedUpText.gameObject.SetActive(true);
            }
            else
            {
                _speedUpText.gameObject.SetActive(false);
            }
        }
        if (_needForSpeed > 1 - 1e-3f)
        {
            if (hasRage) _rage.UpdateRage(_rageBuildRate * Time.fixedDeltaTime);        
        }
        hMove = 0;
        vMove = 0;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            vMove += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            vMove -= 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            hMove -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            hMove += 1;
        }
        sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }
}
