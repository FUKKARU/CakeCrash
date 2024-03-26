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

        GameObject mostLeftCakeSet; // �ō��[�̃P�[�L�Z�b�g
        GameObject mostRightCakeSet; // �ŉE�[�̃P�[�L�Z�b�g

        void Start()
        {
            // �Q�[���J�n���̃P�[�L���Z�b�g�A�b�v
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
            // �V�[�����̃P�[�L�̐����J�E���g
            int cakeNum = GameObject.FindGameObjectsWithTag("Cake").Length;

            if (!GameManager.Instance.IsLeftMode)
            {
                if (mostLeftCakeSet == null) { Debug.LogError("0"); }
                float leftDistance = Mathf.Abs(mostLeftCakeSet.transform.position.x - (-CakeParamsSO.Entity.CakeLimitX));

                // �V�[�����ɃP�[�L��cakeMaxSetNum�Z�b�g�Ȃ��Ȃ�΁C���[�ɏ\���ȃX�y�[�X������Ƃ��C�P�[�L���󂵂����Ă��Ȃ��ꍇ�̂�
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

                // �V�[�����ɃP�[�L��cakeMaxSetNum�Z�b�g�Ȃ��Ȃ�΁C�E�[�ɏ\���ȃX�y�[�X������Ƃ��C�P�[�L���󂵂����Ă��Ȃ��ꍇ�̂�
                if (cakeNum <= (CakeParamsSO.Entity.MaxCakeSet - 1) * 3 && rightDistance >= CakeParamsSO.Entity.CakeOfst)
                {
                    if (!GameManager.Instance.IsAllSmashed)
                    {
                        mostRightCakeSet = MakeACakeSet(new Vector3(CakeParamsSO.Entity.CakeLimitX, 0, 0));
                    }
                }
            }
        }

        // �P�[�L�Z�b�g��1�쐬
        GameObject MakeACakeSet(Vector3 spawnPos)
        {
            // �P�[�L�Z�b�g�̐e���쐬
            GameObject newCakeSet = Instantiate(cakeSetPrfb, spawnPos, Quaternion.identity);
            Transform newCakeSetTf = newCakeSet.transform;

            // Big,Medium,Small�̃P�[�L���쐬
            for (int i = 0; i < 3; i++)
            {
                // Big
                if (i == 0)
                {
                    // 1~3�̃����_���Ȑ����l���쐬�C����ɉ����Đ�������P�[�L�̐F��ύX����CnewCakeSet�̎q�ɂ���
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
                    // 1~3�̃����_���Ȑ����l���쐬�C����ɉ����Đ�������P�[�L�̐F��ύX����CnewCakeSet�̎q�ɂ���
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
                    // 1~3�̃����_���Ȑ����l���쐬�C����ɉ����Đ�������P�[�L�̐F��ύX����CnewCakeSet�̎q�ɂ���
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
