using UnityEngine.ResourceManagement.AsyncOperations;
using LevelsController.TestedModules;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

namespace UI
{
    /// <summary>
    /// The class responsible for the imageChooser panel catches
    /// the selection of an image and, when Select is pressed,
    /// transfers the image to the CustomLevelController panel
    /// </summary>
    public class ImageChooserController : MonoBehaviour
    {
        [SerializeField] private AssetLabelReference assetLabelReferenceImages;

        private readonly List<GameObject> _listImageItems = new();
        private List<Sprite> _listSpriteImages = new();
        private GameObject _prefabImageItem;
        private GameObject _imageContent;
        private GameObject _thisPanel;
        private Button _buttonSelect;
        private Button _buttonCancel;

        private void Start()
        {
            _thisPanel = GameObject.Find("ImageChooserPanel");
            if (_thisPanel.IsUnityNull())
            {
                Debug.Log("Failed to get thisPanel");
                return;
            }

            _imageContent = transform.Find(CommonKeys.Names.ImageContent).gameObject;
            if (_imageContent.IsUnityNull())
            {
                Debug.Log("Failed to get ImageContent");
                return;
            }
            
            var objButtonSelect = GameObject.Find(CommonKeys.StrButtonNames.SelectButton);
            if (objButtonSelect.IsUnityNull())
            {
                Debug.Log("Failed to get ButtonSelect");
                return;
            }
            _buttonSelect = objButtonSelect.GetComponent<Button>();
            _buttonSelect.onClick.AddListener(delegate { ButtonOnClick(_buttonSelect.name); });

            var objButtonCancel = GameObject.Find(CommonKeys.StrButtonNames.CancelButton);
            if (objButtonCancel.IsUnityNull())
            {
                Debug.Log("Failed to get ButtonCancel");
                return;
            }
            _buttonCancel = objButtonCancel.GetComponent<Button>();
            _buttonCancel.onClick.AddListener(delegate { ButtonOnClick(_buttonCancel.name); });
            
            // Getting images and _imageItem template
            var asyncOperationHandleList = Addressables.LoadAssetsAsync<Sprite>(assetLabelReferenceImages, _ => {});
            asyncOperationHandleList.Completed += delegate
            {
                if (asyncOperationHandleList.Status == AsyncOperationStatus.Succeeded)
                {
                    _listSpriteImages = new List<Sprite>(asyncOperationHandleList.Result);

                    var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(CommonKeys.Addressable.ImageItem);
                    asyncOperationHandle.Completed += delegate
                    {
                        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            _prefabImageItem = asyncOperationHandle.Result;
                            FillImageList();
                        }
                        else
                            Debug.Log("Failed to get prefab ImageItem");
                    };
                }
                else
                {
                    Debug.Log("Failed to get images");
                }
            };
        }

        /// <summary>
        /// Fills the viewport with ImageItems
        /// </summary>
        private void FillImageList()
        {
            if (_listSpriteImages.Count == 0)
                return;
            
            // Adding ImageItem to ImageContent
            foreach (var spriteImage in _listSpriteImages)
            {
                var imageItem = Instantiate(_prefabImageItem, Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                
                imageItem.transform.SetParent(_imageContent.transform);
                imageItem.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                imageItem.transform.localPosition = Vector3.zero;
                
                // Adding an image and text to a template
                var imageCreator = new ImageCreator(spriteImage, new Tuple<int, int>(1, 1));
                var transformImage = imageItem.transform.Find(CommonKeys.Names.Image);
                var image = transformImage.GetComponent<Image>();
                if (image.IsUnityNull())
                {
                    Debug.Log("Failed to get image of imageItem");
                    continue;
                }
                image.sprite = imageCreator.CropEntirePainting();

                var transformImageName = imageItem.transform.Find(CommonKeys.Names.ImageName);
                var imageName = transformImageName.GetComponent<TMP_Text>();
                if (imageName.IsUnityNull())
                {
                    Debug.Log("Failed to get imageName of imageItem");
                    continue;
                }
                imageName.text = spriteImage.name;

                var toggleImageItem = imageItem.GetComponent<Toggle>();
                if (toggleImageItem.IsUnityNull())
                {
                    Debug.Log("Failed to get toggleImageItem");
                    continue;
                }
                toggleImageItem.onValueChanged.AddListener(delegate { ToggleImageItemOnChanged(toggleImageItem); });
                
                _listImageItems.Add(imageItem);
            }
            
            // Resizing ImageContent to fit all ImageItems
            var rectImage = _listImageItems[0].GetComponent<RectTransform>();
            var fHeight = rectImage.rect.height * _listImageItems.Count;
            
            var rectTransformContent = _imageContent.GetComponent<RectTransform>();
            rectTransformContent.sizeDelta = new Vector2(0, fHeight);
        }

        /// <summary>
        /// When selected ToggleImageItem disables all others toggle
        /// </summary>
        private void ToggleImageItemOnChanged(Toggle thisToggle)
        {
            _buttonSelect.interactable = false;

            if (!thisToggle.isOn)
            {
                if (CheckSelectedItem())
                    _buttonSelect.interactable = true;
                return;
            }
            // no comment (Disable all other toggleButtons except the current one)
            foreach (var imageItem in _listImageItems)
            {
                var toggleImageItem = imageItem.GetComponent<Toggle>();
                if (toggleImageItem.IsUnityNull())
                    continue;

                if (!toggleImageItem.Equals(thisToggle))
                    toggleImageItem.isOn = false;
            }

            if (CheckSelectedItem())
                _buttonSelect.interactable = true;
        }

        /// <summary>
        /// Checks whether the selected object is in the ImageChooserPanel list
        /// </summary>
        /// <returns>true - if at least one element was selected, otherwise - false</returns>
        private bool CheckSelectedItem()
        {
            foreach (var toggleImageItem in _listImageItems.
                         Select(imageItem => imageItem.GetComponent<Toggle>()))
            {
                if (toggleImageItem.IsUnityNull())
                    return false;

                if (toggleImageItem.isOn)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Listener that fires when ImageChooserPanel buttons are clicked
        /// </summary>
        /// <param name="strButtonName">Name of the button pressed</param>
        private void ButtonOnClick(string strButtonName)
        {
            switch (strButtonName)
            {
                case CommonKeys.StrButtonNames.SelectButton:
                    
                    break;
                case CommonKeys.StrButtonNames.CancelButton:
                    
                    break;
                default:
                    return;
            }
        }
    }
}