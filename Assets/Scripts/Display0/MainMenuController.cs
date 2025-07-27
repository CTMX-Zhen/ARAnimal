using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnPlayClicked()
    {
        SceneManager.LoadScene("Display1");
    }

    public void OnHowClicked()
    {
        SceneManager.LoadScene("Display0-1");
    }
}
