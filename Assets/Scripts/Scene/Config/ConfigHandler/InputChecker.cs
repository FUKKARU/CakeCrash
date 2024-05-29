using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class InputChecker : MonoBehaviour
    {
        [SerializeField] Image redInputCheck;
        [SerializeField] Image blueInputCheck;
        [SerializeField] Image greenInputCheck;
        [SerializeField] Image squatInputCheck;

        void Update()
        {
            redInputCheck.enabled = IA.InputGetter.Instance.IsRed;
            blueInputCheck.enabled = IA.InputGetter.Instance.IsBlue;
            greenInputCheck.enabled = IA.InputGetter.Instance.IsGreen;
            squatInputCheck.enabled = IA.InputGetter.Instance.IsSquat;
        }
    }
}