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
            _animator.SetTrigger("Alert");
        }
        
        public void UpdateValues(float speed)
        {
            _animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        }
    }
}
