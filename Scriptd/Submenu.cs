using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Submenu : FloorSelect {

    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;
    private string selectFloor;
    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (submenufloor == 1)
        {
            selectFloor = "First Floor";
        }
        else if (submenufloor == 2)
        {
            selectFloor = "Second Floor";
        }
        else if (submenufloor == 3)
        {
            selectFloor = "Third Floor";
        }
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch (0).tapCount == 3)
            {
                SceneManager.LoadScene ("Main Menu");
            }
            else if (Input.GetTouch (0).tapCount == 5)
            {

                SceneManager.LoadScene ("Submenu");

            }
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                case TouchPhase.Began:
                    /* this is a new touch */
                    isSwipe = true;
                    fingerStartTime = Time.time;
                    fingerStartPos = touch.position;
                    break;

                case TouchPhase.Canceled:
                    /* The touch is being canceled */
                    isSwipe = false;
                    break;

                case TouchPhase.Ended:

                    float gestureTime = Time.time - fingerStartTime;
                    float gestureDist = (touch.position - fingerStartPos).magnitude;

                    if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
                    {
                        Vector2 direction = touch.position - fingerStartPos;
                        Vector2 swipeType = Vector2.zero;

                        if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y))
                        {
                            // the swipe is horizontal:
                            swipeType = Vector2.right * Mathf.Sign (direction.x);
                        }
                        else {
                            // the swipe is vertical:
                            swipeType = Vector2.up * Mathf.Sign (direction.y);
                        }

                        if (swipeType.x != 0.0f)
                        {
                            if (swipeType.x > 0.0f)
                            {
                                // MOVE RIGHT
                                Debug.Log ("MoveRight");
                                SceneManager.LoadScene ("Rooms");

                                EasyTTSUtil.SpeechFlush ("You selected the Room locations of the building on the " + selectFloor);
                            }
                            else {
                                // MOVE LEFT
                                Debug.Log ("MoveLeft");
                                SceneManager.LoadScene ("Comforts");
                                EasyTTSUtil.SpeechFlush ("You selected the Comfort locations of the building on the " + selectFloor);
                            }
                        }

                        if (swipeType.y != 0.0f)
                        {
                            if (swipeType.y > 0.0f)
                            {
                                // MOVE UP
                                Debug.Log ("MoveUp");
                                EasyTTSUtil.SpeechFlush ("You selected the services list of the building on the "+ selectFloor);
                                SceneManager.LoadScene ("Services");
                            }
                            else {
                                // MOVE DOWN
                                Debug.Log ("MoveDown");
                                SceneManager.LoadScene ("Exits");
                                EasyTTSUtil.SpeechFlush ("You selected the Exit locations of the building on the " + selectFloor);
                            }
                        }

                    }

                    break;
                }
            }
        }

    }
    void Start()
    {
        EasyTTSUtil.Initialize (EasyTTSUtil.UnitedStates);

        EasyTTSUtil.SpeechAdd("You're in the room category selection menu");
        EasyTTSUtil.SpeechAdd  ("Swipe Up for Service Rooms, Swipe Down for Exits," +
                                "Swipe Left for Comfort Rooms, or Swipe Right for " +
                                "other rooms");
    } 

    void OnApplicationQuit()
    {
        EasyTTSUtil.Stop();
    }
}
