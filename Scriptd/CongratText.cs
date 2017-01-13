using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CongratText : MonoBehaviour {

    public Text congratText;
    private int overallscore;
    private int numCol;
    private int countColli;

    // Use this for initialization
    void Start()
    {
        countColli = 0;
        congratText.text = " ";
        overallscore = 1000;
    }

    // Update is called once per frame
    void calcScore()
    {
        overallscore = overallscore - (countColli * 10);
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.tag == "Target")
        {
            calcScore();
            congratText.text = "Congradulations Destination Reached!\n" +
                               "\t\t\tSCORE: " + string.Format ("{0}",
                                       overallscore);
        }
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.CompareTag ("Walls"))
        {
            countColli++;
        }
    }
}
