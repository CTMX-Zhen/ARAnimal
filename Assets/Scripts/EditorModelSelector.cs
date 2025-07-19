using UnityEngine;

public class EditorModelSelector : MonoBehaviour
{
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("🖱️ Mouse Click");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
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
#endif
}
