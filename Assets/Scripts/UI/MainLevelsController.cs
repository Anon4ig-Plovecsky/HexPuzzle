using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Unity.VisualScripting;
using LevelsController;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace UI
{
    public class MainLevelsController : MonoBehaviour
    {
        // Array of main game buttons
        private Button[] _btnMainLevelArray = new Button[LevelParametersMap.LevelInfo.Count];
        // Main level button prefab, for placement on the panel
        private GameObject _levelButtonPrefab;
        // Button offset relative to the center of the previous button
        private const float PaddingButton = 0.9f;
        
        // Start is called before the first frame update
        private void Start()
        {
            // Initially, leave the panel active in order to load all levels into the panel in advance
            gameObject.SetActive(false);

            // Getting the game's main level button prefab
            var asyncOperation = Addressables.LoadAssetAsync<GameObject>(CommonKeys.Addressable.LevelButton);
            asyncOperation.Completed += delegate
            {
                if (asyncOperation.Status == AsyncOperationStatus.Succeeded)
                    _levelButtonPrefab = asyncOperation.Result;
            };
            
            
        }

        /// Creates a button and places it on the panel
        private GameObject CreateButton(Vector2 vPosition, int numberLevel)
        {
            // Install the created button from the prefab and place it on the panel
            var button = Instantiate(_levelButtonPrefab, vPosition, Quaternion.Euler(0, 0, 0));
            if (button.IsUnityNull())
                return null;

            button.name = CommonKeys.Names.MainLevelButton + "(" + Convert.ToString(numberLevel) + ")";

            var strButtonText = button.GetComponent<TMP_Text>();
            if (strButtonText.IsUnityNull())
                return null;
            
            strButtonText.SetText(Convert.ToString(numberLevel));
            
            return button;
        }
        
        /// <summary>
        /// The helper method calculate the button indent relative to the center of the panel
        /// </summary>
        /// <param name="numberLevel">game level number</param>
        /// <returns>x-axis offset relative to panel center</returns>
        public float CalculatePadding(int numberLevel)
        {
            
        
            return 0.0f;
        }
    }
}
