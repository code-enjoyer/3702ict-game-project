using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class Objective : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.CompareTag("Player"))
            {
                LevelManager.Instance.objectivesDelivered++;
                LevelManager.Instance.objectivesRemaining--;
                Destroy(gameObject);
            }
        }
    }
}
