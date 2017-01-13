using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BMCCPathFinding : RoomLocationsBMCC
{

    public Transform target;  
    public Transform player;
    public NavMeshAgent agent;
    private string nextDirection;
    private string crntDirection;
    private string fourDir;

    private Vector3 diff;
    private NavMeshPath path;
      
    private float timer;
    //public GameObject[] elevStair = new GameObject[17];
    private string curMsg;
    private string difMsg;
    //private string dirNeeded;
    NextDirection nextDir;

    void OnApplicationQuit()
    {
        EasyTTSUtil.Stop();
    }

    // Use this for initialization
    void Start()
    {
        EasyTTSUtil.Initialize(EasyTTSUtil.UnitedStates);

        nextDir = new NextDirection();                      // "Information Cloud" (talks to other scripts)

        agent.enabled = false;          // Detach from navMesh
        //player.transform.position = setStartingPoint();     // Move player to selected STARTPOINT
        nextDir.setStart(setStartingPoint());               // Store original startpoint.
        //target.transform.position = setEndingPoint();       // Move target to selected ENDPOINT
        agent.enabled = true;           // Re-attach to navMesh

        path = agent.path;

        calculatePath();                                 

        // variables
        curMsg = "constantly redefined";
        difMsg = "redefined only is curmsg changes";
        //dirNeeded = " ";
        nextDirection = "";
        crntDirection = "";
        fourDir = " ";
    }

    // Update is called once per frame
    void Update()
    {

        calculatePath();
        calculateDirection();
		//agent.CompleteOffMeshLink ();
			
    }

    public void calculatePath()
    {
        //Recalculates path
        NavMesh.CalculatePath(agent.transform.position, target.position, NavMesh.AllAreas, path);
		agent.SetDestination(target.position);
        // Draws out the line along the path
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.cyan);

        //curMsg = isOnSameFloor();
        //if (curMsg != difMsg)
        //{
        //    Debug.Log(curMsg);
        //    difMsg = curMsg;
       // }       
    }
      
    void calculateDirection()
    {
        // diff = Vector3 FROM currentPoint TO origin(nextPoint)

        diff = path.corners[1] - path.corners[0];
        findMajorDirection(diff);
    }

    private void findMajorDirection(Vector3 line)
    {
        float h = line.x;
        float v = line.z;

        if (Mathf.Abs(h) < Mathf.Abs(v))
        {
            if (v >= 0)
            {
                //Debug.Log ("Go Up");
                nextDirection = "Up";
            }
            else
            {
                //Debug.Log ("Go Down");
                nextDirection = "Down";
            }
        }
        else
        {
            if (h >= 0)
            {
                //Debug.Log ("Go Right");
                nextDirection = "Right";
            }
            else
            {
                //Debug.Log ("Go Left");
                nextDirection = "Left";
            }
        }

        if (crntDirection != nextDirection)
        {
            nextDir.setNextDir(nextDirection);
            Debug.Log("Go " + nextDirection);

            EasyTTSUtil.SpeechAdd("Go " + nextDirection);
            crntDirection = nextDirection;
        }
    }

  

    /**
        Used to give user directional feedback
        @param dgre - Number between 0-360 degree(s).
        @return index.
    */
    private string findNextDirection(float dgre)
    {
        if (dgre >= 45 && dgre < 135)
        {
            fourDir = "Down";
            return fourDir;
        }// Go Down
        else if (dgre >= 135 && dgre < 225)
        {
            fourDir = "Right";
            return fourDir;
        }// go Right
        else if (dgre >= 255 && dgre < 345)
        {
            fourDir = "Up";
            return fourDir;
        }// Go Upward
        else if (dgre < 45 || dgre >= 345)
        {
            fourDir = "Left";
            return fourDir;
        }// Go Left
        else
            return fourDir;
    }

    /*
        Find the nearest ascender/descender to player avatar.'
        STEPS:
          1) Determine player current floor
          2) Get all elevStairs on same floor
          3) Get all elevStairs that head to target (up or down)
          4) Determine if its the nearest to player
          5) Store index
          6) return transform

    
    private Transform findNearestElevator(string dir)
    {
        float shortestDistance = 1000;
        int index = -1;

        if (player.transform.position.y <= 5)               // if player is on the 1st FL
        {
            for (int i = 0; i < elevStair.Length; i++)
            {
                if (elevStair[i].transform.position.y <= 5)     // if ascender/descender is on 1st FL too
                {
                    if (elevStair[i].CompareTag(dir))              // if object ascends or descends
                    {   // if its the shortest distance from player
                        if (Mathf.Sqrt(Mathf.Pow(player.transform.position.x - elevStair[i].transform.position.x, 2) +
                                        Mathf.Pow(player.transform.position.y - elevStair[i].transform.position.y, 2) +
                                        Mathf.Pow(player.transform.position.z - elevStair[i].transform.position.z, 2)) < shortestDistance)
                        {

                            shortestDistance = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - elevStair[i].transform.position.x, 2) +
                                                           Mathf.Pow(player.transform.position.y - elevStair[i].transform.position.y, 2) +
                                                           Mathf.Pow(player.transform.position.z - elevStair[i].transform.position.z, 2));
                            index = i;          // save its index
                        }
                    }
                }
            }
        }
        else if (player.transform.position.y <= 205)    // player is on the second floor
        {
            for (int i = 0; i < elevStair.Length; i++)
            {
                if (elevStair[i].transform.position.y <= 205)
                {
                    if (elevStair[i].CompareTag(dir))
                    {
                        if (Mathf.Sqrt(Mathf.Pow(player.transform.position.x - elevStair[i].transform.position.x, 2) +
                                        Mathf.Pow(player.transform.position.y - elevStair[i].transform.position.y, 2) +
                                        Mathf.Pow(player.transform.position.z - elevStair[i].transform.position.z, 2)) < shortestDistance)
                        {

                            shortestDistance = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - elevStair[i].transform.position.x, 2) +
                                                           Mathf.Pow(player.transform.position.y - elevStair[i].transform.position.y, 2) +
                                                           Mathf.Pow(player.transform.position.z - elevStair[i].transform.position.z, 2));
                            index = i;
                        }
                    }
                }
            }

        }
        else                                            // player is on the third floor
        {
            for (int i = 0; i < elevStair.Length; i++)
            {
                if (elevStair[i].transform.position.y <= 405)
                {
                    if (elevStair[i].CompareTag(dir))
                    {
                        if (Mathf.Sqrt(Mathf.Pow(player.transform.position.x - elevStair[i].transform.position.x, 2) +
                                        Mathf.Pow(player.transform.position.y - elevStair[i].transform.position.y, 2) +
                                        Mathf.Pow(player.transform.position.z - elevStair[i].transform.position.z, 2)) < shortestDistance)
                        {

                            shortestDistance = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - elevStair[i].transform.position.x, 2) +
                                                           Mathf.Pow(player.transform.position.y - elevStair[i].transform.position.y, 2) +
                                                           Mathf.Pow(player.transform.position.z - elevStair[i].transform.position.z, 2));
                            index = i;
                        }
                    }
                }
            }
        }
        //Debug.Log(index + ": " + elevStair[index].transform.position);
        return elevStair[index].transform;
    }
*/
    /*
        Figures out what floor is player and target is on,
        initializes variable(dirNeeded) which holds what direction
        does player need to go (to travel up or down)
    
    private string isOnSameFloor()
    {
        if (player.transform.position.y > 205 && target.transform.position.y < 400)     // player on 3 && target not on 3
            if (target.position.y < 5)
            {
                dirNeeded = "GoingDown";
                return "player is on 3rd floor, target on 1st floor";
            }
            else {
                dirNeeded = "GoingDown";
                return "player is on 3rd floor, target on 2nd floor";
            }
        else if (player.transform.position.y <= 5 && target.transform.position.y > 5)   // player on 1 && target not on 1
        {
            if (target.position.y < 205)
            {   //must be second
                dirNeeded = "GoingUp";
                return "player is on 1st floor, target on 2nd floor";
            }
            else {                                // then on third
                dirNeeded = "GoingUp";
                return "player is on 1st floor, target on 3rd floor";
            }
        }
        else if (player.transform.position.y < 205 && player.transform.position.y > 5    // player on 2 && target not on 2                                                                 // player is on 3rd && target not on 3rd
                 && (target.transform.position.y < 5 || target.transform.position.y > 205))
        {
            if (target.position.y < 200)
            {
                dirNeeded = "GoingDown";
                return "player is on 2nd floor, target on 1st floor";
            }
            else {
                dirNeeded = "GoingUp";
                return "player is on 2nd floor, target on 3rd floor";
            }
        }
        else
            return "same floor";
    }
    */
}
