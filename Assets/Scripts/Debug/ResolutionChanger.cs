using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class ResolutionChanger : MonoBehaviour
    {
        bool isResChanged = false; // �ύX�������������ǂ���

        void Update()
        {
            if (IA.InputGetter.Instance.Debug_IsChangeResolution)
            {
                if (!isResChanged)
                {
                    isResChanged = true;

                    Screen.SetResolution(1536, 864, false);
                }
                else
                {
                    isResChanged = false;

                    Screen.SetResolution(1920, 1080, true);
                }
            }
        }
    }
}