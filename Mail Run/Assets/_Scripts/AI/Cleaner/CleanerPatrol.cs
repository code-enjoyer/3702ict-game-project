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
        private int _currentWaypointIndex = 0;
        GameObject player;
        [SerializeField] private GameObject eyes;

        protected override void OnEnter()
        {
            _NPC.NavMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
            player = GameManager.Instance.Player;
            timer = coolDown;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            player = GameManager.Instance.Player;
            timer -= deltaTime;
            if (timer > 0)
            {
                Debug.Log("On cooldown");
            }
            // TODO: Use a variable for "effective" stopping distance
            if (_NPC.NavMeshAgent.remainingDistance < 1f)
            {
                //switch (_patrolMethod)
                //{
                //    case PatrolMethod.PingPong:
                //        // TODO
                //        break;
                //    case PatrolMethod.Loop:
                //        // TODO
                //        break;
                //    default:
                //        break;
                //}
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= _waypoints.Length)
                {
                    _currentWaypointIndex = 0;
                }
                _NPC.NavMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
            }
            if (Random.value <= 0.001f)
            {
                Owner.SetState(_idleState);
            }
        }

        private void OnValidate()
        {
            if (_waypoints.Length == 0)
            {
                Debug.LogWarning("[GenericPatrolState] No transforms in the waypoints array!", gameObject);
            }
            else if (_waypoints.Any(x => x == null))
            {
                Debug.LogWarning("[GenericPatrolState] There are null transforms in the waypoints array!", gameObject);
            }
        }
    }
}
