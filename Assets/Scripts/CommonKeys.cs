using System.Collections.Generic;
using UnityEngine;

public static class CommonKeys
{
    public const int CubeSides = 6;

    // Enumerated type for UI buttons value map
    public enum UiKeys
    {
        Continue,
        ExitGame,
        GoToMainMenu,
        GoToMainMenuWinPanel,
        NewGame,
        Results,
        Settings
    }

    public static readonly Dictionary<UiKeys, string> UiNames = new()
    {
        { UiKeys.NewGame, "NewGameButton" },
        { UiKeys.Results, "ResultsButton" },
        { UiKeys.Settings, "SettingsButton" },
        { UiKeys.ExitGame, "ExitGameButton" },
        { UiKeys.Continue,  "ContinueButton"},
        { UiKeys.GoToMainMenu, "GoToMainMenuButton" },
        { UiKeys.GoToMainMenuWinPanel, "GoToMainMenuButtonWinPanel" }
    };
    
    // Addressable paths
    public static class Addressable
    {
        public const string PartOfPainting = "Assets/Prefabs/PartOfPainting.prefab";

        // Addressable paths to button images
        public static class ButtonImages
        {
            public static readonly ButtonImagePaths Continue = new("Assets/Images/ContinueButton/ContinueButtonImage.png");
            public static readonly ButtonImagePaths ExitGame = new("Assets/Images/ExitButton/ExitGameButtonImage.png");
            public static readonly ButtonImagePaths GoToMainMenu = new("Assets/Images/GoToMainMenuButton/GoToMainMenuButtonImage.png");
            public static readonly ButtonImagePaths GoToMainMenuWinPanel = new("Assets/Images/GoToMainMenuInWinPanelButton/GoToMainMenuInWinPanelButtonImage.png");
            public static readonly ButtonImagePaths NewGame = new("Assets/Images/NewGameButton/NewGameButtonImage.png");
            public static readonly ButtonImagePaths Results = new("Assets/Images/ResultsButton/ResultsButtonImage.png");
            public static readonly ButtonImagePaths Settings = new("Assets/Images/SettingsButton/SettingsButtonImage.png");
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