using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class GenericIdleState : NPCBehaviourState
    {
        [SerializeField] private float _idleTimeMin = 2f;
        [SerializeField] private float _idleTimeMax = 10f;
        [SerializeField] private BehaviourState _harassState;
        [SerializeField] private float coolDown = 3f;
        private float timer;
        PlayerController player;

        private float _timer;

        protected override void OnEnter()
        {
            _NPC.NavMeshAgent.SetDestination(transform.position);
            _timer = Random.Range(_idleTimeMin, _idleTimeMax);
            player = GameManager.Instance.Player.GetComponent<PlayerController>();
            timer = coolDown;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            _timer -= deltaTime;
            timer -= deltaTime;
            if (_timer <= 0f)
            {
                _NPC.StateController.SetState(_NPC.StateController.LastState);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.CompareTag("Player") && timer <= 0)
            {
                Debug.Log("Player found");
                player.NumInteractions++;
                Owner.SetState(_harassState);
            }
        }
    }
}
