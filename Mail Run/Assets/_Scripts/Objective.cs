using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltEvents;

namespace GGD
{
    public class Objective : MonoBehaviour
    {
        [SerializeField] private GameObject _indicator;
        [SerializeField] private GameObject _package;
        public UltEvent OnObjectiveDelivered;

        public bool delivered = false;

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.CompareTag("Player") && !delivered)
            {
                LevelManager.Instance.ObjectivesDelivered++;
                LevelManager.Instance.objectivesRemaining--;
                _indicator.SetActive(false);
                _package.SetActive(true);
                _package.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(-90f, 90f), 0f));
                delivered = true;
                OnObjectiveDelivered?.Invoke();
                // Destroy(gameObject);
            }
        }
    }
}
