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
        private bool _isActive = true;

        private int _numInteractions;
        public int NumInteractions
        {
            get => _numInteractions;
            set
            {
                _numInteractions = value;
                _isInteracting = _numInteractions > 0;
            }
        }
        public Rigidbody Rigidbody => _rigidbody;
        public Vector3 Velocity => _rigidbody.velocity;
        public bool IsInteracting => _isInteracting;
        public virtual bool IsDetectable => true;
        public bool IsActive => _isActive;

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

        public virtual void SetActive(bool value, float duration = 0f)
        {
            _isActive = value;

            if (duration > 0f)
            {
                StartCoroutine(ResetActive(duration));
            }
        }

        private IEnumerator ResetActive(float duration)
        {
            yield return new WaitForSeconds(duration);
            _isActive = !_isActive;
        }    
    }
}
