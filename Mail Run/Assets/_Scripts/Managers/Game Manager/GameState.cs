using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    /// <summary>
    /// Empty base state to separate from other BehaviourStates, to be used by the GameManager.
    /// </summary>
    public class GameState : BehaviourState
    {
        private GameManager _gameManager;

        public override void Initialize(StateController owner)
        {
            base.Initialize(owner);

            _gameManager = GetComponent<GameManager>();
            if (_gameManager == null)
                _gameManager = GameManager.Instance;
        }
    }
}
