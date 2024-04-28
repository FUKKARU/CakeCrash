using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class ResolutionChanger : MonoBehaviour
    {
        bool isResChanged = false; // ïœçXÇ™äÆóπÇµÇΩÇ©Ç«Ç§Ç©

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetKeyDown(KeyCode.F11))
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
}