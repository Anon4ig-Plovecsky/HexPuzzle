using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Valve.VR.Extras;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class LaserHand : MonoBehaviour
    {
        // private Sprite _spriteResult;
    
        // Maps
        private Dictionary<CommonKeys.UiKeys, Image> _mapImages = new();
        // private Dictionary<CommonKeys.UiKeys, Button> _mapButtons;
        private readonly Dictionary<CommonKeys.UiKeys, ButtonImageSprites> _mapBtnImgSpritesMap = new();
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
            { CommonKeys.UiKeys.RightArrow, CommonKeys.Addressable.ButtonImages.RightArrow },
            { CommonKeys.UiKeys.LevelButton, CommonKeys.Addressable.ButtonImages.LevelButtonPath },
            
            // Custom Level
            { CommonKeys.UiKeys.UpArrow, CommonKeys.Addressable.ButtonImages.UpArrow },
            { CommonKeys.UiKeys.DownArrow, CommonKeys.Addressable.ButtonImages.DownArrow }
        };
        
        private SteamVR_LaserPointer _steamVrLaserPointer;

        public bool Active
        {
            set => _steamVrLaserPointer.active = value;
        }

        private void Awake()
        {
            _steamVrLaserPointer = gameObject.GetComponent<SteamVR_LaserPointer>();
            _steamVrLaserPointer.PointerIn += OnPointerIn;
            _steamVrLaserPointer.PointerOut += OnPointerOut;
            _steamVrLaserPointer.PointerClick += OnPointerClick;
        }

        private bool _isSorted;
        protected /*override*/ async void Start()
        {
            // base.Start();
        
            // Getting buttons and images by names
            // foreach(CommonKeys.UiKeys buttonKey in Enum.GetValues(typeof(CommonKeys.UiKeys)))
            // {
            //     _mapButtons.Add(buttonKey, GameObject.Find(CommonKeys.UiButtonNames[buttonKey]).GetComponent<Button>());
            //     _mapImages.Add(buttonKey, _mapButtons[buttonKey].GetComponent<Image>());
            // }
        
            FindGoToMainMenuButtonWinPanel();
        
            // Adding sprites to the map
            foreach (var path in _mapBtnImgStrPaths.ToArray())
            {
                var taskSpriteStandard = Addressables.LoadAssetAsync<Sprite>(path.Value.Name);
                var taskSpriteSelected = Addressables.LoadAssetAsync<Sprite>(path.Value.Selected);

                await taskSpriteStandard.Task;
                // var spriteStandard = taskSpriteStandard.Result;
                await taskSpriteSelected.Task;
                // var spriteSelected = taskSpriteSelected.Result;
                if(taskSpriteSelected.Status == taskSpriteStandard.Status && taskSpriteSelected.Status == AsyncOperationStatus.Succeeded)
                    _mapBtnImgSpritesMap.Add(path.Key, new ButtonImageSprites(taskSpriteStandard.Result, taskSpriteSelected.Result));
            }
        }
        private void OnPointerIn(object sender, PointerEventArgs e)
        {
            var pointerEnterHandler = e.target.GetComponent<IPointerEnterHandler>();
            if (pointerEnterHandler == null)
            {
                return;
            }

            pointerEnterHandler.OnPointerEnter(new PointerEventData(EventSystem.current));

            if (true)
                return;
        
            // Checking that a given sprite exists
            if (!e.target.CompareTag("ButtonUI"))
                return;

            // if (!CommonKeys.UiButtonNames.ContainsValue(e.target.name))
            //     return;

            var btnTarget = e.target.GetComponent<Button>();
            if (btnTarget.IsUnityNull() || !btnTarget.enabled)
                return;

            // Finding the button key
            if (!CommonKeys.UiButtonNames.TryGetValue(e.target.name, out var uiKey))
                return;

            // TODO: Need optimization
            
            // Replacing button sprite with selected sprite
            var imageThis = btnTarget.GetComponent<Image>();
            imageThis.sprite = _mapBtnImgSpritesMap[uiKey].Selected;

            // _mapImages[uiKey].sprite = _mapBtnImgSpritesMap[uiKey].Selected;
        }
        private void OnPointerClick(object sender, PointerEventArgs e)
        {
            // Checking that a given sprite exists
            if (!e.target.CompareTag("ButtonUI"))
                return;

            var btnTarget = e.target.GetComponent<Button>();
            if (btnTarget.IsUnityNull() || !btnTarget.enabled)
                return;
            
            // if (!CommonKeys.UiButtonNames.ContainsValue(e.target.name))
            //     return;
            //
            // // Finding the button key
            // var uiKey = CommonKeys.UiButtonNames.FirstOrDefault(uiName
            //     => uiName.Value.Equals(e.target.name)).Key;
            //
            // // Activating button using the found uiKey
            // _mapButtons[uiKey].onClick.Invoke();
            
            // Immediately cause the button to be pressed, because checking whether the button
            // can be processed is already inside the methods
            btnTarget.onClick.Invoke();
        }
        private void OnPointerOut(object sender, PointerEventArgs e)
        {
            // Checking that a given sprite exists
            if (!e.target.CompareTag("ButtonUI"))
                return;

            // if (!CommonKeys.UiButtonNames.ContainsValue(e.target.name))
            //     return;

            var btnTarget = e.target.GetComponent<Button>();
            if (btnTarget.IsUnityNull() || !btnTarget.enabled)
                return;

            // Finding the button key
            if (!CommonKeys.UiButtonNames.TryGetValue(e.target.name, out var uiKey))
                return;

            // TODO: Need optimization
            
            // Replacing button sprite with selected sprite
            var imageThis = btnTarget.GetComponent<Image>();
            imageThis.sprite = _mapBtnImgSpritesMap[uiKey].Standard;

            // _mapImages[uiKey].sprite = _mapBtnImgSpritesMap[uiKey].Selected;
        }
        private void FindGoToMainMenuButtonWinPanel()
        {
            // if (_goToMainMenuButtonWinPanel.IsUnityNull()) 
            //     return;
            //
            // _goToMainMenuButtonWinPanel = GameObject.Find(CommonKeys.Names.GoToMainMenuButtonWinPanel).GetComponent<Button>();
            // _goToMainMenuButtonWinPanelImage = _goToMainMenuButtonWinPanel.GetComponent<Image>();
        }

        private Task<Sprite> GetSpriteFromAddressables(string strPath)
        {
            Sprite? spriteResult = null;
            var asyncOperationHandle = Addressables.LoadAssetAsync<Sprite>(strPath);

            // await asyncOperationHandle.Task;
            
            asyncOperationHandle.Completed += delegate
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    spriteResult = asyncOperationHandle.Result;
                else 
                    Debug.Log("Failed to load!");
            };

            return asyncOperationHandle.Task;
        }
    }
}