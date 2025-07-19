using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class ButtonClickHandler : MonoBehaviour
{
    private Renderer cubeRenderer;
    private Color originalColor = Color.white;
    private Color toggledColor = Color.green;
    private bool isToggled = false;

    public string animalName;  // 从 Inspector 设置

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = originalColor;
        }
    }

    public void OnCubeClicked()
    {
        isToggled = !isToggled;

        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = isToggled ? toggledColor : originalColor;
        }

        // 记录选中的动物名
        SceneStateManager.SelectedAnimal = animalName;
        SceneStateManager.IsScene2Active = true;

        Debug.Log("🟩 Selected animal: " + animalName);

        // 停用 Vuforia 行为
        if (VuforiaBehaviour.Instance != null)
        {
            VuforiaBehaviour.Instance.enabled = false;
            Debug.Log("📴 VuforiaBehaviour disabled");
        }

        if (VuforiaApplication.Instance != null)
        {
            VuforiaApplication.Instance.Deinit();  // 可选但推荐
            Debug.Log("📴 VuforiaApplication deinitialized");
        }

        // 删除 MainCamera 的 tag
        var arCam = GameObject.FindGameObjectWithTag("MainCamera");
        if (arCam != null)
        {
            arCam.tag = "Untagged";
        }

        // 延迟加载
        Invoke(nameof(LoadScene), 0.5f);
    }

    void LoadScene()
    {
        Debug.Log("🚀 Loading Scene2");
        SceneManager.LoadScene("Display2");
    }
}
