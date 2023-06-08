using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject imagePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private LaserHand laserHand;
    public static CanvasController classCanvasController { get; private set; }
    private readonly List<Renderer> gameObjectsRenderer = new();
    
    //ImagePanel
    private RawImage rawImageOfImagePanel;
    private static readonly Vector2 MaxDimensions = new(0.6f, 0.3f);

    private Color defaultColor;
    private Color grayColor;
    private void Start()
    {
        if (classCanvasController == null)
            classCanvasController = this;
        defaultColor = new Color(1, 1, 1);
        grayColor = new Color(0.432f, 0.432f, 0.432f);
        new List<GameObject>(FindObjectsOfType<GameObject>()).ForEach(obj =>
        {
            Renderer gameObjectRenderer;
            if((gameObjectRenderer = obj.GetComponent<Renderer>()) != null && obj.layer != 5 && obj.layer != 6)
                gameObjectsRenderer.Add(gameObjectRenderer);
        });
        rawImageOfImagePanel = imagePanel.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
    }
    public void SetPause(bool isPaused)
    {
        StopTime(isPaused);
        pausePanel.SetActive(isPaused);
        imagePanel.SetActive(isPaused);
        laserHand.enabled = true;
        laserHand.active = isPaused;
    }
    public IEnumerator ShowImage(Sprite sprite, Vector2 dimension)
    {
        imagePanel.SetActive(true);
        rawImageOfImagePanel.texture = sprite.texture;
        rawImageOfImagePanel.rectTransform.sizeDelta = GetImageDimension(dimension);
        StopTime(true);
        yield return new WaitForSecondsRealtime(4.0f);
        StopTime(false);
        imagePanel.SetActive(false);
    }
    private Vector2 GetImageDimension(Vector2 dimension)
    {
        var scale = Math.Max(Math.Floor(dimension.y / MaxDimensions.y),
            Math.Floor(dimension.x / MaxDimensions.x));
        return new Vector2((float)(dimension.x / scale), (float)(dimension.y / scale));
    }
    public void ShowWinPanel()
    {
        StopTime(true);
        winPanel.SetActive(true);
        laserHand.enabled = true;
        laserHand.active = true;
    }
    private void StopTime(bool isStop)
    {
        Time.timeScale = isStop ? 0 : 1;
        foreach (var rendererGameObject in gameObjectsRenderer)
            rendererGameObject.material.color = isStop ? grayColor : defaultColor;
    }
    public static void DestroyClass()
    {
        classCanvasController = null;
    }
}