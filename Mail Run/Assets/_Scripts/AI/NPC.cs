using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GGD
{
    [RequireComponent(typeof(Rigidbody), typeof(StateController))]
    public class NPC : Character
    {
        [SerializeField] private string _displayName = "PLACEHOLDER";

        private StateController _stateController;

        protected NavMeshAgent _navMeshAgent;

        public StateController StateController => _stateController;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public string DisplayName => _displayName;

        protected override void Awake()
        {
            base.Awake();
            _stateController = GetComponent<StateController>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _stateController.UpdateActive = IsActive;
            Animator.UpdateValues(_navMeshAgent.velocity.magnitude / _navMeshAgent.speed);
        }

        private void OnCollisionEnter(Collision collision)
        {
            _stateController.CurrentState.SendMessage("OnCollisionEnter", collision);
            Debug.Log("Collision");
        }

        public override void SetActive(bool value, float duration = 0)
        {
            base.SetActive(value, duration);
        }
    }
}
