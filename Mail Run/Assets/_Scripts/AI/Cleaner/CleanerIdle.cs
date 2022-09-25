using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class CleanerIdle : NPCBehaviourState
    {
        [SerializeField] private float _idleTimeMin = 2f;
        [SerializeField] private float _idleTimeMax = 10f;
        public GameObject puddle;

        private float _timer;

        protected override void OnEnter()
        {
            _NPC.NavMeshAgent.SetDestination(transform.position);
            _timer = Random.Range(_idleTimeMin, _idleTimeMax);
            GameObject instance = Instantiate(puddle, transform.position,
            transform.rotation);
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            _timer -= deltaTime;
            if (_timer <= 0f)
            {
                _NPC.StateController.SetState(_NPC.StateController.LastState);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.CompareTag("Player"))
            {
                OnEnter();
            }
        }
    }
}
