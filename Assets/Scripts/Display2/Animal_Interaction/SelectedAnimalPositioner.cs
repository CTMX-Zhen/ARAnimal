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
            selectedAnimal.transform.position = Vector3.zero;
            selectedAnimal.transform.rotation = Quaternion.identity;  // 可选：旋转归零
            Debug.Log($"✅ 设置 {selectedName} 到原点");
        }
        else
        {
            Debug.LogWarning("⚠️ 找不到选中的动物：" + selectedName);
        }
    }
}
