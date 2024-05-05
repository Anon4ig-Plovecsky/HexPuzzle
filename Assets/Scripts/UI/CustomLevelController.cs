using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace UI
{
    /// <summary>
    /// Class responsible for behavior between elements in the CustomLevelPanel
    /// </summary>
    public class CustomLevelController : MonoBehaviour
    {
        private Sprite _timeImageDefault;
        private Sprite _timeImageDisabled;
        
        private Toggle _toggleTimer;
        // Pair with arrow buttons that control the timer, the second item maintains the position of the button
        // until the toggleButton is turned off
        private readonly Tuple<Button, bool>[] _pairButtonStages = new Tuple<Button, bool>[4];
        private readonly Image[] _arrTimeImages = new Image[2];
        
        private void Start()
        {
            gameObject.SetActive(false);
            
            var transformToggle = transform.Find(CommonKeys.Names.TimerToggle);
            if (transformToggle.IsUnityNull())
            {
                Debug.Log("Failed to get gameObjToggle");
                return;
            }
            _toggleTimer = transformToggle.GetComponent<Toggle>();

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
                var transformButton = transform.Find(arrStrButtonNames[i]);
                if (transformButton.IsUnityNull())
                {
                    Debug.Log("Failed to get " + arrStrButtonNames[i]);
                    return;
                }

                var button = transformButton.GetComponent<Button>();
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
                var transformImage = transform.Find(arrStrImageNames[i]);
                if (transformImage.IsUnityNull())
                {
                    Debug.Log("Failed to get " + arrStrImageNames[i]);
                    return;
                }

                _arrTimeImages[i] = transformImage.GetComponent<Image>();
            }

            // Getting sprites for timeImage
            var asyncOperationHandleImageDisabled = Addressables.LoadAssetAsync<Sprite>(CommonKeys.Addressable.MainLevelDisabled);
            asyncOperationHandleImageDisabled.Completed += delegate
            {
                if (asyncOperationHandleImageDisabled.Status == AsyncOperationStatus.Succeeded)
                {
                    _timeImageDisabled = asyncOperationHandleImageDisabled.Result;

                    var asyncOperationHandleImageDefault =
                        Addressables.LoadAssetAsync<Sprite>(CommonKeys.Addressable.ButtonImages.LevelButtonPath.Name);
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