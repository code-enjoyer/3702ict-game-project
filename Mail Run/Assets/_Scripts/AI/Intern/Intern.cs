using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class Intern : NPCBehaviourState
    {

        [SerializeField] private BehaviourState _patrolState;
        [SerializeField] private float follow = 5f;
        private float timer;
        public GameObject indicator;

        GameObject player;

        protected override void OnEnter()
        {
            player = GameManager.Instance.Player;
            _NPC.NavMeshAgent.SetDestination(player.transform.position);
            timer = follow;
            indicator.SetActive(true);
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            timer -= deltaTime;
            player = GameManager.Instance.Player;
            if (Vector3.Distance(transform.position, player.transform.position) >= 1f)
            {
                _NPC.NavMeshAgent.SetDestination(player.transform.position);
            }
            else
            {
                _NPC.NavMeshAgent.SetDestination(transform.position);
            }

            if (timer <= 0f)
            {
                indicator.SetActive(false);
                _NPC.StateController.SetState(_patrolState);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
           
        }
    }
}
