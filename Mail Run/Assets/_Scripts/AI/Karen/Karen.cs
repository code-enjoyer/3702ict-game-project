using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class Karen : NPCBehaviourState
    {
        public GameObject cube;
        public GameObject indicator;
        [SerializeField] private int clicks = 10;
        [SerializeField] private BehaviourState _patrolState;

        private int press;

        PlayerController player;

        protected override void OnEnter()
        {
            player = GameManager.Instance.Player.GetComponent<PlayerController>();
            _NPC.NavMeshAgent.SetDestination(player.transform.position);

            player.SetInteracting(true);
            _NPC.SetInteracting(true);
            player.SetIsControllable(false);

            press = clicks;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= 1f)
            {
                _NPC.NavMeshAgent.SetDestination(transform.position);
            }
            cube.SetActive(true);
            indicator.SetActive(true);

            if (Input.GetButtonDown("Fire1"))
            {
                press--;
                Debug.Log(press);
            }

            if (press <= 0)
            {
                cube.SetActive(false);
                indicator.SetActive(false);
                player.SetInteracting(false);
                _NPC.SetInteracting(false);
                player.SetIsControllable(true);
                //TODO player can move
                _NPC.StateController.SetState(_patrolState);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
          
        }
    }
}
