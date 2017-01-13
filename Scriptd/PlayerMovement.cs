using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 8f;
    public float speedAndroid = 8f;
    public float rotateSpeed = 90f;
    public Text collisionText;
    public Text timerText;
    public int moveMode = 0;

    float restartTime;
    float colTime;
    float interTime;
    int countColli;
    //Vector3 startPos;
    const int MOVEMODE0 = 0;
    const int MOVEMODE1 = 1;

    RuntimePlatform platform = Application.platform;
    ArrayList myCols;
    Vector3 movement;
    Vector2 touchDeltaPos;
    Vector2 basePos;
    Rigidbody playerRigidbody;
    NextDirection nexDir;
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
		NavMeshAgent nav = GetComponent<NavMeshAgent> ();

		colTime 	= Time.timeSinceLevelLoad;
		myCols 		= new ArrayList();
        countColli 	= 0;
		endOfGame  	= false;
		interTime 	= 1.0f;
		restartTime = 0.0f;

        if (collisionText != null)
            collisionText.text = "Collision count: " + countColli.ToString();

		nav.SetDestination (target.transform.position);
    }
		
    // Update is called once per frame
    void FixedUpdate()
    {

		timer ();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            resetGame();
        }

        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("Running on Android.");
            /*     if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                 {
                     basePosition = Input.GetTouch(0).position;
                 }*/
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).tapCount == 3)
                {
                    resetGame();
                }
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    basePos = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    switch (moveMode)
                    {
                        case MOVEMODE0:
                            touchDeltaPos = Input.GetTouch(0).position - basePos;
                            break;
                        case MOVEMODE1:
                            touchDeltaPos = Input.GetTouch(0).deltaPosition;
                            break;
                    }
                    touchDeltaPos.Normalize();
                    touchDeltaPos = constraint(touchDeltaPos);
                    Debug.Log("Constrainted position: " + touchDeltaPos);
                    moveInAndroid(touchDeltaPos.x, touchDeltaPos.y);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                    moveInAndroid(speedAndroid * touchDeltaPos.x, speedAndroid * touchDeltaPos.y);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    touchDeltaPos = Vector2.zero;
                }
            }
        }
        else if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
        {
            // Get from Input
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (h != 0f || v != 0f)
                move(h, v);
        }
        //Debug.Log(playerRigidbody.velocity);
    }

	/*  Updates the timer label on the Canvas
	 * >>Should be in Unity's Update Function<<
	 */
	void timer()
	{
		int secs = (int)(Time.timeSinceLevelLoad - restartTime) % 60;
		int mins = (int)(Time.timeSinceLevelLoad - restartTime) / 60;

		if (timerText != null)
			timerText.text = "Timer: " + string.Format("{0}", mins) + " : " + string.Format("{0:00}", secs);
	}

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
	 * Movement method to move the avatar with keyboard 
	 * on the COMPUTER (Unity Editor).
	 * Use Arrow Keys: Up, Down, Left, Right
	 * Use LetterKeys: W, S, A, D
	*/
    void move(float h, float v)
    {
        movement.Set(h, 0f, v);

        // Normalization is for keep speed consistent
        movement = movement.normalized * speed * Time.deltaTime * 10;

        playerRigidbody.MovePosition(transform.position + movement);

        rotate(h, v);
    }

	/* 
	 * Movement method to move the avatar with swipes 
	 * on the ANDROID device (Mobile Application).
	 * @param h - Horizontal direction
	 * @param v - Vertical direction
	*/
    void moveInAndroid(float h, float v)
    {
        string userDir = "";

        if (Mathf.Abs(h) < Mathf.Abs(v))
        {
            if (v >= 0)
                userDir = "Up";
            else
                userDir = "Down";

            movement.Set(0f, 0f, v);
        }
        else
        {
            if (h >= 0)
                userDir = "Right";
            else
                userDir = "Left";

            movement.Set(h, 0f, 0f);
        }

        // Normalization is for keep speed consistent
        movement = movement.normalized * speedAndroid * Time.deltaTime * 10;

        if (isMoveAllowed(userDir) && !(endOfGame))
        {
            playerRigidbody.MovePosition(transform.position + movement);
            constraintPos = transform.position;
        }
        else
        {
            if ((transform.position - constraintPos).magnitude < 5.0f) // <---Controls ease of restriction
            {
                playerRigidbody.MovePosition(transform.position + movement);
            }
        }

        rotate(h, v);
    }

	/*
	 * Communticates with PathFinding Script
	 * to get back next waypoint's direction
	 */
    bool isMoveAllowed(string dir)
    {
        nexDir = new NextDirection();	
        if (nexDir.getNextDir().Equals(dir))	
            return true;
        return false;
    }

    void rotate(float h, float v)
    {
        Vector3 target = new Vector3(h, 0f, v);

        Quaternion targetDirection = Quaternion.LookRotation(target, Vector3.up);

        Quaternion newDirection = Quaternion.Lerp(playerRigidbody.rotation, targetDirection, rotateSpeed * Time.deltaTime);

        playerRigidbody.MoveRotation(newDirection);
    }

    void resetGame()
    {
        //playerRigidbody.MovePosition(new Vector3(0, transform.position.y, 0));
        nexDir = new NextDirection();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        agent.enabled = false;  
        playerRigidbody.MovePosition(nexDir.getStart());
        EasyTTSUtil.SpeechFlush("Your location has been reset to your original starting point");
        agent.enabled = true;

        countColli = 0;

        collisionText.text = "Collision count: " + countColli.ToString();

        restartTime = Time.timeSinceLevelLoad;
        Debug.Log("resartTime " + restartTime);
    }

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
    }

    void changeToEndGameScene()
    {
        SceneManager.LoadScene("EndScreen");
    }
}