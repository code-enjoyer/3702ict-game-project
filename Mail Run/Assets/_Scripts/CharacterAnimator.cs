using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class CharacterAnimator : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Alert()
        {
            // _animator.SetTrigger("Alerted"); // Not working properly? (animator BS and exit times methinks)
            _animator.CrossFade("Alert", 0.1f);
        }
        
        public void UpdateValues(float speed)
        {
            _animator.SetFloat("Speed", speed);
        }
    }
}
