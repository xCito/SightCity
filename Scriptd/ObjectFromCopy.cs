using UnityEngine;
using System.Collections;

public class ObjectFromCopy : MonoBehaviour {

    const float HEIGHT = 4;
    Vector2 a1 = new Vector2 (-32, -32);
    Vector2 a2 = new Vector2 ( -35, -32 );
    Vector2 a3 = new Vector2 ( -35, -24 );
    Vector2 a4 = new Vector2 ( -27, -24 );
    Vector2 a5 = new Vector2 ( -27, -32 );
    Vector2 a6 = new Vector2 ( -30, -32 );

    ArrayList myRoom;

    GameObject wall;
    GameObject[] walls;
    GameObject room;

    void Awake()
    {
        wall = GameObject.FindGameObjectWithTag ("Base_Wall");
        walls = new GameObject[5];
        room = new GameObject ("Room");
        myRoom = new ArrayList();
        myRoom.Add (a1);
        myRoom.Add (a2);
        myRoom.Add (a3);
        myRoom.Add (a4);
        myRoom.Add (a5);
        myRoom.Add (a6);
    }

    // Use this for initialization
    void Start () {
        //room.transform.position = new Vector3(16, HEIGHT, 16);
        //createWall(a1, a2, room, 5);
        createRoom (myRoom, 2);
        for (int i = 0; i < 5; i++)
        {
            walls[i] = Instantiate (wall);
            walls[i].name = "Wall_" + i;
            walls[i].transform.parent = room.transform;
            //walls[i].transform.Translate();
            walls[i].transform.Rotate (0, 180, 0);
            walls[i].transform.localScale = new Vector3 (16f, 2*HEIGHT, 1);
            //walls[i].transform.position = new Vector3(16, HEIGHT, 16 + 2*i);
            walls[i].tag = "Wall";
        }
    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < 5; i++)
        {
            walls[i].transform.position = new Vector3 (16, HEIGHT, 16) + new Vector3 (0, 0, 2 * i);
            //walls[i].transform.Translate(new Vector3(16, HEIGHT, 16 + 2 * i));
            // walls[i].transform.Rotate(0, i * Time.deltaTime, 0);
        }
    }

    void createRoom (ArrayList corners, int index)
    {
        GameObject room02 = new GameObject ("Room" + index);
        int numberOfCorners = corners.Count;
        for (int i = 0; i < numberOfCorners - 1; i++)
        {
            createWall ((Vector2)corners[i], (Vector2)corners[i + 1], room02, i);
        }
    }

    GameObject createWall (Vector2 corner1, Vector2 corner2, GameObject parent, int index)
    {
        float length = Mathf.Sqrt (Mathf.Pow (corner1[0] - corner2[0], 2f) + Mathf.Pow (corner1[1] - corner2[1], 2f));
        Vector3 position = new Vector3 ((corner1[0] + corner2[0])/2, HEIGHT, (corner1[1] + corner2[1])/2);
        float rotation = corner1[0] == corner2[0]? 90 : -Mathf.Atan ((corner1[1] - corner2[1]) / (corner1[0] - corner2[0])) * 180f / Mathf.PI;
        GameObject newWall = Instantiate (wall);
        newWall.name = "Wall_" + index;
        newWall.transform.parent = parent.transform;
        newWall.transform.position = position;
        newWall.transform.Rotate (0, rotation, 0);
        newWall.transform.localScale = new Vector3 (length, 2*HEIGHT, 0.2f);
        newWall.tag = "Wall";
        return newWall;
    }
}