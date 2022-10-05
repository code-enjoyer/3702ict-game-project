using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GGD
{
    public enum BoolSetBehaviour
    {
        None = 0,
        Set = 1,
        Invert = 2,
    }


    public class SetBoolAnimBehaviour : StateMachineBehaviour
    {
        [SerializeField] private string _value;

        [SerializeField] private BoolSetBehaviour _behaviourOnEnter = BoolSetBehaviour.None;  
        [ShowIf("@_behaviourOnEnter == BoolSetBehaviour.Set")]
        [SerializeField] private bool _valueOnEnter = true;

        [SerializeField] private BoolSetBehaviour _behaviourOnExit = BoolSetBehaviour.None;
        [ShowIf("@_behaviourOnExit == BoolSetBehaviour.Set")]
        [SerializeField] private bool _valueOnExit = true;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_behaviourOnEnter == BoolSetBehaviour.Set)
                animator.SetBool(_value, _valueOnEnter);
            else if (_behaviourOnEnter == BoolSetBehaviour.Invert)
                animator.SetBool(_value, !animator.GetBool(_value));
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_behaviourOnExit == BoolSetBehaviour.Set)
                animator.SetBool(_value, _valueOnExit);
            else if (_behaviourOnExit == BoolSetBehaviour.Invert)
                animator.SetBool(_value, !animator.GetBool(_value));
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
