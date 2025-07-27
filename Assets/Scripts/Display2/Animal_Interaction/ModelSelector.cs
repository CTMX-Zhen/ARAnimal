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

            // 是不是动物模型
            if (!hit.collider.CompareTag("Animal"))
            {
                DeselectModel();
                return;
            }

            // 如果点击的是当前已选中对象（含子物体）→ 忽略
            if (selectedModel != null && IsChildOf(hitObj, selectedModel))
            {
                Debug.Log("🟩 点击的是已选中模型，忽略");
                return;
            }

            SelectModel(hit.collider.gameObject);
        }
        else
        {
            DeselectModel(); // 点空白处取消
        }
    }

    void SelectModel(GameObject model)
    {
        // ✅ 先取消旧选中
        DeselectModel();

        // ✅ 设置新选中
        selectedModel = model;

        if (model.TryGetComponent<ModelTouchInteractor>(out var interactor))
        {
            interactor.isSelected = true;
        }

        // ✅ 创建高亮指示器
        if (indicatorPrefab != null)
        {
            selectionIndicator = Instantiate(indicatorPrefab, model.transform);

            if (model.TryGetComponent<BoxCollider>(out var col))
            {
                var outline = selectionIndicator.AddComponent<SelectionOutline>();
                outline.targetCollider = col;
            }
        }

        // ✅ 展示 InfoPanel
        if (infoPanelController != null)
        {
            infoPanelController.ShowInfo(model.name);
        }
        else
        {
            Debug.LogWarning("⚠️ 未绑定 InfoPanelController");
        }

        Debug.Log("✅ 已选中模型：" + model.name);
    }

    void DeselectModel()
    {
        if (selectedModel != null)
        {
            if (selectedModel.TryGetComponent<ModelTouchInteractor>(out var interactor))
            {
                interactor.isSelected = false;
            }

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
