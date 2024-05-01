using Unity.VisualScripting;
using System.Globalization;
using UnityEngine.UI;
using UnityEngine;
// using UnityEditor;
using System;
using TMPro;

namespace UI
{
    /// <summary>
    /// The class responsible for the interaction of elements in the CustomLevelPanel user interface panel
    /// </summary>
    public class CustomLevelButtonsController : ButtonsController
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void Start()
        {
            base.Start();
            
            
        }

        /// <summary>
        /// Overridable method responsible for the logic of the CustomLevelPanel buttons
        /// </summary>
        protected override void OnClickButton()
        {
            if (ButtonThis.IsUnityNull())
                return;

            switch (ButtonThis.name)
            {
                case CommonKeys.StrButtonNames.MinuteDownButton:
                    ChangeTime(TimeType.Minute, false);
                    break;
                case CommonKeys.StrButtonNames.MinuteUpButton:
                    ChangeTime(TimeType.Minute, true);
                    break;
                case CommonKeys.StrButtonNames.SecondDownButton:
                    ChangeTime(TimeType.Second, false);
                    break;
                case CommonKeys.StrButtonNames.SecondUpButton:
                    ChangeTime(TimeType.Second, true);
                    break;
                case CommonKeys.StrButtonNames.ChooseImage:
                    LoadImage();
                    break;
                default:
                    base.OnClickButton();
                    break;
            }
        }

        /// <summary>
        /// Increases or decreases timу
        /// </summary>
        /// <param name="timeType">Time type: minute, second</param>
        /// <param name="bIncrease">If true increases time, otherwise decreases</param>
        private void ChangeTime(TimeType timeType, bool bIncrease)
        {
            var timeText = timeType switch
            {
                TimeType.Minute => objThisPanel.transform.Find(CommonKeys.Names.MinuteText).GetComponent<TMP_Text>(),
                TimeType.Second => objThisPanel.transform.Find(CommonKeys.Names.SecondText).GetComponent<TMP_Text>(),
                _ => throw new ArgumentOutOfRangeException(nameof(timeType), timeType, null)
            };

            // Increase or decrease the time, also disable the buttons if the limit is reached
            var rTime = Convert.ToInt32(timeText.text);
            switch (timeType)
            {
                case TimeType.Minute:
                    rTime += bIncrease ? 1 : -1;
                    var buttonCalled = objCalled.GetComponent<Button>();
                    if (buttonCalled.IsUnityNull())
                    {
                        Debug.Log("Failed to get MinuteDownButton");
                        return;
                    }

                    // Foolproof
                    if (rTime < 1)
                        rTime = 1;
                    if (rTime > 90)
                        rTime = 90;

                    if(rTime is 1 or 90)
                        ButtonThis.interactable = false;
                    else if (!buttonCalled.interactable)
                        buttonCalled.interactable = true;
                    break;
                case TimeType.Second:
                    rTime = (60 + rTime + (bIncrease ? 10 : -10)) % 60;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeType), timeType, null);
            }
            
            timeText.SetText(rTime.ToString(CultureInfo.InvariantCulture));
        }

        private void LoadImage()
        {
            // OpenFilePanelExample.Apply();

            
        }
    }

    public enum TimeType
    {
        Minute,
        Second
    }
}
// public class OpenFilePanelExample : EditorWindow
// {
//     [MenuItem( "Example/Overwrite Texture" )]
//     public static void Apply()
//     {
//         // var texture = Selection.activeObject as Texture2D;
//         // if( texture == null )
//         // {
//         //     EditorUtility.DisplayDialog( "Select Texture", "You must select a texture first!", "OK" );
//         //     return;
//         // }
//     
//         var path = EditorUtility.OpenFilePanel( "Overwrite with png", "", "png" );
//         if (path.Length == 0) return;
//         var www = new WWW( "file:///" + path );
//         // www.LoadImageIntoTexture( texture );
//     }
//     
// }