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
        [Header("���[�h���������Ă��牽�b���\n�g�����W�V�����̉��o�����邩")] public float DurAfterLoadCompleted;
        [Header("���[�h������̃g�����W�V�����ɂ����āA\n���b�ŉ�ʂ�������/�����邩")] public float LoadTransisionDur;
        [Header("������g�����W�V�����̌�A\n��������g�����W�V�����̑O�ɁA\n���b�̃N�[���^�C�������ނ�")] public float BetweenTransisionDur;
        [Header("���C�g�̊p�x (�J�n�C�I��)")] public Vector2 LightTheta;
        [Header("��ʂ��t�F�[�h�A�E�g����X�s�[�h")] public float FadeOutSpeed;
        [Header("�P�[�L���͈͊O��UI�������܂ł̕b��")] public float CakeOutOfRangeUIHideDuration;
        [Header("�Q�[���I�[�o�[�̉��o���ɃJ�����������I�C���[��]�px")] public float GameOverCameraRotationX;
        [Header("�e��{�^���������Ă��炷���ɃV�[���J�ڂ���ꍇ�C\r\n����܂ł̕b��")] public float SceneChangeWaitTime;
    }
}