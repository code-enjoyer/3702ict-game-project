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
            Owner.NavMeshAgent.SetDestination(transform.position);
            player = GameManager.Instance.Player;
            press = clicks;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            //TODO stop player

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
