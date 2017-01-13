using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerPathCCNY : MonoBehaviour
{

	public Transform target;
	public Transform player; 
	public NavMeshAgent agent;
	public int distanceFeet;
	public int distanceMeter;

	//private Vector3 startPoint;
	//private Vector3 endPoint;
	private string nextDirection;
	private string crntDirection;
	private string fourDir;
	//RoomLocations rm;

	private int numWayPoint;
	private Vector3 diff;
	//private float degree; 
	private NavMeshPath path;
	private float elapsed;
	private float updateFreq = 0.5f;

	private float timer;
	//private int once;
	private string curMsg;
	private string difMsg;

	int curVal;
	NextDirection nextDir;
	RoomLocationsConf rlc;
	TheInformationBridge infoBrg;

	void OnApplicationQuit()
	{
		EasyTTSUtil.Stop();
	}

	// Use this for initialization
	void Start()
	{
		EasyTTSUtil.Initialize (EasyTTSUtil.UnitedStates);

		nextDir = new NextDirection();
		rlc = new RoomLocationsConf ();
		infoBrg = new TheInformationBridge ();
		curVal = 0;

		agent.enabled = false;

		player.transform.position = rlc.setStartingPoint();
		nextDir.setStart(rlc.setStartingPoint());

		target.transform.position = rlc.setEndingPoint();

		agent.enabled = true;



		path = agent.path;

		nextDirection = "";
		crntDirection = "";
		fourDir = " ";

		//degree = 0;           No longer used to get next direction to way point

		calculatePath();
		numWayPoint = path.corners.Length;
		elapsed = 0;

		curMsg = "constantly redefined";
		difMsg = "redefined only is curmsg changes";
	}

	// Update is called once per frame
	void Update()
	{
		elapsed += Time.deltaTime;
		if (elapsed > updateFreq) 
		{
			elapsed -= updateFreq;
			calculatePath ();
			calculateDirection ();							
			infoBrg.setInstruct(nextInstruction ());			// Sent to 'PlayerMovementCCNY.cs'
			distanceInformation ();
			calcAngleDifference ();
		}
		drawPath ();


		if (path.corners.Length != curVal) {
			Debug.Log ("Num of Waypoints: " + path.corners.Length);		//<----DEBUGGING: Number of waypoints
			curVal = path.corners.Length;
		}

		autoWalkToNextWayPoint ();

	}

	public void distanceInformation()
	{	
		distanceFeet = (int)(Mathf.Sqrt(	Mathf.Pow((path.corners[1].x - path.corners [0].x),2) + 
									Mathf.Pow((path.corners[1].z - path.corners [0].z),2) )/3);
		distanceMeter = distanceFeet /3;

		infoBrg.setDistanceF (distanceFeet);
		infoBrg.setDistanceM (distanceMeter);
	}

	public void calculatePath()
	{

		//Recalculates path
		NavMesh.CalculatePath (agent.transform.position, target.position, NavMesh.AllAreas, path);


		if (curMsg != difMsg)
		{
			Debug.Log (curMsg); 
			difMsg = curMsg;
		}
	}

	/*
	 * Draws a line connecting all waypoints from beginning 
	 * to end. *Color of line can be changed*
	 */
	public void drawPath()
	{
		// Draws out the line along the path
		for (int i = 0; i < path.corners.Length - 1; i++)
			Debug.DrawLine (path.corners[i], path.corners[i + 1], Color.cyan);
		
	}
	public void calculateDirection()
	{
		// diff = Vector3 FROM currentPoint TO origin(nextPoint)
		if (path.corners != null) {
			diff = path.corners [1] - path.corners [0];
			findMajorDirection (diff);
		}
		/*
                degree = calculateDegree(diff);
                nextDirection = findNextDirection(degree);

                if (crntDirection != nextDirection)
                {
                    nextDir.setNextDir(nextDirection);
                    Debug.Log("Go " + nextDirection);

                    EasyTTSUtil.SpeechAdd("Go " + nextDirection);
                    crntDirection = nextDirection;
                }
        */
	}

	/*
        Currently not in use...
    */
	private void findMajorDirection (Vector3 line)
	{
		float h = line.x;
		float v = line.z;

		if (Mathf.Abs (h) < Mathf.Abs (v))
		{
			if (v >= 0){
				//Debug.Log ("Go Up");
				nextDirection = "Up";
			}else{
				//Debug.Log ("Go Down");
				nextDirection = "Down";
			}
		}
		else
		{
			if (h >= 0){
				//Debug.Log ("Go Right");
				nextDirection = "Right";
			}else{
				//Debug.Log ("Go Left");
				nextDirection = "Left";
			}
		}

		if (crntDirection != nextDirection)
		{
			nextDir.setNextDir (nextDirection);
			//Debug.Log ("Go " + nextDirection);

			//EasyTTSUtil.SpeechAdd("Go " + nextDirection); 
			crntDirection = nextDirection;
		}
	}

	/*
        Currently not in use...
    */
	private float calculateDegree (Vector3 line)
	{
		float degreeTan = 0;
		int quadrant = 0;

		quadrant = findQuadrant (line);                 // Find line current quadrant.
		degreeTan = Mathf.Atan (line.z / line.x);       // Find angle of line.
		degreeTan = 180 * degreeTan / Mathf.PI;         // Convert from radians to degree.
		degreeTan = addToDegree (degreeTan, quadrant);  // Add number depending on what quadrant
		return degreeTan;                               // line fall under on coordinate.
	}

	/*
        Currently not in use...
    */
	private int findQuadrant (Vector3 vec)
	{
		if (vec.x < 0 && vec.z < 0)
			return 1;
		else if (vec.x >= 0 && vec.z >= 0)
			return 3;
		else if (vec.x < 0 && vec.z >= 0)
			return 4;
		else
			return 2;
	}

	/*
        Currently not in use...
    */
	private float addToDegree (float degree, int quadrant)
	{
		if (quadrant > 1)
		if (quadrant >= 4)
			degree += 360;
		else
			degree += 180;
		return degree;
	}

	/*
        Currently not in use...
        **Bird's Eye view instruction**
    */
	private string findNextDirection (float dgre)
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
	 * 			**FIRST PERSON INSTRUCTIONS**
	 * Determines the direction of next waypoint with
	 * respect to the avatar. Decides whether if next waypoint is
	 * either in Front, Behind, Left, or Right of the avatar.
	 * @return userDir - returns the direction of next waypoint
	 * 					 with respect to the avatar as a string.
	 */
	private string nextInstruction()
	{
		string userDir = ""; 
		Vector3 avtr2Pnt = transform.position - path.corners[1]; // A-C
		avtr2Pnt = transform.InverseTransformDirection(avtr2Pnt);

		if (Mathf.Abs(avtr2Pnt.x) > Mathf.Abs(avtr2Pnt.z)) {
			if(avtr2Pnt.x > 0){
				userDir = "Left";
			}
			else{ 
				userDir = "Right";
			}
		} 
		else 
		{
			if(avtr2Pnt.z > 0){
				userDir = "Behind";
			}
			else {
				userDir = "Forward";
			}
		}
		return userDir;
	}

	/*
	 * 	Used for the auto-walk feature to stop avatar at the
	 *  next waypoint in its path. The continues walking until
	 *  arrived at next waypoint.
	 */
	private void autoWalkToNextWayPoint()
	{
		int curNumWP = path.corners.Length;

		if (infoBrg.hasPermission()) 
		{
			agent.destination = target.position;

			Debug.Log("Has Permission to move");
			if (numWayPoint == curNumWP) {
				agent.Resume ();
				Debug.Log ("Should be moving");
			}
			else
			{
				agent.Stop ();
				Debug.Log ("Should not be moving");
				infoBrg.setPermission (false);
				numWayPoint = curNumWP;
			}
		}
			
	}

	private void calcAngleDifference()
	{
		Vector3 facing = transform.forward;
		Vector3 toWP = path.corners [1] - path.corners [0];

		float angle = Mathf.Rad2Deg*(Mathf.Acos ((Vector3.Dot (facing, toWP))/ (facing.magnitude * toWP.magnitude)));
		infoBrg.setAngleOfNextWP (angle);
	}

}
