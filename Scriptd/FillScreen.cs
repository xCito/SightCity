using UnityEngine;

public class FillScreen : MonoBehaviour
{
    public Camera cam;
    void Update()
    {
        float pos = (cam.nearClipPlane + 0.01f);

        transform.position = cam.transform.position + cam.transform.forward * pos;

        float h = Mathf.Tan (cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2f;

        transform.localScale = new Vector3 (h * cam.aspect, h, 1f);
        transform.rotation = Quaternion.Euler (270f,0f,0f);
    }
}