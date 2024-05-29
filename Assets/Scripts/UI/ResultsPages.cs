using UnityEngine.ResourceManagement.AsyncOperations;
using LevelsController.TestedModules;
using UnityEngine.AddressableAssets;
using CommonScripts.TestedModules;
using System.Collections.Generic;
using Unity.VisualScripting;
using UI.TestedModules;
using UnityEngine;
using System.Linq;
using TMPro;

namespace UI
{
    public class ResultsPages : AbstractPageTabHorizontalSwitcher<GameObject[]>
    {
        private GameObject[] _arrTimesInfo;
        private List<SavedResults> _listSavedResults;

        private GameObject _prefabTimeInfo;
        private TMP_Text _textLevelInfo;
        
        protected override async void Start()
        {
            base.Start();
            gameObject.SetActive(false);
            
            MaxPage = LevelParametersMap.LevelInfo.Count - 1;

            _listSavedResults = SaveManager.ReadData<SavedResults>();
            if(_listSavedResults is null)
            {
                Debug.Log("ListSavedResults is empty");
                _listSavedResults = new List<SavedResults>();
            }

            // Receiving a prefab with information about completing the level
            var asyncOperationHandlePrefabTime = Addressables.LoadAssetAsync<GameObject>(CommonKeys.Addressable.PrefabTimeInfo);
            asyncOperationHandlePrefabTime.Completed += delegate
            {
                if (asyncOperationHandlePrefabTime.Status == AsyncOperationStatus.Succeeded)
                    _prefabTimeInfo = asyncOperationHandlePrefabTime.Result;
                else
                    Debug.Log("Failed to get PrefabTimeInfo");
            };
            await asyncOperationHandlePrefabTime.Task;
            if (_prefabTimeInfo.IsUnityNull())
            {
                _prefabTimeInfo = asyncOperationHandlePrefabTime.WaitForCompletion();
                if (_prefabTimeInfo.IsUnityNull())
                    return;
            }

            _textLevelInfo = CommonKeys.GetComponentFromTransformOfType<TMP_Text>(transform, CommonKeys.Names.TextLevelInfo);
            if(_textLevelInfo.IsUnityNull())
                return;
            
            CreatePages();
        }
        
        public override GameObject[] CreatePages()
        {
            var arrTimesInfo = new GameObject[LevelParametersMap.LevelInfo.Count];
            
            for (var i = 0; i <= MaxPage; i++)
            {
                var timeInfo = Instantiate(_prefabTimeInfo, Vector3.zero, Quaternion.identity);
                if (timeInfo.IsUnityNull())
                    return null;
                
                timeInfo.transform.SetParent(tfmPage);

                // Setting the position
                var rPosition = PageDistance * i;
                timeInfo.transform.localPosition = new Vector3(rPosition, 0, 0);
                timeInfo.transform.localRotation = Quaternion.identity;
                timeInfo.name = CommonKeys.Names.TextTimeInfo + "(" + i + ")";

                arrTimesInfo[i] = timeInfo;

                var savedResult = _listSavedResults.FirstOrDefault(result => result.LvlNumber == i + 1);
                if(savedResult is null)
                    continue;

                // Takes TMP_Text in TimeInfo and change the values in accordance with the received saved data
                if (timeInfo.transform.childCount != 2)
                {
                    Debug.LogError("Incorrect number of child in TimeInfo");
                    return null;
                }
                var textBestTime = timeInfo.transform.GetChild(0).GetComponent<TMP_Text>();
                var textRecentTime = timeInfo.transform.GetChild(1).GetComponent<TMP_Text>();
                if (textBestTime.IsUnityNull() || textRecentTime.IsUnityNull())
                {
                    Debug.LogError("Failed to get child components");
                    return null;
                }
                SetTextInTimeInfo(textBestTime, CommonKeys.StrBestTime, savedResult.TimeBest);
                SetTextInTimeInfo(textRecentTime, CommonKeys.StrRecentTime, savedResult.TimeRecent);
            }
            
            return arrTimesInfo;
        }

        /// <summary>
        /// Sets text values into TMP_Text
        /// </summary>
        /// <param name="textTimeInfo">TMP_Text, which will contain the results of completing the level</param>
        /// <param name="strTypeName">Time type name</param>
        /// <param name="rTime">Timer value from saved data</param>
        private static void SetTextInTimeInfo(TMP_Text textTimeInfo, string strTypeName, float rTime)
        {
            var strTime = CanvasController.GetStrTime(rTime);
            textTimeInfo.text = strTypeName + strTime;
        }
        
        /// <inheritdoc cref="AbstractPageTabHorizontalSwitcher{T}.NavigationButtonPressed"/>
        /// Also changes the level number on the header. 
        public override void NavigationButtonPressed(ArrowDirection arrowDirection)
        {
            base.NavigationButtonPressed(arrowDirection);

            var strHeader = _textLevelInfo.text.Split(' ')[0];
            _textLevelInfo.text = $"{strHeader} {CurrentPage + 1}";
        }
    }
}
