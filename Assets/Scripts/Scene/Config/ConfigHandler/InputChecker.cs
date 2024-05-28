using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class InputChecker : MonoBehaviour
    {
        public static float IsRed { get; set; } = 0;
        public static float IsBlue { get; set; } = 0;
        public static float IsGreen { get; set; } = 0;
        public static float IsSquat { get; set; } = 0;

        [SerializeField] Image redInputCheck;
        [SerializeField] Image blueInputCheck;
        [SerializeField] Image greenInputCheck;
        [SerializeField] Image squatInputCheck;

        void Update()
        {
            redInputCheck.enabled = IsRed >= 0.99f;
            blueInputCheck.enabled = IsBlue >= 0.99f;
            greenInputCheck.enabled = IsGreen >= 0.99f;
            squatInputCheck.enabled = IsSquat >= 0.99f;
        }
    }
}