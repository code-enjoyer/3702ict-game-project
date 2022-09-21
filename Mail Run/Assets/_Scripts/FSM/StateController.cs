using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    /// <summary>
    /// Base class for controlling and executing states. Deriving state controllers can
    /// provide further public members and specific control over the flow of states.
    /// e.g. NPC provides access to a NavMeshAgent.
    /// </summary>
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
        /// Simply calls UpdateCurrentState().
        /// Override to add further logic for the flow of states (e.g. an exit condition for any state
        /// the controller might be in).
        /// NOTE: Must call base.Update() or UpdateCurrentState().
        /// </summary>
        protected virtual void Update()
        {
            UpdateCurrentState(Time.deltaTime);
        }

        /// <summary>
        /// Simply calls FixedUpdateCurrentState().
        /// Override to add further logic (e.g. physics logic that is independant of the current state).
        /// NOTE: Must call base.FixedUpdate() or FixedUpdateCurrentState().
        /// </summary>
        protected virtual void FixedUpdate()
        {
            FixedUpdateCurrentState(Time.fixedDeltaTime);
        }

        /// <summary>
        /// Simply used to update the current state.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected void UpdateCurrentState(float deltaTime)
        {
            _currentState?.ExecuteUpdate(deltaTime);
        }

        /// <summary>
        /// Simply used to update the current state in FixedUpdate().
        /// </summary>
        /// <param name="deltaTime"></param>
        protected void FixedUpdateCurrentState(float deltaTime)
        {
            _currentState?.ExecuteFixedUpdate(deltaTime);
        }

        /// <summary>
        /// Sets the current state to the specified state, calling the relevant Exit and Enter methods.
        /// </summary>
        /// <param name="targetState">The state to change to.</param>
        /// <param name="executeExit">Whether to call the Exit function on the old state.</param>
        /// <param name="executeEnter">Whether to call the Enter function on the new state.</param>
        /// <param name="executeIfSameState">Whether to execute the state change if the new state is the current state.</param>
        public void SetState(BehaviourState targetState, bool executeExit = true, bool executeEnter = true, bool executeIfSameState = false)
        {
            if (executeIfSameState || _currentState != targetState)
            {
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
