using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Renderer renderObj = GetComponent<Renderer>();
        renderObj.material.color = Color.white;
    }

    // Update is called once per frame
    void OnMouseEnter()
    {
        Renderer renderObj = GetComponent<Renderer>();
        renderObj.material.color = Color.blue;
    }

    void OnMouseExit()
    {
        Renderer renderObj = GetComponent<Renderer>();
        renderObj.material.color = Color.white;
    }
}
