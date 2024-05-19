using LevelsController.TestedModules;
using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace UI.TestedModules
{
    /// CanvasController is responsible for the behavior of the game's GUI panels,
    /// as well as their interaction with the game environment
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject imagePanel;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private LaserHand laserHand;
        public static CanvasController ClassCanvasController { get; private set; }
        private readonly List<Renderer> _gameObjectsRenderer = new();

        // Timer
        private float _rTimer;
        private bool _isCountdown;
        private bool _isTimerEnable;
        [SerializeField] private TMP_Text textTimer;
        
        //ImagePanel
        private RawImage _rawImageOfImagePanel;
        private static readonly Vector2 MaxDimensions = new(0.6f, 0.3f);

        private Color _defaultColor;
        private Color _grayColor;
        private void Start()
        {
            if (ClassCanvasController == null)
                ClassCanvasController = this;
            _defaultColor = new Color(1, 1, 1);
            _grayColor = new Color(0.432f, 0.432f, 0.432f);
            new List<GameObject>(FindObjectsOfType<GameObject>()).ForEach(obj =>
            {
                Renderer gameObjectRenderer;
                if((gameObjectRenderer = obj.GetComponent<Renderer>()) != null && obj.layer != 5 && obj.layer != 6)
                    _gameObjectsRenderer.Add(gameObjectRenderer);
            });
            _rawImageOfImagePanel = imagePanel.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();

            // Enable countdown if the timer is set when starting the level
            var levelInfoTransfer = LevelInfoTransfer.GetInstance();
            if (!(levelInfoTransfer.Timer > 0)) 
                return;
            _rTimer = levelInfoTransfer.Timer;
            _isCountdown = true;
        }

        /// <summary>
        /// Increases/decreases the timer when enabled, if the timer reaches zero, activates the damage panel
        /// </summary>
        private void Update()
        {
            if (_isTimerEnable)
                _rTimer += _isCountdown ? -Time.deltaTime : Time.deltaTime;
        }
        
        public void SetPause(bool isPaused)
        {
            StopTime(isPaused);

            textTimer.text = GetStrTime();
            pausePanel.SetActive(isPaused);
            
            imagePanel.SetActive(isPaused);
            
            laserHand.enabled = true;
            laserHand.Active = isPaused;
        }
        
        /// <summary>
        /// Pauses the game by showing the player an image that he needs to collect
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="dimension">Image size in one part of the cube</param>
        /// <returns></returns>
        public IEnumerator ShowImage(Sprite sprite, Vector2 dimension)
        {
            imagePanel.SetActive(true);
            _rawImageOfImagePanel.texture = sprite.texture;
            _rawImageOfImagePanel.rectTransform.sizeDelta = GetImageDimension(dimension);
            StopTime(true);
            yield return new WaitForSecondsRealtime(4.0f);
            StopTime(false);
            imagePanel.SetActive(false);

            _isTimerEnable = true;
        }
        private static Vector2 GetImageDimension(Vector2 dimension)
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
            laserHand.Active = true;
        }
        private void StopTime(bool isStop)
        {
            Time.timeScale = isStop ? 0 : 1;
            foreach (var rendererGameObject in _gameObjectsRenderer)
                rendererGameObject.material.color = isStop ? _grayColor : _defaultColor;
        }
        public static void DestroyClass()
        {
            ClassCanvasController = null;
        }

        /// <summary>
        /// Returns the string time format "00:00:000" of the timer
        /// </summary>
        private string GetStrTime()
        {
            // Getting string minutes of format "00"
            var strMinute = Convert.ToInt32(Math.Floor(_rTimer / 60)).ToString();
            strMinute = strMinute.Length < 2 ? "0" + strMinute : strMinute;

            // Getting string seconds of format "00"
            var strSecond = Convert.ToInt32(Math.Floor(_rTimer % 60)).ToString();
            strSecond = strSecond.Length < 2 ? "0" + strSecond : strSecond;

            // Getting string millisecond of format "000"
            var strMillisecond = "";
            var strMillisecondFull = Convert.ToString(Math.Round(_rTimer - (int)_rTimer, 3), CultureInfo.InvariantCulture);
            var arrTimer = strMillisecondFull.Split('.', ',');
            if (arrTimer.Length > 1)
                strMillisecond = arrTimer[1].Length > 3 ? arrTimer[1][..3] : arrTimer[1];
            for (var i = strMillisecond.Length; i < 3; i++)
                strMillisecond += '0';

            // Creating the format "00:00:000"
            var strTime = $"{strMinute}:{strSecond}:{strMillisecond}";
            
            return strTime;
        }
    }
}