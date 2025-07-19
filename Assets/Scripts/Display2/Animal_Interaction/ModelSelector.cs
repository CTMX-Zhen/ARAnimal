using UnityEngine;

public class ModelSelector : MonoBehaviour
{
    private GameObject selectedModel;
    private GameObject selectionIndicator;

    public GameObject indicatorPrefab;

    public InfoPanelController infoPanelController;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            TrySelect(ray);
        }
#endif

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPos = Input.GetTouch(0).position;
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            TrySelect(ray);
        }
    }

    void TrySelect(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObj = hit.collider.gameObject;

            // ✅ 如果点到当前已选中的模型 → 忽略，不取消
            if (selectedModel != null && IsChildOf(hitObj, selectedModel))
            {
                Debug.Log("🟩 点击的是选中模型或其子物体（如 greenbox），不取消");
                return;
            }

            // ✅ 点到新模型 → 切换选中
            if (hit.collider.CompareTag("Animal"))
            {
                SelectModel(hit.collider.gameObject);
                return;
            }
        }

        // ✅ 其他情况（空白或非动物物体）→ 取消选中
        DeselectModel();
    }

    void SelectModel(GameObject model)
    {
        DeselectModel();

        selectedModel = model;
        selectedModel.GetComponent<ModelTouchInteractor>().isSelected = true;

        if (indicatorPrefab != null)
        {
            selectionIndicator = Instantiate(indicatorPrefab, model.transform);
            BoxCollider col = model.GetComponent<BoxCollider>();
            if (col)
            {
                var outline = selectionIndicator.AddComponent<SelectionOutline>();
                outline.targetCollider = col;
            }
        }

        // 🌟 显示 InfoPanel
        if (infoPanelController != null)
        {
            infoPanelController.ShowInfo(model.name);
            // infoPanelController.ShowInfo(model.name, model.transform);
        }
        else
        {
            Debug.LogWarning("⚠️ 未找到 InfoPanelController");
        }

        Debug.Log("✅ 选中模型：" + model.name);
    }

    void DeselectModel()
    {
        if (selectedModel != null)
        {
            selectedModel.GetComponent<ModelTouchInteractor>().isSelected = false;
            selectedModel = null;
        }

        if (selectionIndicator != null)
        {
            Destroy(selectionIndicator);
            selectionIndicator = null;
        }

        if (infoPanelController != null)
        {
            infoPanelController.HideInfo();
        }
    }

    bool IsChildOf(GameObject obj, GameObject parent)
    {
        Transform t = obj.transform;
        while (t != null)
        {
            if (t == parent.transform) return true;
            t = t.parent;
        }
        return false;
    }
}
