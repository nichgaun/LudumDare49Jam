using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    const string TRACK1 = "number1";
    const string TRACK2 = "number2";
    const string TRACK3 = "number3";
    const string TRACK4 = "number4";
    const string BASE_RADIO_TEXT = "Now Playing: ";

    [SerializeField] int numSongs; //set in editor

    int currentSong;

    [SerializeField] float timeShiftScalingRate; //set in editor
    float goalTimeScale;

    Rage playerRage; //set in start
    Car playerCar; //set in start
    Health playerHealth; //set in start

    Text radioText; //set in start

    // Start is called before the first frame update
    void Start()
    {
        currentSong = 1;
        goalTimeScale = 1f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRage = player.GetComponent<Rage>();
        playerHealth = player.GetComponent<Health>();
        playerCar = player.GetComponent<Car>();

        radioText = GameObject.FindGameObjectWithTag("RadioText").GetComponent<Text>();

        UpdateSongEffect();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            currentSong++;
            if (currentSong > numSongs) currentSong = numSongs;
            UpdateSongEffect();
        }
        if (Input.GetKeyDown(KeyCode.Z))
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
                //all music off
                SoundPlayer.Stop(TRACK1);
                SoundPlayer.Stop(TRACK2);
                SoundPlayer.Stop(TRACK3);

                //reset radio rage
                playerRage?.SetMultiplier("radioRage", 1f);

                //reset damage multiplier
                playerHealth?.SetMultiplier("radioDamage", 1f);

                //reset time scale
                goalTimeScale = 1f;

                playerCar.CanSprint = false;

                radioText.text = BASE_RADIO_TEXT + "---";
                break;
            case 1:
                //select song 1
                SoundPlayer.Stop(TRACK2);
                SoundPlayer.Stop(TRACK3);
                SoundPlayer.Play(TRACK1, true, false);

                //reset radio rage
                playerRage?.SetMultiplier("radioRage", 1f);

                //reset damage multiplier
                playerHealth?.SetMultiplier("radioDamage", 1f);

                //reset time scale
                goalTimeScale = 1f;

                playerCar.CanSprint = true;

                radioText.text = BASE_RADIO_TEXT + "Press Shift To Sprint";
                break;
            case 2:
                //select song 2
                SoundPlayer.Stop(TRACK1);
                SoundPlayer.Stop(TRACK3);
                SoundPlayer.Play(TRACK2, true, false);

                //set radio rage
                playerRage?.SetMultiplier("radioRage", 3f);

                //set damage multiplier
                playerHealth?.SetMultiplier("radioDamage", 0.5f);

                //reset time scale
                goalTimeScale = 1f;

                playerCar.CanSprint = false;

                radioText.text = BASE_RADIO_TEXT + "Resistant To Damage";
                break;
            case 3:
                //select song 3
                SoundPlayer.Stop(TRACK1);
                SoundPlayer.Stop(TRACK2);
                SoundPlayer.Play(TRACK3, true, false);

                //reset radio rage
                playerRage?.SetMultiplier("radioRage", 1f);

                //reset damage multiplier
                playerHealth?.SetMultiplier("radioDamage", 1f);

                //set time scale to  s l o w
                goalTimeScale = 0.2f;

                playerCar.CanSprint = false;

                radioText.text = BASE_RADIO_TEXT + "Time Slows Down";
                break;
            default:
                break;
        }
    }

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
