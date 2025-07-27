using UnityEngine;
using Vuforia;

public class Display1Manager : MonoBehaviour
{
    void Start()
    {
        if (!VuforiaBehaviour.Instance.enabled)
        {
            Debug.Log("🔄 Re-enabling Vuforia...");
            VuforiaBehaviour.Instance.enabled = true;
        }

        if (!VuforiaApplication.Instance.IsRunning)
        {
            VuforiaApplication.Instance.Initialize();  // ✅ 重新初始化
        }
    }
}
