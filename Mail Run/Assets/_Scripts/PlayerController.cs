using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _baseSpeed = 4f;
        [SerializeField] private float _baseAccelaration = 20f;

        private Rigidbody _rigidbody;

        private float _xInput;
        private float _yInput;
        private Vector3 _targetVelocity;

        public Vector3 Velocity => _rigidbody.velocity;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

        }

        private void Start()
        {
            
        }

        private void Update()
        {
            _xInput = Input.GetAxis("Horizontal");
            _yInput = Input.GetAxis("Vertical");
            
            Vector3 direction = new Vector3(_xInput, 0f, _yInput);
            
            if (direction.sqrMagnitude > 1f)
            {
                direction.Normalize();
            }

            _targetVelocity = direction * _baseSpeed;
        }

        private void FixedUpdate()
        {
            Vector3 newVelocity = Vector3.MoveTowards(Velocity, _targetVelocity, _baseAccelaration * Time.fixedDeltaTime);
            Vector3 requiredAccelaration = newVelocity - Velocity;
            _rigidbody.AddForce(requiredAccelaration, ForceMode.VelocityChange);
        }
    }
}
