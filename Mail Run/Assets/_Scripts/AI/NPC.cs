using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GGD
{
    [RequireComponent(typeof(Rigidbody))]
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

            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            _currentState.SendMessage("OnCollisionEnter", collision);
            Debug.Log("Collision");
        }
    }
}
