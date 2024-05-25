using UnityEngine.ResourceManagement.AsyncOperations;
using LevelsController.TestedModules;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using UI.TestedModules;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// Class responsible for behavior between elements in the CustomLevelPanel
    /// </summary>
    public class CustomLevelController : MonoBehaviour
    {
        public static string SelectedImage = "";
        private Sprite _timeImageDefault;
        private Sprite _timeImageDisabled;
        
        // Pair with arrow buttons that control the timer, the second item maintains the position of the button
        // until the toggleButton is turned off
        private readonly Tuple<Button, bool>[] _pairButtonStages = new Tuple<Button, bool>[4];
        private readonly Image[] _arrTimeImages = new Image[2];

        // UI Elements
        private Toggle _toggleTimer;
        private TMP_Text _textMinutes;
        private TMP_Text _textSeconds;
        private Button _buttonStartGame;
        private TMP_Dropdown _dropdownDifficult;
        private TMP_Text _chooseImageButtonText;
        
        private void Start()
        {
            gameObject.SetActive(false);

            _chooseImageButtonText =
                CommonKeys.GetComponentFromTransformOfType<TMP_Text>(transform, CommonKeys.Names.ChooseImageButtonText);
            if (_chooseImageButtonText.IsUnityNull())
                return;

            _buttonStartGame =
                CommonKeys.GetComponentFromTransformOfType<Button>(transform, CommonKeys.Names.StartLevelButton);
            if (_buttonStartGame.IsUnityNull())
                return;

            _toggleTimer = CommonKeys.GetComponentFromTransformOfType<Toggle>(transform, CommonKeys.Names.TimerToggle);
            if (_toggleTimer.IsUnityNull())
                return;

            _dropdownDifficult = CommonKeys.GetComponentFromTransformOfType<TMP_Dropdown>(transform,
                CommonKeys.Names.DropdownDifficult);
            if (_dropdownDifficult.IsUnityNull())
                return;

            _textMinutes = CommonKeys.GetComponentFromTransformOfType<TMP_Text>(transform, 
                CommonKeys.Names.MinuteTextImage + "/MinuteText");
            if (_textMinutes.IsUnityNull())
                return;
            
            _textSeconds = CommonKeys.GetComponentFromTransformOfType<TMP_Text>(transform,
                CommonKeys.Names.SecondTextImage + "/SecondText");
            if (_textSeconds.IsUnityNull())
                return;

            // Getting TimerButtons
            var arrStrButtonNames = new[]
            {
                CommonKeys.Names.MinuteUpButton,
                CommonKeys.Names.MinuteDownButton,
                CommonKeys.Names.SecondUpButton,
                CommonKeys.Names.SecondDownButton
            };
            for (var i = 0; i < arrStrButtonNames.Length; i++)
            {
                var button = CommonKeys.GetComponentFromTransformOfType<Button>(transform, arrStrButtonNames[i]);
                if (button.IsUnityNull())
                    return;
                _pairButtonStages[i] = new Tuple<Button, bool>(button, button.interactable);
            }
            
            // Getting TimeImages
            var arrStrImageNames = new[]
            {
                CommonKeys.Names.MinuteTextImage,
                CommonKeys.Names.SecondTextImage
            };
            for (var i = 0; i < _arrTimeImages.Length; i++)
            {
                _arrTimeImages[i] = CommonKeys.GetComponentFromTransformOfType<Image>(transform, arrStrImageNames[i]);
                if(_arrTimeImages[i].IsUnityNull())
                    return;
            }

            // Getting sprites for timeImage
            var asyncOperationHandleImageDisabled = Addressables.LoadAssetAsync<Sprite>(CommonKeys.Addressable.MainLevelDisabled);
            asyncOperationHandleImageDisabled.Completed += delegate
            {
                if (asyncOperationHandleImageDisabled.Status == AsyncOperationStatus.Succeeded)
                {
                    _timeImageDisabled = asyncOperationHandleImageDisabled.Result;

                    var asyncOperationHandleImageDefault =
                        Addressables.LoadAssetAsync<Sprite>(CommonKeys.Addressable.LevelButtonPath);
                    asyncOperationHandleImageDefault.Completed += delegate
                    {
                        if (asyncOperationHandleImageDefault.Status == AsyncOperationStatus.Succeeded)
                        {
                            _timeImageDefault = asyncOperationHandleImageDefault.Result;
                            
                            // After receiving all the necessary sprites, add a listener for the ToggleButton and StartButton
                            _toggleTimer.onValueChanged.AddListener(ToggleButtonChanged);
                            _buttonStartGame.onClick.AddListener(ButtonStartGameClicked);
                        }
                        else
                            Debug.Log("Failed to get MainLevelDefault Sprite");
                    };
                }
                else
                    Debug.Log("Failed to get MainLevelDisabled Sprite");
            };
        }

        /// <summary>
        /// Changes the selected image in ChooseImageButton
        /// </summary>
        private void OnEnable()
        {
            if (_buttonStartGame.IsUnityNull())
                return;
            
            if (SelectedImage == "" || _chooseImageButtonText.IsUnityNull())
            {
                _buttonStartGame.interactable = false;
                return;
            }
            
            _chooseImageButtonText.text = SelectedImage;
            _buttonStartGame.interactable = true;
            SelectedImage = "";
        }

        /// <summary>
        /// Disables/enables timer control buttons
        /// <param name="isOn">true - disables all timerButtons, false - enables</param>
        /// </summary>
        private void ToggleButtonChanged(bool isOn)
        {
            // Disable/enable buttons
            if (!isOn)
            {
                for(var i = 0; i < _pairButtonStages.Length; i++)
                {
                    _pairButtonStages[i] = new Tuple<Button, bool>(_pairButtonStages[i].Item1,
                        _pairButtonStages[i].Item1.interactable);
                    _pairButtonStages[i].Item1.interactable = false;
                }
            }
            else
                foreach (var pairButtonStage in _pairButtonStages)
                    pairButtonStage.Item1.interactable = pairButtonStage.Item2;

            // Disable/enable timeImage
            foreach (var timeImage in _arrTimeImages)
                timeImage.sprite = isOn ? _timeImageDefault : _timeImageDisabled;
        }

        /// <summary>
        /// Runs the created user level
        /// </summary>
        private void ButtonStartGameClicked()
        {
            const int lvlNumber = CommonKeys.CustomLevelNumber;
            var gridSize = new Tuple<int, int>(2, 3);
            switch (_dropdownDifficult.value)
            {
                case 0:
                    gridSize = new Tuple<int, int>(2, 3);
                    break;
                case 1:
                    gridSize = new Tuple<int, int>(3, 4);
                    break;
                case 2:
                    gridSize = new Tuple<int, int>(4, 5);
                    break;
                default:
                    Debug.Log("Unknown difficulty value");
                    break;
            }
            var listImageNames = new List<string>(1) { _chooseImageButtonText.text };
            var levelInfoTransfer = LevelInfoTransfer.SetInstance(lvlNumber, gridSize, listImageNames);

            // Add the timer value if the timer is enabled
            if (_toggleTimer.isOn)
            {
                if (_textMinutes.text == "" || _textSeconds.text == "")
                    return;
                var rTimer = Convert.ToSingle(_textMinutes.text) * 60.0f + Convert.ToSingle(_textSeconds.text);
                levelInfoTransfer.Timer = rTimer;
            }
            
            // Start level
            Time.timeScale = 1;
            if (CanvasController.ClassCanvasController != null)
                CanvasController.DestroyClass();
            SceneManager.LoadScene(CommonKeys.Names.SceneNature); //TODO: 1) Change to selected location; 2) Copy-paste from ButtonsController
        }
    }
}