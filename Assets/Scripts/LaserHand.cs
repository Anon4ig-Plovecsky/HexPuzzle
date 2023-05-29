using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using Valve.VR.Extras;
using UnityEngine.UI;
using UnityEngine;

public class LaserHand : SteamVR_LaserPointer
{
    private readonly List<Sprite> sprites = new();
    //Images
    private Image newGameButtonImage;
    private Image resultsButtonImage;
    private Image settingsButtonImage;
    private Image exitGameButtonImage;
    //Buttons
    private Button newGameButton;
    private Button resultsButton;
    private Button settingsButton;
    private Button exitGameButton;
    
    private const string NameNewGameButton = "NewGameButton";
    private const string NameResultsButton = "ResultsButton";
    private const string NameSettingsButton = "SettingsButton";
    private const string NameExitGameButton = "ExitGameButton";
    private readonly string[] paths =
    {
        "Assets/Images/NewGameButton/NewGameButtonImage.png",           // 0
        "Assets/Images/NewGameButton/NewGameButtonSelected.png",        // 1
        "Assets/Images/ExitButton/ExitGameButtonImage.png",             // 2
        "Assets/Images/ExitButton/ExitGameButtonImageSelected.png",     // 3
        "Assets/Images/ResultsButton/ResultsButtonImage.png",           // 4
        "Assets/Images/ResultsButton/ResultsButtonImageSelected.png",   // 5
        "Assets/Images/SettingsButton/SettingsButtonImage.png",         // 6
        "Assets/Images/SettingsButton/SettingsButtonImageSelected.png", // 7
    };
    private void Start()
    {
        newGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
        resultsButton = GameObject.Find("ResultsButton").GetComponent<Button>();
        settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        exitGameButton = GameObject.Find("ExitGameButton").GetComponent<Button>();
        newGameButtonImage = newGameButton.GetComponent<Image>();
        resultsButtonImage = resultsButton.GetComponent<Image>();
        settingsButtonImage = settingsButton.GetComponent<Image>();
        exitGameButtonImage = exitGameButton.GetComponent<Image>();
        foreach (var path in paths)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<Sprite>(path);
            asyncOperationHandle.Completed += delegate { OnLoadDone(asyncOperationHandle); };
        }
        foreach(var sprite in sprites)
            Debug.Log(sprite.name);
    }
    public override void OnPointerIn(PointerEventArgs e)
    {
        base.OnPointerIn(e);
        if (!e.target.CompareTag("ButtonUI")) return;
        switch (e.target.name)
        {
            case NameNewGameButton:
                newGameButtonImage.sprite = sprites[1]; 
                break;
            case NameExitGameButton:
                exitGameButtonImage.sprite = sprites[3];
                break;
            case NameResultsButton:
                resultsButtonImage.sprite = sprites[5];
                break;
            case NameSettingsButton:
                settingsButtonImage.sprite = sprites[7];
                break;
        }
    }
    public override void OnPointerClick(PointerEventArgs e)
    {
        base.OnPointerClick(e);
        if (!e.target.CompareTag("ButtonUI")) return;
        switch (e.target.name)
        {
            case NameNewGameButton:
                newGameButton.onClick.Invoke();
                break;
            case NameExitGameButton:
                exitGameButton.onClick.Invoke();
                break;
        }
    }
    public override void OnPointerOut(PointerEventArgs e)
    {
        base.OnPointerOut(e);
        if (!e.target.CompareTag("ButtonUI")) return;
        switch (e.target.name)
        {
            case NameNewGameButton:
                newGameButtonImage.sprite = sprites[0];
                break;
            case NameExitGameButton:
                exitGameButtonImage.sprite = sprites[2];
                break;
            case NameResultsButton:
                resultsButtonImage.sprite = sprites[4];
                break;
            case NameSettingsButton:
                settingsButtonImage.sprite = sprites[6];
                break;
        }
    }
    private void Update()
    {
        
    }
    private void OnLoadDone(AsyncOperationHandle<Sprite> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            sprites.Add(asyncOperationHandle.Result);
        }
        else 
            Debug.Log("Failed to load!");
    }
    // private void ListAll()
    // {
    //     if (sprites.Count != paths.Length) return;
    //     foreach(var sprite in sprites)
    //         Debug.Log(sprite);
    // }
}
