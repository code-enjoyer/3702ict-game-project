using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class ManagerNPC : BehaviourState
    {

        [SerializeField] private BehaviourState _patrolState;
        [SerializeField] private float delay = 1f;
        private float timer;
        public GameObject indicator;

        GameObject player;

        protected override void OnEnter()
        {
            player = GameManager.Instance.Player;
            Owner.NavMeshAgent.SetDestination(player.transform.position);
            timer = delay;
            indicator.SetActive(true);
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            timer -= deltaTime;
            player = GameManager.Instance.Player;
            _owner.NavMeshAgent.SetDestination(transform.position);
           
            if (timer <= 0f)
            {
                player.transform.position = new Vector3(0,0,0);
                indicator.SetActive(false);
                Owner.SetState(_patrolState);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {

        }
    }
}

