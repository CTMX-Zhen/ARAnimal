using UnityEngine;

public class FoodSelector : MonoBehaviour
{
    public GameObject indicatorPrefab;

    private GameObject selectedFood;
    private GameObject selectionIndicator;
    private int foodLayerMask;

    void Start()
    {
        foodLayerMask = LayerMask.GetMask("Food");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleClick(Input.GetTouch(0).position);
        }
    }

    void HandleClick(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, foodLayerMask))
        {
            GameObject hitObj = hit.collider.gameObject;

            // 若点击的是已选中模型或其子对象，则跳过
            if (selectedFood != null && IsChildOf(hitObj, selectedFood))
            {
                Debug.Log("🟩 点击到当前选中食物");
                return;
            }

            if (hitObj.CompareTag("Food"))
            {
                SelectFood(hitObj);
                return;
            }
        }

        DeselectFood();
    }

    void SelectFood(GameObject food)
    {
        DeselectFood();

        FoodTouchInteractor interactor = food.GetComponentInParent<FoodTouchInteractor>();
        if (interactor != null)
        {
            interactor.isSelected = true;
            selectedFood = interactor.gameObject;
        }
        else
        {
            selectedFood = food;
        }

        if (indicatorPrefab != null)
        {
            selectionIndicator = Instantiate(indicatorPrefab, selectedFood.transform);
            BoxCollider col = selectedFood.GetComponent<BoxCollider>();
            if (col != null)
            {
                var outline = selectionIndicator.AddComponent<SelectionOutline>();
                outline.targetCollider = col;
            }
        }

        Debug.Log("✅ 已选中食物：" + selectedFood.name);
    }

    void DeselectFood()
    {
        if (selectedFood != null)
        {
            FoodTouchInteractor interactor = selectedFood.GetComponent<FoodTouchInteractor>();
            if (interactor != null)
            {
                interactor.isSelected = false;
            }
        }

        selectedFood = null;

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
