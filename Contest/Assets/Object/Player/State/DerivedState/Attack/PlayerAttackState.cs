using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

// ======================================
// �v���C���[�̍U�����
// ======================================

public class PlayerAttackState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerAttackState instance;
    // weponData
    private static BaseAttackData weponData;
    // �t���[�����v��
    float freams = 0.0f;

    // �C���X�^���X���擾����֐�
    public static PlayerAttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerAttackState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(PlayerState playerState)
    {
        // �U���̃t���[�����߂�����
        if (freams >= weponData.GetAttackStartupFrames(playerState.GetPlayerConbo()) +
            weponData.GetAttackSuccessFrames(playerState.GetPlayerConbo()))
        {
            // �ォ��d����ԂɈڍs����
            playerState.ChangeState(PlayerAttackRecoveryState.Instance);
            return;
        }
        if (Input.GetButtonDown("Counter"))
        {
            // �R���{������������
            playerState.SetPlayerCombo(0);
            // �J�E���^�[��ԂɈڍs
            playerState.ChangeState(PlayerCounterStanceState.Instance);
            return;
        }
        if (Input.GetButtonDown("Throw"))
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
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

        // �A�j���[�V�����Đ�
        // Animator ���擾
        var anim = playerState.GetPlayerAnimator();
        var animClip = weponData.GetAttackAnimation(playerState.GetPlayerConbo());

        if (anim != null && animClip != null)
        {
            anim.CrossFade(animClip.name, 0.1f);
        }

#if UNITY_EDITOR
        Debug.LogError($"PlayerAttackState : �J�n�iCombo���F{playerState.GetPlayerConbo() + 1}�j");

        if (weponData == null)
        {
            Debug.LogError("PlayerAttackState : WeponData��������܂���");
            return;
        }
        // Debug.Log($"Play����A�j���[�V����: {animClip.name}");
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        playerState.HandleDamage();

        // �U�������ON�ɂ���
        if (freams >= weponData.GetAttackStartupFrames(playerState.GetPlayerConbo()))
        {
            playerState.GetPlayerWeponManager().EnableAllWeaponAttacks();
        }

        // ��ԕύX�����Ƃ���Input�𖳌��ɂ���@�U���{�^���������Ă�����A���̍U����\��
        if (freams > 0.1)
        {
            if (Input.GetButtonDown("Attack"))
            {
                playerState.SetPlayerNextReseved(RESEVEDSTATE.ATTACK);
            }
            
        }

        freams += playerState.GetPlayerSpeed();
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        // �U�������OFF
        playerState.GetPlayerWeponManager().DisableAllWeaponAttacks();
        // ������
        freams = 0.0f;
    }



}
