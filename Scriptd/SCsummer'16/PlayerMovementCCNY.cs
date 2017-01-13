using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovementCCNY : MonoBehaviour { 


	public float speed = 1.2f;
	public float speedAndroid = 1.2f;
	public float rotateSpeed = 90f;
	public Text collisionText;
	public Text timerText;
	public int moveMode = 0;
	public bool autoWalk = false;

	float restartTime;
	float colTime;
	float interTime;

	int countColli;
	string instruct;
	const int MOVEMODE0 = 0;
	const int MOVEMODE1 = 1;

	Vector2 startpos;
	bool couldBeSwipe;
	float comfortZone = 5000f;
	float minSwipeDist = 100f;

	string nearBy;
	string[] nearByQueue;
	Vector2 touchDeltaPos;
	Vector2 basePos;

	bool isMoving;
	AudioSource source;
	public AudioClip footSteps; 
	RuntimePlatform platform = Application.platform;
	ArrayList myCols;
	Vector3 movement;
	Rigidbody playerRigidbody;
	NavMeshAgent agent;
	NextDirection nexDir;
	TheInformationBridge infoBrg;

	//Possibly defined by tayo AS script
	string start;
	string end;

	// Huang's work
	Vector3 constraintPos;
	bool endOfGame;
	public Transform target;

	// Use this for initialization
	void Awake()
	{
		// Initialize the player
		EasyTTSUtil.Initialize(EasyTTSUtil.UnitedStates);	
		playerRigidbody = GetComponent<Rigidbody>();
		infoBrg = new TheInformationBridge ();
		agent = GetComponent<NavMeshAgent> ();
		source = GetComponent<AudioSource> ();

		start = "Not defined yet";
		end = "Not defined yet";

		colTime 	= Time.timeSinceLevelLoad;
		myCols 		= new ArrayList();
		countColli 	= 0;
		endOfGame  	= false;
		isMoving 	= false;
		interTime 	= 1.0f;
		restartTime = 0.0f;
		instruct 	= "";
		nearBy 		= "";
		nearByQueue = new string[3];
		source.clip = footSteps;
		source.volume = 1f;

		if (collisionText != null)
			collisionText.text = "Collision count: " + countColli.ToString();
	}

	// Update is called once per frame
	void Update()
	{

		timer ();
		restartFunction ();
		quitFunction ();
		printNewInstruction ();
		distanceFromWayPoint2 ();
		preventDriftingRotation ();
		walkingSound ();
		doorNearby2 ();
		//Debug.Log ("Facing: " + transform.rotation.eulerAngles.y);
		//Debug.Log ("Difference: " + (infoBrg.getAngleOfNextWP()) );

		Debug.Log ("Start position from TAYO: " + start);
		Debug.Log ("Ending position from TAYO: " + end);

		if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer){	
			windowsInputFunction ();
			androidTouchInput2 ();
		}
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
			androidTouchInput2 ();
	}


	/*
	 * 				v	.__                .  .___          ,           	v
	 *			v	v	[ __ _ ._  _ ._. _.|  [__ . .._  _.-+-* _ ._  __	v	v
	 *		v	v	v	[_./(/,[ )(/,[  (_]|  |   (_|[ )(_. | |(_)[ )_) 	v	v	v
     *  v	v	v	v                                        				v	v	v	v
	 */


	/*  
	 * Updates the timer label on the Canvas
	 * >>Should be in Unity's Update Function<<
	 */

	void walkingSound()
	{
		if (isMoving) {
			if (!source.isPlaying) {
				source.loop = true;
				source.PlayScheduled(5f);
				source.Play ();
			}
			//Debug.Log ("Hear walking sound");
		} else {
			source.Stop ();
			//Debug.Log ("Not hear walking sound");
		}
	}
	void timer()
	{
		int secs = (int)(Time.timeSinceLevelLoad - restartTime) % 60;
		int mins = (int)(Time.timeSinceLevelLoad - restartTime) / 60;

		if (timerText != null)
			timerText.text = "Timer: " + string.Format("{0}", mins) + " : " + string.Format("{0:00}", secs);
	}

	/*
	void doorNearby()
	{
		RaycastHit hit;
		float radius = 6;
		string newNearByMsg = "";

		// Draw Ray vertically and horizontally
		Debug.DrawRay (transform.position, -(transform.right*radius)*1.5f, Color.green, 0f, true);
		Debug.DrawRay (transform.position, (transform.right*radius)*1.5f, Color.green, 0f, true);
		Debug.DrawRay (transform.position, (transform.forward*radius)*1.5f, Color.yellow, 0f, true);
		Debug.DrawRay (transform.position, -(transform.forward*radius)*1.5f, Color.red, 0f, true);

		// Draw Ray diagonally
		Debug.DrawRay (transform.position, (transform.forward+transform.right)*(radius), Color.blue, 0f, true);
		Debug.DrawRay (transform.position, (transform.forward-transform.right)*(radius), Color.magenta, 0f, true);
		Debug.DrawRay (transform.position, -(transform.forward+transform.right)*(radius), Color.grey, 0f, true);
		Debug.DrawRay (transform.position, -(transform.forward-transform.right)*(radius), Color.white, 0f, true);

		// Shoot Raycast Forward
		if (Physics.Raycast (transform.position, transform.forward, out hit, radius * 1.5f)) {
			if (hit.collider.gameObject.tag == "Door") {
				//Debug.Log ("RAYCAST HIT in FRONT: ---> " + hit.collider.gameObject.name);
				newNearByMsg = "room: " + hit.collider.gameObject.name + " in front.";

				if (nearBy != newNearByMsg)
				{
					EasyTTSUtil.SpeechAdd (newNearByMsg);
					Debug.Log (newNearByMsg);
					if(hit.transform != null)
						hit.transform.SendMessage ("AvatarNearBy");
					nearBy = newNearByMsg;
				}
			}
		}

		// Shoot Raycast Behind
		if (Physics.Raycast (transform.position, -transform.forward, out hit, radius * 1.5f)) {
			if (hit.collider.gameObject.tag == "Door") { 
				Debug.Log ("RAYCAST HIT in BEHIND: ---> " + hit.collider.gameObject.name);
			}
		}
		// Shoot Raycast Left
		if (Physics.Raycast (transform.position, -transform.right, out hit, radius * 1.5f)) {
			if (hit.collider.gameObject.tag == "Door") {
				//Debug.Log ("Passing by room: <--- " + hit.collider.gameObject.name + " door on LEFT");
				newNearByMsg = "...: " + hit.collider.gameObject.name + " to your LEFT.";

				if (nearBy != newNearByMsg)
				{
					EasyTTSUtil.SpeechAdd (newNearByMsg);
					Debug.Log (newNearByMsg);
					if(hit.transform != null)
						hit.transform.SendMessage ("AvatarNearBy");
					nearBy = newNearByMsg;
				}
			}
		}
		// Shoot Raycast Right
		if (Physics.Raycast (transform.position, transform.right, out hit, radius * 1.5f)) {
			if (hit.collider.gameObject.tag == "Door") {
				//Debug.Log ("Passing by room: ---> " + hit.collider.gameObject.name + " door on RIGHT");
				newNearByMsg = "...: " + hit.collider.gameObject.name + " to your Right.";

				if (nearBy != newNearByMsg)
				{
					EasyTTSUtil.SpeechAdd (newNearByMsg);
					Debug.Log (newNearByMsg);
					if(hit.transform != null)
						hit.transform.SendMessage ("AvatarNearBy");
					nearBy = newNearByMsg;
				}
			}
		}			
	}
	*/
	void doorNearby2()
	{
		RaycastHit hit;
		float radius = 6;
		string newNearByMsg = "";

		// Draw Ray vertically and horizontally
		Debug.DrawRay (transform.position, -(transform.right*radius)*1.5f, Color.magenta, 0f, true);
		Debug.DrawRay (transform.position, (transform.right*radius)*1.5f, Color.red, 0f, true);
		Debug.DrawRay (transform.position, (transform.forward*radius)*1.5f, Color.white, 0f, true);
		Debug.DrawRay (transform.position, -(transform.forward*radius)*1.5f, Color.black, 0f, true);

		// Shoot Raycast Forward
		if (Physics.Raycast (transform.position, transform.forward, out hit, radius * 1.5f)) {
			if (hit.collider.gameObject.tag == "Door") {
				//Debug.Log ("RAYCAST HIT in FRONT: ---> " + hit.collider.gameObject.name);
				newNearByMsg = "...: " + hit.collider.gameObject.name + " ahead.";

				if (!inQueue(newNearByMsg))
				{
					EasyTTSUtil.SpeechAdd (newNearByMsg);
					Debug.Log (newNearByMsg);
					if(hit.transform != null)
						hit.transform.SendMessage ("AvatarNearBy");
					addToQueue(newNearByMsg);
				}
			}
		}

		// Shoot Raycast Behind
		if (Physics.Raycast (transform.position, -transform.forward, out hit, radius * 1.5f)) {
			if (hit.collider.gameObject.tag == "Door") { 
				//Debug.Log ("RAYCAST HIT in BEHIND: ---> " + hit.collider.gameObject.name);
			}
		}
		// Shoot Raycast Left
		if (Physics.Raycast (transform.position, -transform.right, out hit, radius * 1.5f)) {
			if (hit.collider.gameObject.tag == "Door") {
				//Debug.Log ("Passing by room: <--- " + hit.collider.gameObject.name + " door on LEFT");
				newNearByMsg = "...: " + hit.collider.gameObject.name + " to your LEFT.";

				if (!inQueue(newNearByMsg))
				{
					EasyTTSUtil.SpeechAdd (newNearByMsg);
					Debug.Log (newNearByMsg);
					if(hit.transform != null)
						hit.transform.SendMessage ("AvatarNearBy");
					addToQueue(newNearByMsg);
				}
			}
		}
		// Shoot Raycast Right
		if (Physics.Raycast (transform.position, transform.right, out hit, radius * 1.5f)) {
			if (hit.collider.gameObject.tag == "Door") {
				//Debug.Log ("Passing by room: ---> " + hit.collider.gameObject.name + " door on RIGHT");
				newNearByMsg = "...: " + hit.collider.gameObject.name + " to your Right.";

				if (!inQueue(newNearByMsg))
				{
					EasyTTSUtil.SpeechAdd (newNearByMsg);
					Debug.Log (newNearByMsg);
					if(hit.transform != null)
						hit.transform.SendMessage ("AvatarNearBy");
					addToQueue(newNearByMsg);
				}
			}
		}			
	}

	bool inQueue(string value)
	{
		for (int i = 0; i < nearByQueue.Length; i++)
			if (nearByQueue [i] == value)
				return true;
		return false;
	}

	void addToQueue(string value)
	{
		for (int i = nearByQueue.Length - 2; i >= 0; i--)
			nearByQueue[i+1] = nearByQueue [i];

		nearByQueue [0] = value;
	}
	void restartFunction()
	{
		if (Input.GetKeyDown(KeyCode.Space))	
			resetGame();
	}
	void distanceFromWayPoint2()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow)) 
		{
			Debug.Log ("Next-Waypoint is about " + infoBrg.getDistanceF () + " feet away");
			Debug.Log ("Next-Waypoint is about " + infoBrg.getDistanceM () + " meters away");
		}
	}
	void quitFunction()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();	
	}

	void windowsInputFunction()
	{
		// Get from Input
		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			isMoving = true;
			if (autoWalk)
				autoWalkInEditor ();
			else
				moveForward ();
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
			//isMoving = true;
			rotateAngle (1);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
			//isMoving = true;
			rotateAngle (2);
		}

		if (Input.GetKeyUp (KeyCode.UpArrow))
			isMoving = false;
	}

	void preventDriftingRotation()
	{
		//Prevents Slight Drifting of Y rotation
		if (transform.rotation.eulerAngles.y > 180 && transform.rotation.eulerAngles.y < 181) {
			transform.rotation = Quaternion.Euler (0, 180, 0);
			//EasyTTSUtil.SpeechAdd ("Ahhh bug");
		}
		if(transform.rotation.eulerAngles.y > 0 && transform.rotation.eulerAngles.y < 10)
			transform.rotation = Quaternion.Euler(0,0,0);
		if(transform.rotation.eulerAngles.y > 90 && transform.rotation.eulerAngles.y < 91)
			transform.rotation = Quaternion.Euler(0,90,0);
	}


	/*
	 *					v   .  .     ,      .___  .  ,         .___          ,           	v
	 *				v	v	|  |._ *-+-  .  [__  _|*-+- _ ._.  [__ . .._  _.-+-* _ ._  __	v	v
	 *			v	v	v	|__|[ )| | \_|  [___(_]| | (_)[    |   (_|[ )(_. | |(_)[ )_) 	v	v	v
     *		v	v	v	v  	           ._|                                                	v	v	v	v
	 */



	/**
	 * Movement method to move the avatar with keyboard 
	 * on the COMPUTER (Unity Editor).
	 * Use Arrow Keys: Up, Down, Left, Right
	 * Use LetterKeys: W, S, A, D
	*/
	void moveForward()
	{
		movement = transform.forward;
		movement = movement.normalized * speed * Time.deltaTime * 10;
		playerRigidbody.MovePosition(transform.position + movement);
	}

	void autoWalkInEditor()	
	{		
		// Moves the avatar forward
		playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		infoBrg.setPermission(true);
		//playerRigidbody.constraints = ~RigidbodyConstraints.FreezeRotationY;
	}
	void rotateAngle(int direction)
	{
		Vector3 rot = transform.rotation.eulerAngles;
		Vector3 newRot;
		if (direction == 1) 
		{ 						// Left
			transform.Rotate (0, -90f, 0);
			newRot = transform.rotation.eulerAngles;
			Debug.Log("Degree Turned: " + readDegreeTurnMade (rot, newRot));
			//Debug.Log ("Left Turn...");
		} 
		else if (direction == 2) 
		{ 						// Right
			transform.Rotate (0, 90f, 0);
			newRot = transform.rotation.eulerAngles;
			Debug.Log("Degree Turned: " + readDegreeTurnMade (rot, newRot));
			//Debug.Log ("Right Turn...");
		}
			
	}

	float readDegreeTurnMade(Vector3 oldPoint, Vector3 newPoint)
	{
		if(Mathf.Abs(oldPoint.y - newPoint.y) >= 185)
			return Mathf.Abs(oldPoint.y - newPoint.y) - 180;
		else
			return Mathf.Abs(oldPoint.y - newPoint.y);
	}

	/*
	 * Controls the number of times directional instruction 
	 * is printed in the update Function.		
	 */
	void printNewInstruction()
	{
		string newInstruct = infoBrg.getNextInstruction ();
	
		if (newInstruct != instruct) 
		{
			Debug.Log ("Waypoint direction: " + newInstruct);  		// <----DEBUGGING

			if(newInstruct == "Forward")
				EasyTTSUtil.SpeechAdd("Walk " + newInstruct);
			else if (newInstruct == "Behind")
				EasyTTSUtil.SpeechAdd(/* "Waypoint is " + newInstruct */"");
			else
				EasyTTSUtil.SpeechAdd("Turn " + newInstruct);
			
			instruct = newInstruct;	
		}
	}



