using System.Collections.Generic;
using UnityEngine;

public static class CommonKeys
{
    public const int CubeSides = 6;

    // Enumerated type for UI buttons value map
    public enum UiKeys
    {
        Continue,
        QuitGame,
        ExitToMenu,
        ExitToMenuWinPanel,
        GoToMenu,
        StartGame,
        Results,
        Settings,
        MainLevels,
        CustomLevel
    }

    public static class StrButtonNames
    {
        public const string StartGame = "StartGameButton";
        public const string Results = "ResultsButton";
        public const string Settings = "SettingsButton";
        public const string QuitGame = "QuitGameButton";
        public const string Continue = "ContinueButton";
        public const string ExitToMenu = "ExitToMenuButton";
        public const string ExitToMenuWinPanel = "ExitToMenuButtonWinPanel";
    }

    public static readonly Dictionary<UiKeys, string> UiButtonNames = new()
    {
        { UiKeys.StartGame, StrButtonNames.StartGame },
        { UiKeys.Results, StrButtonNames.Results },
        { UiKeys.Settings, StrButtonNames.Settings },
        { UiKeys.QuitGame, StrButtonNames.QuitGame },
        { UiKeys.Continue,  StrButtonNames.Continue },
        { UiKeys.ExitToMenu, StrButtonNames.ExitToMenu },
        { UiKeys.ExitToMenuWinPanel, StrButtonNames.ExitToMenuWinPanel }
    };
    
    // Addressable paths
    public static class Addressable
    {
        public const string PartOfPainting = "Assets/Prefabs/PartOfPainting.prefab";

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

            // Start Game
            public static readonly ButtonImagePaths MainLevels = new("Assets/Images/UI/StartGame/MainLevels/MainLevels.png");
            public static readonly ButtonImagePaths CustomLevel = new("Assets/Images/UI/StartGame/CustomLevel/CustomLevels.png");
        }
    }
}

// Class template for address paths of button images
public class ButtonImagePaths
{
    // Path of a button with a standard name
    private string _name;
    public string Name
    {
        init
        {
            _name = value;

            var arrSplitPath = _name.Split(".png");
            _selected = arrSplitPath[0] + "Selected.png";
        }
        get => _name;
    }

    // Path to the highlighted button
    private string _selected;
    public string Selected => _selected;

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