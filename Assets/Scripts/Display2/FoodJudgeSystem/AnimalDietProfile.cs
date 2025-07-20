using System.Collections.Generic;
using UnityEngine;

public enum DietType
{
    Herbivore,
    Carnivore,
    Omnivore
}

public class AnimalDietProfile : MonoBehaviour
{
    public DietType dietType;

    [Tooltip("这只动物可以吃的肉类（名称小写）")]
    public List<string> acceptableMeatFoods = new List<string>();

    [Tooltip("这只动物可以吃的蔬菜类（名称小写）")]
    public List<string> acceptableVegeFoods = new List<string>();

    // 默认肉类食物
    private static readonly List<string> defaultMeatFoods = new List<string> {
        "bird", "duck", "frog", "grasshoper", "meat", "mouse", "sheep", "spider", "worm", "fish"
    };

    // 默认蔬菜类食物
    private static readonly List<string> defaultVegeFoods = new List<string> {
        "apple", "banana", "broccoli", "cabbage", "carrot", "celery", "corn",
        "pumpkin", "sweet potato", "tomato", "watermelon", "asparagus"
    };

#if UNITY_EDITOR
    private void Reset()
    {
        // 💡 只在第一次添加脚本时触发一次
        switch (dietType)
        {
            case DietType.Herbivore:
                acceptableMeatFoods.Clear();
                acceptableVegeFoods = new List<string>(defaultVegeFoods);
                break;

            case DietType.Carnivore:
                acceptableMeatFoods = new List<string>(defaultMeatFoods);
                acceptableVegeFoods.Clear();
                break;

            case DietType.Omnivore:
                acceptableMeatFoods = new List<string>(defaultMeatFoods);
                acceptableVegeFoods = new List<string>(defaultVegeFoods);
                break;
        }
    }
#endif
}
