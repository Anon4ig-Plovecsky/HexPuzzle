using Unity.VisualScripting;
using UI.TestedModules;
using UnityEngine;
using Valve.VR;
using System;

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
                !_objPlayer.IsUnityNull() && actionBoolean.GetStateDown(InputSource)
                && pauseController.activeSelf)
            {
                _isPaused = Time.timeScale != 0;
                CanvasController.ClassCanvasController.SetPause(_isPaused);
            }
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