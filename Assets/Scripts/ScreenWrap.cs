using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    [SerializeField] Vector3 defaultPos;
    [SerializeField] Vector3 secondPos;
    [SerializeField] int zOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerExit(Collider other){
        if (other.tag == "Player"){
            StartCoroutine(Wrap(other));
        }
    }
    IEnumerator Wrap(Collider other){
        yield return new WaitForSeconds(2);
        Car car = other.GetComponent<Car>();
        if(car.GetDirection() == 1){
            other.transform.position = secondPos;
            car.ReverseDir();
            car.transform.RotateAround(car.transform.position, transform.up, 180f);
        }
        else{
            other.transform.position = defaultPos;
            car.ReverseDir();
            car.transform.RotateAround(car.transform.position, transform.up, 180f);
        }
    }
}
