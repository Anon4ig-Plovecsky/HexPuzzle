using UnityEngine.SceneManagement;
using UnityEngine;

namespace UI
{
    public class ButtonsController : MonoBehaviour
    {
        /// Types of canvas panels for navigating menus
        private const string StrStartGame = "StartGame";
        private const string StrMainMenu = "MainMenu";
        
        public void GoToPanel(GameObject objPanel)
        {
            switch (objPanel.name)
            {
                case StrStartGame:
                    objPanel.SetActive(true);
                    break;
                case StrMainMenu:
                    
                    break;
                default:
                    Debug.Log("Invalid file name to go to");
                    return;
            }

            gameObject.transform.parent.parent.gameObject.SetActive(false);
        }
        
        public void ChangeScene(string nameOfScene)
        {
            Time.timeScale = 1;
            if (CanvasController.ClassCanvasController != null)
                CanvasController.DestroyClass();
            SceneManager.LoadScene(nameOfScene);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}