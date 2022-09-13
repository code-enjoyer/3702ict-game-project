using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GGD
{
    public class NPC : StateController
    {
        protected NavMeshAgent _navMeshAgent;

        public NavMeshAgent NavMeshAgent => _navMeshAgent;

        protected override void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();

            base.Awake();
        }
    }
}
