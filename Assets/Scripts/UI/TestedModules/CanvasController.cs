using UnityEngine.ResourceManagement.AsyncOperations;
using LevelsController.TestedModules;
using UnityEngine.AddressableAssets;
using CommonScripts.TestedModules;
using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
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
        [SerializeField] private GameObject statusPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private LaserHand laserHand;
        public static CanvasController ClassCanvasController { get; private set; }
        private readonly List<Renderer> _gameObjectsRenderer = new();

        private LevelInfoTransfer _levelInfoTransfer;
        private SoundsController _soundsController;
        private List<AudioClip> _listAudioClips = new();
        private IEnumerator<int> _audioIterator;

        // Timer
        private float _rTimer;
        private bool _isCountdown;
        private bool _isTimerEnable;
        [SerializeField] private TMP_Text textTimer;
        
        // ImagePanel
        private RawImage _rawImageOfImagePanel;
        private static readonly Vector2 MaxDimensions = new(0.6f, 0.3f);
        
        // StatusPanel
        private TMP_Text _textResultTime;
        private GameObject _timeResult;
        private TMP_Text _statusText;
        
        // SettingsPanel
        private float _settingsPositionY;

        private Color _defaultColor;
        private Color _grayColor;
        private async void Start()
        {
            // Getting SoundsController and music resources
            var objSoundsController = GameObject.Find(CommonKeys.Names.SoundsController);
            if (objSoundsController is null)
            {
                Debug.Log("Failed to get SoundsController");
                return;
            }
            _soundsController = objSoundsController.GetComponent<SoundsController>();
            var asyncOperationHandle = Addressables.LoadAssetsAsync<AudioClip>
                (CommonKeys.Addressable.LevelMusic, _ => {});
            asyncOperationHandle.Completed += delegate
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    _listAudioClips = new List<AudioClip>(asyncOperationHandle.Result);
                else
                    Debug.Log("Failed to load music");
            };
            await asyncOperationHandle.Task;
            if (_listAudioClips.Count == 0)
                asyncOperationHandle.WaitForCompletion();
            _audioIterator = GetNextAudioIndex();
            
            // Get LaserHand of player, if not debug
            var objPlayer = GameObject.Find(CommonKeys.Names.Player);
            if (objPlayer != null)
            {
                var objRightHand = objPlayer.transform.GetChild(0).GetChild(2);
                if (objRightHand is null)
                    return;
                laserHand = objRightHand.GetComponent<LaserHand>();
                if (laserHand is null)
                {
                    Debug.Log("Failed to get LaserHand from RightHand");
                    return;
                }

                laserHand.Active = false;
            }
            
            _defaultColor = new Color(1, 1, 1);
            _grayColor = new Color(0.432f, 0.432f, 0.432f);
            new List<GameObject>(FindObjectsOfType<GameObject>()).ForEach(obj =>
            {
                Renderer gameObjectRenderer;
                if((gameObjectRenderer = obj.GetComponent<Renderer>()) != null && obj.layer != 5 && obj.layer != 6)
                    _gameObjectsRenderer.Add(gameObjectRenderer);
            });
            _rawImageOfImagePanel = imagePanel.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();

            _levelInfoTransfer = LevelInfoTransfer.GetInstance();
            
            // Enable countdown if the timer is set when starting the level
            if (_levelInfoTransfer.Timer > 0)
            {
                _rTimer = _levelInfoTransfer.Timer;
                _isCountdown = true;
            }
            
            statusPanel.SetActive(true);
            _timeResult = CommonKeys.GetComponentFromTransformOfType<Transform>(statusPanel.transform, 
                CommonKeys.Names.TimeResult)?.gameObject;
            _textResultTime = CommonKeys.GetComponentFromTransformOfType<TMP_Text>(statusPanel.transform,
                CommonKeys.Names.TextResultTime);
            _statusText = CommonKeys.GetComponentFromTransformOfType<TMP_Text>(statusPanel.transform,
                CommonKeys.Names.StatusText);
            statusPanel.SetActive(false);

            // Save the original position of settingsPanel
            _settingsPositionY = settingsPanel.transform.localPosition.y;
            
            if (ClassCanvasController == null)
                ClassCanvasController = this;
        }

        /// <summary>
        /// Returns the next index for playing music
        /// </summary>
        /// <returns>Next index to play, if the index is the last - returns to the beginning</returns>
        private IEnumerator<int> GetNextAudioIndex()
        {
            var hashSetMusicOrder = CommonKeys.GenerateNumbers(_listAudioClips.Count, _listAudioClips.Count);
            for (var i = 0; i < hashSetMusicOrder.Count;)
            {
                yield return hashSetMusicOrder.ElementAt(i);
                if (++i == hashSetMusicOrder.Count)
                    i = 0;
            }
        }

        /// <summary>
        /// Increases/decreases the timer when enabled, if the timer reaches zero, activates the damage panel
        /// </summary>
        private void Update()
        {
            if (_isTimerEnable)
                _rTimer += _isCountdown ? -Time.deltaTime : Time.deltaTime;
                    
            if(_rTimer < 10e-3 && _isTimerEnable && _isCountdown)
                ShowLosePanel();

            // If the music has stopped playing, turn on the next random music
            if (_soundsController.MusicSource.isPlaying) 
                return;
            if (_listAudioClips.Count == 0 || _audioIterator is null || !_audioIterator.MoveNext())
                return;
            var currentIndex = _audioIterator.Current;
            _soundsController.PlaySound(SoundsController.AudioSourceType.MusicSource,
                _listAudioClips[currentIndex]);
        }
        
        public void SetPause(bool isPaused)
        {
            if (Time.timeScale == 0 && isPaused && statusPanel.activeInHierarchy)
                return;
            
            StopTime(isPaused);

            textTimer.text = GetStrTime(_rTimer);
            
            // Restoring the position of the panels before and after turning on the pause
            var vBufPosition = pausePanel.transform.localPosition;
            pausePanel.transform.localPosition = new Vector3(vBufPosition.x, 0.0f, vBufPosition.z);
            vBufPosition = settingsPanel.transform.localPosition;
            settingsPanel.transform.localPosition = new Vector3(vBufPosition.x, _settingsPositionY, vBufPosition.z);
            settingsPanel.SetActive(false);
            
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
            statusPanel.SetActive(true);

            _isTimerEnable = false;
            
            _textResultTime.text = GetStrTime(_isCountdown ? _levelInfoTransfer.Timer - _rTimer : _rTimer);

            // Save Result
            if(_levelInfoTransfer.LvlNumber != CommonKeys.CustomLevelNumber)
            {
                // If this is the last level completed from the available ones, increase the counter of completed levels
                var numLevels = 0;
                bool bRes;
                var completedLevels = SaveManager.ReadData<CompletedLevels>();
                if (completedLevels != null && completedLevels.Count != 0)
                    numLevels = completedLevels.First().NumLevels;
                if(numLevels + 1 == _levelInfoTransfer.LvlNumber)
                {
                    bRes = SaveManager.WriteData(new List<CompletedLevels> { new(numLevels + 1) });
                    if (!bRes)
                        Debug.Log("Failed to save completed levels data");
                }
                
                var savedResult = SaveManager.ReadData(_levelInfoTransfer.LvlNumber);
                if (savedResult != null)
                    savedResult.TimeRecent = _rTimer;
                else
                    savedResult = new SavedResults(_levelInfoTransfer.LvlNumber, _rTimer, _rTimer);
                bRes = SaveManager.WriteData(savedResult);
                if (!bRes)
                    Debug.Log("Failed to save results data");
            }
            
            laserHand.enabled = true;
            laserHand.Active = true;
        }

        /// <summary>
        /// If the timer reaches zero, the game ends
        /// </summary>
        public void ShowLosePanel()
        {
            StopTime(true);
            statusPanel.SetActive(true);
            
            _isTimerEnable = false;
            
            _timeResult.SetActive(false);
            _statusText.text = CommonKeys.TimeIsOver;

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
        public static string GetStrTime(float rTime)
        {
            // Getting string minutes of format "00"
            var strMinute = Convert.ToInt32(Math.Floor(rTime / 60)).ToString();
            strMinute = strMinute.Length < 2 ? "0" + strMinute : strMinute;

            // Getting string seconds of format "00"
            var strSecond = Convert.ToInt32(Math.Floor(rTime % 60)).ToString();
            strSecond = strSecond.Length < 2 ? "0" + strSecond : strSecond;

            // Getting string millisecond of format "000"
            var strMillisecond = "";
            var strMillisecondFull = Convert.ToString(Math.Round(rTime - (int)rTime, 3), CultureInfo.InvariantCulture);
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