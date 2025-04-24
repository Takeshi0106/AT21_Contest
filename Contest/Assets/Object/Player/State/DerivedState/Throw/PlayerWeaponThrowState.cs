using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================
// ����𓊂�����
// ===============================

public class PlayerWeaponThrowState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerWeaponThrowState instance;
    // ����̏��
    private static BaseAttackData weponData;

    // �����镐��̓�����܂ł̃t���[��
    int throwStartUpFreams = 0;
    // ��������̍d���t���[��
    int throwStaggerFreams = 0;

    // ��ԕύX�܂ł̎���
    int freams = 0;



    // �C���X�^���X���擾����
    public static PlayerWeaponThrowState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerWeaponThrowState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(PlayerState playerState)
    {
        // ������Ԃɖ߂�
        if (freams > throwStartUpFreams + throwStaggerFreams)
        {
            playerState.ChangeState(PlayerCounterStaggerState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        // ����f�[�^���擾����
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

        AnimationClip animClip = weponData.GetThrowAnimation();
        var childAnim = playerState.GetPlayerWeponManager().GetCurrentWeaponAnimator();

        throwStartUpFreams = weponData.GetThrowStartUp();
        throwStaggerFreams = weponData.GetThrowStagger();

        // �A�j���[�V�����J�n����
        if (animClip != null && childAnim != null)
        {
            childAnim.CrossFade(animClip.name, 0.2f);
        }

#if UNITY_EDITOR
        Debug.LogError("PlayerWeaponThrowState : �J�n");

        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        playerState.GetPlayerRenderer().material.color = Color.green;
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        // �_���[�W������L���ɂ���
        playerState.HandleDamage(playerState.GetPlayerEnemyAttackTag());

        // ������
        if (freams == throwStartUpFreams)
        {
            // ��������폜
            playerState.GetPlayerWeponManager().RemoveWeapon(playerState.GetPlayerWeponNumber());

#if UNITY_EDITOR
            playerState.GetPlayerRenderer().material.color = Color.blue;
#endif
        }

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
