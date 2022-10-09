using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _audioClips;

        private void Awake()
        {
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();
        }

        [ContextMenu("Test")]
        public void PlayRandom()
        {
            // _audioSource.PlayOneShot(_audioClips[Random.Range(0, _audioClips.Length)]);
            _audioSource.clip = _audioClips[Random.Range(0, _audioClips.Length)];
            _audioSource.Play();
        }
    }
}
