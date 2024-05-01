using System.Collections.Generic;
using UnityEngine;

public abstract record CommonKeys
{
    // Number of visible main level buttons on the panel
    public const int LevelsOnPanel = 5;
    
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
        LevelButton,
        
        // CustomLevel
        UpArrow,
        DownArrow,
        ChooseImage
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
    }

    public static readonly Dictionary<string, UiKeys> UiButtonNames = new()
    {
        // MainMenu
        { StrButtonNames.StartGame, UiKeys.StartGame },
        { StrButtonNames.Results, UiKeys.Results },
        { StrButtonNames.Settings, UiKeys.Settings },
        { StrButtonNames.QuitGame, UiKeys.QuitGame },
        { StrButtonNames.Continue, UiKeys.Continue },
        { StrButtonNames.ExitToMenu, UiKeys.ExitToMenu },
        { StrButtonNames.ExitToMenuWinPanel, UiKeys.ExitToMenuWinPanel },
        
        // Common
        { StrButtonNames.GoToMenu, UiKeys.GoToMenu },
        { StrButtonNames.MainLevels, UiKeys.MainLevels },
        
        // Start Game
        { StrButtonNames.CustomLevel, UiKeys.CustomLevel },
        { StrButtonNames.BackToMenu, UiKeys.BackToMenu },
        
        // MainLevels
        { StrButtonNames.LeftArrow, UiKeys.LeftArrow },
        { StrButtonNames.RightArrow, UiKeys.RightArrow },
        { StrButtonNames.LevelButton, UiKeys.LevelButton },
        
        // CustomLevel
        { StrButtonNames.MinuteUpButton, UiKeys.UpArrow },
        { StrButtonNames.SecondUpButton, UiKeys.UpArrow },
        { StrButtonNames.MinuteDownButton, UiKeys.DownArrow },
        { StrButtonNames.SecondDownButton, UiKeys.DownArrow },
        { StrButtonNames.ChooseImage, UiKeys.ChooseImage }
    };
    
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
    }
    
    // Addressable paths
    public static class Addressable
    {
        public const string PartOfPainting = "Assets/Prefabs/PartOfPainting.prefab";
        public const string LevelButtonPrefab = "Assets/Prefabs/UI/LevelButton.prefab";
        public const string CellPrefab = "Assets/Prefabs/Cell.prefab";
        public const string MainLevelDisabled = "Assets/Images/UI/MainLevels/LevelButton/MainLevelDisabled.png";

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
            public static readonly ButtonImagePaths LevelButtonPath = new("Assets/Images/UI/MainLevels/LevelButton/LevelButton.png");
            
            // CustomLevel
            public static readonly ButtonImagePaths UpArrow = new("Assets/Images/UI/CustomLevel/ArrowUpButton/ArrowUpButton.png");
            public static readonly ButtonImagePaths DownArrow = new("Assets/Images/UI/CustomLevel/ArrowDownButton/ArrowDownButton.png");
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