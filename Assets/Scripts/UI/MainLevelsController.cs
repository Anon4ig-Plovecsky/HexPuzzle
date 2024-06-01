using UnityEngine.ResourceManagement.AsyncOperations;
using LevelsController.TestedModules;
using UnityEngine.AddressableAssets;
using CommonScripts.TestedModules;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

namespace UI
{
    /// Special class for buttons in the MainLevels panel
    public class MainLevelsController : AbstractPageTabHorizontalSwitcher<Button[]>
    {
        // Array of main game buttons
        private Button[] _arrMainLevelButtons = new Button[LevelParametersMap.LevelInfo.Count];
        // Main level button prefab, for placement on the panel
        private GameObject _levelButtonPrefab;
        // Button offset relative to the center of the previous button
        private const float PaddingButton = 0.09f;
        // Number of levels completed
        private int _numLevels;
        
        // Start is called before the first frame update
        protected override async void Start()
        {
            base.Start();
            
            MaxPage = Convert.ToInt32(Math.Ceiling(LevelParametersMap.LevelInfo.Count /
                                                   Convert.ToDouble(CommonKeys.LevelsOnPanel))) - 1;

            // Get the number of levels completed
            var completedLevels = SaveManager.ReadData<CompletedLevels>();
            if (completedLevels != null && completedLevels.Count != 0)
                _numLevels = completedLevels.First().NumLevels;
            
            // Initially, leave the panel active in order to load all levels into the panel in advance
            gameObject.SetActive(false);

            // Getting the game's main level button prefab
            var asyncTask = CommonKeys.LoadResource<GameObject>(CommonKeys.Addressable.LevelButtonPrefab);
            await asyncTask;
            _levelButtonPrefab = asyncTask.Result;
            _arrMainLevelButtons = CreatePages();
        }

        /// Creates a button and places it on the panel
        private GameObject CreateButton(Vector2 vPosition, int numberLevel)
        {
            // Install the created button from the prefab and place it on the panel
            var objButton = Instantiate(_levelButtonPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0));
            if (objButton.IsUnityNull())
                return null;

            // Place the button in the group object
            objButton.transform.SetParent(tfmPage);
            
            objButton.transform.localPosition = vPosition;
            objButton.transform.localRotation = Quaternion.Euler(0, 0, 0);
            objButton.name = CommonKeys.Names.MainLevelButton + "(" + Convert.ToString(numberLevel) + ")";

            // We leave only completed levels and the first uncompleted one
            var button = objButton.GetComponent<Button>();
            if (button.IsUnityNull())
                return null;
            if (numberLevel > _numLevels + 1)
                button.interactable = false;

            var strButtonText = objButton.GetComponentInChildren<TMP_Text>();
            if (strButtonText.IsUnityNull())
                return null;
            
            strButtonText.SetText(Convert.ToString(numberLevel));
            
            return objButton;
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
        public override Button[] CreatePages()
        {
            var arrButtons = new Button[LevelParametersMap.LevelInfo.Count];
            
            for (var i = 0; i < LevelParametersMap.LevelInfo.Count; i++)
            {
                var iSeqNumber = Convert.ToInt32(i / CommonKeys.LevelsOnPanel);
                var fRelativePadding = CalculatePadding(i);

                var fResPadding = fRelativePadding + PageDistance * iSeqNumber;

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