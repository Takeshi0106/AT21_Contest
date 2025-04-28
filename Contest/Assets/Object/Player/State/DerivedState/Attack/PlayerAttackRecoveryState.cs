using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =================================
// �U����̍d����ԁi�R���{�P�\��ԁj
// =================================

public class PlayerAttackRecoveryState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerAttackRecoveryState instance;
    // weponData
    private static BaseAttackData weponData;
    // �t���[�����v��
    int freams = 0;



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
        if ((Input.GetButtonDown("Attack") || playerState.GetPlayerNextReseved() == RESEVEDSTATE.ATTACK)
            && playerState.GetPlayerConbo() < weponData.GetMaxCombo() - 1)
        {
            // �R���{�𑝂₷
            playerState.SetPlayerCombo(playerState.GetPlayerConbo() + 1);
            // �U����ԂɈڍs
            playerState.ChangeState(PlayerAttackState.Instance);
            return;
        }
        // �J�E���^�[��ԂɕύX
        if (Input.GetButtonDown("Counter") || playerState.GetPlayerNextReseved() == RESEVEDSTATE.COUNTER)
        {
            // �R���{������������
            playerState.SetPlayerCombo(0);
            // �J�E���^�[��ԂɈڍs
            playerState.ChangeState(PlayerCounterStanceState.Instance);
            return;
        }
        // ����𓊂����ԂɕύX
        if (Input.GetButtonDown("Throw") || playerState.GetPlayerNextReseved() == RESEVEDSTATE.THROW)
        {
            // �R���{������������
            playerState.SetPlayerCombo(0);

            if (playerState.GetPlayerWeponManager().GetWeaponCount() <= 1)
            {
                // ����𓊂���̎��s��ԂɈڍs
                playerState.ChangeState(PlayerThrowFailedState.Instance);
            }
            else
            {
                // ����𓊂����ԂɈڍs
                playerState.ChangeState(PlayerWeaponThrowState.Instance);
            }

            return;
        }
        //�󒆂ɕ����Ă�����
        if ((Input.GetButtonDown("Jump") && !playerState.GetPlayerAirFlag()) ||
            playerState.GetPlayerAirFlag())
        {
            // �R���{������������
            playerState.SetPlayerCombo(0);

            playerState.ChangeState(PlayerJumpState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

        // �A�j���[�V�����Đ�
        // Animator ���擾
        //var anim = playerState.GetPlayerAnimator();
        // AnimationClip ���擾
        var animClip = weponData.GetAttackStaggerAnimation(playerState.GetPlayerConbo());
        var childAnim = playerState.GetPlayerWeponManager().GetCurrentWeaponAnimator();
        /*
        if (anim != null && animClip != null)
        {
            // anim.CrossFade(animClip.name, 0.2f);
        }
        */
        if (childAnim != null && animClip != null)
        {
            childAnim.CrossFade(animClip.name, 0.2f);
        }

#if UNITY_EDITOR
        Debug.LogError("PlayerAttackRecoveryState : �J�n");

        if (weponData == null)
        {
            Debug.LogError("PlayerAttackState : WeponData��������܂���");
            return;
        }

        playerState.GetPlayerRenderer().material.color = Color.blue;

#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        playerState.HandleDamage();

        freams++;
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        // ������
        freams = 0;
        // �X�g�b�N�̏�����
        playerState.SetPlayerNextReseved(RESEVEDSTATE.NOTHING);

#if UNITY_EDITOR
        playerState.GetPlayerRenderer().material.color = Color.white;
#endif
    }



}
