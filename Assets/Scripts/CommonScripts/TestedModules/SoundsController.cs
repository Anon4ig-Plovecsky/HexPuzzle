using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using System;

namespace CommonScripts.TestedModules
{
    public class SoundsController : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] [MaybeNull] private AudioClip musicSound;

        private AudioClip _audioCube;
        private AudioClip _audioPillow;
        private AudioClip _audioDishes;
        private AudioClip _audioMetal;

        public AudioSource MusicSource => musicSource;
        public bool MuteSfx
        {
            get => sfxSource.mute;
            set => sfxSource.mute = value;
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
            audioSource.PlayOneShot(audioClip);
        }
        
        /// <summary>
        /// Plays the sound of the corresponding object
        /// </summary>
        /// <param name="objType">One of the enumerable object types</param>
        public void PlaySound(ObjectType objType)
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
                sfxSource.PlayOneShot(audioClip);
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