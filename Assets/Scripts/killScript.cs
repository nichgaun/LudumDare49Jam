using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class killScript : MonoBehaviour
{
    [SerializeField] Text myText; // set in editor

    // Start is called before the first frame update
    void Start()
    {
        myText.text = "Banana Count: " + StaticInformation.myInt;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)){
            SceneManager.LoadScene("Steven");
        }
        if (Input.GetKeyDown(KeyCode.J)){
            StaticInformation.myInt += 1;
            myText.text = "Banana Count: " + StaticInformation.myInt;
        }
    }
}
