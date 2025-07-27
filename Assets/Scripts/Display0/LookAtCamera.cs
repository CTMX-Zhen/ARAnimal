using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public bool onlyRotateY = true;

    void Update()
    {
        if (cameraTransform == null) return;

        Vector3 targetPos = cameraTransform.position;

        if (onlyRotateY)
        {
            // 只在Y轴旋转：保留当前X和Z，改变Y朝向相机
            Vector3 lookDirection = new Vector3(
                targetPos.x,
                transform.position.y,  // 保持本地Y
                targetPos.z
            );
            transform.LookAt(lookDirection);
        }
        else
        {
            // 全3D方向朝向相机（通常不推荐教学板用）
            transform.LookAt(targetPos);
        }
    }
}
