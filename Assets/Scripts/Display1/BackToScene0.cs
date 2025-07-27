using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToScene0 : MonoBehaviour
{
    public void GoBackToScene1()
    {
        SceneManager.LoadScene("Display0");
    }
}
