using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGD
{
    public class CleanerPatrol : NPCBehaviourState
    {
        public enum PatrolMethod
        {
            PingPong,
            Loop
        }

        [SerializeField] private PatrolMethod _patrolMethod = PatrolMethod.PingPong;
        [SerializeField] private Transform[] _waypoints = new Transform[1];
        [SerializeField] private BehaviourState _idleState;
        [SerializeField] private float los = 3;
        [SerializeField] private float fov = 45;
        [SerializeField] private LayerMask mask = 1;
        [SerializeField] private float coolDown = 3f;
        private float timer;
        private bool _waypointDirection = true;
        private int _currentWaypointIndex = 0;
        [SerializeField] private GameObject eyes;

        protected override void OnEnter()
        {
            _NPC.NavMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
            timer = coolDown;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            timer -= deltaTime;
            // TODO: Use a variable for "effective" stopping distance
            if (_NPC.NavMeshAgent.remainingDistance < 1f)
            {
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
            if (timer <= 0f)
            {
                Owner.SetState(_idleState);
            }
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
        }

        public void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.CompareTag("Player"))
            {
                Owner.SetState(_idleState);
            }
        }
    }
}
