using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(menuName = "ParamsSO/Sound", fileName = "SoundParamsSO")]
    public class SoundParamsSO : ScriptableObject
    {
        #region QOL���㏈��
        // ParamsSO���ۑ����Ă���ꏊ�̃p�X
        public const string PATH = "SoundParamsSO";

        // ParamsSO�̎���
        private static SoundParamsSO _entity = null;
        public static SoundParamsSO Entity
        {
            get
            {
                // ���A�N�Z�X���Ƀ��[�h����
                if (_entity == null)
                {
                    _entity = Resources.Load<SoundParamsSO>(PATH);

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

        [Header("BGM")]
        [Header("�^�C�g��")] public AudioClip TitleBGM;
        [Header("�Q�[����")] public AudioClip GameBGM;
        [Space(50)]
        [Header("SE")]
        [Header("�t�N���E����")] public AudioClip OwlHootSE;
        [Header("�P�[�L����������")] public AudioClip CakeFallSE;
        [Header("�n���}�[��U��")] public AudioClip HammerSmashSE;
        [Header("�n���}�[�ƃP�[�L���Փ˂���")] public AudioClip HammerHitCakeSE;
        [Header("�P�[�L���ǂɓ������ĂԂ�C�N���[�����ǂɂ�")] public AudioClip CakeCrushSE;
        [Header("�͈͊O�̃P�[�L��@�����Ƃ���")] public AudioClip CakeOutOfRangeSE;
        [Header("�x�����������Ă���")] public AudioClip GuardManComeSE;
        [Header("���������J��")] public AudioClip DoorHalfOpenSE;
        [Header("�����傫���J��")] public AudioClip DoorOpenSE;
        [Header("�����܂�")] public AudioClip DoorCloseSE;
        [Header("�x��������������")] public AudioClip GuardManGoSE;
        [Header("���Ⴊ��")] public AudioClip SquatSE;
        [Header("���Ⴊ��ő����Ђ��߁C�S���������ɋ���")] public AudioClip HideSE;
        [Header("�����オ��")] public AudioClip StandUpSE;
        [Header("�N���[������ɂ�")] public AudioClip CreamHitFaceSE;
        [Header("�X�^�~�i���؂��")] public AudioClip StaminaRunOutSE;
        [Header("�x�����Ɍ�����")] public AudioClip FoundByGuardManSE;
        [Header("�N���A��ʂŐ����t��")] public AudioClip StarSE;
        [Header("�N���A��ʂŃX�R�A�̃e�L�X�g���o��")] public AudioClip ScoreTextSE;
        [Space(50)]
        [Header("System")]
        [Header("�{�^���̃N���b�N")] public AudioClip ButtonClickSystem;
    }
}