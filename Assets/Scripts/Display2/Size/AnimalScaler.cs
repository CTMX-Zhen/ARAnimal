using UnityEngine;

public class AnimalScaler : MonoBehaviour
{
    public GameObject currentAnimal; // 当前显示的动物模型
    public float scaleStep = 1f;
    public float minScale = 1f;
    public float maxScale = 10f;

    void Start()
    {
        string animalName = SceneStateManager.SelectedAnimal;
        if (!string.IsNullOrEmpty(animalName))
        {
            // 在 3DModels 下找名字包含 animalName 的物体
            Transform modelsRoot = GameObject.Find("3DModels")?.transform;
            if (modelsRoot != null)
            {
                foreach (Transform animal in modelsRoot)
                {
                    if (animal.name.ToLower().Contains(animalName.ToLower()))
                    {
                        currentAnimal = animal.gameObject;
                        Debug.Log($"✅ 找到动物模型: {animal.name}");
                        break;
                    }
                }
            }

            if (currentAnimal == null)
            {
                Debug.LogWarning($"❌ 没有在 3DModels 下找到包含名字 {animalName} 的动物");
            }
        }
    }

    public void LargerAnimal()
    {
        if (currentAnimal == null) return;

        Camera cam = Camera.main;
        Vector3 direction = (currentAnimal.transform.position - cam.transform.position).normalized;

        // Move toward camera
        Vector3 newPos = currentAnimal.transform.position - direction * scaleStep;
        float distance = Vector3.Distance(cam.transform.position, newPos);

        if (distance >= minScale)  // min distance to camera
        {
            currentAnimal.transform.position = newPos;
            Debug.Log($"📍 Closer: {distance:F2} units");
        }
    }

    public void SmallerAnimal()
    {
        if (currentAnimal == null) return;

        Camera cam = Camera.main;
        Vector3 direction = (currentAnimal.transform.position - cam.transform.position).normalized;

        // Move away from camera
        Vector3 newPos = currentAnimal.transform.position + direction * scaleStep;
        float distance = Vector3.Distance(cam.transform.position, newPos);

        if (distance <= maxScale)  // max distance allowed
        {
            currentAnimal.transform.position = newPos;
            Debug.Log($"📍 Farther: {distance:F2} units");
        }
    }
}
