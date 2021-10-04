using UnityEngine;

public class CleanUp : MonoBehaviour
{
    float _avgVel;
    float _lastX;
    float _alive;
    Car _player;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(TagName.Player).GetComponent<Car>();
        _lastX = transform.position.x;
    }

    void FixedUpdate()
    {
        if (Time.fixedDeltaTime > 1e-12)
        {
            _avgVel = Mathf.Lerp((transform.position.x - _lastX) / Time.fixedDeltaTime - _player.HSpeed, _avgVel, Mathf.Pow(1f - 0.25f, Time.fixedDeltaTime));
            _lastX = transform.position.x;
            _alive += Time.fixedDeltaTime;
            if (_alive > 10f && Mathf.Sign(_avgVel) * (transform.position.x - _player.transform.position.x) > 20f)
            {
                Destroy(gameObject);
            }
        }
    }
}
