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

        PlayerController player;

        protected override void OnEnter()
        {
            player = GameManager.Instance.Player.GetComponent<PlayerController>();
            player.MultiplySpeedMultiplier(0.5f);

            _NPC.NavMeshAgent.SetDestination(player.transform.position);
            timer = follow;

            indicator.SetActive(true);
            player.NumInteractions++;
            _NPC.NumInteractions++;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            timer -= deltaTime;
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
                player.NumInteractions--;
                _NPC.NumInteractions--;
                player.MultiplySpeedMultiplier(1f / 0.5f);
                _NPC.StateController.SetState(_patrolState);
            }
        }
    }
}
