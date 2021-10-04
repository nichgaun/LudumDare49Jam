using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckEngineLightsManager : MonoBehaviour
{
    Health playerHealth; //set in start

    Image light1; //set in start
    Image light2; //set in start
    Image light3; //set in start
    Image roadslip; //set in start
    Image sicko; //set in start
    Image warning; //set in start

    Color yellowOn = new Color(1f, 200f / 255f, 0f);
    Color redOn = new Color(215f / 255f, 0f, 0f);

    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag(TagName.Player).GetComponent<Health>();

        foreach (Transform trans in transform)
        {
            switch(trans.name)
            {
                case "CheckEngine1":
                    light1 = trans.gameObject.GetComponent<Image>();
                    break;
                case "CheckEngine2":
                    light2 = trans.gameObject.GetComponent<Image>();
                    break;
                case "CheckEngine3":
                    light3 = trans.gameObject.GetComponent<Image>();
                    break;
                case "RoadSlip":
                    roadslip = trans.gameObject.GetComponent<Image>();
                    break;
                case "Sicko":
                    sicko = trans.gameObject.GetComponent<Image>();
                    break;
                case "Warning":
                    warning = trans.gameObject.GetComponent<Image>();
                    break;
                default:
                    break;
            }
        }

        light1.color = Color.clear;
        light2.color = Color.clear;
        light3.color = Color.clear;
        sicko.color = Color.clear;
        warning.color = Color.clear;
        roadslip.color = Color.clear;
    }

    void Update()
    {
        float healthPercentage = (float) playerHealth.CurrentHealth / playerHealth.MaxHealth;
        
        if (healthPercentage > 0.8f)
        {
            light1.color = Color.clear;
            light2.color = Color.clear;
            light3.color = Color.clear;
            sicko.color = Color.clear;
            warning.color = Color.clear;
        }
        else if (healthPercentage > 0.7f)
        {
            light1.color = yellowOn;
            light2.color = Color.clear;
            light3.color = Color.clear;
            sicko.color = Color.clear;
            warning.color = Color.clear;
        }
        else if (healthPercentage > 0.6f)
        {
            light1.color = Color.clear;
            light2.color = yellowOn;
            light3.color = Color.clear;
            sicko.color = Color.clear;
            warning.color = Color.clear;
        }
        else if (healthPercentage > 0.5f)
        {
            light1.color = Color.clear;
            light2.color = yellowOn;
            light3.color = Color.clear;
            sicko.color = Color.clear;
            warning.color = yellowOn;
        }
        else if (healthPercentage > 0.4f)
        {
            light1.color = Color.clear;
            light2.color = redOn;
            light3.color = Color.clear;
            sicko.color = Color.clear;
            warning.color = yellowOn;
        }
        else if (healthPercentage > 0.3f)
        {
            light1.color = Color.clear;
            light2.color = redOn;
            light3.color = Color.clear;
            sicko.color = Color.clear;
            warning.color = redOn;
        }
        else if (healthPercentage > 0.2f)
        {
            light1.color = Color.clear;
            light2.color = redOn;
            light3.color = Color.clear;
            sicko.color = yellowOn;
            warning.color = redOn;
        }
        else if (healthPercentage > 0.1f)
        {
            light1.color = Color.clear;
            light2.color = Color.clear;
            light3.color = redOn;
            sicko.color = yellowOn;
            warning.color = redOn;
        }
        else
        {
            light1.color = Color.clear;
            light2.color = Color.clear;
            light3.color = redOn;
            sicko.color = redOn;
            warning.color = redOn;
        }
    }
}
