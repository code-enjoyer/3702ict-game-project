using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class GenericAnimatorHooks : MonoBehaviour
    {
        public void DisableGameObject()
        {
            gameObject.SetActive(false);
        }
    }
}
