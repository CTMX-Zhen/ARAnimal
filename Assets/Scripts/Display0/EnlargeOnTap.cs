using UnityEngine;

public class EnlargeOnTap : MonoBehaviour
{
    private bool isEnlarged = false;
    private Vector3 originalLocalPosition;
    private Vector3 originalLocalScale;
    private Quaternion originalLocalRotation;

    public Transform arCamera;
    public float enlargeDistance = 600f; // 放大距离（单位为本地单位，依据你的比例缩放）
    public float enlargeScale = 1.3f;
    public float lerpSpeed = 5f;

    private bool isAnimating = false;
    private float t = 0f;

    void Start()
    {
        originalLocalPosition = transform.localPosition;
        originalLocalScale = transform.localScale;
        originalLocalRotation = transform.localRotation;
    }

    void Update()
    {
        if (isAnimating)
        {
            t += Time.deltaTime * lerpSpeed;

            if (isEnlarged)
            {
                // 移动到相机前方的位置（世界坐标 → 转为父物体的局部坐标）
                Vector3 worldTargetPos = arCamera.position + arCamera.forward * enlargeDistance;
                Vector3 localTargetPos = transform.parent.InverseTransformPoint(worldTargetPos);

                transform.localPosition = Vector3.Lerp(transform.localPosition, localTargetPos, t);
                transform.localScale = Vector3.Lerp(transform.localScale, originalLocalScale * enlargeScale, t);

                // 保持 Y轴 -90 角度，只修改方向朝向相机
                Quaternion targetRot = Quaternion.LookRotation(arCamera.forward);
                Vector3 euler = targetRot.eulerAngles;
                euler.y = -90f;  // 保持你原本的 -90 y 角度
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(euler), t);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPosition, t);
                transform.localScale = Vector3.Lerp(transform.localScale, originalLocalScale, t);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, originalLocalRotation, t);
            }

            if (t >= 1f)
            {
                isAnimating = false;
                t = 0f;
            }
        }
    }

    void OnMouseDown()
    {
        isEnlarged = !isEnlarged;
        isAnimating = true;
        t = 0f;
    }
}
