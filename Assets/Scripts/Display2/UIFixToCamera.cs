using UnityEngine;

public class UIFixToCamera : MonoBehaviour
{
    public Camera cam;
    public Vector3 offset = new Vector3(0f, 0f, 2f);  // 相机前方 2 米

    void LateUpdate()
    {
        if (cam == null) return;

        transform.position = cam.transform.position + cam.transform.rotation * offset;
        transform.rotation = cam.transform.rotation;
    }
}
