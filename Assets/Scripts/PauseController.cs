using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public static PauseController classPauseController { get; private set; }
    private readonly List<Renderer> gameObjectsRenderer = new();
    private Color defaultColor;
    private Color grayColor;
    private void Start()
    {
        if (classPauseController == null)
            classPauseController = this;
        defaultColor = new Color(1, 1, 1);
        grayColor = new Color(0.432f, 0.432f, 0.432f);
        new List<GameObject>(FindObjectsOfType<GameObject>()).ForEach(obj =>
        {
            Renderer gameObjectRenderer;
            if((gameObjectRenderer = obj.GetComponent<Renderer>()) != null && !obj.CompareTag("UI"))
                gameObjectsRenderer.Add(gameObjectRenderer);
        });
    }
    public void SetPause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        foreach (var rendererGameObject in gameObjectsRenderer)
            rendererGameObject.material.color = isPaused ? grayColor : defaultColor;
        pausePanel.SetActive(isPaused);
    }
}