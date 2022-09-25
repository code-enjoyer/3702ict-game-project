using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class Puddle : MonoBehaviour
    {
        [SerializeField] private float muliplier = 0.5f;
        public float lifeTime = 6.0f;
        private float elapsedTime;
        private bool playerInside;

        PlayerController player;
        // Use this for initialization
        void Start()
        {
            elapsedTime = 0.0f;
            player = GameManager.Instance.Player.GetComponent<PlayerController>();
        }
        // Update is called once per frame
        void Update()
        {
            if (elapsedTime >= lifeTime)
            {
                if (playerInside)
                {
                    player.MultiplySpeedMultiplier(1f / muliplier);
                    player.NumInteractions--;
                }

                Destroy(gameObject);
            }

            elapsedTime += Time.deltaTime;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                player.MultiplySpeedMultiplier(muliplier);
                player.NumInteractions ++;
                playerInside = true;
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                player.MultiplySpeedMultiplier(1f / muliplier);
                player.NumInteractions--;
                playerInside = false;
            }
        }
    }
}