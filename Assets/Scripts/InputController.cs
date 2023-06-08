using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private GameObject pauseController;
    private bool isPaused;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseController.activeSelf)
        {
            isPaused = Time.timeScale != 0;
            CanvasController.classCanvasController.SetPause(isPaused);
        }
    }
}