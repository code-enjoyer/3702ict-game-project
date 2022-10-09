using System;
using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;

namespace GGD
{
    public abstract class BehaviourState : MonoBehaviour
    {
        public UltEvent _onEnter;
        public UltEvent _onExit;

        private StateController _owner;
        private bool _isInitialized = false;

        public StateController Owner => _owner;
        public bool IsInitialized => _isInitialized;

        public void Enter()
        {
            OnEnter();
            _onEnter?.Invoke();
        }

        public void Exit()
        {
            OnExit();
            _onExit?.Invoke();
        }

        public virtual void Initialize(StateController owner)
        {
            _isInitialized = true;
            _owner = owner;
        }

        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }

        public virtual void ExecuteUpdate(float deltaTime) { }
        public virtual void ExecuteFixedUpdate(float deltaTime) { }
    }
}
