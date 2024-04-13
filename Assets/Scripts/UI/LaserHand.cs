using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using Valve.VR.Extras;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

namespace UI
{
    public class LaserHand : SteamVR_LaserPointer
    {
        private Sprite _spriteResult;
    
        // Maps
        private Dictionary<CommonKeys.UiKeys, Image> _mapImages;
        private Dictionary<CommonKeys.UiKeys, Button> _mapButtons;
        private Dictionary<CommonKeys.UiKeys, ButtonImageSprites> _mapBtnImgSpritesMap;
        private readonly Dictionary<CommonKeys.UiKeys, ButtonImagePaths> _mapBtnImgStrPaths = new()
        {
            // MainMenu
            { CommonKeys.UiKeys.StartGame, CommonKeys.Addressable.ButtonImages.StartGame},
            { CommonKeys.UiKeys.Results, CommonKeys.Addressable.ButtonImages.Results },
            { CommonKeys.UiKeys.Settings, CommonKeys.Addressable.ButtonImages.Settings },
            { CommonKeys.UiKeys.QuitGame, CommonKeys.Addressable.ButtonImages.QuitGame },
            { CommonKeys.UiKeys.Continue, CommonKeys.Addressable.ButtonImages.Continue },
            { CommonKeys.UiKeys.ExitToMenu, CommonKeys.Addressable.ButtonImages.ExitToMenu },
            { CommonKeys.UiKeys.ExitToMenuWinPanel, CommonKeys.Addressable.ButtonImages.ExitToMenuWinPanel },
            
            // Common
            { CommonKeys.UiKeys.GoToMenu, CommonKeys.Addressable.ButtonImages.GoToMainMenu },
            { CommonKeys.UiKeys.BackToMenu, CommonKeys.Addressable.ButtonImages.BackToMenu },
            
            // Start Game
            { CommonKeys.UiKeys.MainLevels, CommonKeys.Addressable.ButtonImages.MainLevels },
            { CommonKeys.UiKeys.CustomLevel, CommonKeys.Addressable.ButtonImages.CustomLevel },
            
            // Main Levels
            { CommonKeys.UiKeys.LeftArrow, CommonKeys.Addressable.ButtonImages.LeftArrow },
            { CommonKeys.UiKeys.RightArrow, CommonKeys.Addressable.ButtonImages.RightArrow }
        };

        private bool _isSorted;
        protected /*override*/ void Start()
        {
            // base.Start();
        
            // Getting buttons and images by names
            foreach(CommonKeys.UiKeys buttonKey in Enum.GetValues(typeof(CommonKeys.UiKeys)))
            {
                _mapButtons.Add(buttonKey, GameObject.Find(CommonKeys.UiButtonNames[buttonKey]).GetComponent<Button>());
                _mapImages.Add(buttonKey, _mapButtons[buttonKey].GetComponent<Image>());
            }
        
            FindGoToMainMenuButtonWinPanel();
        
            // Adding sprites to the map
            foreach (var path in _mapBtnImgStrPaths.ToArray())
            {
                var spriteStandard = GetSpriteFromAddressables(path.Value.Name);
                var spriteSelected = GetSpriteFromAddressables(path.Value.Selected);
            
                _mapBtnImgSpritesMap.Add(path.Key, new ButtonImageSprites(spriteStandard, spriteSelected));
            }
        }
        public override void OnPointerIn(PointerEventArgs e)
        {
            base.OnPointerIn(e);
        
            // Checking that a given sprite exists
            if (!e.target.CompareTag("ButtonUI"))
                return;

            if (!CommonKeys.UiButtonNames.ContainsValue(e.target.name))
                return;

            var btnTarget = e.target.GetComponent<Button>();
            if (btnTarget.IsUnityNull() || !btnTarget.enabled)
                return;

            // Finding the button key
            var uiKey = CommonKeys.UiButtonNames.FirstOrDefault(uiName
                => uiName.Value.Equals(e.target.name)).Key;

            // Replacing button sprite with selected sprite
            _mapImages[uiKey].sprite = _mapBtnImgSpritesMap[uiKey].Selected;
        }
        public override void OnPointerClick(PointerEventArgs e)
        {
            base.OnPointerClick(e);
        
            // Checking that a given sprite exists
            if (!e.target.CompareTag("ButtonUI"))
                return;

            var btnTarget = e.target.GetComponent<Button>();
            if (btnTarget.IsUnityNull() || !btnTarget.enabled)
                return;
            
            if (!CommonKeys.UiButtonNames.ContainsValue(e.target.name))
                return;

            // Finding the button key
            var uiKey = CommonKeys.UiButtonNames.FirstOrDefault(uiName
                => uiName.Value.Equals(e.target.name)).Key;

            // Activating button using the found uiKey
            _mapButtons[uiKey].onClick.Invoke();
        }
        public override void OnPointerOut(PointerEventArgs e)
        {
            base.OnPointerOut(e);
        
            // Checking that a given sprite exists
            if (!e.target.CompareTag("ButtonUI"))
                return;

            if (!CommonKeys.UiButtonNames.ContainsValue(e.target.name))
                return;

            // Finding the button key
            var uiKey = CommonKeys.UiButtonNames.FirstOrDefault(uiName
                => uiName.Value.Equals(e.target.name)).Key;

            // Replacing button sprite with standard sprite
            _mapImages[uiKey].sprite = _mapBtnImgSpritesMap[uiKey].Standard;
        }
        private void FindGoToMainMenuButtonWinPanel()
        {
            // if (_goToMainMenuButtonWinPanel.IsUnityNull()) 
            //     return;
            //
            // _goToMainMenuButtonWinPanel = GameObject.Find(CommonKeys.Names.GoToMainMenuButtonWinPanel).GetComponent<Button>();
            // _goToMainMenuButtonWinPanelImage = _goToMainMenuButtonWinPanel.GetComponent<Image>();
        }
        private void OnLoadDone(
            AsyncOperationHandle<Sprite> asyncOperationHandle
        )
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                _spriteResult = asyncOperationHandle.Result;
            else 
                Debug.Log("Failed to load!");
        }

        private Sprite GetSpriteFromAddressables(string strPath)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<Sprite>(strPath);
            asyncOperationHandle.Completed += delegate
            {
                OnLoadDone(asyncOperationHandle);
            };

            return _spriteResult;
        }
    }
}