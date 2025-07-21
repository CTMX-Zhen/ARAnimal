using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToScene1 : MonoBehaviour
{
    public void GoBackToScene1()
    {
        SceneManager.LoadScene("Display1");
    }
}
