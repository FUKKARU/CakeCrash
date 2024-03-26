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

        [Header("�P�[�L���ŏ��ɐ�������x���W�̃��X�g(�~��)")] public float[] CakeStartGenerateXList = { 40f, 0f, -40f };
        [Header("�P�[�L����ʂ��������x���W(��)")] public float CakeLimitX = 43f;
        [Header("�P�[�L���m�̊Ԋu")] public float CakeOfst = 40f;
        [Header("�P�[�L�𐶐�����y���W(����)")] public float[] CakeGenerateYList = { 34f, 39f, 44f };
        [Header("�P�[�L�����[������͂ݏo�鋫�E��z���W(��)\r\n(��C���C��)")] public float[] LaneLimitZList = { 23f, 20.6f, 18.2f };
        [Header("�P�[�L��������悤�ɂȂ鋫�E��x���W(��)\r\n(��C���C��)")] public float[] DeletableXList = { 15.5f, 13.1f, 10.7f };
        [Header("�P�[�L�������Ɂu�߂��v�ʒu�ɂ����\r\n���肷�鋫�E��x���W�͈̔͂��A���̉��{�ɂ��邩")] public float IsNearCenterXCoef = 1.15f;
        [Header("�V�[�����ɃP�[�L�����Z�b�g�܂ŋ��e���邩")] public int MaxCakeSet = 3;
        [Header("�P�[�L�̗����X�s�[�h")] public float CakeSpeed = 7f;
        [Header("�P�[�L�𐁂���΂�����\r\n(��C���C��)")] public Vector3[] SmashVector3 = { new Vector3(-0.75f, 0.25f, 1f), new Vector3(-0.75f, 0.25f, 1f), new Vector3(-0.75f, 0.25f, 1f) };
        [Header("�P�[�L�𐁂���΂���\r\n(��C���C��)")] public float[] SmashPower = { 300f, 300f, 300f };
    }
}