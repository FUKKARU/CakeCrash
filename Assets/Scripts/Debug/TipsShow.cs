using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class TipsShow : MonoBehaviour
    {
        GameObject tips;

        void Start()
        {
            tips = GameObject.FindGameObjectWithTag("Tips");
            tips.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    tips.SetActive(!tips.activeSelf);
                }
            }
        }
    }
}