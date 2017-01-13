using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using SimpleJSON;

public class CreateFromJSON : MonoBehaviour {

    public TextAsset file;
    public GameObject wall;
    public float X_Drift = 0;
    public float Y_Drift = 0;
    public float scale = 1;

    GameObject floor;
    const float HEIGHT = 4;

    private JSONNode myJSON;

    // Use this for initialization
    void Start () {
        //wall = GameObject.FindGameObjectWithTag("Base_Wall");
        floor = new GameObject ("Floor");
        if (file != null)
        {
            Debug.Log ("JSON file exists.");
            myJSON = JSONNode.Parse (file.text);
        }
        else
            Debug.Log ("JSON file is not existing.");

        Debug.Log (myJSON.Count);
        Debug.Log (myJSON.AsObject.GetKeys().Count);

        ArrayList roomNames = myJSON.AsObject.GetKeys();
        for (int i = 0; i < myJSON.Count; i++)
        {
            //string name = (string)roomNames[i];
            Debug.Log ("Creating " + i + "th Room.");
            createRoomFromJSON (myJSON[i],
                                (string)roomNames[i]);
        }
#if UNITY_EDITOR
        NavMeshBuilder.BuildNavMesh();
#endif
    }

    // Update is called once per frame
    void Update () {

    }

    void createRoomFromJSON (JSONNode corners,
                             string name)
    {
        // Make sure there are more than one corners
        int numberOfCorners = corners.Count;
        if (numberOfCorners > 2)
        {
            GameObject room = new GameObject (name);
            room.tag = "Room";
            room.transform.parent = floor.transform;

            for (int i = 0; i < numberOfCorners - 1; i++)
            {
                createWallFromJSON (corners[i],
                                    corners[i + 1],
                                    room,
                                    i);
            }
        }

    }

    GameObject createWallFromJSON (JSONNode p1,
                                   JSONNode p2,
                                   GameObject parent,
                                   int index)
    {
        // Cast data to float array
        float[] corner1 = { p1[0].AsFloat * scale + X_Drift,
                            -p1[1].AsFloat * scale + Y_Drift
                          };
        float[] corner2 = { p2[0].AsFloat * scale + X_Drift,
                            -p2[1].AsFloat * scale + Y_Drift
                          };

        float length = Mathf.Sqrt (Mathf.Pow (corner1[0] - corner2[0],
                                              2f) + Mathf.Pow (corner1[1] - corner2[1],
                                                      2f));
        Vector3 position = new Vector3 ((corner1[0] + corner2[0]) / 2,
                                        HEIGHT,
                                        (corner1[1] + corner2[1]) / 2);
        float rotation = corner1[0] == corner2[0] ? 90 : -Mathf.Atan ((corner1[1] - corner2[1]) / (corner1[0] - corner2[0])) * 180f / Mathf.PI;
        GameObject newWall = Instantiate (wall);
        newWall.name = "Wall_" + index;
        newWall.transform.parent = parent.transform;
        newWall.transform.position = position;
        newWall.transform.Rotate (0,
                                  rotation,
                                  0);
        newWall.transform.localScale = new Vector3 (length,
                2 * HEIGHT,
                0.2f);
        newWall.tag = "Wall";

#if UNITY_EDITOR
        int layer = GameObjectUtility.GetNavMeshAreaFromName ("Not Walkable");
        StaticEditorFlags staticFlags = GameObjectUtility.GetStaticEditorFlags (newWall);
        staticFlags = StaticEditorFlags.NavigationStatic;
        GameObjectUtility.SetStaticEditorFlags (newWall,
                                                staticFlags);
        GameObjectUtility.SetNavMeshArea (newWall,
                                          layer);
#endif

        return newWall;
    }

}
