using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/GameState", fileName = "GameStateParamsSO")]
    public class GameStateParamsSO : ScriptableObject
    {
        #region QOL���㏈��
        // ParamsSO���ۑ����Ă���ꏊ�̃p�X
        public const string PATH = "GameStateParamsSO";

        // ParamsSO�̎���
        private static GameStateParamsSO _entity = null;
        public static GameStateParamsSO Entity
        {
            get
            {
                // ���A�N�Z�X���Ƀ��[�h����
                if (_entity == null)
                {
                    _entity = Resources.Load<GameStateParamsSO>(PATH);

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

        [Header("�𑜓x(ex. 1920*1080)")] public Vector2Int Resolution;
        [Header("�t���X�N���[���ɂ���")] public bool IsFullScreen;
        [Header("Vsync���I���ɂ���")] public bool IsVsyncOn;
        [Header("(Vsync���I�t�̎��̂�)�^�[�Q�b�g�t���[�����[�g")] public int TargetFrameRate;
    }
}