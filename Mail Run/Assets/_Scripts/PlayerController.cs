using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _baseSpeed = 4f;
        [SerializeField] private float _baseAccelaration = 20f;
        [SerializeField] private AnimationCurve _inverseAccelarationCurve = new AnimationCurve(
            new Keyframe(0f, 2f),
            new Keyframe(0.5f, 1f),
            new Keyframe(1f, 1f));

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

            _targetVelocity = Vector3.Lerp(_targetVelocity, direction * _baseSpeed, Time.deltaTime * 69f);
        }

        private void FixedUpdate()
        {
            float accelarationFactor = _inverseAccelarationCurve.Evaluate(Vector3.Dot(_targetVelocity, Velocity));
            Vector3 newVelocity = Vector3.MoveTowards(Velocity, _targetVelocity, _baseAccelaration * accelarationFactor * Time.fixedDeltaTime);
            Vector3 requiredAccelaration = (newVelocity - Velocity) / Time.fixedDeltaTime;
            _rigidbody.AddForce(requiredAccelaration, ForceMode.Acceleration);
        }
    }
}
