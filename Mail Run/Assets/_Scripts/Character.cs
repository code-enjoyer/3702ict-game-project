using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGD
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onInteract;

        private Rigidbody _rigidbody;
        private bool _isInteracting = false;

        public Rigidbody Rigidbody => _rigidbody;
        public Vector3 Velocity => _rigidbody.velocity;
        public bool IsInteracting => _isInteracting;
        public virtual bool IsDetectable => true;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SetInteracting(bool value, bool raiseEvent = true)
        {
            _isInteracting = value;
            if (raiseEvent)
            {
                _onInteract?.Invoke();
            }
        }
    }
}
