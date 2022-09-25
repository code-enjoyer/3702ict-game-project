using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class Puddle : MonoBehaviour
    {
        public float lifeTime = 3.0f;
        private float elapsedTime;
        // Use this for initialization
        void Start()
        {
            elapsedTime = 0.0f;
        }
        // Update is called once per frame
        void Update()
        {
            if (elapsedTime >= lifeTime)
            {
                Destroy(gameObject);
            }

            elapsedTime += Time.deltaTime;
        }
    }
}