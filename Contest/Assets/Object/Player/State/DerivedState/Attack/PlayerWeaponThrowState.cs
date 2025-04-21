using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponThrowState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerWeaponThrowState instance;

    // ��ԕύX�܂ł̎���
    int changTime = 60;
    int freams = 0;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // ���̐F��ۑ�
    Color originalColor;
#endif



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
        if (freams > changTime)
        {
            playerState.ChangeState(PlayerCounterStaggerState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        if (playerState.GetPlayerWeponManager().GetWeaponCount() > 1)
        {
            // ��������폜
            playerState.GetPlayerWeponManager().RemoveWeapon(playerState.GetPlayerWeponNumber());

            changTime = 20;
        }
        else
        {
            // ���킪�P�����Ȃ��Ƃ��̏����������i�A�j���[�V�����Ȃǁj
        }

#if UNITY_EDITOR
        Debug.LogError("PlayerWeaponThrowState : �J�n");

        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // ���̐F��ۑ�
            playerState.playerRenderer.material.color = Color.blue;    // �J�E���^�[�\�����̐F
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
        freams = 0;
        changTime = 60;

#if UNITY_EDITOR
        // �G�f�B�^���s���ɐF�����ɖ߂�
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // ���̐F�ɖ߂�
        }
#endif
    }



}
