using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Config", fileName = "ConfigParamsSO")]
    public class ConfigParamsSO : ScriptableObject
    {
        #region QOL���㏈��
        // GuardManParamsSO���ۑ����Ă���ꏊ�̃p�X
        public const string PATH = "ConfigParamsSO";

        // GuardManParamsSO�̎���
        private static ConfigParamsSO _entity = null;
        public static ConfigParamsSO Entity
        {
            get
            {
                // ���A�N�Z�X���Ƀ��[�h����
                if (_entity == null)
                {
                    _entity = Resources.Load<ConfigParamsSO>(PATH);

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

        [Header("�����ݒ�")]
        [Header("���胂�[�h (0:�I�t 1:�I��)")] public int DefaultLeftMode = 0;
        [Header("�����̖��邳 (-10:0.5�{ 0:1�{ 10:2�{)\r\n(���邳��2^(n/10)�{�ɂȂ�)"), Range(-10, 10)] public int DefaultBrightness = 0;
        [Header("����(dB) (BGM,SE,System)"), Range(-10, 10)] public int[] DefaultSoundVolume = { 0, 0, 0 };
        [Header("��Փx (0:�ȒP 1:���� 2:��� 3:�ƂĂ����)")] public int DefaultDifficulty = 1;
        [Space(50)]
        [Header("�����̖��邳�̌v�Z�� (I = d(=var0) * var1^(n/var2))")] public float[] DefaultBrightnessVars = { 0.5f, 2f, 10f };
    }
}