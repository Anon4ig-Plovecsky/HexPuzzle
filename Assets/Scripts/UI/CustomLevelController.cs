using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace UI
{
    /// <summary>
    /// Class responsible for behavior between elements in the CustomLevelPanel
    /// </summary>
    public class CustomLevelController : MonoBehaviour
    {
        public static string SelectedImage = "";
        private TMP_Text _chooseImageButtonText;
        
        private Sprite _timeImageDefault;
        private Sprite _timeImageDisabled;
        
        private Toggle _toggleTimer;
        // Pair with arrow buttons that control the timer, the second item maintains the position of the button
        // until the toggleButton is turned off
        private readonly Tuple<Button, bool>[] _pairButtonStages = new Tuple<Button, bool>[4];
        private readonly Image[] _arrTimeImages = new Image[2];

        private Button _buttonStartGame;
        
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
                        Addressables.LoadAssetAsync<Sprite>(CommonKeys.Addressable.LevelButtonPath.Name);
                    asyncOperationHandleImageDefault.Completed += delegate
                    {
                        if (asyncOperationHandleImageDefault.Status == AsyncOperationStatus.Succeeded)
                        {
                            _timeImageDefault = asyncOperationHandleImageDefault.Result;
                            
                            // After receiving all the necessary sprites, add a listener for the ToggleButton
                            _toggleTimer.onValueChanged.AddListener(ToggleButtonChanged);
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
            if (SelectedImage == "" || _chooseImageButtonText.IsUnityNull()) 
                return;
            
            _chooseImageButtonText.text = SelectedImage;
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
    }
}