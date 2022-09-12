using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class MrBig : BehaviourState
    {

        [SerializeField] private BehaviourState _patrolState;
        [SerializeField] private float follow = 5f;
        [SerializeField] private float forceApplied = 100;

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
            if (Vector3.Distance(transform.position, player.transform.position) > 0f)
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

        private void OnCollisionEnter(Collision collision)
        {
            
            if(collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.contacts[0].normal * forceApplied);
            }
        }
    }
}
