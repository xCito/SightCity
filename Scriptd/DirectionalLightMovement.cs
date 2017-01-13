using UnityEngine;
using System.Collections;

public class DirectionalLightMovement : MonoBehaviour {

    Transform playerTransform;
    Vector3 relCameraPos;
    Vector3 stadardPos;
    public float smooth = 1.5f;

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
        transform.position = playerTransform.position + new Vector3 (0,
                             3,
                             0);
        // Relative position from player to camera
        relCameraPos = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stadardPos = playerTransform.position + relCameraPos;

        transform.position = Vector3.Lerp (transform.position,
                                           stadardPos,
                                           smooth * Time.deltaTime);
    }
}
