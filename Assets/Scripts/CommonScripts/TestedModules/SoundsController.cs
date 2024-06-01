using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CommonScripts.TestedModules
{
    public class SoundsController : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] [MaybeNull] private AudioClip musicSound;

        private void Start()
        {
            if(musicSound != null)
                PlaySound(AudioSourceType.MusicSource, musicSound);
        }
        public void PlaySound(AudioSourceType audioSourceType, AudioClip audioClip)
        {
            var audioSource = audioSourceType == AudioSourceType.MusicSource ? musicSource : sfxSource;
            audioSource.PlayOneShot(audioClip);
        }

        public enum AudioSourceType
        {
            MusicSource,
            SfxSource
        }
    }
}