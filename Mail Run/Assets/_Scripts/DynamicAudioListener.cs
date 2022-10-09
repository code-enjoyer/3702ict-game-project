using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class DynamicAudioListener : MonoBehaviour
    {
        [SerializeField] private AudioListener _audioListener;

        // Update is called once per frame
        void LateUpdate()
        {
            if (GameManager.IsInitialized && GameManager.Instance.Player != null)
            {
                _audioListener.transform.position = GameManager.Instance.Player.transform.position;
            }
            else
            {
                _audioListener.transform.position = Camera.main.transform.position;
            }
        }
    }
}
