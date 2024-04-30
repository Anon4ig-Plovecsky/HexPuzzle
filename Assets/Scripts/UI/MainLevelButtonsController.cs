using Unity.VisualScripting;
using LevelsController;
using UnityEngine.UI;
using UnityEngine;
using PrimeTween;
using System;

namespace UI
{
    public class MainLevelButtonsController : ButtonsController
    {
        private int _maxPage;
        private float _panelWidth;
        private Transform _levelsGroup;
        private Button _buttonSecondArrow;              // Second navigation arrow button (to turn it on)
        
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            _maxPage = Convert.ToInt32(Math.Ceiling(LevelParametersMap.LevelInfo.Count /
                                                    Convert.ToDouble(CommonKeys.LevelsOnPanel))) - 1;
            
            if (ButtonThis.name.Equals(CommonKeys.StrButtonNames.LeftArrow))
            {
                ButtonThis.enabled = false;
                ButtonThis.interactable = false;
            }
            
            _buttonSecondArrow = objCalled.GetComponent<Button>();
            if (_buttonSecondArrow.IsUnityNull())
            {
                Debug.Log("Failed to get second navigation button");
                return;
            }
            
            // Getting panel length
            var transformPanel = objThisPanel.GetComponent<RectTransform>();
            if (transformPanel.IsUnityNull())
            {
                Debug.Log("Failed to get RectTransform of current panel");
                return;
            }
            _panelWidth = transformPanel.rect.width;
            
            _levelsGroup = objThisPanel.transform.Find(CommonKeys.Names.LevelsGroup);
            if (_levelsGroup.IsUnityNull())
                Debug.Log("Could not find levelsGroup");
        }

        protected override void OnClickButton()
        {
            if (ButtonThis.IsUnityNull())
                return;

            switch (ButtonThis.name)
            {
                case CommonKeys.StrButtonNames.LeftArrow:
                    MoveLevelButtons(false);
                    break;
                case CommonKeys.StrButtonNames.RightArrow:
                    MoveLevelButtons(true);
                    break;
                case CommonKeys.StrButtonNames.LevelButton:
                    break;
                default:
                    base.OnClickButton();
                    return;
            }
        }

        /// <summary>
        /// Moves to next/previous main levels
        /// </summary>
        /// <param name="bNext">True - goes forward to levels, false - back</param>
        private void MoveLevelButtons(bool bNext)
        {
            var groupPosition = _levelsGroup.localPosition.x;
            var iCurrentPage = groupPosition > 0.0
                ? 0
                : Convert.ToInt32(Math.Abs(Math.Round(groupPosition / _panelWidth)));

            // Go to next/previous page
            Tween.LocalPositionX(
                _levelsGroup, groupPosition + (bNext ? -_panelWidth : _panelWidth), 0.2f, Ease.InOutQuad);
            iCurrentPage += bNext ? 1 : -1;

            // Disable the button if the page is the last one
            if (iCurrentPage == _maxPage || iCurrentPage == 0)
            {
                ButtonThis.enabled = false;
                ButtonThis.interactable = false;
            }

            // Enable the second navigation button if it is disabled
            if (_buttonSecondArrow.enabled) 
                return;
            _buttonSecondArrow.enabled = true;
            _buttonSecondArrow.interactable = true;
        }
    }
}