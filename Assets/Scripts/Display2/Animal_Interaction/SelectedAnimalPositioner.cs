using UnityEngine;

public class SelectedAnimalPositioner : MonoBehaviour
{
    void Start()
    {
        string selectedName = SceneStateManager.SelectedAnimal;
        if (string.IsNullOrEmpty(selectedName)) return;

        GameObject selectedAnimal = GameObject.Find(selectedName);
        if (selectedAnimal != null)
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("❌ 没有找到 MainCamera");
                return;
            }

            Vector3 spawnPos = cam.transform.position + cam.transform.forward * 5.0f + Vector3.up * 0.3f;
            selectedAnimal.transform.position = spawnPos;
            selectedAnimal.transform.rotation = Quaternion.LookRotation(-cam.transform.forward);

            Debug.Log($"✅ 设置 {selectedName} 在相机前方: {spawnPos}");
        }
        else
        {
            Debug.LogWarning("⚠️ 找不到选中的动物：" + selectedName);
        }

        // 启用食物面板
        GameObject foodPanel = GameObject.Find("FoodPanel");
        if (foodPanel != null) foodPanel.SetActive(true);
    }
}
