using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltEvents;

namespace GGD
{
    public class TagBasedTrigger : TriggerEvents3D
    {
        [SerializeField] private string _requiredTag = "Untagged";

        public override void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(_requiredTag))
                base.OnTriggerEnter(collider);
        }

        public override void OnTriggerExit(Collider collider)
        {
            if (collider.CompareTag(_requiredTag))
                base.OnTriggerExit(collider);
        }

        public override void OnTriggerStay(Collider collider)
        {
            if (collider.CompareTag(_requiredTag))
                base.OnTriggerStay(collider);
        }
    }
}
