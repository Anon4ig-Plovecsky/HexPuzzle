using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace CommonScripts.TestedModules
{
    public class SoundsController : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] [MaybeNull] private AudioClip musicSound;
        [SerializeField] [MaybeNull] private GameObject objPlayer;

        private AudioClip _audioCube;
        private AudioClip _audioPillow;
        private AudioClip _audioDishes;
        private AudioClip _audioMetal;

        public AudioSource MusicSource => musicSource;
        public AudioSource SfxSource => sfxSource;
        public bool MuteSfx
        {
            get => sfxSource.mute;
            set => sfxSource.mute = value;
        }
        public bool MuteMusic
        {
            get => musicSource.mute;
            set => musicSource.mute = value;
        }

        private async void Start()
        {
            var asyncTask = CommonKeys.LoadResource<AudioClip>(CommonKeys.StrAudioNames.PillowCollision);
            await asyncTask;
            _audioPillow = asyncTask.Result;
            
            asyncTask = CommonKeys.LoadResource<AudioClip>(CommonKeys.StrAudioNames.CubeCollision);
            await asyncTask;
            _audioCube = asyncTask.Result;
            
            asyncTask = CommonKeys.LoadResource<AudioClip>(CommonKeys.StrAudioNames.DishesCollision);
            await asyncTask;
            _audioDishes = asyncTask.Result;
            
            asyncTask = CommonKeys.LoadResource<AudioClip>(CommonKeys.StrAudioNames.MetalCollision);
            await asyncTask;
            _audioMetal = asyncTask.Result;

            var objVRPlayer = GameObject.Find(CommonKeys.Names.Player);
            if (!objVRPlayer.IsUnityNull())
                objPlayer = objVRPlayer;
            
            if (musicSound == null) 
                return;
            musicSource.clip = musicSound;
            musicSource.Play();
        }
        
        /// <summary>
        /// Plays a specified sound of a specific type
        /// </summary>
        /// <param name="audioSourceType">Music or SFX</param>
        /// <param name="audioClip">Audio file</param>
        public void PlaySound(AudioSourceType audioSourceType, AudioClip audioClip)
        {
            var audioSource = audioSourceType == AudioSourceType.MusicSource ? musicSource : sfxSource;
            PlaySourceOneShoot(audioSource, audioClip);
        }
        
        /// <summary>
        /// Plays the sound of the corresponding object
        /// </summary>
        /// <param name="objType">One of the enumerable object types</param>
        /// <param name="posSource">Sound source location</param>
        public void PlaySound(ObjectType objType, Vector3 posSource)
        {
            var audioClip = objType switch
            {
                ObjectType.Cube => _audioCube,
                ObjectType.Pillow => _audioPillow,
                ObjectType.Dishes => _audioDishes,
                ObjectType.Metal => _audioMetal,
                _ => throw new ArgumentOutOfRangeException(nameof(objType), objType, null)
            };

            if(audioClip != null)
                PlaySfxByDistance(audioClip, posSource);
        }

        /// <summary>
        /// Considering the distance reproduces the sound at the calculated volume
        /// </summary>
        private void PlaySfxByDistance(AudioClip audioClip, Vector3 posSource)
        {
            if (objPlayer is null || objPlayer.IsUnityNull())
            {
                Debug.LogWarning("objPlayer is null");
                PlaySourceOneShoot(sfxSource, audioClip);
                return;
            }

            if(!MuteSfx)
                AudioSource.PlayClipAtPoint(audioClip, posSource, sfxSource.volume);
        }

        /// <summary>
        /// Kludge: if pause is set (time is 0), turns it off while the sound is playing,
        /// then turns on pause again (returns time to 0)
        /// </summary>
        private void PlaySourceOneShoot(AudioSource audioSource, AudioClip audioClip)
        {
            var time = Time.timeScale;
            if (time == 0)
                Time.timeScale = 1.0f;
            audioSource.PlayOneShot(audioClip);
            if (time == 0)
                Time.timeScale = time;
        }

        public enum AudioSourceType
        {
            MusicSource,
            SfxSource
        }

        public enum ObjectType
        {
            Cube,
            Pillow,
            Dishes,
            Metal
        }
    }
}