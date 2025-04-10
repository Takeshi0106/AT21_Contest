using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRecoveryState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerAttackRecoveryState instance;
    // weponData
    private static BaseAttackData weponData;
    // �t���[�����v��
    int freams = 0;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // ���̐F��ۑ�
    Color originalColor;
#endif



    // �C���X�^���X���擾����֐�
    public static PlayerAttackRecoveryState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerAttackRecoveryState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(PlayerState playerState)
    {
        // �U���d���t���[�����I���Ɨ�����Ԃɖ߂�
        if (freams >= weponData.GetAttackStaggerFrames(playerState.GetPlayerConbo()))
        {
            //�@�R���{������������
            playerState.SetPlayerCombo(0);
            // �ォ��d����ԂɈڍs����
            playerState.ChangeState(PlayerStandingState.Instance);
            return;
        }
        // �U����ԂɕύX
        if (Input.GetButtonDown("Attack") && playerState.GetPlayerConbo() < weponData.GetMaxCombo() - 1)
        {
            // �R���{�𑝂₷
            playerState.SetPlayerCombo(playerState.GetPlayerConbo() + 1);
            // �U����ԂɈڍs
            playerState.ChangeState(PlayerAttackState.Instance);
            return;
        }
        // �J�E���^�[��ԂɕύX
        if (Input.GetButtonDown("Counter"))
        {
            // �R���{������������
            playerState.SetPlayerCombo(0);
            // �J�E���^�[��ԂɈڍs
            playerState.ChangeState(PlayerCounterStanceState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

#if UNITY_EDITOR
        Debug.LogError("PlayerAttackRecoveryState : �J�n");

        if (weponData == null)
        {
            Debug.LogError("PlayerAttackState : WeponData��������܂���");
            return;
        }
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // ���̐F��ۑ�
            playerState.playerRenderer.material.color = Color.blue;    // �J�E���^�[�������̐F
        }
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        freams++;
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        // ������
        freams = 0;

#if UNITY_EDITOR
        // �G�f�B�^���s���ɐF�����ɖ߂�
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // ���̐F�ɖ߂�
        }
#endif
    }



}
