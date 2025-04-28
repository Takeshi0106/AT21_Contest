using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =====================================
// �����镐�킪�Ȃ��Ƃ��̏��
// =====================================

public class PlayerThrowFailedState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerThrowFailedState instance;
    // ����̏��
    private static BaseAttackData weponData;

    int freams = 0;



    // �C���X�^���X���擾����
    public static PlayerThrowFailedState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerThrowFailedState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(PlayerState playerState)
    {
        // ������Ԃɖ߂�
        if (freams > playerState.GetThrowFailedFreams())
        {
            playerState.ChangeState(PlayerCounterStaggerState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        AnimationClip animClip = playerState.GetThrowFailedAnimation();
        var childAnim = playerState.GetPlayerWeponManager().GetCurrentWeaponAnimator();

        // ����f�[�^���擾����
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

        // �A�j���[�V�����J�n����
        if (animClip != null && childAnim != null)
        {
            childAnim.CrossFade(animClip.name, 0.2f);
        }

#if UNITY_EDITOR
        Debug.LogError("PlayerWeaponThrowFailedState : �J�n");

        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        playerState.GetPlayerRenderer().material.color = Color.blue;
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        // �_���[�W������L���ɂ���
        playerState.HandleDamage();

        freams++;
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        freams = 0;

#if UNITY_EDITOR
        // �G�f�B�^���s���ɐF�����ɖ߂�
        playerState.GetPlayerRenderer().material.color = Color.white;
#endif
    }



}
