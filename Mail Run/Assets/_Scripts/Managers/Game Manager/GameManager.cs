using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    /// <summary>
    /// Responsible for the game "flow" and state.
    /// Provides utils such as a reference to the player and public functions.
    /// </summary>
    [DefaultExecutionOrder(-60)]
    [RequireComponent(typeof(StateController))]
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] GameState _playingState;
        [SerializeField] GameState _pausedState;


        // TODO: reference the "player" class instead of GameObject
        private PlayerController _player;
        private StateController _stateController;

        /// <summary>
        /// A reference to the player object (if it exists).
        /// NOTE: Can be null.
        /// </summary>
        public PlayerController Player => _player;

        public GameState PlayingState => _playingState;
        public GameState PausedState => _pausedState;

        protected override void Awake()
        {
            base.Awake();

            // Initialize references
            // TODO: Swap to FindGameObjectOfType<T>() when player class is made?
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
                _player = go.GetComponent<PlayerController>();

            _stateController = GetComponent<StateController>();
        }

        public void SetState(GameState state)
        {
            _stateController.SetState(state);
        }
    }
}
