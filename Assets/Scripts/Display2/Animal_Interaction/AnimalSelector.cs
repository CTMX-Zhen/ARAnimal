using UnityEngine;

public class AnimalSelector : MonoBehaviour
{
    public GameObject[] animals;  // 在 Inspector 中拖入所有动物模型

    void Start()
    {
        string selected = SceneStateManager.SelectedAnimal.ToLower();

        foreach (GameObject animal in animals)
        {
            if (animal.name.ToLower() == selected)
            {
                animal.SetActive(true);
                Debug.Log("✅ Showing: " + animal.name);
            }
            else
            {
                animal.SetActive(false);
            }
        }

        // 清理状态（如果你希望后续重启时干净）
        SceneStateManager.IsScene2Active = false;
        SceneStateManager.IsVuforiaStopped = false;
    }
}
