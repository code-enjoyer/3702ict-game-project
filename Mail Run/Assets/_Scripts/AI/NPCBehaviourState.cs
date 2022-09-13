using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class NPCBehaviourState : BehaviourState
    {
        protected NPC _NPC;

        public NPC NPC => _NPC;

        public override void Initialize(StateController owner)
        {
            base.Initialize(owner);

            _NPC = owner as NPC;
        }
    }
}
