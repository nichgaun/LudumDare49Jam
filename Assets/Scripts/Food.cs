using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{

    private enum DiskState
    {
        IDLE,
        COLLECTING,
    }

    [SerializeField] float angularSpeed; // set in editor
    [SerializeField] float collectingMultiplier; // set in editor
    [SerializeField] float collectingVerticalSpeed; // set in editor
    DiskState _state = DiskState.IDLE;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // spin
        switch (_state) {
        case DiskState.IDLE:
            transform.Rotate(angularSpeed * Time.deltaTime * new Vector3(0, angularSpeed, 0));
            break;
        case DiskState.COLLECTING:
            transform.Rotate(angularSpeed * collectingMultiplier * Time.deltaTime * new Vector3(0, angularSpeed, 0));
            transform.position += Vector3.up * collectingVerticalSpeed * Time.deltaTime;
            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _state = DiskState.COLLECTING;
            SoundPlayer.Play("hitFood");
        }
    }
}
