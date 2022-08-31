using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class GenericIdleState : NPCBehaviourState
    {
        [SerializeField] private float _idleTimeMin = 2f;
        [SerializeField] private float _idleTimeMax = 10f;

        private float _timer;

        protected override void OnEnter()
        {
            _NPC.NavMeshAgent.SetDestination(transform.position);
            _timer = Random.Range(_idleTimeMin, _idleTimeMax);
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            _timer -= deltaTime;
            if (_timer <= 0f)
            {
                _NPC.SetState(_NPC.LastState);
            }
        }
    }
}
