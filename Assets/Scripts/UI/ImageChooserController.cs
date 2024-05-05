using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

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

        private List<GameObject> _listImageItems = new();
        private List<Sprite> _listSpriteImages = new();
        private GameObject _prefabImageItem;
        private GameObject _imageContent;
        private GameObject _thisPanel;
        
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
            foreach (var imageItem in _listSpriteImages.Select(_ =>
                         Instantiate(_prefabImageItem, Vector3.zero, 
                         Quaternion.Euler(0.0f, 0.0f, 0.0f))))
            {
                imageItem.transform.SetParent(_imageContent.transform);
                imageItem.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                imageItem.transform.localPosition = Vector3.zero;
                
                _listImageItems.Add(imageItem);
            }
            
            // Resizing ImageContent to fit all ImageItems
            var rectImage = _listImageItems[0].GetComponent<RectTransform>();
            var fHeight = rectImage.rect.height * _listImageItems.Count;
            
            var rectTransformContent = _imageContent.GetComponent<RectTransform>();
            rectTransformContent.sizeDelta = new Vector2(0, fHeight);
            
            // var rectContent = rectTransformContent.rect;
            // rectContent.Set(rectContent.x, rectContent.y, rectContent.width, fHeight);
            
        }
    }
}