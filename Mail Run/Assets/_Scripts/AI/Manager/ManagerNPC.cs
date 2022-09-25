using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class ManagerNPC : NPCBehaviourState
    {

        [SerializeField] private BehaviourState _patrolState;
        [SerializeField] private float delay = 1f;
        private float timer;
        public GameObject indicator;

        PlayerController player;

        protected void Start()
        {
            player = GameManager.Instance.Player.GetComponent<PlayerController>();
        }

        protected override void OnEnter()
        {
            if (!player.IsInteracting)
            {
                _NPC.StateController.SetState(_patrolState);
                return;
            }
            _NPC.NavMeshAgent.SetDestination(player.transform.position);
            timer = delay;
            indicator.SetActive(true);
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            if (!player.IsInteracting)
            {
                indicator.SetActive(false);
                _NPC.StateController.SetState(_patrolState);
                return;
            }
            timer -= deltaTime;
            _NPC.NavMeshAgent.SetDestination(transform.position);

            if (timer <= 0f)
            {
                player.transform.position = new Vector3(0, 0, 0);
                indicator.SetActive(false);
                player.NumInteractions--;
                _NPC.StateController.SetState(_patrolState);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {

        }
    }
}

