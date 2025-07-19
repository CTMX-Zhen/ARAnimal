using UnityEngine;

public class InfoPanelFollower : MonoBehaviour
{
    public Transform target;         // 模型 Transform，例如 Eagle
    public Vector3 offset = new Vector3(0, 1.5f, 0);  // 在模型头上显示

    public Camera cam;               // 相机
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

        // 世界位置 → 屏幕位置
        Vector3 worldPos = target.position + offset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        // 屏幕坐标转到 UI Canvas 内部
        rectTransform.position = worldPos;
    }
}
