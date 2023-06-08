using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    public void ChangeScene(string nameOfScene)
    {
        Time.timeScale = 1;
        if (CanvasController.classCanvasController != null)
            CanvasController.DestroyClass();
        SceneManager.LoadScene(nameOfScene);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
