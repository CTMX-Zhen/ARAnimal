using UnityEngine;

public class CanvasFollower : MonoBehaviour
{
    public float offsetDistance = 0.5f;  // 距离模型前方多少
    public Vector3 panelOffset = new Vector3(0, 0.5f, 0);  // 微调上移
    public Camera cam;

    void Start()
    {
        string selectedName = SceneStateManager.SelectedAnimal;
        GameObject target = GameObject.Find(selectedName);
        if (target == null || cam == null)
        {
            Debug.LogWarning("❗ 找不到动物或相机！");
            return;
        }

        // 获取动物前方朝向（朝向相机方向）
        Vector3 toCamera = (cam.transform.position - target.transform.position).normalized;

        // 设置 Canvas 到动物前方 offsetDistance，并稍微上移
        transform.position = target.transform.position + toCamera * offsetDistance + panelOffset;

        // 让 Canvas 朝向相机
        transform.LookAt(cam.transform);
        transform.forward = -transform.forward; // 翻面，否则会背对相机

        // 可选：缩小整体大小
        transform.localScale = Vector3.one * 0.002f;

        // 对齐 InfoPanel 到左上角
        AlignInfoPanel();
    }

    void AlignInfoPanel()
    {
        RectTransform rt = GetComponentInChildren<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = new Vector2(20, -20);
        }
    }
}
