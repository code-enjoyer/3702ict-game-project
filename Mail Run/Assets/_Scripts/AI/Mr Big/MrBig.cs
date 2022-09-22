using UnityEngine;

namespace GGD
{
    public class MrBig : NPCBehaviourState
    {
        [SerializeField] private BehaviourState _patrolState;
        [SerializeField] private float follow = 5f;
        [SerializeField] private float forceApplied = 15;
        public GameObject indicator;

        private float timer;

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
            if (Vector3.Distance(transform.position, player.transform.position) > 0f)
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
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Player found");
                collision.gameObject.GetComponent<Rigidbody>().AddForce(-collision.contacts[0].normal * forceApplied, ForceMode.Impulse);
            }
        }
    }
}
