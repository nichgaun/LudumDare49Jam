using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roadblock : MonoBehaviour
{

    [SerializeField] GameObject barricadeLeft;
    [SerializeField] GameObject barricadeRight;
    [SerializeField] GameObject cop;


    void Start() {
        barricadeLeft.GetComponent<BarricadeComponent>().setSpinDirection(-1);
        barricadeRight.GetComponent<BarricadeComponent>().setSpinDirection(1);
    }


    void Update()
    {

    }
}
