using UnityEngine;

public class AutoScaleByCameraDistance : MonoBehaviour
{
    public Transform cameraTransform;
    public float scaleMultiplier = 0.3f;
    public float minScale = 0.1f;
    public float maxScale = 1.5f;

    void Update()
    {
        if (cameraTransform == null) return;

        float distance = Vector3.Distance(transform.position, cameraTransform.position);
        float scale = Mathf.Clamp(distance * scaleMultiplier, minScale, maxScale);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
