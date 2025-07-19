// using UnityEngine;
// using TMPro;

// public class InfoPanelController : MonoBehaviour
// {
//     public GameObject infoPanel;
//     public TextMeshProUGUI nameText;
//     public TextMeshProUGUI dietText;
//     public InfoPanelFollower panelFollower;

//     void Start()
//     {
//         infoPanel.SetActive(false);  // 启动时隐藏
//     }

//     public void ShowInfo(string animalName, Transform modelTransform)
//     {
//         string diet = GetDietType(animalName);
//         nameText.text = $"Name: {animalName}";
//         dietText.text = $"Diet: {diet}";

//         panelFollower.target = modelTransform;  // 设置跟随目标
//         infoPanel.SetActive(true);
//     }

//     public void HideInfo()
//     {
//         infoPanel.SetActive(false);
//     }

//     private string GetDietType(string name)
//     {
//         switch (name.ToLower())
//         {
//             case "rabbit": return "Herbivore";
//             case "elephant": return "Herbivore";
//             case "eagle": return "Carnivore";
//             case "snake": return "Carnivore";
//             case "chicken": return "Omnivore";
//             case "monkey": return "Omnivore";
//             default: return "Unknown";
//         }
//     }
// }
