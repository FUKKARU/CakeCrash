using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class CakeGenerator : MonoBehaviour
    {
        [SerializeField] GameObject cakeSetPrfb;
        [SerializeField] GameObject redCakeBigPrfb;
        [SerializeField] GameObject redCakeMediumPrfb;
        [SerializeField] GameObject redCakeSmallPrfb;
        [SerializeField] GameObject greenCakeBigPrfb;
        [SerializeField] GameObject greenCakeMediumPrfb;
        [SerializeField] GameObject greenCakeSmallPrfb;
        [SerializeField] GameObject blueCakeBigPrfb;
        [SerializeField] GameObject blueCakeMediumPrfb;
        [SerializeField] GameObject blueCakeSmallPrfb;

        GameObject mostLeftCakeSet; // 最左端のケーキセット
        GameObject mostRightCakeSet; // 最右端のケーキセット

        void Start()
        {
            // ゲーム開始時のケーキをセットアップ
            foreach (float generateX in CakeParamsSO.Entity.CakeStartGenerateXList)
            {
                if (!GameManager.Instance.IsLeftMode)
                {
                    mostLeftCakeSet = MakeACakeSet(new Vector3(generateX, 0, 0));
                }
                else
                {
                    mostRightCakeSet = MakeACakeSet(new Vector3(-generateX, 0, 0));
                }
            }
        }

        void Update()
        {
            // シーン内のケーキの数をカウント
            int cakeNum = GameObject.FindGameObjectsWithTag("Cake").Length;

            if (!GameManager.Instance.IsLeftMode)
            {
                if (mostLeftCakeSet == null) { Debug.LogError("0"); }
                float leftDistance = Mathf.Abs(mostLeftCakeSet.transform.position.x - (-CakeParamsSO.Entity.CakeLimitX));

                // シーン内にケーキがcakeMaxSetNumセットないならば，左端に十分なスペースがあるとき，ケーキを壊しきっていない場合のみ
                if (cakeNum <= (CakeParamsSO.Entity.MaxCakeSet - 1) * 3 && leftDistance >= CakeParamsSO.Entity.CakeOfst)
                {
                    if (!GameManager.Instance.IsAllSmashed)
                    {
                        mostLeftCakeSet = MakeACakeSet(new Vector3(-CakeParamsSO.Entity.CakeLimitX, 0, 0));
                    }
                }
            }
            else
            {
                float rightDistance = Mathf.Abs(CakeParamsSO.Entity.CakeLimitX - mostRightCakeSet.transform.position.x);

                // シーン内にケーキがcakeMaxSetNumセットないならば，右端に十分なスペースがあるとき，ケーキを壊しきっていない場合のみ
                if (cakeNum <= (CakeParamsSO.Entity.MaxCakeSet - 1) * 3 && rightDistance >= CakeParamsSO.Entity.CakeOfst)
                {
                    if (!GameManager.Instance.IsAllSmashed)
                    {
                        mostRightCakeSet = MakeACakeSet(new Vector3(CakeParamsSO.Entity.CakeLimitX, 0, 0));
                    }
                }
            }
        }

        // ケーキセットを1つ作成
        GameObject MakeACakeSet(Vector3 spawnPos)
        {
            // ケーキセットの親を作成
            GameObject newCakeSet = Instantiate(cakeSetPrfb, spawnPos, Quaternion.identity);
            Transform newCakeSetTf = newCakeSet.transform;

            // Big,Medium,Smallのケーキを作成
            for (int i = 0; i < 3; i++)
            {
                // Big
                if (i == 0)
                {
                    // 1~3のランダムな整数値を作成，それに応じて生成するケーキの色を変更する，newCakeSetの子にする
                    int cakeIndex = Random.Range(1, 4);
                    if (cakeIndex == 1)
                    {
                        Instantiate(redCakeBigPrfb, newCakeSetTf.position + new Vector3(0, CakeParamsSO.Entity.CakeGenerateYList[0], 0), Quaternion.identity, newCakeSetTf);
                    }
                    else if (cakeIndex == 2)
                    {
                        Instantiate(greenCakeBigPrfb, newCakeSetTf.position + new Vector3(0, CakeParamsSO.Entity.CakeGenerateYList[0], 0), Quaternion.identity, newCakeSetTf);
                    }
                    else
                    {
                        Instantiate(blueCakeBigPrfb, newCakeSetTf.position + new Vector3(0, CakeParamsSO.Entity.CakeGenerateYList[0], 0), Quaternion.identity, newCakeSetTf);
                    }
                }
                // Medium
                else if (i == 1)
                {
                    // 1~3のランダムな整数値を作成，それに応じて生成するケーキの色を変更する，newCakeSetの子にする
                    int cakeIndex = Random.Range(1, 4);
                    if (cakeIndex == 1)
                    {
                        Instantiate(redCakeMediumPrfb, newCakeSetTf.position + new Vector3(0, CakeParamsSO.Entity.CakeGenerateYList[1], 0), Quaternion.identity, newCakeSetTf);
                    }
                    else if (cakeIndex == 2)
                    {
                        Instantiate(greenCakeMediumPrfb, newCakeSetTf.position + new Vector3(0, CakeParamsSO.Entity.CakeGenerateYList[1], 0), Quaternion.identity, newCakeSetTf);
                    }
                    else
                    {
                        Instantiate(blueCakeMediumPrfb, newCakeSetTf.position + new Vector3(0, CakeParamsSO.Entity.CakeGenerateYList[1], 0), Quaternion.identity, newCakeSetTf);
                    }
                }
                // Small
                else
                {
                    // 1~3のランダムな整数値を作成，それに応じて生成するケーキの色を変更する，newCakeSetの子にする
                    int cakeIndex = Random.Range(1, 4);
                    if (cakeIndex == 1)
                    {
                        Instantiate(redCakeSmallPrfb, newCakeSetTf.position + new Vector3(0, CakeParamsSO.Entity.CakeGenerateYList[2], 0), Quaternion.identity, newCakeSetTf);
                    }
                    else if (cakeIndex == 2)
                    {
                        Instantiate(greenCakeSmallPrfb, newCakeSetTf.position + new Vector3(0, CakeParamsSO.Entity.CakeGenerateYList[2], 0), Quaternion.identity, newCakeSetTf);
                    }
                    else
                    {
                        Instantiate(blueCakeSmallPrfb, newCakeSetTf.position + new Vector3(0, CakeParamsSO.Entity.CakeGenerateYList[2], 0), Quaternion.identity, newCakeSetTf);
                    }
                }
            }
            return newCakeSet;
        }
    }
}
