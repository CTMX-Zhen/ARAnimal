using UnityEngine;

public class EditorModelSelector : MonoBehaviour
{
#if UNITY_EDITOR
    public float scrollSpeed = 0.1f;

    private void Update()
    {
        HandleClick();
        HandleScrollMovement();
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("🖱️ Mouse Click");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("🎯 Raycast hit: " + hit.collider.name);

                if (hit.collider.CompareTag("Animal"))
                {
                    Debug.Log("✅ 模型点击成功: " + hit.collider.name);
                }
                else
                {
                    Debug.Log("❌ 点到的不是 Animal");
                }
            }
            else
            {
                Debug.Log("📭 没有打到任何物体");
            }
        }
    }

    void HandleScrollMovement()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            // 默认 X 轴，按住 Shift 时移动 Y 轴
            Vector2 moveDir = Input.GetKey(KeyCode.LeftShift) ? Vector2.up : Vector2.right;
            Vector3 move = new Vector3(moveDir.x, moveDir.y, 0f) * scroll * scrollSpeed;

            transform.position += move;

            Debug.Log($"📦 滚轮移动：{move}，当前位置：{transform.position}");
        }
    }
#endif
}
