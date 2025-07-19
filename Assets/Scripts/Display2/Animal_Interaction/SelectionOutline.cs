using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class SelectionOutline : MonoBehaviour
{
    public BoxCollider targetCollider;

    private LineRenderer lr;

    void Start()
    {
        InitLineRenderer();
        if (targetCollider == null)
        {
            // 尝试从父物体自动获取 BoxCollider（默认就是包裹模型的）
            targetCollider = GetComponentInParent<BoxCollider>();
            if (targetCollider == null)
            {
                Debug.LogWarning("❌ SelectionOutline: 没有找到 BoxCollider，无法绘制边框");
                DestroyImmediate(gameObject);
                return;
            }
        }

        DrawOutline();
    }

    void InitLineRenderer()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;  // ✅ 改为世界坐标
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.green;
        lr.endColor = Color.green;
        lr.loop = false;
    }

    void DrawOutline()
    {
        Vector3 size = targetCollider.size;
        Vector3 center = targetCollider.center;

        Transform tf = targetCollider.transform;

        // 计算世界坐标下的8个顶点
        Vector3[] corners = new Vector3[8];
        int i = 0;
        for (int x = -1; x <= 1; x += 2)
        {
            for (int y = -1; y <= 1; y += 2)
            {
                for (int z = -1; z <= 1; z += 2)
                {
                    Vector3 localCorner = center + Vector3.Scale(size, new Vector3(x, y, z)) * 0.5f;
                    corners[i++] = tf.TransformPoint(localCorner);  // ✅ 转为世界坐标
                }
            }
        }

        // 线框连接顺序（同你之前的）
        Vector3[] lines = new Vector3[]
        {
            corners[0], corners[1], corners[3], corners[2], corners[0], // bottom
            corners[4], corners[5], corners[7], corners[6], corners[4], // top
            corners[1], corners[5], corners[7], corners[3], corners[2], corners[6]
        };

        lr.positionCount = lines.Length;
        lr.SetPositions(lines);
    }

    void Update()
    {
    #if UNITY_EDITOR
        if (!Application.isPlaying) return;
    #endif
        if (targetCollider != null)
        {
            DrawOutline(); // 每帧根据 collider 实时刷新边框
        }
    }

}
