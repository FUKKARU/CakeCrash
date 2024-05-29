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
            if (IA.InputGetter.Instance.Debug_IsShowTips) tips.SetActive(!tips.activeSelf);
        }
    }
}