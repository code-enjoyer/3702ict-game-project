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
            Vector3 camPos = Camera.main.transform.position;
            camPos.y = transform.position.y;
            transform.LookAt(camPos, Vector3.up);
            //transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
        }
    }
}
