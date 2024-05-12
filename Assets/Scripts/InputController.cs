using UI.TestedModules;
using UnityEngine;
using Valve.VR;
using System;
using Unity.VisualScripting;

public class InputController : MonoBehaviour
{
    private bool _isError;
    
    [SerializeField] private GameObject pauseController;
    private bool _isPaused;

    [SerializeField] private SteamVR_Action_Boolean actionBoolean;
    private const SteamVR_Input_Sources InputSource = SteamVR_Input_Sources.LeftHand;

    private GameObject _objPlayer;

    private void Start()
    {
        _objPlayer = GameObject.Find("Player");
    }
    
    private void Update()
    {
        try
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                !_objPlayer.IsUnityNull() && SteamVR_Actions._default.InteractUI.GetStateDown(SteamVR_Input_Sources.Any)
                && pauseController.activeSelf)
            {
                _isPaused = Time.timeScale != 0;
                CanvasController.ClassCanvasController.SetPause(_isPaused);
            }
            
            if (!_objPlayer.IsUnityNull() && actionBoolean.GetStateDown(InputSource))
                Debug.Log("InteractUI");
        }
        catch (NullReferenceException e)
        {
            if (_isError)
                return;
            _isError = true;
            Debug.LogError(e.Message);
        }
    }
}