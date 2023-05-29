using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    public void ChangeScene(string nameOfScene)
    {
        SceneManager.LoadScene(nameOfScene);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
