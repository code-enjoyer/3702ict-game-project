using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class PlayerController : Character
    {
        [SerializeField] private float _baseSpeed = 4f;
        [SerializeField] private float _baseAccelaration = 20f;
        [SerializeField] private AnimationCurve _inverseAccelarationCurve = new AnimationCurve(
            new Keyframe(0f, 2f),
            new Keyframe(0.5f, 1f),
            new Keyframe(1f, 1f));
        [SerializeField] private float _turnSpeed = 4.2f;

        private float _xInput;
        private float _yInput;
        private Vector3 _targetMoveDirection;
        private bool _isControllable = true;
        private bool _isInCart = false;

        public Vector3 TargetMoveDirection => _targetMoveDirection;
        public bool IsControllable => _isControllable;
        public bool IsInCart => _isInCart;
        public override bool IsDetectable => !_isInCart; // NOTE: Can expand on this later

        private void Update()
        {
            HandleInput();
            HandleRotation();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleInput()
        {
            if (!_isControllable)
            {
                return;
            }

            _xInput = Input.GetAxis("Horizontal");
            _yInput = Input.GetAxis("Vertical");

            // Set target move direction to the Player's input in relation to the Camera's orientation (excluding its x axis / pitch)
            Vector3 newTargetMoveDirection = new Vector3(_xInput, 0f, _yInput);
            Vector3 camOrientation = Camera.main.transform.rotation.eulerAngles;
            camOrientation.x = 0f;
            newTargetMoveDirection = Quaternion.Euler(camOrientation) * newTargetMoveDirection;

            _targetMoveDirection = newTargetMoveDirection;
        }

        private void HandleRotation()
        {
            RotateTowards(_targetMoveDirection);
        }

        private void HandleMovement()
        {
            float accelarationFactor = _inverseAccelarationCurve.Evaluate(Vector3.Dot(TargetMoveDirection, Velocity.normalized));
            Vector3 targetVelocity = TargetMoveDirection * _baseSpeed;
            Vector3 newVelocity = Vector3.MoveTowards(Velocity, targetVelocity, _baseAccelaration * accelarationFactor * Time.fixedDeltaTime);
            Vector3 requiredAccelaration = newVelocity - Velocity;
            Rigidbody.AddForce(requiredAccelaration, ForceMode.VelocityChange);
        }

        private void RotateTowards(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.01f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            targetRotation = Quaternion.Slerp(Rigidbody.rotation, targetRotation, _turnSpeed * Time.deltaTime);
            Rigidbody.MoveRotation(targetRotation);
        }

        public void SetTargetMoveDirection(Vector3 direction)
        {
            _targetMoveDirection = direction;
            if (_targetMoveDirection.sqrMagnitude > 1f)
            {
                _targetMoveDirection.Normalize();
            }
        }

        public void SetIsControllable(bool value, bool resetTargetMoveDirection = true)
        {
            _isControllable = value;

            if (resetTargetMoveDirection)
            {
                _targetMoveDirection = Vector3.zero;
            }
        }
    }
}
