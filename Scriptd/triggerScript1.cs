using UnityEngine;
using System.Collections;

public class triggerScript1 : MonoBehaviour {

    public AudioSource directionSound;

    void OnTriggerEnter (Collider other) {
        directionSound.Play();
    }

}
