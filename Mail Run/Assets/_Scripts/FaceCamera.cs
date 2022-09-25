using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    public class FaceCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(Camera.main.transform.position, Vector3.up);
        }
    }
}
