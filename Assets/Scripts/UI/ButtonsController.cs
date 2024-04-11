using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    ///  ButtonsController is responsible for events when interacting with buttons in the game's GUI
    public class ButtonsController : MonoBehaviour
    {
        /// Types of canvas panels for navigating menus
        private const string StrStartGame = "StartGame";
        private const string StrMainMenu = "MainMenu";

        [SerializeField] private GameObject objCalled;      /// Object that caused the click
        private Button _btnThis;                            /// Current button

        private void Start()
        {
            _btnThis = GetComponent<Button>();
            _btnThis.onClick.AddListener(OnClickButton);
        }
        
        /// <summary>
        /// Activates the specified object and disables the current one, playing an animation
        /// </summary>
        private void GoToPanel()
        {
            switch (objCalled.name)
            {
                case StrStartGame:
                    objCalled.SetActive(true);
                    break;
                case StrMainMenu:
                    
                    break;
                default:
                    Debug.Log("Invalid file name to go to");
                    return;
            }

            gameObject.transform.parent.parent.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Changes the scene and returns game time
        /// </summary>
        /// <param name="strSceneName">Game scene name</param>
        private void ChangeScene(string strSceneName)
        {
            Time.timeScale = 1;
            if (CanvasController.ClassCanvasController != null)
                CanvasController.DestroyClass();
            SceneManager.LoadScene(strSceneName);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        /// Listener triggered when a button is clicked in the UI
        public void OnClickButton()
        {
            if (_btnThis.IsUnityNull())
                return;
            
            switch (_btnThis.name)
            {
                case CommonKeys.StrButtonNames.StartGame:
                    GoToPanel();
                    break;
                case CommonKeys.StrButtonNames.ExitToMenu:
                case CommonKeys.StrButtonNames.ExitToMenuWinPanel:
                    ChangeScene(StrMainMenu);
                    break;
                case CommonKeys.StrButtonNames.QuitGame:
                    QuitGame();
                    break;
                default:
                    Debug.Log("Unknown button detected: " + _btnThis.name);
                    return;
            }
        }
    }
}