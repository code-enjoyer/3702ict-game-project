using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGD
{
    public abstract class StateController : MonoBehaviour
    {
        [SerializeField] private BehaviourState _initialState;

        private BehaviourState _currentState;
        private BehaviourState _lastState;

        public BehaviourState CurrentState => _currentState;
        public BehaviourState LastState => _lastState;

        /// <summary>
        /// Used for initializing references.
        /// NOTE: Must call base.Awake().
        /// </summary>
        protected virtual void Awake()
        {
            foreach (BehaviourState s in GetComponentsInChildren<BehaviourState>())
            {
                s.Initialize(this);
            }

            if (!_initialState.IsInitialized)
            {
                _initialState.Initialize(this);
            }
        }

        /// <summary>
        /// Used for further setup.
        /// NOTE: Must call base.Start().
        /// </summary>
        protected virtual void Start()
        {
            SetState(_initialState);
        }

        /// <summary>
        /// NOTE: Must call base.Update().
        /// </summary>
        protected virtual void Update()
        {
            _currentState?.ExecuteUpdate(Time.deltaTime);
        }

        /// <summary>
        /// NOTE: Must call base.FixedUpdate().
        /// </summary>
        protected virtual void FixedUpdate()
        {
            _currentState?.ExecuteFixedUpdate(Time.fixedDeltaTime);
        }

        /// <summary>
        /// Sets the current state to the specified state, calling the relevant Exit and Enter methods.
        /// </summary>
        /// <param name="targetState">The state to change to.</param>
        /// <param name="executeExit">Whether to call the Exit function on the old state.</param>
        /// <param name="executeEnter">Whether to call the Enter function on the new state.</param>
        public void SetState(BehaviourState targetState, bool executeExit = true, bool executeEnter = true)
        {
            string fromString = _currentState == null ? "No State" : _currentState.name;
            Debug.Log($"[{name}: StateController] Swapping from {fromString} to {targetState?.name}.", gameObject);

            if (executeExit && _currentState != null)
            {
                _currentState.Exit();
            }

            if (executeEnter && targetState != null)
            {
                targetState.Enter();
            }

            _lastState = _currentState;
            _currentState = targetState;
        }

        /// <summary>
        /// Resets the current state to the initial state for this controller.
        /// </summary>
        /// <param name="executeTransitionFunctions">Whether to execute the relevant Enter and Exit methods.</param>
        public void ResetToInitialState(bool executeTransitionFunctions = true)
        {
            if (executeTransitionFunctions)
            {
                SetState(_initialState);
            }
            else
            {
                _currentState = _initialState;
            }
        }
    }
}
