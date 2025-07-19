using UnityEngine;

public class EnsureMainCamera : MonoBehaviour
{
    void Awake()
    {
        // 找出当前所有带有 "MainCamera" tag 的对象
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (GameObject cam in cameras)
        {
            if (cam != gameObject)
            {
                cam.tag = "Untagged";
                Debug.Log("⛔ 取消旧的 MainCamera: " + cam.name);
            }
        }

        // 设置当前这个为 MainCamera
        gameObject.tag = "MainCamera";
        Debug.Log("✅ 当前相机已设为 MainCamera: " + gameObject.name);
    }
}
