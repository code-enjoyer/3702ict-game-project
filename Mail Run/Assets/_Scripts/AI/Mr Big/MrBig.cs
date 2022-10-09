using UnityEngine;

namespace GGD
{
    public class MrBig : NPCBehaviourState
    {
        [SerializeField] private BehaviourState _patrolState;
        [SerializeField] private float chaseSpeedMultiplier = 2f;
        [SerializeField] private float followTime = 5f;
        [SerializeField] private float forceApplied = 15;
        public GameObject indicator;

        private float timer;
        private float _lastMoveSpeed;
 
        PlayerController player;
        protected override void OnEnter()
        {
            player = GameManager.Instance.Player.GetComponent<PlayerController>();
            _NPC.NavMeshAgent.SetDestination(player.transform.position);
           // _NPC.NavMeshAgent.updateRotation = false;
            timer = followTime;
            indicator.SetActive(true);
            player.NumInteractions++;
            _NPC.NumInteractions++;
            _lastMoveSpeed = _NPC.NavMeshAgent.speed;
            _NPC.NavMeshAgent.speed = _NPC.NavMeshAgent.speed * chaseSpeedMultiplier;
        }

        protected override void OnExit()
        {
            _NPC.NavMeshAgent.speed = _lastMoveSpeed;
            indicator.SetActive(false);
            player.NumInteractions--;
            _NPC.NumInteractions--;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            timer -= deltaTime;
         
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
              //  _NPC.NavMeshAgent.updateRotation = true;
                _NPC.StateController.SetState(_patrolState);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Player found");
                Vector3 direction = collision.transform.position - transform.position;
                direction.y = 0f;
                direction.Normalize();
                collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * forceApplied, ForceMode.Impulse);
                _NPC.StateController.SetState(_patrolState);
            }

            //TODO collision with cart force
        }
    }
}
