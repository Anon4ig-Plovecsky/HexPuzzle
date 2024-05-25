using Unity.VisualScripting;
using UI.TestedModules;
using UnityEngine.UI;
using UnityEngine;
using PrimeTween;
using System;

namespace UI
{
    /// <summary>
    /// Provides basic logic for switching between horizontal UI tab pages
    /// The MaxPage member is initially 0
    /// </summary>
    /// <typeparam name="T">Type of page elements created and returned</typeparam>
    public abstract class AbstractPageTabHorizontalSwitcher<T> : MonoBehaviour, IPageTab
    {
        /// <summary>
        /// Places page elements on a single object
        /// </summary>
        /// <returns>Objects on a page of type T</returns>
        public abstract T CreatePages();

        [SerializeField] protected Transform tfmPage;
        [SerializeField] protected RectTransform rtfmMask;
        protected Button ButtonLeftArrow;
        protected Button ButtonRightArrow;

        // Distance from the center of one page to the adjacent one
        protected float PageDistance;
        protected const float PageSpacing = 0.1f;
        protected int MaxPage;              // Maximum number of pages available (INCLUSIVE)
        // If frequently switch levels, the real position of the object may be lost,
        // so records it here and update it after each successful animation
        private Vector2 _controlPosition = Vector2.zero;
        
        protected virtual void Start()
        {
            PageDistance = rtfmMask.rect.width + PageSpacing;
            
            // Receiver of left and right buttons
            var listButtons = GetComponentsInChildren<Button>();
            foreach (var button in listButtons)
            {
                if (!ButtonLeftArrow.IsUnityNull() && !ButtonRightArrow.IsUnityNull())
                    break;
                
                switch (button.name)
                {
                    case CommonKeys.StrButtonNames.LeftArrow:
                        ButtonLeftArrow = button;
                        break;
                    case CommonKeys.StrButtonNames.RightArrow:
                        ButtonRightArrow = button;
                        break;
                }
            }
            
            if(ButtonLeftArrow.IsUnityNull() && ButtonRightArrow.IsUnityNull())
            {
                Debug.LogError("Failed to get button arrows!");
                return;
            }
            
            // As a safety measure, disable the left arrow button, and also,
            // if the maximum number of pages is 1, also disable the right arrow button
            ButtonLeftArrow.enabled = false;
            ButtonLeftArrow.interactable = false;
            // The Start method of an abstract class is executed before the overridden
            // method in the child class, so such a check is not allowed
            // CheckNavigationButton(ButtonRightArrow, 0, MaxPage);
        }
        
        /// <remarks>The maximum value is specified in the MaxPage member</remarks>
        /// <inheritdoc cref="IPageTab.NavigationButtonPressed"/>
        public virtual void NavigationButtonPressed(ArrowDirection arrowDirection)
        {
            var currentPosition = _controlPosition.x;
            var iCurrentPage = currentPosition > 0.0
                ? 0
                : Convert.ToInt32(Math.Abs(Math.Round(currentPosition / PageDistance)));

            var rShiftPositionX = arrowDirection == ArrowDirection.RightArrow ? -PageDistance : PageDistance;
            _controlPosition = new Vector2(_controlPosition.x + rShiftPositionX, 0);
            
            // Go to next/previous page
            Tween.LocalPositionX(
                tfmPage, currentPosition + rShiftPositionX, 0.2f, Ease.InOutQuad)
                .OnComplete(delegate { tfmPage.localPosition = _controlPosition; });
            iCurrentPage += arrowDirection == ArrowDirection.RightArrow ? 1 : -1;

            // Disable the button if the page is the last one
            CheckNavigationButton(ButtonLeftArrow, iCurrentPage, 0);
            CheckNavigationButton(ButtonRightArrow, iCurrentPage, MaxPage);
        }

        /// <summary>
        /// Checks the current page number and if it reaches the disable value,
        /// disables the button, otherwise, checks if the button is disabled and enable it.
        /// </summary>
        /// <param name="button">Button to disable/enable</param>
        /// <param name="iCurrentPage">Current page number</param>
        /// <param name="iDisableValue">Page value at which the button should be disabled</param>
        private static void CheckNavigationButton(Button button, int iCurrentPage, int iDisableValue)
        {
            if (iCurrentPage == iDisableValue)
            {
                button.enabled = false;
                button.interactable = false;
            }
            else if (!button.interactable)
            {
                button.enabled = true;
                button.interactable = true;
            }
        }
    }
}