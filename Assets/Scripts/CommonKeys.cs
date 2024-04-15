using System.Collections.Generic;
using UnityEngine;

public abstract record CommonKeys
{
    public const int CubeSides = 6;

    // Enumerated type for UI buttons value map
    public enum UiKeys
    {
        // Main Menu
        StartGame,
        Results,
        Settings,
        QuitGame,
        Continue,
        ExitToMenu,
        ExitToMenuWinPanel,

        // Common
        BackToMenu,
        GoToMenu,

        // Start Game
        MainLevels,
        CustomLevel,
        
        // Main Levels
        LeftArrow,
        RightArrow,
        MainLevel
    }

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
        public const string CustomLevel = "CustomLevelButton";
        
        // Main Levels
        public const string LeftArrow = "LeftArrowButton";
        public const string RightArrow = "RightArrowButton";
        public const string MainLevel = "MainLevel";
    }

    public static readonly Dictionary<UiKeys, string> UiButtonNames = new()
    {
        // MainMenu
        { UiKeys.StartGame, StrButtonNames.StartGame },
        { UiKeys.Results, StrButtonNames.Results },
        { UiKeys.Settings, StrButtonNames.Settings },
        { UiKeys.QuitGame, StrButtonNames.QuitGame },
        { UiKeys.Continue,  StrButtonNames.Continue },
        { UiKeys.ExitToMenu, StrButtonNames.ExitToMenu },
        { UiKeys.ExitToMenuWinPanel, StrButtonNames.ExitToMenuWinPanel },
        
        // Common
        { UiKeys.GoToMenu, StrButtonNames.GoToMenu },
        { UiKeys.MainLevels, StrButtonNames.MainLevels },
        
        // Start Game
        { UiKeys.CustomLevel, StrButtonNames.CustomLevel },
        { UiKeys.BackToMenu, StrButtonNames.BackToMenu },
        
        // MainLevels
        { UiKeys.LeftArrow, StrButtonNames.LeftArrow },
        { UiKeys.RightArrow, StrButtonNames.RightArrow },
        { UiKeys.MainLevel, StrButtonNames.MainLevel }
    };
    
    // Names
    public static class Names
    {
        public const string MainLevelButton = "LevelButton";
    }
    
    // Addressable paths
    public static class Addressable
    {
        public const string PartOfPainting = "Assets/Prefabs/PartOfPainting.prefab";
        public const string LevelButton = "Assets/Prefabs/UI/LevelButton.prefab";

        // Addressable paths to button images
        public static class ButtonImages
        {
            // Main menu
            public static readonly ButtonImagePaths Continue = new("Assets/Images/UI/MainMenu/ContinueButton/ContinueButtonImage.png");
            public static readonly ButtonImagePaths QuitGame = new("Assets/Images/UI/MainMenu/QuitButton/QuitGameButtonImage.png");
            public static readonly ButtonImagePaths ExitToMenu = new("Assets/Images/UI/MainMenu/ExitToMenuButton/ExitToMenuButtonImage.png");
            public static readonly ButtonImagePaths ExitToMenuWinPanel = new("Assets/Images/UI/MainMenu/ExitToMenuInWinPanelButton/ExitToMenuInWinPanelButtonImage.png");
            public static readonly ButtonImagePaths StartGame = new("Assets/Images/UI/MainMenu/StartGameButton/StartGameButtonImage.png");
            public static readonly ButtonImagePaths Results = new("Assets/Images/UI/MainMenu/ResultsButton/ResultsButtonImage.png");
            public static readonly ButtonImagePaths Settings = new("Assets/Images/UI/MainMenu/SettingsButton/SettingsButtonImage.png");
            
            // Common
            public static readonly ButtonImagePaths GoToMainMenu = new("Assets/Images/UI/GoToMenuButton/GoToMenu.png");
            public static readonly ButtonImagePaths BackToMenu = new("Assets/Images/UI/BackToMenuButton/BackToMenu.png");

            // Start Game
            public static readonly ButtonImagePaths MainLevels = new("Assets/Images/UI/StartGame/MainLevels/MainLevels.png");
            public static readonly ButtonImagePaths CustomLevel = new("Assets/Images/UI/StartGame/CustomLevel/CustomLevels.png");
            
            // MainLevels
            public static readonly ButtonImagePaths LeftArrow = new("Assets/Images/UI/MainLevels/LeftArrow/LeftArrowStandard.png");
            public static readonly ButtonImagePaths RightArrow = new("Assets/Images/UI/MainLevels/RightArrow/RightArrowStandard.png");
            public static readonly ButtonImagePaths MainLevel = new("Assets/Images/UI/MainLevels/LevelButton/LevelButton.png");
        }
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

// Button sprites
public class ButtonImageSprites
{
    public Sprite Standard { get; }
    public Sprite Selected { get; }

    public ButtonImageSprites(Sprite standard, Sprite selected)
    {
        Standard = standard;
        Selected = selected;
    }
}

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {}
}