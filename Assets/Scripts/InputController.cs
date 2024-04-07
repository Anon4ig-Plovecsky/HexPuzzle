using UnityEngine;
using UI;

public class InputController : MonoBehaviour
{
    [SerializeField] private GameObject pauseController;
    private bool _isPaused;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseController.activeSelf)
        {
            _isPaused = Time.timeScale != 0;
            CanvasController.ClassCanvasController.SetPause(_isPaused);
        }
    }
}