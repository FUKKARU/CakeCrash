using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    // このクラスがインスタンス化された時，ノルマクリア回数のセーブデータを全て削除する。
    public class ClearTimesResetter : MonoBehaviour
    {
        void Start()
        {
            PlayerPrefs.DeleteKey("EasyClearTimes");
            PlayerPrefs.DeleteKey("NormalClearTimes");
            PlayerPrefs.DeleteKey("HardClearTimes");
            PlayerPrefs.DeleteKey("SuperHardClearTimes");
        }
    }
}