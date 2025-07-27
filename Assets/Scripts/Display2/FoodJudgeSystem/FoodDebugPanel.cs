using TMPro;
using UnityEngine;
using System.IO;

public class FoodDebugPanel : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    [Header("由 FoodJudgeSystem 调用更新")]
    public string currentFood = "";
    public string closestAnimal = "";
    public bool inCenter = false;
    public bool judged = false;
    public bool resetDone = false;

    private string logFilePath;

    void Start()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "debug_log.txt");

        // 第一次清空旧内容
        File.WriteAllText(logFilePath, $"[Session Start] {System.DateTime.Now}\n");
    }
}
