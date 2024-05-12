using UnityEngine.ResourceManagement.AsyncOperations;
using LevelsController.TestedModules;
using UnityEngine.AddressableAssets;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace UI
{
    /// Special class for buttons in the MainLevels panel
    public class MainLevelsController : MonoBehaviour
    {
        // Array of main game buttons
        private Button[] _arrMainLevelButtons = new Button[LevelParametersMap.LevelInfo.Count];
        // Main level button prefab, for placement on the panel
        private GameObject _levelButtonPrefab;
        // Button offset relative to the center of the previous button
        private const float PaddingButton = 0.09f;
        
        private Transform _levelsGroup;
        
        // Start is called before the first frame update
        private void Start()
        {
            // Initially, leave the panel active in order to load all levels into the panel in advance
            gameObject.SetActive(false);

            _levelsGroup = transform.Find(CommonKeys.Names.LevelsGroup);
            if (_levelsGroup.IsUnityNull())
            {
                Debug.Log("Couldn't find levels group");
                return;
            }

            // Getting the game's main level button prefab
            var asyncOperation = Addressables.LoadAssetAsync<GameObject>(CommonKeys.Addressable.LevelButtonPrefab);
            asyncOperation.Completed += delegate
            {
                if (asyncOperation.Status != AsyncOperationStatus.Succeeded) 
                    return;
                _levelButtonPrefab = asyncOperation.Result;
                _arrMainLevelButtons = PlaceLevelsButton();
            };
        }

        /// Creates a button and places it on the panel
        private GameObject CreateButton(Vector2 vPosition, int numberLevel)
        {
            // Install the created button from the prefab and place it on the panel
            var button = Instantiate(_levelButtonPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0));
            if (button.IsUnityNull())
                return null;

            // Place the button in the group object
            button.transform.SetParent(_levelsGroup);
            
            button.transform.localPosition = vPosition;
            button.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.name = CommonKeys.Names.MainLevelButton + "(" + Convert.ToString(numberLevel) + ")";

            var strButtonText = button.GetComponentInChildren<TMP_Text>();
            if (strButtonText.IsUnityNull())
                return null;
            
            strButtonText.SetText(Convert.ToString(numberLevel));
            
            return button;
        }
        
        /// <summary>
        /// The helper method calculate the button indent relative to the center of the panel
        /// </summary>
        /// <param name="indexLevel">game level index</param>
        /// <returns>x-axis offset relative to panel center</returns>
        private static float CalculatePadding(int indexLevel)
        {
            // Getting the level number in sequence of visible levels on the panel and shift it
            // half to the left in order to find the padding relative to the center of the panel
            var iNumberInSeq = indexLevel % CommonKeys.LevelsOnPanel;
            var iHalfLevelsOnPanel = Convert.ToInt32(Math.Floor(Convert.ToDouble(CommonKeys.LevelsOnPanel) / 2.0));
            iNumberInSeq -= iHalfLevelsOnPanel;

            var fPadding = iNumberInSeq * PaddingButton;
            return fPadding;
        }

        /// <summary>
        /// Places all levels specified in LevelParametersMap into the MainLevels panel.
        /// Each sequence of LevelsOnPanel is placed at a distance of MainPanel.width.
        /// </summary>
        private Button[] PlaceLevelsButton()
        {
            var arrButtons = new Button[LevelParametersMap.LevelInfo.Count];
            
            var rectTransform = GetComponent<RectTransform>();
            var fPanelWidth = rectTransform.rect.width;
            
            for (var i = 0; i < LevelParametersMap.LevelInfo.Count; i++)
            {
                var iSeqNumber = Convert.ToInt32(i / CommonKeys.LevelsOnPanel);
                var fRelativePadding = CalculatePadding(i);

                var fResPadding = fRelativePadding + fPanelWidth * iSeqNumber;

                // Creating a button and adding it to the array
                var vPosition = new Vector2(fResPadding, 0.0f);
                var levelButtonGameObject = CreateButton(vPosition, i + 1);

                var levelButton = levelButtonGameObject.GetComponent<Button>();
                if (levelButton.IsUnityNull())
                {
                    Debug.Log("Failed to get created button");
                    continue;
                }
                
                arrButtons[i] = levelButton;
            }

            return arrButtons;
        }
    }
}
