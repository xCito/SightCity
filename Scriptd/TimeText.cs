using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeText : MonoBehaviour {

    public Text timerText;
    private float time;
    private int secs;
    private int mins;
    private bool gameEnd;

    // Use this for initialization
    void Start ()
    {
        mins = 0;
        secs = 0;
        time = 0;
        gameEnd = false;
    }

    // Update is called once per frame
    void Update () {
        time = Time.time;

        secs = (int)time % 60;
        mins = (int)time / 60;

        if (gameEnd == false)
            timerText.text = "Timer  " + string.Format ("{0}", mins) + ":" + string.Format ("{0:00}", secs);
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.tag == "target")
            gameEnd = true;
    }

    string getEndTime()
    {
        return (gameEnd)? "" + mins + secs : "Game still in Progress";
    }
}
