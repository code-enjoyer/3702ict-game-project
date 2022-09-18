using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GGD
{
    public class NPC : MonoBehaviour
    {
        [SerializeField] private BehaviourState _initialState;

        protected NavMeshAgent _navMeshAgent;
        protected BehaviourState _currentState;
        protected BehaviourState _lastState;

        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public BehaviourState CurrentState => _currentState;
        public BehaviourState LastState => _lastState;

        protected virtual void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();

            foreach (BehaviourState s in GetComponentsInChildren<BehaviourState>())
            {
                s.Initialize(this);
            }

            if (!_initialState.IsInitialized)
            {
                _initialState.Initialize(this);
            }

            SetState(_initialState);
        }

        protected virtual void Update()
        {
            _currentState?.ExecuteUpdate(Time.deltaTime);
        }

        protected virtual void FixedUpdate()
        {
            _currentState?.ExecuteFixedUpdate(Time.fixedDeltaTime);
        }

        public void SetState(BehaviourState targetState)
        {
            Debug.Log($"[NPC] Swapping from {_currentState?.name} to {targetState?.name}.", gameObject);

            if (_currentState != null)
            {
                _currentState.Exit();
            }

            if (targetState != null)
            {
                targetState.Enter();
            }

            _lastState = _currentState;
            _currentState = targetState;
        }

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

        private void OnCollisionEnter(Collision collision)
        {
            _currentState.SendMessage("OnCollisionEnter", collision);
        }

    }
}
