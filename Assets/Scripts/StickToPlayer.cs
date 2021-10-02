using System.Collections.Generic;
using UnityEngine;

public class StickToPlayer : MonoBehaviour
{
    private Car _player;
    private Vector3 _initialDiff;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(TagName.Player).GetComponent<Car>();
        _initialDiff = transform.position - _player.transform.position;
    }

    private void FixedUpdate()
    {
        StartCoroutine(LateFixedUpdate());
    }

    private IEnumerator<WaitForFixedUpdate> LateFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        transform.position = _player.transform.position + _initialDiff;
    }
}
