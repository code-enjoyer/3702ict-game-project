using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GGD
{
    public class ManagerNPCPatrol : NPCBehaviourState
    {
        public enum PatrolMethod
        {
            PingPong,
            Loop
        }

        [SerializeField] private PatrolMethod _patrolMethod = PatrolMethod.PingPong;
        [SerializeField] private Transform[] _waypoints = new Transform[1];
        [SerializeField] private List<float> _waitTimes = new List<float>();
        [SerializeField] private BehaviourState _idleState;
        [SerializeField] private float los = 3;
        [SerializeField] private float fov = 45;
        [SerializeField] private LayerMask mask = 1;
        [SerializeField] private float coolDown = 3f;
        [SerializeField] private float _idleTimeMin = 2f;
        [SerializeField] private float _idleTimeMax = 10f;

        private bool _waypointDirection = true; // True means ascending, false means descending - used for ping-pong patrol method.
        private float delay = 1f;
        private float timer;
        private float timer2;
        private float _idleTimer;
        private bool sighted;
        private int _currentWaypointIndex = 0;
        private bool idle;
        private bool _waited;
        PlayerController player;
        Character person;
        [SerializeField] private GameObject eyes;
        public GameObject indicator;

        protected override void OnEnter()
        {
            _NPC.NavMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
            player = GameManager.Instance.Player?.GetComponent<PlayerController>();
            timer2 = delay;
            if (!(_NPC.StateController.LastState == _idleState))
                timer = coolDown;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            timer -= deltaTime;
            if (timer > 0)
            {
                // Debug.Log("On cooldown");
            }
            // TODO: Use a variable for "effective" stopping distance
            if (!idle && _NPC.NavMeshAgent.remainingDistance < 0.5f)
            {
                if (_waitTimes[_currentWaypointIndex] > 0f)
                {
                    idle = true;
                    _idleTimer = _waitTimes[_currentWaypointIndex];
                }

                _currentWaypointIndex += _waypointDirection ? 1 : -1;

                switch (_patrolMethod)
                {
                    case PatrolMethod.PingPong:
                        if (_currentWaypointIndex >= _waypoints.Length || _currentWaypointIndex < 0)
                        {
                            _waypointDirection = !_waypointDirection;
                            _currentWaypointIndex += _waypointDirection ? 1 : -1;
                        }
                        break;
                    case PatrolMethod.Loop:
                        if (_currentWaypointIndex >= _waypoints.Length)
                        {
                            _currentWaypointIndex = 0;
                        }
                        break;
                    default:
                        break;
                }

                _NPC.NavMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
            }

            //if (Random.value <= 0.0001f && !(idle))
            //{
            //    _idleTimer = Random.Range(_idleTimeMin, _idleTimeMax);
            //    idle = true;
            //}

            if(idle)
            {
                _idleTimer -= deltaTime;

                if (_idleTimer > 0)
                    Idle();
                else
                    idle = false;
            }

            if(timer <= 0f)
                LineOfSight();

            if (sighted)
            {
                _NPC.NavMeshAgent.SetDestination(player.transform.position);
                indicator.SetActive(true);
                _NPC.NavMeshAgent.SetDestination(transform.position);
                timer2 -= deltaTime;
                if(timer2 <= 0)
                    Caught();
            }
        }

        private void Idle()
        {
            NPC.NavMeshAgent.SetDestination(transform.position);
            
        }

        private void OnValidate()
        {
            //if (_waypoints.Length == 0)
            //{
            //    Debug.LogWarning("[GenericPatrolState] No transforms in the waypoints array!", gameObject);
            //}
            //else if (_waypoints.Any(x => x == null))
            //{
            //    Debug.LogWarning("[GenericPatrolState] There are null transforms in the waypoints array!", gameObject);
            //}

            while (_waitTimes.Count < _waypoints.Length)
            {
                _waitTimes.Add(0f);
            }
            while (_waitTimes.Count > _waypoints.Length)
            {
                _waitTimes.RemoveAt(_waitTimes.Count - 1);
            }
        }

        void LineOfSight()
        {
            RaycastHit hit;

            //if(Physics.Raycast(eyes.transform.position + (Vector3.left * 0.2f), direction, out hit, los))
            Collider[] items = Physics.OverlapSphere(eyes.transform.position, los, mask);

            if (items.Length > 0)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] == GetComponent<Collider>())
                    {
                        continue;
                    }

                    if (Vector3.Angle(transform.forward, items[i].transform.position - transform.position) <= fov)
                    {
                        if (Physics.Raycast(eyes.transform.position, items[i].transform.position - transform.position, out hit, los, mask))
                        {
                            if (hit.transform.CompareTag("NPC") || hit.transform.CompareTag("Player"))
                            {
                                person = hit.transform.GetComponent<Character>();
                                if (person.IsInteracting)
                                    sighted = true;
                            }
                        }
                    }
                }
            }
        }

        void Caught()
        {
            player.transform.position = LevelManager.Instance.SpawnLocation;
            indicator.SetActive(false);
            player.NumInteractions--;
            sighted = false;
            OnEnter();
        }

        void OnDrawGizmos()
        {

            Gizmos.color = Color.red;

            Quaternion upRayRotation = Quaternion.AngleAxis(-fov, Vector3.up);
            Quaternion downRayRotation = Quaternion.AngleAxis(fov, Vector3.up);

            Vector3 upRayDirection = upRayRotation * transform.forward * los;
            Vector3 downRayDirection = downRayRotation * transform.forward * los;

            Gizmos.DrawRay(eyes.transform.position, upRayDirection);
            Gizmos.DrawRay(eyes.transform.position, downRayDirection);
            Gizmos.DrawLine(eyes.transform.position + downRayDirection, eyes.transform.position + upRayDirection);
        }

        public void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.tag == "Player" && timer <= 0)
            {
                Debug.Log("Player found");
                player.NumInteractions++;
                sighted = true;
            }
        }
    }
}