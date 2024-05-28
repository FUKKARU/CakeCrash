using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class GuardManAnim : MonoBehaviour
    {
        public float speed;
        Transform target;

        private void Update()
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }

        public void Setup(Transform pos)
        {
            target = pos;
        }
    }
}