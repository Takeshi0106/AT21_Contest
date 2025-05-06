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
    float freams = 0.0f;



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

        playerState.GetPlayerRenderer().material.color = Color.blue;
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {

        playerState.HandleDamage();

        freams += playerState.GetPlayerSpeed();
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        // ������
        freams = 0.0f;


#if UNITY_EDITOR
        // �G�f�B�^���s���ɐF�����ɖ߂�
        playerState.GetPlayerRenderer().material.color = Color.white;
#endif
    }



}
