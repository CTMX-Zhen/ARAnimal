using UnityEngine;

public class FoodTouchInteractor : MonoBehaviour
{
    public bool isSelected = false;
    
    public float dragSpeed = 0.0005f;

    public float rotationSpeed = 0.02f;

    private Vector2 prevTouch1, prevTouch2;
    private Vector3 originalLocalPosition;
    private bool initialized = false;
    private float lastTapTime = 0f;
    private float doubleTapThreshold = 0.3f;
    private Quaternion originalLocalRotation;

    void Start()
    {
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        initialized = true;
    }

    void Update()
    {
        if (!isSelected || !initialized) return;

#if UNITY_EDITOR
        // 鼠标拖动（仅用于编辑器调试）
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
            transform.Translate(delta * dragSpeed * 100f, Space.World);
        }
#endif

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                // 检查是否点击到自己，处理双击复位
                Ray ray = Camera.main.ScreenPointToRay(t.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (IsHitSelfOrChild(hit.collider.gameObject))
                    {
                        float dt = Time.time - lastTapTime;
                        if (dt <= doubleTapThreshold)
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
        Debug.Log($"🔄 [{name}] 已回到原始位置 {originalLocalPosition}");
    }

    bool IsHitSelfOrChild(GameObject obj)
    {
        return obj.transform.IsChildOf(transform);
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