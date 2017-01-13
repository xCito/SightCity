using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ChangingFloors : MonoBehaviour {

    // First Floor ascenders
    public GameObject firstUpStair1;
    public GameObject upStairA1;
    public GameObject firstUpStair2;
    public GameObject upStairA2;
    public GameObject firstUpStair3;
    public GameObject upStairA3;
    public GameObject firstUpElev1;
    public GameObject upElevA1;
    public GameObject firstUpElev2;
    public GameObject upElevA2;

    // Second Floor Ascenders
    public GameObject secondUpStair1;
    public GameObject upStairB1;
    public GameObject secondUpStair2;
    public GameObject upStairB2;
    //public GameObject secondUpStair3;
    //public GameObject upStairB3;
    //public GameObject secondUpStair4;
    //public GameObject upStairB4;
    public GameObject secondUpElev1;
    public GameObject upElevB1;
    public GameObject secondUpElev2;
    public GameObject upElevB2;
    // Second Floor Descenders
    public GameObject secondDnStair1;
    public GameObject dnStairB1;
    public GameObject secondDnStair2;
    public GameObject dnStairB2;
    public GameObject secondDnStair3;
    public GameObject dnStairB3;
    //public GameObject secondDnStair4;
    //public GameObject dnStairB4;
    public GameObject secondDnElev1;
    public GameObject dnElevB1;
    public GameObject secondDnElev2;
    public GameObject dnElevB2;

    // Third Floor Descenders
    public GameObject thirdDnStair1;
    public GameObject dnStairC1;
    public GameObject thirdDnStair2;
    public GameObject dnStairC2;
    //public GameObject thirdDnStair3;
    //public GameObject dnStairC3;
    //public GameObject thirdDnStair4;
    //public GameObject dnStairC4;
    public GameObject thirdDnElev1;
    public GameObject dnElevC1;
    public GameObject thirdDnElev2;
    public GameObject dnElevC2;

    public Dictionary<GameObject,
           GameObject> gmObj;
    NavMeshAgent agent;

    // Use this for initialization
    void Start()
    {
        EasyTTSUtil.Initialize (EasyTTSUtil.UnitedStates);
        agent = GetComponent<NavMeshAgent>();
        gmObj = new Dictionary<GameObject,
        GameObject>()
        {
            // First Floor
            {   firstUpStair1,
                upStairA1
            },
            {   firstUpStair2,
                upStairA2
            },
            //Stairs going up
            {   firstUpStair3,
                upStairA3
            },

            {   firstUpElev1,
                upElevA1
            },
            {   firstUpElev2,
                upElevA2
            },
            //Elevators West and East going up

            // Second Floor
            {   secondUpStair1,
                upStairB1
            },
            {   secondUpStair2,
                upStairB2
            },
            //Stairs going up
            {   secondDnStair1,
                dnStairB1
            },
            {   secondDnStair2,
                dnStairB2
            },
            //Stairs going down
            {   secondDnStair3,
                dnStairB3
            },

            {   secondUpElev1,
                upElevB1
            },
            {   secondUpElev2,
                upElevB2
            },
            //Elevators West and East going up
            {   secondDnElev1,
                dnElevB1
            },
            {   secondDnElev2,
                dnElevB2
            },
            //Elevators West and East going down

            // Third Floor
            {   thirdDnStair1,
                dnStairC1
            },
            {   thirdDnStair2,
                dnStairC2
            },
            //Stairs going down
            {   thirdDnElev1,
                dnElevC1
            },
            {   thirdDnElev2,
                dnElevC2
            },
            //Elevators West and East going down

        };
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.tag == "GoingUp")
        {
            Debug.Log ("Going Up!");
            EasyTTSUtil.SpeechAdd ("Going Up");
            agent.enabled = false;                                              // To Release player from navMesh
            transform.position = gmObj[col.gameObject].transform.position + new Vector3 (5,
                                 0,
                                 5);
            agent.enabled = true;                                               // To Attach player back to navMesh
        }

        if (col.gameObject.tag == "GoingDown")
        {
            Debug.Log ("Going Down!");
            EasyTTSUtil.SpeechAdd ("Going Down");
            agent.enabled = false;                                               // To Release player from navMesh
            transform.position = gmObj[col.gameObject].transform.position + new Vector3 (-5,
                                 0,
                                 -5);
            agent.enabled = true;                                                // To Attach player back to navMesh
        }
    }

    void OnApplicationQuit()
    {
        EasyTTSUtil.Stop();
    }
}
