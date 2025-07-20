using UnityEngine;

public class FoodPanelPositioner : MonoBehaviour
{
    public RectTransform foodPanel;

    void Start()
    {
        if (foodPanel == null) return;

        // 固定到底部中央
        foodPanel.anchorMin = new Vector2(0.5f, 0f);
        foodPanel.anchorMax = new Vector2(0.5f, 0f);
        foodPanel.pivot = new Vector2(0.5f, 0f);
        foodPanel.anchoredPosition = new Vector2(0f, 50f); // 向上偏移 50，避免贴边
    }
}
