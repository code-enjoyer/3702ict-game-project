using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace GGD
{
    public class GenericPatrolState : BehaviourState
    {
        public enum PatrolMethod
        {
            PingPong,
            Loop
        }

        [SerializeField] private PatrolMethod _patrolMethod = PatrolMethod.PingPong;
        [SerializeField] private Transform[] _waypoints = new Transform[1];
        [SerializeField] private BehaviourState _idleState;
        [SerializeField] private BehaviourState _harassState;
        [SerializeField] private float los = 3;
        [SerializeField] private float coolDown = 3f;
        private float timer;
        private int _currentWaypointIndex = 0;
        GameObject player;

        protected override void OnEnter()
        {
            _owner.NavMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
            player = GameManager.Instance.Player;
            timer = coolDown;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            player = GameManager.Instance.Player;
            timer -= deltaTime;
            // TODO: Use a variable for "effective" stopping distance
            if (_owner.NavMeshAgent.remainingDistance < 1f)
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
                _owner.NavMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
            }
            if (Random.value <= 0.001f)
            {
                Owner.SetState(_idleState);
            }

            if(Vector3.Distance(transform.position, player.transform.position) <= los && timer <= 0f)
            {
                Owner.SetState(_harassState);
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

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, los);
        }

    }
}