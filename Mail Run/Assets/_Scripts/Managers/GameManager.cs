using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    // NOTE: Should be sufficient but can always swap to a full-fledged FSM
    // TODO: Figure out the states for the game, these are just placeholders
    public enum GameState
    {
        Paused,
        Playing,
    }

    /// <summary>
    /// Responsible for the game "flow" and state.
    /// Provides utils such as a reference to the player and public functions.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        // TODO: reference the "player" class instead of GameObject
        private GameObject _player;
        private GameState _state;

        /// <summary>
        /// A reference to the player object (if it exists).
        /// NOTE: Can be null.
        /// </summary>
        public GameObject Player => _player;
        public GameState State => _state;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            // Initialize references
            // TODO: Swap to FindGameObjectOfType<T>() when player class is made
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            switch (_state)
            {
                case GameState.Paused:
                    ExecutePaused();
                    break;
                case GameState.Playing:
                    ExecutePlaying();
                    break;
                default:
                    break;
            }
        }

        private void ExecutePaused()
        {
            // TODO: 
        }

        private void ExecutePlaying()
        {
            // TODO: 
        }
    }
}
