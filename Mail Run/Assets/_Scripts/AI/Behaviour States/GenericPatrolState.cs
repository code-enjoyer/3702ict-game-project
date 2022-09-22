using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace GGD
{
    public class GenericPatrolState : NPCBehaviourState
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
            if(timer > 0)
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
                _NPC.StateController.SetState(_idleState);
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

            //if(Physics.Raycast(eyes.transform.position + (Vector3.left * 0.2f), direction, out hit, los))
            Collider[] items = Physics.OverlapSphere(eyes.transform.position, los, mask);

            if (items.Length > 0)
            {
                for(int i =0; i< items.Length; i++)
                {
                    if(items[i] == GetComponent<Collider>())
                    {
                        continue;
                    }

                    if(Vector3.Angle(transform.forward, items[i].transform.position - transform.position) <= fov)
                    {
                        if (Physics.Raycast(eyes.transform.position, items[i].transform.position - transform.position, out hit, los, mask))
                        {
                            Debug.DrawLine(eyes.transform.position, hit.point);

                            if (hit.transform.CompareTag("Player"))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
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
            Gizmos.DrawLine(eyes.transform.position+ downRayDirection, eyes.transform.position + upRayDirection);
        }

    }
}