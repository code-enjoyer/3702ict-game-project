using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class Intern : BehaviourState
    {

        [SerializeField] private BehaviourState _patrolState;
        [SerializeField] private float follow = 5f;
        private float timer;

        GameObject player;

        protected override void OnEnter()
        {
            player = GameManager.Instance.Player;
            Owner.NavMeshAgent.SetDestination(player.transform.position);
            timer = follow;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            timer -= deltaTime;
            player = GameManager.Instance.Player;
            if (Vector3.Distance(transform.position, player.transform.position) >= 1f)
            {
                _owner.NavMeshAgent.SetDestination(player.transform.position);
            }
            else
            {
                _owner.NavMeshAgent.SetDestination(transform.position);
            }

            if (timer <= 0f)
            {
                Owner.SetState(_patrolState);
            }
        }
    }
}
