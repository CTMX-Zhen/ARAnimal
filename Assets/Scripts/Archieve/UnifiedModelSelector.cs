using UnityEngine;

public class UnifiedModelSelector : MonoBehaviour
{
    private GameObject selectedObject;
    private GameObject selectionIndicator;

    public GameObject indicatorPrefab;

    // 仅检测带有这两个 Layer 的物体（如需更严谨，可做 Layer + Tag 配对）
    private int layerMask;

    void Start()
    {
        layerMask = LayerMask.GetMask("Animal", "Food"); // 只检测这两类
    }

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
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            GameObject hitObj = hit.collider.gameObject;

            if (selectedObject != null && IsChildOf(hitObj, selectedObject))
            {
                Debug.Log("🟩 点到当前选中的物体或其子物体，保持选中");
                return;
            }

            string tag = hit.collider.tag;
            if (tag == "Animal" || tag == "Food")
            {
                SelectObject(hit.collider.gameObject);
                return;
            }
        }

        DeselectObject();
    }

    void SelectObject(GameObject obj)
    {
        DeselectObject();

        selectedObject = obj;

        // 设置 isSelected 状态
        var animalScript = obj.GetComponent<ModelTouchInteractor>();
        if (animalScript != null)
            animalScript.isSelected = true;

        var foodScript = obj.GetComponent<FoodTouchInteractor>();
        if (foodScript != null)
            foodScript.isSelected = true;

        if (indicatorPrefab != null)
        {
            selectionIndicator = Instantiate(indicatorPrefab, obj.transform);
            BoxCollider col = obj.GetComponent<BoxCollider>();
            if (col == null)
            {
                Debug.LogWarning("❌ 缺少 BoxCollider 无法绘制边框: " + obj.name);
            }
            else
            {
                Debug.Log("🟢 准备绘制边框给: " + obj.name);
                var outline = selectionIndicator.AddComponent<SelectionOutline>();
                outline.targetCollider = col;
            }
        }

        Debug.Log("✅ 选中模型：" + obj.name);
    }

    void DeselectObject()
    {
        if (selectedObject != null)
        {
            var animalScript = selectedObject.GetComponent<ModelTouchInteractor>();
            if (animalScript != null) animalScript.isSelected = false;

            var foodScript = selectedObject.GetComponent<FoodTouchInteractor>();
            if (foodScript != null) foodScript.isSelected = false;

            selectedObject = null;
        }

        if (selectionIndicator != null)
        {
            Destroy(selectionIndicator);
            selectionIndicator = null;
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
