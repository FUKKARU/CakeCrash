using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Human", fileName = "HumanParamsSO")]
    public class HumanParamsSO : ScriptableObject
    {
        #region QOL���㏈��
        // GuardManParamsSO���ۑ����Ă���ꏊ�̃p�X
        public const string PATH = "HumanParamsSO";

        // GuardManParamsSO�̎���
        private static HumanParamsSO _entity = null;
        public static HumanParamsSO Entity
        {
            get
            {
                // ���A�N�Z�X���Ƀ��[�h����
                if (_entity == null)
                {
                    _entity = Resources.Load<HumanParamsSO>(PATH);

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

        [Header("�e�X�g�p�F�K�[�h�}�������Ȃ��Ȃ�")] public bool GuardManStop;
        [Header("���_�̍ő剔���U�ꕝ")] public float DoubleA_YRange;
        [Header("���_�̂������X�s�[�h")] public float W_Speed;
        [Header("�n���}�[��U��N�[���^�C��")] public float HammerCooltime;
        [Header("�n���}�[�𐶐�������W(=�n���}�[�{�̂̒��S)")] public Vector3 HammerGeneratePosition;
        [Header("�n���}�[��U��I�C���[�pz (�J�n�C�I��)")] public Vector2 HammerEulerZ;
        [Header("�n���}�[��U�鎞��")] public float HammerDur;
        [Header("�X�^�~�i�̍ő��")] public int MaxStamina;
        [Header("�X�^�~�i�̉񕜕p�x")] public float StaminaRecoverSpan;
        [Header("�K�[�h�}��������Ԋu[�b]")] public float EventSpan;
        [Header("�K�[�h�}�����t�F�C���g���g���m��(�̕���)")] public int FeintProbability;
        [Header("�K�[�h�}�����h�A����ĉ��b��ɁA�P�[�L�̃~�X�J�E���g�̏������ĊJ���邩")] public float GuardManAfterClosedDur;
        [Header("�~�X�����ۂ̃R���{�̌�����")] public int OnMissComboDel;
    }
}