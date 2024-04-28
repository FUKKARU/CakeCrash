using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Other", fileName = "OtherParamsSO")]
    public class OtherParamsSO : ScriptableObject
    {
        #region QOL���㏈��
        // ParamsSO���ۑ����Ă���ꏊ�̃p�X
        public const string PATH = "OtherParamsSO";

        // ParamsSO�̎���
        private static OtherParamsSO _entity = null;
        public static OtherParamsSO Entity
        {
            get
            {
                // ���A�N�Z�X���Ƀ��[�h����
                if (_entity == null)
                {
                    _entity = Resources.Load<OtherParamsSO>(PATH);

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

        [Header("���C�g�̊p�x (�J�n�C�I��)")] public Vector2 LightTheta = new Vector2(-10f, -190f);
        [Header("��ʂ��t�F�[�h�A�E�g����X�s�[�h")] public float FadeOutSpeed = 100f;
        [Header("�P�[�L���͈͊O��UI�������܂ł̕b��")] public float CakeOutOfRangeUIHideDuration = 2f;
        [Header("�Q�[���I�[�o�[�̉��o���ɃJ�����������I�C���[��]�px")] public float GameOverCameraRotationX = -30f;
        [Header("Quit(0)�{�^��������������ɂȂ�b��")] public float QuitHoldPeriod = 2f;
        [Header("�m���}�B���񐔍폜\r\n(Ctrl + Shift + Alt + �� + �� + �� + ��)\r\n�{�^��������������ɂȂ�b��")] public float ClearTimesResetHoldPeriod = 5f;
        [Header("�e��{�^���������Ă��炷���ɃV�[���J�ڂ���ꍇ�C\r\n����܂ł̕b��")] public float SceneChangeWaitTime = 0.2f;
    }
}