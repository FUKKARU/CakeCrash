using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Cream", fileName = "CreamParamsSO")]
    public class CreamParamsSO : ScriptableObject
    {
        #region QOL���㏈��
        // ParamsSO���ۑ����Ă���ꏊ�̃p�X
        public const string PATH = "CreamParamsSO";

        // ParamsSO�̎���
        private static CreamParamsSO _entity = null;
        public static CreamParamsSO Entity
        {
            get
            {
                // ���A�N�Z�X���Ƀ��[�h����
                if (_entity == null)
                {
                    _entity = Resources.Load<CreamParamsSO>(PATH);

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

        [Header("�N���[���𐶐�����z���W(�߂荞�ݖh�~�̂���)")] public float CreamGenerateZ;
        [Header("�N���[���𐶐��������W����J�����ɋ߂Â��鋗��")] public float CreamGenerateOffset;
        [Header("�~�X�������́C�N���[���𐶐�������W")] public Vector3 MissCreamGeneratePos;
        [Header("�N���[����������܂ł̎���(�b)")] public float CreamFadePeriod;
        [Header("�~�X�������́C�N���[����������܂ł̎���(�b)")] public float MissCreamFadePeriod;
    }
}