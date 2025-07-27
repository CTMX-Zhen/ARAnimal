using UnityEngine;

public class OverlayMenuFollower : MonoBehaviour
{
    public Transform arCameraTransform;
    public Vector3 offset = new Vector3(0, 0, 2f);  // 2 米距离

    void LateUpdate()
    {
        if (arCameraTransform == null) return;

        // 让菜单在相机前方固定位置
        transform.position = arCameraTransform.position + arCameraTransform.forward * offset.z;

        // 始终朝向相机
        transform.rotation = Quaternion.LookRotation(transform.position - arCameraTransform.position);
    }
}
