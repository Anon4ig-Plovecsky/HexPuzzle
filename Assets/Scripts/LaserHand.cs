using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using Valve.VR.Extras;
using UnityEngine.UI;
using UnityEngine;
using System;

public class LaserHand : SteamVR_LaserPointer
{
    private readonly List<Sprite> sprites = new();
    //Images
    private Image newGameButtonImage;
    private Image resultsButtonImage;
    private Image settingsButtonImage;
    private Image exitGameButtonImage;
    private Image continueButtonImage;
    private Image goToMainMenuButtonImage;
    private Image goToMainMenuButtonWinPanelImage;
    
    //Buttons
    private Button newGameButton;
    private Button resultsButton;
    private Button settingsButton;
    private Button exitGameButton;
    private Button continueButton;
    private Button goToMainMenuButton;
    private Button goToMainMenuButtonWinPanel;

    private bool isSorted;
    private const string NameNewGameButton = "NewGameButton";
    private const string NameResultsButton = "ResultsButton";
    private const string NameSettingsButton = "SettingsButton";
    private const string NameExitGameButton = "ExitGameButton";
    private const string NameContinueButton = "ContinueButton";
    private const string NameGoToMainMenuButton = "GoToMainMenuButton";
    private const string NameGoToMainMenuButtonWinPanel = "GoToMainMenuButtonWinPanel";
    private readonly string[] paths =
    {
        "Assets/Images/ContinueButton/ContinueButtonImage.png",                                     //  0
        "Assets/Images/ContinueButton/ContinueButtonImageSelected.png",                             //  1
        "Assets/Images/ExitButton/ExitGameButtonImage.png",                                         //  2
        "Assets/Images/ExitButton/ExitGameButtonImageSelected.png",                                 //  3
        "Assets/Images/GoToMainMenuButton/GoToMainMenuButtonImage.png",                             //  4
        "Assets/Images/GoToMainMenuButton/GoToMainMenuButtonImageSelected.png",                     //  5
        "Assets/Images/GoToMainMenuInWinPanelButton/GoToMainMenuInWinPanelButtonImage.png",         //  6
        "Assets/Images/GoToMainMenuInWinPanelButton/GoToMainMenuInWinPanelButtonImageSelected.png", //  7
        "Assets/Images/NewGameButton/NewGameButtonImage.png",                                       //  8
        "Assets/Images/NewGameButton/NewGameButtonSelected.png",                                    //  9
        "Assets/Images/ResultsButton/ResultsButtonImage.png",                                       // 10
        "Assets/Images/ResultsButton/ResultsButtonImageSelected.png",                               // 11
        "Assets/Images/SettingsButton/SettingsButtonImage.png",                                     // 12
        "Assets/Images/SettingsButton/SettingsButtonImageSelected.png",                             // 13
    };
    protected override void Start()
    {
        base.Start();
        newGameButton = GameObject.Find(NameNewGameButton).GetComponent<Button>();
        resultsButton = GameObject.Find(NameResultsButton).GetComponent<Button>();
        settingsButton = GameObject.Find(NameSettingsButton).GetComponent<Button>();
        exitGameButton = GameObject.Find(NameExitGameButton).GetComponent<Button>();
        continueButton = GameObject.Find(NameContinueButton).GetComponent<Button>();
        goToMainMenuButton = GameObject.Find(NameGoToMainMenuButton).GetComponent<Button>();
        
        newGameButtonImage = newGameButton.GetComponent<Image>();
        resultsButtonImage = resultsButton.GetComponent<Image>();
        settingsButtonImage = settingsButton.GetComponent<Image>();
        exitGameButtonImage = exitGameButton.GetComponent<Image>();
        continueButtonImage = continueButton.GetComponent<Image>();
        goToMainMenuButtonImage = goToMainMenuButton.GetComponent<Image>();
        FindGoToMainMenuButtonWinPanel();
        foreach (var path in paths)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<Sprite>(path);
            asyncOperationHandle.Completed += delegate { OnLoadDone(asyncOperationHandle); };
        }
        foreach(var sprite in sprites)
            Debug.Log(sprite.name);
    }
    protected override void Update()
    {
        if (sprites.Count == paths.Length && !isSorted)
        {
            isSorted = true;
            sprites.Sort((sprite1, sprite2) => 
                string.Compare(sprite1.name, sprite2.name, StringComparison.Ordinal));
        }
        base.Update();
    }
    public override void OnPointerIn(PointerEventArgs e)
    {
        base.OnPointerIn(e);
        if (!e.target.CompareTag("ButtonUI")) return;
        switch (e.target.name)
        {
            case NameNewGameButton:
                newGameButtonImage.sprite = sprites[9]; 
                break;
            case NameExitGameButton:
                exitGameButtonImage.sprite = sprites[3];
                break;
            case NameResultsButton:
                resultsButtonImage.sprite = sprites[11];
                break;
            case NameSettingsButton:
                settingsButtonImage.sprite = sprites[13];
                break;
            case NameContinueButton:
                continueButtonImage.sprite = sprites[1];
                break;
            case NameGoToMainMenuButton:
                goToMainMenuButtonImage.sprite = sprites[5];
                break;
            case NameGoToMainMenuButtonWinPanel:
                FindGoToMainMenuButtonWinPanel();
                goToMainMenuButtonWinPanelImage.sprite = sprites[7];
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
            case NameContinueButton:
                continueButton.onClick.Invoke();
                break;
            case NameGoToMainMenuButton:
                goToMainMenuButton.onClick.Invoke();
                break;
            case NameGoToMainMenuButtonWinPanel:
                goToMainMenuButtonWinPanel.onClick.Invoke();
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
                newGameButtonImage.sprite = sprites[8];
                break;
            case NameExitGameButton:
                exitGameButtonImage.sprite = sprites[2];
                break;
            case NameResultsButton:
                resultsButtonImage.sprite = sprites[10];
                break;
            case NameSettingsButton:
                settingsButtonImage.sprite = sprites[12];
                break;
            case NameContinueButton:
                continueButtonImage.sprite = sprites[0];
                break;
            case NameGoToMainMenuButton:
                goToMainMenuButtonImage.sprite = sprites[4];
                break;
            case NameGoToMainMenuButtonWinPanel:
                goToMainMenuButtonWinPanelImage.sprite = sprites[6];
                break;
        }
    }
    private void FindGoToMainMenuButtonWinPanel()
    {
        if (goToMainMenuButtonWinPanel != null) return;
        goToMainMenuButtonWinPanel = GameObject.Find(NameGoToMainMenuButtonWinPanel).GetComponent<Button>();
        goToMainMenuButtonWinPanelImage = goToMainMenuButtonWinPanel.GetComponent<Image>();
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
}
