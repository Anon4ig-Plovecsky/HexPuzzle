using LevelsController.TestedModules;
using Sequence = PrimeTween.Sequence;
using CommonScripts.TestedModules;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;
using UI.TestedModules;
using UnityEngine.UI;
using UnityEngine;
using PrimeTween;
using System;
using TMPro;

namespace UI
{
    ///  ButtonsController is responsible for events when interacting with buttons in the game's GUI
    public class ButtonsController : MonoBehaviour
    {
        private float _position;                                /// Position for animation

        [SerializeField] protected GameObject objCalled;        /// Object that caused the click
        [SerializeField] protected GameObject objThisCanvas;    /// Current panel or canvas on which the button is located
        protected Button ButtonThis;                            /// Current button

        private SoundsController _soundsController;

        protected virtual void Start()
        {
            // Creating a Button Click Listener
            ButtonThis = GetComponent<Button>();
            ButtonThis.onClick.AddListener(OnClickButton);
            
            InitPanels();
        }

        /// <summary>
        /// Initializes panels for interaction with them
        /// </summary>
        protected void InitPanels()
        {
            // Getting the current panel GUI
            if(objThisCanvas.IsUnityNull())
                objThisCanvas = GetCurrentPanel();
            if (objThisCanvas.IsUnityNull())
            {
                Debug.Log("Could not find current panel");
                return;
            }
            
            // Getting of the distance from the called panel to the current
            if (objCalled.IsUnityNull())
                return;
            _position = objCalled.transform.localPosition.y;
            // If the current panel is not the main one (it is not located at the zero coordinate),
            // then we take the position value from it
            if (_position < 0.00001)
                _position = objThisCanvas.transform.localPosition.y;
        }
        
