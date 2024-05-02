using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class UISwitch : MonoBehaviour
    {
        GameObject canvas;

        void Start()
        {
            canvas = GameObject.FindGameObjectWithTag("Canvas");
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    canvas.SetActive(!canvas.activeSelf);
                }
            }
        }
    }
}