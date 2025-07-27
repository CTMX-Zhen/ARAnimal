using UnityEngine;

public class ModelTouchInteractor : MonoBehaviour
{
    public bool isSelected = false;

    private Vector2 prevTouch1, prevTouch2;

    [Header("拖动（单指）")]
    public float dragSpeed = 0.005f;

    [Header("旋转（双指）")]
    public float rotationSpeed = 0.2f;

    private float lastTapTime = 0f;
    private float doubleTapThreshold = 0.3f;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    private Vector3 originalLocalScale;

    void Start()
    {
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        originalLocalScale = transform.localScale;
    }

    void Update()
    {
        if (!isSelected) return;

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                // ✅ 检查点击是否在模型或它的 LineRenderer 区域
                Ray ray = Camera.main.ScreenPointToRay(t.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (IsHitSelfOrIndicator(hit.collider.gameObject))
                    {
                        float timeSinceLastTap = Time.time - lastTapTime;
                        if (timeSinceLastTap <= doubleTapThreshold)
                        {
                            ResetModel();
                        }
                        lastTapTime = Time.time;
                    }
                }
            }

            if (t.phase == TouchPhase.Moved)
            {
                Vector2 delta = t.deltaPosition;
                Vector3 move = new Vector3(delta.x, delta.y, 0f) * dragSpeed;
                transform.Translate(move, Space.World);

                // 🌟 限制在相机视野内
                ClampToCameraView();
            }
            return;
        }

        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                prevTouch1 = t1.position;
                prevTouch2 = t2.position;
                return;
            }

            Vector2 currTouch1 = t1.position;
            Vector2 currTouch2 = t2.position;

            Vector2 prevMid = (prevTouch1 + prevTouch2) / 2f;
            Vector2 currMid = (currTouch1 + currTouch2) / 2f;
            Vector2 dragDelta = currMid - prevMid;

            float rotateY = -dragDelta.x * rotationSpeed;
            float rotateX = dragDelta.y * rotationSpeed;

            transform.Rotate(Vector3.up, rotateY, Space.World);
            transform.Rotate(Vector3.right, rotateX, Space.World);

            prevTouch1 = currTouch1;
            prevTouch2 = currTouch2;
        }
    }

    void ResetModel()
    {
        transform.localPosition = originalLocalPosition;
        transform.localRotation = originalLocalRotation;
        transform.localScale = originalLocalScale;
        Debug.Log("🔄 模型已重置为原始状态");
    }

    bool IsHitSelfOrIndicator(GameObject hitObject)
    {
        if (hitObject == this.gameObject) return true;

        Transform parent = hitObject.transform;
        while (parent != null)
        {
            if (parent == this.transform) return true;
            parent = parent.parent;
        }
        return false;
    }

    void ClampToCameraView()
    {
        Camera cam = Camera.main;
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);

        float padding = 100f; // 可选：避免贴边太紧

        screenPos.x = Mathf.Clamp(screenPos.x, padding, Screen.width - padding);
        screenPos.y = Mathf.Clamp(screenPos.y, padding, Screen.height - padding);

        Vector3 clampedWorldPos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, screenPos.z));
        transform.position = clampedWorldPos;
    }
}
