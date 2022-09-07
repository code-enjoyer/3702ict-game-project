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
        [SerializeField] private GameObject eyes;

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
             //   Owner.SetState(_idleState);
            }

            

            if(LineOfSight() && timer <= 0f)
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

        bool LineOfSight()
        {
            RaycastHit hit;

            Vector3 direction = player.transform.position - transform.position;

            if(Physics.Raycast(eyes.transform.position + (Vector3.left * 0.2f), direction, out hit, los))
            {
                Debug.DrawLine(eyes.transform.position + (Vector3.left * 0.2f), hit.point);

                if (hit.transform.CompareTag("Player"))
                {
                    return true;
                }
                
            }

            return false;
        }

        //void OnDrawGizmos()
        //{

        //    Gizmos.color = Color.red;
        //    float angle = 30.0f;
            
        //    float halfFOV = angle / 2.0f;
        //    float height = 1.4f;
            

        //    Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV , Vector3.up);
        //    Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV , Vector3.up);

        //    Vector3 upRayDirection = upRayRotation * transform.forward * los;
        //    Vector3 downRayDirection = downRayRotation * transform.forward * los;

        //    Gizmos.DrawRay(transform.position + (Vector3.up * height), upRayDirection);
        //    Gizmos.DrawRay(transform.position + (Vector3.up * height), downRayDirection);
        //    Gizmos.DrawLine(transform.position + (Vector3.up * height) + downRayDirection, transform.position + (Vector3.up * height) + upRayDirection);
        //}

    }
}