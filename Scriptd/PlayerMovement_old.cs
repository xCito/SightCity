using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement_old : MonoBehaviour {

    public float speed = 8f;
    public float speed2 = 100.0f;
    private Vector2 vecA;
    private Vector2 vecB;
    private Vector3 moveTouch;
    //public Text congratText;

    Vector3 movement;
    Rigidbody playerRigidbody;

    // Use this for initialization
    void Awake ()
    {
        // Initialize the player
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {

        /***************************/
        /*  Get Touch Screen Input */
        /*       For Movement      */
        /***************************/

        /**/      //touchCount returns # of fingers on screen
        /**/ if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began)
            /**/     vecA = Input.GetTouch (0).position;
        /**/
        /**/ if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Stationary)
            /**/ {
            /**/     vecB = Input.GetTouch (0).position;
            /**/     moveTouch = vecB - vecA;
            /**/     moveTouch.z = moveTouch.y;
            /**/     moveTouch.y = 0;
            /**/     moveTouch = moveTouch.normalized; //* speed2 * Time.deltaTime * 10;
            /**/     transform.position = (moveTouch + transform.position);
            /**/
        }

        //Get from Input
        float h = Input.GetAxisRaw ("Horizontal");
        float v = Input.GetAxisRaw ("Vertical");

        move (h, v);
    }
    // Move base on Input
    void move (float h, float v)
    {
        movement.Set (h, 0f, v);

        // Normalization is for keep speed consistent
        movement = movement.normalized * speed * Time.deltaTime * 10;

        playerRigidbody.MovePosition (transform.position + movement);
    }

}