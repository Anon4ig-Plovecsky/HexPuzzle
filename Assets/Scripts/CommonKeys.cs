﻿using Unity.VisualScripting;
using UnityEngine;

public abstract record CommonKeys
{
    // Number of visible main level buttons on the panel
    public const int LevelsOnPanel = 5;
    
    public const int CubeSides = 6;

    public const string DefaultChooseImageName = "Выберите файл";

    public static class StrButtonNames
    {
        // Main Menu
        public const string StartGame = "StartGameButton";
        public const string Results = "ResultsButton";
        public const string Settings = "SettingsButton";
        public const string QuitGame = "QuitGameButton";
        public const string Continue = "ContinueButton";
        public const string ExitToMenu = "ExitToMenuButton";
        public const string ExitToMenuWinPanel = "ExitToMenuButtonWinPanel";
        
        // Common
        public const string GoToMenu = "GoToMenuButton";
        public const string BackToMenu = "BackToMenuButton";
        
        // Start Game
        public const string MainLevels = "MainLevelsButton";
        public const string CustomLevel = "CustomPanelButton";
        
        // Main Levels
        public const string LeftArrow = "LeftArrowButton";
        public const string RightArrow = "RightArrowButton";
        public const string LevelButton = "LevelButton";
        
        // Custom Level
        public const string MinuteUpButton = "MinuteUpButton";
        public const string MinuteDownButton = "MinuteDownButton";
        public const string SecondUpButton = "SecondUpButton";
        public const string SecondDownButton = "SecondDownButton";
        public const string ChooseImage = "ChooseImageButton";
        
        // ImageChooser
        public const string SelectButton = "SelectButton";
        public const string CancelButton = "CancelButton";
    }
    
    // Names
    public static class Names
    {
        public const string MainLevelButton = "LevelButton";
        public const string MaskLevels = "MainLevelsPanel/MaskLevels";
        public const string LevelsGroup = "MainLevelsPanel/MaskLevels/LevelsGroup";
        public const string Grid = "Grid";

        // Panels
        public const string MainLevels = "MainLevels";
        public const string MainMenu = "MainMenu";
        
        // Scenes
        public const string SceneNature = "Nature";
        
        // CustomLevel
        public const string TimerToggle = "CustomLevelPanel/Timer/TimerToggle";
        public const string MinuteTextImage = "CustomLevelPanel/Timer/Minute/MinuteTextImage";
        public const string SecondTextImage = "CustomLevelPanel/Timer/Second/SecondTextImage";
        public const string MinuteText = "Timer/Minute/MinuteTextImage/MinuteText";
        public const string SecondText = "Timer/Second/SecondTextImage/SecondText";
        public const string MinuteUpButton = "CustomLevelPanel/Timer/Minute/MinuteUpButton";
        public const string MinuteDownButton = "CustomLevelPanel/Timer/Minute/MinuteDownButton";
        public const string SecondUpButton = "CustomLevelPanel/Timer/Second/SecondUpButton";
        public const string SecondDownButton = "CustomLevelPanel/Timer/Second/SecondDownButton";
        public const string ChooseImageButtonText = "CustomLevelPanel/MainImage/ChooseImageButton/ChooseImageText";
        public const string StartLevelButton = "CustomLevelPanel/StartLevelButton";
        
        // ImageChooser
        public const string ImageContent = "ImageChooserPanel/Scroll View/Viewport/ScrollMask/ImageContent";
        public const string ImageName = "ImageName";
        public const string Image = "Image";

        public const string Player = "Player";
    }

    /// <summary>
    /// Gets a component of an object obtained along the path from the Transform of the current object with log output
    /// </summary>
    /// <param name="transformThis">Transform the current object</param>
    /// <param name="strObjPath">Path to the object to be found, starting from the current object</param>
    /// <typeparam name="T">Type of component you are looking for</typeparam>
    /// <returns>If an object of type T is found, then returns a reference to it, otherwise null</returns>
    public static T GetComponentFromTransformOfType<T>(Transform transformThis, string strObjPath) where T : Component
    {
        var tfmFound = transformThis.Find(strObjPath);
        if (tfmFound.IsUnityNull())
        {
            Debug.Log($"Failed to get transform of {strObjPath}");
            return null;
        }

        var component = tfmFound.GetComponent<T>();
        if (!component.IsUnityNull())
            return component;
        
        Debug.Log($"Failed to get {nameof(T)} from {strObjPath}");
        return null;
    }
    
    // Addressable paths
    public static class Addressable
    {
        public const string PartOfPainting = "Assets/Prefabs/PartOfPainting.prefab";
        public const string LevelButtonPrefab = "Assets/Prefabs/UI/LevelButton.prefab";
        public const string CellPrefab = "Assets/Prefabs/Cell.prefab";
        public const string MainLevelDisabled = "Assets/Images/UI/MainLevels/LevelButton/MainLevelDisabled.png";
        public const string ImageItem = "Assets/Prefabs/UI/ImageItem.prefab";
        public static readonly ButtonImagePaths LevelButtonPath = new("Assets/Images/UI/MainLevels/LevelButton/LevelButton.png");
    }
}

// Class template for address paths of button images
public class ButtonImagePaths
{
    // Path of a button with a standard name
    private readonly string _name;
    public string Name
    {
        private init
        {
            _name = value;

            var arrSplitPath = _name.Split(".png");
            Selected = arrSplitPath[0] + "Selected.png";
        }
        get => _name;
    }

    // Path to the highlighted button
    public string Selected { get; private set; }

    public ButtonImagePaths(string name)
    {
        Name = name;
    }
}

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {}
}