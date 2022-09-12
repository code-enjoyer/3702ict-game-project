using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class Karen : BehaviourState
    {
        public GameObject cube;
        [SerializeField] private int clicks = 10;
        [SerializeField] private BehaviourState _patrolState;

        private int press;

        GameObject player;

        protected override void OnEnter()
        {
            player = GameManager.Instance.Player;
            Owner.NavMeshAgent.SetDestination(player.transform.position);
            
            press = clicks;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            //TODO stop player

            if (Vector3.Distance(transform.position, player.transform.position) <= 1f)
            {
                Owner.NavMeshAgent.SetDestination(transform.position);
            }
            cube.SetActive(true);

            if (Input.GetButtonDown("Fire1"))
            {
                press--;
                Debug.Log(press);
            }

            if (press <= 0)
            {
                cube.SetActive(false);
                //TODO player can move
                Owner.SetState(_patrolState);
            }
        }
    }
}
