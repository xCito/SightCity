using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class DynamicRooms : FloorSelect {


    void Start()
    {

    }

    public string getStartPoint()
    {
        return Register.starting;

    }

    public string getEndPoint()
    {
        return Register.destination;

    }
    public int floorPick() {
        return floor;
    }

    public void TryHand(string choice)
    {
        Debug.Log(choice + "<---Choice vAR");
        if (Input.touchCount > 0||Input.GetKeyDown ("space"))
        {
            if (Input.GetTouch (0).tapCount == 2 || Input.GetKeyDown ("space"))
            {
                if (Register.starting == "")
                {
                    Register.starting = choice;
                    Debug.Log("Set starting point to :" + getStartPoint());
                    EasyTTSUtil.SpeechFlush("You have chosen " + getStartPoint() + " as your starting point" +
                                            ". Now going back to the floor selection menu to select " +
                                            "destination point");
                    SceneManager.LoadScene("FloorPick");

                }
                else
                {
                    Register.destination = choice;

                    if (getStartPoint() == getEndPoint())
                    {
                        warningMessage();
                        SceneManager.LoadScene("FloorPick");
                    }
                    else {
                        Debug.Log("Set ending point to :" + getEndPoint());
                        EasyTTSUtil.SpeechFlush("You have chosen " + getEndPoint() + " as your destination point" +
                                                ". The game will be starting shortly");
                        SceneManager.LoadScene("ConferenceSecondFloor");
                    }
                    }
            }
            print (getStartPoint());
        }
    }

    void warningMessage()
    {
        EasyTTSUtil.SpeechFlush("WARNING. You have selected the same location for"+
            " both start and end point. You have been redirected to the floor selection menu");
    }
}
