using UnityEngine;

public class AnimalCameraAligner : MonoBehaviour
{
    public float distance = 2.5f;  // 相机距离
    public float height = 1.0f;    // 相机高度偏移
    public Vector3 lookOffset = Vector3.up * 0.5f; // 看向动物中间

    void Start()
    {
        string selectedName = SceneStateManager.SelectedAnimal;
        GameObject target = GameObject.Find(selectedName);

        if (target != null)
        {
            // 设置相机位置（后上方）
            Vector3 camDirection = new Vector3(0, height, -distance);
            transform.position = target.transform.position + camDirection;

            // 相机朝向目标 + 偏移
            transform.LookAt(target.transform.position + lookOffset);
            Debug.Log($"📷 相机对准 {selectedName}，位置: {transform.position}");
        }
        else
        {
            Debug.LogWarning("⚠️ 找不到模型：" + selectedName);
        }
    }
}
