using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Disk : MonoBehaviour
{
    [SerializeField] float angularSpeed; // set in editor
    Text bananaCountTextField;

    // Start is called before the first frame update
    void Start()
    {
        bananaCountTextField = GameObject.FindGameObjectWithTag("UIBananaCount").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // spin
        transform.Rotate(angularSpeed * Time.deltaTime * new Vector3(0, angularSpeed, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StaticInformation.myInt += 1;
            bananaCountTextField.text = "Banana Count: " + StaticInformation.myInt;
            Destroy(gameObject);
        }
    }
}
