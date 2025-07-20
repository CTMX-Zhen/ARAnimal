using UnityEngine;
using System.Collections.Generic;

public class FoodJudgeSystem : MonoBehaviour
{
    [Header("音效设置")]
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip meatEatingSound;
    public AudioClip vegeEatingSound;
    public AudioSource audioSource;

    [Header("判定参数")]
    public float centerThreshold = 0.1f;

    private Camera mainCam;
    private Dictionary<string, string> animalDiets;
    private HashSet<string> meatFoods;
    private HashSet<string> vegeFoods;
    private FoodDebugPanel debugPanel;


    void Start()
    {
        mainCam = Camera.main;

        debugPanel = FindObjectOfType<FoodDebugPanel>();

        animalDiets = new Dictionary<string, string>
        {
            {"rabbit", "Herbivore"},
            {"elephant", "Herbivore"},
            {"eagle", "Carnivore"},
            {"snake", "Carnivore"},
            {"chicken", "Omnivore"},
            {"monkey", "Omnivore"}
        };

        meatFoods = new HashSet<string> {
            "bird", "duck", "frog", "grasshoper", "meat", "mouse", "sheep", "spider", "worm", "fish"
        };

        vegeFoods = new HashSet<string> {
            "apple", "banana", "broccoli", "cabbage", "carrot", "celery", "corn", "pumpkin",
            "sweet potato", "tomato", "watermelon", "asparagus"
        };
    }

    void Update()
    {
        foreach (var interactor in FindObjectsOfType<FoodTouchInteractor>())
        {
            if (!interactor.isSelected) continue;

            Vector3 viewPos = mainCam.WorldToViewportPoint(interactor.transform.position);
            float dx = Mathf.Abs(viewPos.x - 0.5f);
            float dy = Mathf.Abs(viewPos.y - 0.5f);

            if (dx < centerThreshold && dy < centerThreshold)
            {
                JudgeFood(interactor);
                break;
            }
            if (debugPanel != null)
            {
                debugPanel.currentFood = interactor.name.ToLower();
                debugPanel.inCenter = (dx < centerThreshold && dy < centerThreshold);
                debugPanel.judged = false;
                debugPanel.resetDone = false;
            }
        }
    }

    void JudgeFood(FoodTouchInteractor food)
    {
        food.isSelected = false;

        string foodName = food.name.ToLower();
        string animalName = SceneStateManager.SelectedAnimal.ToLower();  // ✅ 使用全局动物名

        if (!animalDiets.ContainsKey(animalName))
        {
            Debug.LogWarning($"⚠️ 无法找到动物：{animalName}");
            return;
        }

        string dietType = animalDiets[animalName];

        bool isMeat = meatFoods.Contains(foodName);
        bool isVege = vegeFoods.Contains(foodName);
        bool isCorrect = false;

        if (dietType == "Carnivore") isCorrect = isMeat;
        else if (dietType == "Herbivore") isCorrect = isVege;
        else if (dietType == "Omnivore") isCorrect = isMeat || isVege;

        Debug.Log($"🧪 [{foodName}] 给 [{animalName}] => {dietType} => {(isCorrect ? "✅正确" : "❌错误")}");

        PlaySound(isCorrect, isMeat, isVege, dietType);
        food.SendMessage("ResetModel", SendMessageOptions.DontRequireReceiver);

        if (debugPanel != null)
        {
            debugPanel.closestAnimal = animalName;
            debugPanel.judged = true;

            string status = isCorrect ? "CORRECT" : "WRONG";
            debugPanel.LogEvent(status);
        }
    }


    void PlaySound(bool correct, bool isMeat, bool isVege, string dietType)
    {
        if (audioSource == null) return;

        audioSource.Stop(); // 强制停止上一个音效

        if (correct)
        {
            if (dietType == "Carnivore" && isMeat && meatEatingSound != null)
            {
                audioSource.clip = meatEatingSound;
            }
            else if (dietType == "Herbivore" && isVege && vegeEatingSound != null)
            {
                audioSource.clip = vegeEatingSound;
            }
            else if (correctSound != null)
            {
                audioSource.clip = correctSound;
            }
        }
        else
        {
            if (wrongSound != null)
            {
                audioSource.clip = wrongSound;
            }
        }

        // 播放（确保 clip 已设置）
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    string GetClosestAnimalName()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("Animal"))
        {
            if (obj.activeInHierarchy) return obj.name.ToLower();
        }
        return "unknown";
    }
}
