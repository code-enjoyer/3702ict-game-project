using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGD
{
    public abstract class BehaviourState : MonoBehaviour
    {
        [SerializeField] protected UnityEvent _entered;
        [SerializeField] protected UnityEvent _exited;

        private StateController _owner;
        protected bool _isInitialized = false;

        public StateController Owner => _owner;
        public bool IsInitialized => _isInitialized;

        public void Enter()
        {
            OnEnter();
            _entered?.Invoke();
        }

        public void Exit()
        {
            OnExit();
            _exited?.Invoke();
        }

        public virtual void Initialize(StateController owner)
        {
            _owner = owner;
        }

        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }

        public virtual void ExecuteUpdate(float deltaTime) { }
        public virtual void ExecuteFixedUpdate(float deltaTime) { }
    }
}
