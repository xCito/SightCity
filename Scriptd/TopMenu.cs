using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TopMenu : MonoBehaviour
{
    void Update()
    {
        //Touch touch = Input.GetTouch(0);
        if (Input.GetKey ("up"))
        {
            print (SceneManager.GetActiveScene().name);
            SceneManager.LoadScene ("FloorPick");
        }
        if (Input.GetKey ("down"))
        {
            print ("down arrow key is held down");
            SceneManager.LoadScene ("FloorPick");
        }

    }
}