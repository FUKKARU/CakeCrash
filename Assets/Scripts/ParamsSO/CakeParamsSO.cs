using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Cake", fileName = "CakeParamsSO")]
    public class CakeParamsSO : ScriptableObject
    {
        #region QOL���㏈��
        // CakeParamsSO���ۑ����Ă���ꏊ�̃p�X
        public const string PATH = "CakeParamsSO";

        // CakeParamsSO�̎���
        private static CakeParamsSO _entity = null;
        public static CakeParamsSO Entity
        {
            get
            {
                // ���A�N�Z�X���Ƀ��[�h����
                if (_entity == null)
                {
                    _entity = Resources.Load<CakeParamsSO>(PATH);

                    //���[�h�o���Ȃ������ꍇ�̓G���[���O��\��
                    if (_entity == null)
                    {
                        Debug.LogError(PATH + " not found");
                    }
                }

                return _entity;
            }
        }
        #endregion

        [Header("�P�[�L���ŏ��ɐ�������x���W�̃��X�g(�~��)")] public float[] CakeStartGenerateXList;
        [Header("�P�[�L����ʂ��������x���W(��)")] public float CakeLimitX;
        [Header("�P�[�L���m�̊Ԋu")] public float CakeOfst;
        [Header("�P�[�L�𐶐�����y���W(����)")] public float[] CakeGenerateYList;
        [Header("�P�[�L�����[������͂ݏo�鋫�E��z���W(��)\r\n(��C���C��)")] public float[] LaneLimitZList;
        [Header("�P�[�L��������悤�ɂȂ鋫�E��x���W(��)\r\n(��C���C��)")] public float[] DeletableXList;
        [Header("�P�[�L�������Ɂu�߂��v�ʒu�ɂ����\r\n���肷�鋫�E��x���W�͈̔͂��A���̉��{�ɂ��邩")] public float IsNearCenterXCoef;
        [Header("�V�[�����ɃP�[�L�����Z�b�g�܂ŋ��e���邩")] public int MaxCakeSet;
        [Header("�P�[�L�̗����X�s�[�h")] public float CakeSpeed;
        [Header("�P�[�L�𐁂���΂�����\r\n(��C���C��)")] public Vector3[] SmashVector3;
        [Header("�P�[�L�𐁂���΂���\r\n(��C���C��)")] public float[] SmashPower;
        [Header("�P�[�L�����󂷂��ƂɁA�X�R�A�Ɋ��Z���鉉�o�����邩�B")] public int ToScoreDur;
        [Header("�X�R�A�̍����e�L�X�g�������܂ł̕b��")] public float DelTextFadeDur;
        [Header("�Q�[���I�[�o�ɂȂ�P�[�L�̐�")] public int GameOverCakeNum;
        [Header("��񓖂���̉����̏㏸�l")] public int SpeedIncrementValue;
    }
}