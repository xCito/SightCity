using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    NextDirection nextDir;
    public Text collisions;
    public Text timer;
    public Text highScore;
    private Vector2 fingerStartPos;
    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 0.5f;
    public float fingerStartTime { get; private set; }

    // Use this for initialization
    void Start()
    {

        EasyTTSUtil.Initialize(EasyTTSUtil.UnitedStates);
        nextDir = new NextDirection();

        readGameResults();
        readNextChoice();
    }

    void Update()
    {
        collectSwipeInput();
    }

    void readGameResults()
    {
        timer.text = "Time Spent: " + nextDir.getEndTime();
        EasyTTSUtil.SpeechAdd(timer.text);
        collisions.text = "Number of Collisions: " + nextDir.getNumOfCollisions();
        EasyTTSUtil.SpeechAdd(collisions.text);
        highScore.text = "Your Score is " + (1000 - nextDir.getNumOfCollisions() * 10);
        EasyTTSUtil.SpeechAdd(highScore.text);
    }

    /*-----------------------------------------------------
        Changes to certain Scenes depending on swipe input
        Swipe Left to go to MainMenu Scene
        Swipe Right to close application
        Swipe Down to Restart same path/course
     -----------------------------------------------------*/
    void collectSwipeInput()
    {
        if (Input.touchCount > 0)
        {

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

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                // the swipe is horizontal:
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            else {
                                // the swipe is vertical:
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if (swipeType.x != 0.0f)
                            {
                                if (swipeType.x > 0.0f || Input.GetKey("right"))
                                {
                                    // MOVE RIGHT
                                    Debug.Log("MoveRight");
                                    EasyTTSUtil.SpeechFlush("You swiped Right");
                                    Application.Quit();
                                }
                                else if (swipeType.x < 0.0f || Input.GetKey("left"))
                                {
                                    // MOVE LEFT
                                    Debug.Log("MoveLeft");
                                    EasyTTSUtil.SpeechFlush("You swiped Left");
                                    SceneManager.LoadScene("Main Menu");
                                }
                            }

                            if (swipeType.y != 0.0f)
                            {
                                if (swipeType.y > 0.0f || Input.GetKey("up"))
                                {
                                    // MOVE UP
                                    EasyTTSUtil.SpeechFlush("Up Swipe is not a choice");
                                }
                                else if (swipeType.y < 0.0f || Input.GetKey("down"))
                                {
                                    // MOVE DOWN
                                    SceneManager.LoadScene("ConferenceSecondFloor");
                                }
                            }

                        }

                        break;
                }
            }
        }
    }

    // Read back to user current menu
    // options on screen
    void readNextChoice()
    {
        EasyTTSUtil.SpeechAdd("Swipe Left to choose new destination");
        EasyTTSUtil.SpeechAdd("Swipe Right to quit and close application");
        EasyTTSUtil.SpeechAdd("Swipe Down to restart same path");
    }

    void OnApplicationQuit()
    {
        EasyTTSUtil.Stop();
    }
}
