using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamePlay : MonoBehaviour {

    RoomLocations roomL;
    RoomLocations roomL2;
    public InputField textField1;
    public InputField textField2;
    public Transform player;
    public Transform destination;
    private string cur = "Enter Room Name";
    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool oneTime = true;

    // Use this for initialization
    void Start () {
        roomL = new RoomLocations();
        roomL2 = new RoomLocations();
        textField1.text = cur;
        textField2.text = cur;
        startPoint = new Vector3();
        endPoint = new Vector3();

    }

    // Update is called once per frame
    void Update () {

        if (oneTime)
            setNewPoints();
    }

    void setNewPoints()
    {
        if (roomL.getLocationCoor (textField1.text) != new Vector3 (0, 0, 0))
        {
            startPoint = roomL.getLocationCoor (textField1.text);
        }

        if (roomL2.getLocationCoor (textField2.text) != new Vector3 (0, 0, 0))
        {
            endPoint = roomL2.getLocationCoor (textField2.text);
            Debug.Log ("Got Ending point");

            player.position = startPoint;
            Debug.Log ("Got Starting point: " + player.position + " ..." + startPoint);

            float y = destination.position.y;
            endPoint.y = y;
            destination.position = endPoint;
            oneTime = false;
        }
    }
}