/*
 *   			v	   .__.     .         .  .__                 .___          ,          		v 
 *   		v	v      [__]._  _|._. _ * _|  |  \ _ .  ,* _. _   [__ . .._  _.-+-* _ ._  __		v	v
 *   	v	v	v      |  |[ )(_][  (_)|(_]  |__/(/, \/ |(_.(/,  |   (_|[ )(_. | |(_)[ )_) 		v	v	v
 *   v	v	v	v                                                               				v	v	v	v
 */

	void androidTouchInput2()
	{
		Touch touch;
		float swipeDist = 0;

		if (Input.touchCount > 0 && !infoBrg.isGestureModeActive()) 
		{
			
	//***********************************************************************					
			if (Input.GetTouch (0).tapCount == 3)	// Triple Tap Function 
				resetGame ();						// on Android Device
			
	//***********************************************************************

			touch = Input.GetTouch (0);
			switch(touch.phase)
			{
			case TouchPhase.Began:					// 						v TOUCH BEGINNING PHASE v
				couldBeSwipe = true;				//  					chance of being a swipe
				startpos = touch.position;			//  					take of first position touched	
				break;

	//******************************************************************

			case TouchPhase.Moved:									// 		v TOUCH WHILE MOVING v
				swipeDist = touch.position.y - startpos.y; 			//		take note of how far swiped

				if(minSwipeDist < Mathf.Abs(swipeDist))				// 			CHOOSE ONLY 1 OR 2 (BELOW)	
				{
					isMoving = true;
					if(!autoWalk)	
						moveInAndroid (swipeDist * speedAndroid); 		//	<-1-- Allows Free-Movement in Android Device
					else
						autoWalkInAndroid(swipeDist * speedAndroid);	//	<-2-- Auto-walks to next waypoint 
				}
				if (Mathf.Abs (touch.position.y - startpos.y) > comfortZone) 		// is swipe too far?
				{																	// not a valid swipe
					couldBeSwipe = false;											
				}
				break;

	//*******************************************************************

			case TouchPhase.Stationary:								//	 	v TOUCH HELD DOWN (STATIONARY) v
				swipeDist = touch.position.y - startpos.y;			// take note of how far swiped
					
				if (minSwipeDist < Mathf.Abs (swipeDist)) {				// CONDITION: 	Is swipe distance greater than minSwipeDistance?	
					isMoving = true;
					if (!autoWalk)
						moveInAndroid (swipeDist * speedAndroid); 		//	<-1-- Allows Free-Movement in Android Device
						else
						autoWalkInAndroid (swipeDist * speedAndroid);	
				}
					break;
	//*******************************************************************
			case TouchPhase.Ended:										// 		v TOUCH OFF SCREEN (ENDED)
				swipeDist = (touch.position - startpos).magnitude;		
				isMoving = false;
				if (couldBeSwipe && (swipeDist > minSwipeDist)) 
				{
					float xDistance = touch.position.x - startpos.x;
					float yDistance = touch.position.y - startpos.y;	

					if (Mathf.Abs (xDistance) > Mathf.Abs (yDistance)) { 	// If horizontal swipe// rotate the avatar 
						if (!autoWalk)
							rotate (xDistance);								// (Turn avatar left or right 90 degrees)
						else
							autoRotate (xDistance);
						
					} else if (Mathf.Abs (xDistance) < Mathf.Abs (yDistance)) {
						if (yDistance < 0)
							distanceFromWayPoint ();
					}
				}
					break;
				default:
					break;	
			}

		}

	}

	/* 
	 * Movement method to move the avatar FORWARD with 
	 * swipes on the ANDROID device (Mobile Application).
	 * @param v - Vertical direction swipe
	 */
	void moveInAndroid(float v)		
	{
		if (v >= 0) { 		// Only takes upward swipes
			// Moves the avatar forward
			movement.Set (0f, 0f, v);						
			movement = transform.forward;
			movement = movement.normalized * speed * Time.deltaTime * 10;
			playerRigidbody.MovePosition (transform.position + movement);
		} 
	}

	void distanceFromWayPoint()
	{
		Debug.Log ("Next-Waypoint is about " + infoBrg.getDistanceF () + " feet away");
		Debug.Log ("Next-Waypoint is about " + infoBrg.getDistanceM () + " meters away");
		EasyTTSUtil.SpeechFlush( "" + infoBrg.getDistanceF () + " feet away. " + infoBrg.getDistanceM() + " meters away.");
	}

	/* 
	 * Auto-Movement method to direct the avatar to walk to 
	 * the next waypoint on his path to destination.
	 * @param v - Vertical direction swipe
	 */
	void autoWalkInAndroid(float v)		
	{
		if (v >= 0) 		// Only takes upward swipes
		{
			// Moves the avatar forward
			playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			infoBrg.setPermission(true);
			//playerRigidbody.constraints = ~RigidbodyConstraints.FreezeRotationY;
		}

	}

	/*
	 * Movement method to rotate the avatar left or right
	 * 90 DEGREES with swipes on the ANDROID device (Mobile Application).
	 * @param h - Horizontal direction swipe
	 */
	void rotate(float h)	
	{
		if (h >= 0) 								// Determines if left or right swipe
		{
			transform.Rotate (0, 90f, 0);			//** Right Rotation
			EasyTTSUtil.SpeechFlush ("Right " + 90 + " degree turn made");

		}else{
			transform.Rotate (0, -90f, 0);			// **Left Rotation
			EasyTTSUtil.SpeechFlush ("Left " + 90 + " degree turn made");
		}
	}

	void autoRotate(float h)	
	{
		//Debug.Log ("Facing: " + transform.rotation.eulerAngles.y);
		//Debug.Log ("Difference: " + (infoBrg.getAngleOfNextWP()) );
		//Debug.Log ();

		playerRigidbody.constraints = ~RigidbodyConstraints.FreezeRotation;
		if (h >= 0) 								// Determines if left or right swipe
			transform.rotation = Quaternion.Euler(0,(transform.rotation.eulerAngles.y + infoBrg.getAngleOfNextWP()),0);		//** Left Rotation
		else
			transform.rotation = Quaternion.Euler(0,(transform.rotation.eulerAngles.y - infoBrg.getAngleOfNextWP()),0);		// **Right Rotation
		playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
	}

	/*
	 * Currently not in use...
	 */
	bool isMoveAllowed(string dir)
	{
		nexDir = new NextDirection();	
		if (nexDir.getNextDir().Equals(dir))	
			return true;
		return false; 
	}

	/*
	 * Currently not in use...
	 */
	Vector2 constraint(Vector2 v)
	{
		float x = v.x;
		float y = v.y;

		if (x >= y)
		{
			if (x > -y)
				v = Vector2.right;
			else
				v = Vector2.down;
		}
		else
		{
			if (x > -y)
				v = Vector2.up;
			else
				v = Vector2.left;
		}

		return new Vector2(x, y);
	}

	/*
	 * When avatar comes in contact with an object
	 * @param col - the object that was collided
	 */
	void OnCollisionEnter(Collision col)
	{

		if (col.gameObject.CompareTag("Base_Wall") || col.gameObject.CompareTag("Wall"))
		{
			Debug.Log("Enter " + col.transform.name);
			if (myCols.Count == 0)
			{
				interTime = Time.timeSinceLevelLoad - colTime;
				colTime = Time.timeSinceLevelLoad;
				if (colTime > 0.1f)
				{
					Handheld.Vibrate();
					countColli++;
					collisionText.text = "Collision count: " + countColli.ToString();
				}
			}
			else
			{
				Vector3 myNormal = col.contacts[0].normal.normalized;
				Vector3 wallForward = col.transform.forward;
				Debug.Log(col.transform.name + myNormal);
				Debug.Log(col.transform.name + "forward: " + wallForward);
				foreach (ContactPoint n in col.contacts)
					Debug.Log("Contact Point " + n.normal);

				if (myNormal == wallForward || myNormal == -wallForward)
				{
					interTime = Time.timeSinceLevelLoad - colTime;
					colTime = Time.timeSinceLevelLoad;
					if (interTime > 0.3f)
					{
						Vector3 myDirection = playerRigidbody.transform.forward;

						float dotProduct = Vector3.Dot(myNormal, myDirection);
						if (dotProduct < -0.2f)
						{
							Debug.Log(col.transform.name);
							Handheld.Vibrate();
							countColli++;
							collisionText.text = "Collision count: " + countColli.ToString();

						}
						Debug.Log(myDirection);
					}
				}
			}

			myCols.Add(col);
			Debug.Log("Collision: " + countColli);
		}
	}


	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.CompareTag("Base_Wall") || col.gameObject.CompareTag("Wall"))
		{
			Debug.Log("Exist " + col.transform.name);
			myCols.RemoveAt(myCols.Count - 1);
		}
	}


	/*
	 * Resets avatar to their starting position and resets
	 * the timer.
	 */ 
	void resetGame()
	{
		nexDir = new NextDirection();
		NavMeshAgent agent = GetComponent<NavMeshAgent>();

		agent.enabled = false;  
		transform.rotation = Quaternion.Euler(0,0,0);
		playerRigidbody.MovePosition(nexDir.getStart());
		EasyTTSUtil.SpeechFlush("Your location has been reset to your original starting point");
		agent.enabled = true;

		countColli = 0;

		collisionText.text = "Collision count: " + countColli.ToString();

		restartTime = Time.timeSinceLevelLoad;
		Debug.Log("resartTime " + restartTime);
	}
		
	/*
	 * When avatar come into contact with a TRIGGER
	 * such as the destination cube.
	 * @param col - the trigger that was collided
	 */ 
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("target"))
		{
			endOfGame = true;
			nexDir = new NextDirection();
			nexDir.setEndTime(timerText.text);
			nexDir.setNumOfCollisions(countColli);

			EasyTTSUtil.SpeechFlush("Congratulations, you've reached your destination.");
			Debug.Log("Game Over, you've completed the course");

			Invoke("changeToEndGameScene", 4); // Wait 4 seconds and then call the "changeToEndScene" Method.
		}

		if (col.gameObject.tag == "Door") 
		{
			Debug.Log ("Walking through room " + col.gameObject.name + " door");
			EasyTTSUtil.SpeechAdd("Walking through room " + col.gameObject.name + " door");
		}
	}

	void changeToEndGameScene()
	{
		SceneManager.LoadScene("EndScreen");
	}
		

	// CALLED FROM ANDROID STUDIO						// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	public void setStartInUnity(string startpos)		// <<<<<<<<<<<<<<<<<<<<<<<<<<<<      <<<<<<<<<<<
	{													// <<<<<<<<<<<   <<<<<<<<<<<<   <<<<<  <<<<<<<<
		start = startpos;								// <<<<<<<<<<  <  <<<<<<<<<<   <<<<<<<<<<<<<<<<
	}													// <<<<<<<<<  <<<  <<<<<<<<<<<   <<<<<<<<<<<<<<
	public void setEndInUnity(string endpos)			// <<<<<<<<  <<<<<  <<<<<<<<<<<<   <<<<<<<<<<<<
	{													// <<<<<<<           <<<<<<<<<<<<<<  <<<<<<<<<<
		end = endpos;									// <<<<<<  <<<<<<<<<  <<<<<  <<<<<<<  <<<<<<<<<<<
	}													// <<<<<  <<<<<<<<<<<  <<<<<<        <<<<<<<<<<<
}  														// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
														// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
// For Text art:  
//		http://patorjk.com/software/taag/#p=display&f=Contessa&t=Android%20Device%20Functions