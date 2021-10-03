using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    [SerializeField] int numSongs; //set in editor

    int currentSong;

    // Start is called before the first frame update
    void Start()
    {
        currentSong = 0;
        goalTimeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            currentSong++;
            if (currentSong > numSongs) currentSong = numSongs;
            UpdateSongEffect();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentSong--;
            if (currentSong < 0) currentSong = 0;
            UpdateSongEffect();
        }

        ChangeTimeScale();
    }

    void UpdateSongEffect()
    {
        switch (currentSong)
        {
            case 0:
                SoundPlayer.Stop("number1");
                SoundPlayer.Stop("number2");
                SoundPlayer.Stop("number3");

                goalTimeScale = 1f;
                break;
            case 1:
                SoundPlayer.Stop("number2");
                SoundPlayer.Stop("number3");
                SoundPlayer.Play("number1", true, false);

                goalTimeScale = 1f;
                break;
            case 2:
                SoundPlayer.Stop("number1");
                SoundPlayer.Stop("number3");
                SoundPlayer.Play("number2", true, false);

                goalTimeScale = 1f;
                break;
            case 3:
                SoundPlayer.Stop("number1");
                SoundPlayer.Stop("number2");
                SoundPlayer.Play("number3", true, false);

                goalTimeScale = 0.2f;
                break;
            default:
                break;
        }
    }

    [SerializeField] float timeShiftScalingRate; //set in editor
    float goalTimeScale;

    //Alter the time scale to approach our goal
    //After we step towards it, check if we overshot it
    //If so, set it to be the goal exactly
    void ChangeTimeScale()
    {
        if (goalTimeScale < Time.timeScale)
        {
            Time.timeScale -= timeShiftScalingRate * Time.deltaTime;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            if (Time.timeScale < goalTimeScale)
            {
                Time.timeScale = goalTimeScale;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
        }
        else if (goalTimeScale > Time.timeScale)
        {
            Time.timeScale += timeShiftScalingRate * Time.deltaTime;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            if (Time.timeScale > goalTimeScale)
            {
                Time.timeScale = goalTimeScale;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
        }
    }
}
