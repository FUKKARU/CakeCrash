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

        [Header("�e�X�g�p�F�K�[�h�}�������Ȃ��Ȃ�")] public bool GuardManStop = false;
        [Header("���_�̍ő剔���U�ꕝ")] public float DoubleA_YRange = 21f;
        [Header("���_�̂������X�s�[�h")] public float W_Speed = 10f;
        [Header("�n���}�[��U��N�[���^�C��")] public float HammerCooltime = 0.1f;
        [Header("�n���}�[�𐶐�������W(=�n���}�[�{�̂̒��S)")] public Vector3 HammerGeneratePosition = new Vector3(0.5f, 30.35f, -25f);
        [Header("�n���}�[��U��I�C���[�pz (�J�n�C�I��)")] public Vector2 HammerEulerZ = new Vector2(0f, 90f);
        [Header("�n���}�[��U�鎞��")] public float HammerDur = 0.1f;
        [Header("�X�^�~�i�̍ő��")] public int MaxStamina = 1000;
        [Header("�X�^�~�i�̉񕜕p�x")] public float StaminaRecoverSpan = 1.5f;
        [Header("�K�[�h�}��������Ԋu[�b]")] public float EventSpan = 15f;
        [Header("�K�[�h�}�����t�F�C���g���g���m��(�̕���)")] public int FeintProbability = 10;
    }
}