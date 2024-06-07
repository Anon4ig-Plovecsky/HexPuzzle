using CommonScripts.TestedModules;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Checks the saved data for saving and enters it into the settings panel
    /// </summary>
    public class SettingsController : MonoBehaviour
    {
        private Toggle _toggleMusic, _toggleSfx;
        private SoundsController _soundsController;

        private void Start()
        {
            _toggleMusic = CommonKeys.GetComponentFromTransformOfType<Toggle>(
                transform, CommonKeys.Names.MusicTogglePath);
            _toggleSfx = CommonKeys.GetComponentFromTransformOfType<Toggle>(
                transform, CommonKeys.Names.SfxTogglePath);
            if (_toggleMusic.IsUnityNull() || _toggleSfx.IsUnityNull())
                return;

            var objSoundsController = GameObject.Find(CommonKeys.Names.SoundsController);
            if (objSoundsController is null)
            {
                Debug.Log("Failed to get soundsController");
                return;
            }
            _soundsController = objSoundsController.GetComponent<SoundsController>();
            if (_soundsController is null)
                return;
            
            _toggleMusic.onValueChanged.AddListener(delegate { SettingsButtonOnClick(_toggleMusic); });
            _toggleSfx.onValueChanged.AddListener(delegate { SettingsButtonOnClick(_toggleSfx); });

            // Loading saved results
            var sfxStatus = PlayerPrefs.GetInt(CommonKeys.Names.SfxStatus);
            var musicStatus = PlayerPrefs.GetInt(CommonKeys.Names.MusicStatus);
            _soundsController.MuteSfx = sfxStatus == 1;
            _soundsController.MuteMusic = musicStatus == 1;
            _toggleSfx.isOn = !_soundsController.MuteSfx;
            _toggleMusic.isOn = !_soundsController.MuteMusic;
            
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Disables/enables music/sound effects
        /// </summary>
        private void SettingsButtonOnClick(Toggle toggle)
        {
            var isOn = toggle.isOn;
            var audioSourceType = toggle.name.Equals(CommonKeys.StrButtonNames.MusicToggle)
                ? SoundsController.AudioSourceType.MusicSource
                : SoundsController.AudioSourceType.SfxSource;
            
            if (_soundsController is null)
            {
                var objSoundsController = GameObject.Find(CommonKeys.Names.SoundsController);
                if (objSoundsController is null)
                    return;
                _soundsController = objSoundsController.GetComponent<SoundsController>();
            }

            if (audioSourceType == SoundsController.AudioSourceType.MusicSource)
                _soundsController.MuteMusic = !isOn;
            else
                _soundsController.MuteSfx = !isOn;
        }

        /// <summary>
        /// Saves settings when exiting the settings panel
        /// </summary>
        private void OnDisable()
        {
            if (_soundsController is null)
                return;
            PlayerPrefs.SetInt(CommonKeys.Names.SfxStatus, _soundsController.MuteSfx ? 1 : 0);
            PlayerPrefs.SetInt(CommonKeys.Names.MusicStatus, _soundsController.MuteMusic ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}