        /// <summary>
        /// Activates the specified object and disables the current one, playing an animation
        /// </summary>
        protected void GoToPanel()
        {
            if(objThisCanvas.IsUnityNull())
                return;
            
            // Animations
            if (!objCalled.IsUnityNull())   // If the panel is called, If the panel is called, then two animations
                                            // are triggered in sequence: for displaying and hiding panels
            {
                ChangeAlphaPanel(objCalled.GetComponent<CanvasGroup>(), 0.0f);
                objCalled.SetActive(true);

                Sequence.Create(useUnscaledTime: true)
                    .Group(HidePanel(objThisCanvas))
                    .Group(ShowPanel(objCalled));
            }
            else
                Sequence.Create(HidePanel(objThisCanvas));
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
        protected virtual void OnClickButton()
        {
            if (ButtonThis.IsUnityNull())
                return;
            
            if(ButtonThis.name.Contains(CommonKeys.StrButtonNames.LevelButton))
            {
                ActivateSelectedLevel();
                return;
            }

            switch (ButtonThis.name)
            {
                case CommonKeys.StrButtonNames.StartGame:
                case CommonKeys.StrButtonNames.GoToMenu:
                case CommonKeys.StrButtonNames.MainLevels:
                case CommonKeys.StrButtonNames.BackToMenu:
                case CommonKeys.StrButtonNames.CustomLevel:
                case CommonKeys.StrButtonNames.Results:
                case CommonKeys.StrButtonNames.Settings:
                    GoToPanel();
                    break;
                case CommonKeys.StrButtonNames.ExitToMenu:
                case CommonKeys.StrButtonNames.ExitToMenuWinPanel:
                    ChangeScene(CommonKeys.Names.MainMenu);
                    break;
                case CommonKeys.StrButtonNames.QuitGame:
                    QuitGame();
                    break;
                case CommonKeys.StrButtonNames.LeftArrow:
                case CommonKeys.StrButtonNames.RightArrow:
                    var pageTab = objThisCanvas.GetComponent<IPageTab>();
                    if (pageTab.IsUnityNull())
                    {
                        Debug.Log($"Failed to get PageTab from {objThisCanvas.name}");
                        return;
                    }
                    var arrowDirection = ButtonThis.name.Equals(CommonKeys.StrButtonNames.LeftArrow)
                        ? ArrowDirection.LeftArrow
                        : ArrowDirection.RightArrow;
                    pageTab.NavigationButtonPressed(arrowDirection);
                    break;
                default:
                    Debug.Log("Unknown button detected: " + ButtonThis.name);
                    return;
            }
        }
        
        /// <summary>
        /// Panel display animation
        /// </summary>
        /// <param name="objShow">Object to be shown</param>
        /// <param name="fDuration">Animation duration</param>
        private static Tween ShowPanel(GameObject objShow,  float fDuration = 0.8f)
        {
            return Tween.LocalPositionY(objShow.transform, 0.0f, fDuration, Ease.OutQuad)
                .OnUpdate(target: objShow, (_, tween) =>
                    ChangeAlphaPanel(objShow.GetComponent<CanvasGroup>(), EaseOutQuad(tween.progress)));
        }

        /// <summary>
        /// Panel hidden animation
        /// </summary>
        /// <param name="objHide">Object to be hidden</param>
        /// <param name="fDuration">Animation duration</param>
        private Tween HidePanel(GameObject objHide, float fDuration = 0.8f)
        {
            return Tween.LocalPositionY(objHide.transform, -_position, fDuration, Ease.InQuad)
                .OnUpdate(target: objHide, (obj, tween) => 
                    ChangeAlphaPanel(obj.GetComponent<CanvasGroup>(), 1 - EaseInQuad(tween.progress)))
                .OnComplete(target: this, _ =>
                {
                    // Changing the position of hidden window
                    objHide.SetActive(false);
                    ChangeAlphaPanel(objHide.GetComponent<CanvasGroup>(), 1.0f);
                    
                    // Raise the panel two positions higher from the current one so that the animation works correctly again
                    objHide.transform.localPosition += new Vector3(0.0f, _position * 2.0f, 0.0f);
                    
                    // If the player go to a new panel before the previous animation ends, we will re-enable
                    // the called panel to avoid all panels disappearing
                    objCalled.SetActive(true);
                });
        }

        /// <summary>
        /// Changes the alpha value for each panel element
        /// </summary>
        /// <param name="canvasGroup">The group of objects for which the alpha value will be changed</param>
        /// <param name="fAlpha">The parent object for which the alpha value will be changed, as well as for its parents</param>
        private static void ChangeAlphaPanel(CanvasGroup canvasGroup, float fAlpha)
        {
            if (canvasGroup.IsUnityNull())
                return;

            canvasGroup.alpha = fAlpha;
        }

        /// <summary>
        /// Returns the currently active UI panel
        /// </summary>
        /// <returns>Returns the current panel's GameObject if found, otherwise returns null</returns>
        private GameObject GetCurrentPanel() =>
            ButtonThis.name.Equals(CommonKeys.StrButtonNames.LevelButton)
                ? ButtonThis.transform.parent.parent.parent.gameObject
                : ButtonThis.transform.parent.parent.gameObject;

        /// <summary>
        /// Loads the necessary data for the level into a static class and loads the scene
        /// </summary>
        private void ActivateSelectedLevel()
        {
            var strButtonText = ButtonThis.GetComponentInChildren<TMP_Text>();
            var iLevelNumber = Convert.ToInt32(strButtonText.text);

            if (!LevelParametersMap.LevelInfo.ContainsKey(iLevelNumber))
            {
                Debug.Log("The key is not contained in LevelInfo");
                return;
            }

            var gridSize = LevelParametersMap.LevelInfo[iLevelNumber].GridSize;
            var imageNameList = new List<string>(LevelParametersMap.LevelInfo[iLevelNumber].ImageNameList);
            var sceneName = LevelParametersMap.LevelInfo[iLevelNumber].StrSceneName;
            
            LevelInfoTransfer.SetInstance(iLevelNumber, gridSize, imageNameList, sceneName);
            
            ChangeScene(sceneName);
        }

        private static float EaseOutQuad(float x) =>
            1.0f - MathF.Pow(1.0f - x, 2.0f);

        private static float EaseInQuad(float x) =>
            MathF.Pow(x, 2.0f);
    }
}