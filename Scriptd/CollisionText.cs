/*using UnityEngine;
using UnityEngine.UI;

public class CollisionText : MonoBehaviour {

    public  Text collisionText;
    private int countColli;
    public  AudioClip bumpFX;
    private AudioSource source;

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
        countColli = 0;
        collisionText.text = "Collisions: " + countColli.ToString();
    }

	// Update is called once per frame
	void Update () {

	}
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            source.PlayOneShot(bumpFX,
 1f);
            countColli++;
            collisionText.text = "Collisions: " + countColli.ToString();
        }
    }

    int getNumberCollision()
    {
        return countColli;
    }
}
*/
