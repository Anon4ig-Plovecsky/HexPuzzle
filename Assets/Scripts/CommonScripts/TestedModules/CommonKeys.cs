using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public abstract record CommonKeys
{
    // Controller shift at which ScrollView drag is considered
    public const float DragLength = 0.015f;
    
    // Number of visible main level buttons on the panel
    public const int CustomLevelNumber = -1;
    public const int LevelsOnPanel = 5;
    
    public const int CubeSides = 6;

    public const string DefaultChooseImageName = "Выберите файл";
    public const string TimeIsOver = "Время вышло!";
    public const string StrBestTime = "Лучшее время................";
    public const string StrRecentTime = "Последнее время...........";

    public const string SavedFormatFile = ".bin";

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
        
        // Settings
        public const string SfxToggle = "SfxToggle";
        public const string MusicToggle = "MusicToggle";
    }
    public static class StrAudioNames
    {
        public const string LaserHover = "Assets/Music & Sounds/SFX/DM-CGS-01.wav";
        public const string LaserClick = "Assets/Music & Sounds/SFX/DM-CGS-32.wav";
        public const string CubeCollision = "Assets/Music & Sounds/SFX/SFX-impact-simple-03_wav.wav";
        public const string MetalCollision = "Assets/Music & Sounds/SFX/SFX-impact-metal-01_wav.wav";
        public const string PillowCollision = "Assets/Music & Sounds/SFX/SFX-impact-punch-bag-01_wav.wav";
        public const string DishesCollision = "Assets/Music & Sounds/SFX/SFX-impact-pot-02_wav.wav";

        public const string MenuMusic = "Assets/Music & Sounds/Music/Neutrin05 - Timeless.wav";
    }
    
    // Names
    public static class Names
    {
        // Tags
        public const string ScrollElement = "ScrollElement";
        
        public const string MainLevelButton = "LevelButton";
        public const string MaskLevels = "MainLevelsPanel/MaskLevels";
        public const string LevelsGroup = "MainLevelsPanel/MaskLevels/LevelsGroup";
        public const string Grid = "Grid";
        public const string Cubes = "Cubes";
        public const string PointsOfSpawn = "PointsOfSpawn";
        public const string SoundsController = "SoundsController";
        
        // Images
        public const string Paintings = "Paintings";

        // Panels
        public const string MainLevels = "MainLevels";
        public const string MainMenu = "MainMenu";
        public const string Settings = "Settings";
        
        // Scenes
        public const string SceneNature = "Nature";
        public const string SceneHome = "Home";
        
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
        public const string DropdownDifficult = "CustomLevelPanel/Difficult/Dropdown";
        public const string DropdownScene = "CustomLevelPanel/LocationType/LocationDropdown";
        
        // ImageChooser
        public const string ImageContent = "ImageChooserPanel/Scroll View/Viewport/ScrollMask/ImageContent";
        public const string ImageName = "ImageName";
        public const string Image = "Image";
        public const string ScrollbarVertical = "Scrollbar Vertical";

        public const string Player = "Player";
        public const string PlayerDebug= "PlayerDebug";
        
        // PausePanel
        public const string TextTimer = "TextTimer";
        
        // StatusPanel
        public const string TimeResult = "StatusPanel/TimeResult";
        public const string TextResultTime = TimeResult + "/TextResultTime";
        public const string StatusText = "StatusPanel/StatusText";
        
        // Results
        public const string TextTimeInfo = "TextTimeInfo";
        public const string TextLevelInfo = "ResultsPanel/LevelInfo/TextLevelInfo";
        
        // Settings
        public const string SfxTogglePath = "SettingsPanel/SfxToggle";
        public const string MusicTogglePath = "SettingsPanel/MusicToggle";
        
        // Keys
        public const string SfxStatus = "SfxStatus";
        public const string MusicStatus = "MusicStatus";
    }
    
    /// <summary>
    /// Returns a Task on an audio file from game resources
    /// </summary>
    /// <param name="strPath">Path to audio file in Addressables</param>
    /// <returns>Task with AudioClip</returns>
    #nullable enable
    public static Task<T> LoadResource<T>(string strPath)
    {
        T? audioClip = default;
        var asyncOperationHandle = Addressables.LoadAssetAsync<T>(strPath);
        asyncOperationHandle.Completed += delegate
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                audioClip = asyncOperationHandle.Result;
            else
                Debug.Log($"Failed to get {strPath}");
        };
        asyncOperationHandle.WaitForCompletion();

        return Task.FromResult(audioClip!);
    }
    #nullable disable

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
            Debug.LogError($"Failed to get transform of {strObjPath}");
            return null;
        }

        var component = tfmFound.GetComponent<T>();
        if (!component.IsUnityNull())
            return component;
        
        Debug.LogError($"Failed to get {typeof(T).FullName} from {strObjPath}");
        return null;
    }
    
    /// <summary>
    /// Returns a set of random numbers in a given range
    /// </summary>
    /// <param name="size">Number of random numbers</param>
    /// <param name="range">Number range from 0 to specified(not inclusive)</param>
    /// <returns>Set of random non-repeating numbers</returns>
    public static HashSet<int> GenerateNumbers(int size, int range)
    {
        var hashSet = new HashSet<int>();
            
        // if the range is less than the size, then there will be an Endless Loop
        if (range < size)
            return null;
            
        while (hashSet.Count < size)
            hashSet.Add(Random.Range(0, range));
        return new HashSet<int>(hashSet);
    }
    
    // Addressable paths
    public static class Addressable
    {
        public const string PartOfPainting = "Assets/Prefabs/PartOfPainting.prefab";
        public const string LevelButtonPrefab = "Assets/Prefabs/UI/LevelButton.prefab";
        public const string CellPrefab = "Assets/Prefabs/Cell.prefab";
        public const string MainLevelDisabled = "Assets/Images/UI/MainLevels/LevelButton/MainLevelDisabled.png";
        public const string ImageItem = "Assets/Prefabs/UI/ImageItem.prefab";
        public const string LevelButtonPath = "Assets/Images/UI/MainLevels/LevelButton/LevelButton.png";
        public const string PrefabTimeInfo = "Assets/Prefabs/UI/TimeInfo.prefab";
        public const string LevelMusic = "LevelMusic";
    }
}

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {}
}