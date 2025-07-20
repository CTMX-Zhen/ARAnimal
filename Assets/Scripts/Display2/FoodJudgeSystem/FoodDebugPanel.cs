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

    void Update()
    {
        debugText.text =
            $"🍎 Food: {currentFood}\n" +
            $"🦁 Animal: {closestAnimal}\n" +
            $"🎯 In Center: {(inCenter ? "YES" : "NO")}\n" +
            $"🧪 Judged: {(judged ? "YES" : "NO")}\n" +
            $"🔄 Reset: {(resetDone ? "YES" : "NO")}";
    }

    public void LogEvent(string status)
    {
        string line = $"{System.DateTime.Now:HH:mm:ss} | Food: {currentFood}, Animal: {closestAnimal}, Result: {status}";
        File.AppendAllText(logFilePath, line + "\n");
        Debug.Log($"📝 Log written: {line}");
    }
}
