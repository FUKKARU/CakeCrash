using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Main
{
    public class ScoreShower : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreTMPro;

        void Update()
        {
            scoreTMPro.text = $"<color=#C8C800>{GameManager.Instance.leftNum}</color><SIZE=20> / {GameManager.Instance.CakeMaxNum}</SIZE>";
        }
    }
}