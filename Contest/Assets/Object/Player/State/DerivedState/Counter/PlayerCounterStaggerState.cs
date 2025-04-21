using System.Collections.Generic;
using UnityEngine;

// ================================
// �v���C���[�̃J�E���^�[���s���
// ================================

public class PlayerCounterStaggerState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerCounterStaggerState instance;
    // �t���[�����v��
    int freams = 0;


#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // ���̐F��ۑ�
    Color originalColor;
#endif



    // �C���X�^���X���擾����
    public static PlayerCounterStaggerState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerCounterStaggerState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(PlayerState playerState)
    {
        // �J�E���^�[���s�t���[�����߂�����
        if (freams >= playerState.GetPlayerCounterManager().GetCounterStaggerFrames())
        {
            playerState.ChangeState(PlayerStandingState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
#if UNITY_EDITOR
        Debug.LogError("CounterStaggerState : �J�n");

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
        playerState.HandleDamage(playerState.GetPlayerEnemyAttackTag());

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
