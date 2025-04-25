using System.Collections.Generic;
using UnityEngine;

// ================================
// �v���C���[�̃J�E���^�[�U�����
// ================================

public class PlayerCounterStrikeState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerCounterStrikeState instance;
    // �t���[�����v��
    int freams = 0;



    // �C���X�^���X���擾����
    public static PlayerCounterStrikeState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerCounterStrikeState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(PlayerState playerState)
    {
        // �J�E���^�[�̗L���t���[�����߂�����
        if (freams >= playerState.GetPlayerCounterManager().GetCounterSuccessFrames())
        {
            playerState.ChangeState(PlayerStandingState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        // �Q�[�W�ʃA�b�v
        playerState.GetPlayerCounterManager().IncreaseGauge();

        // Sphere �������T�C�Y�ɂ��A�A�N�e�B�u��
        playerState.GetPlayerCounterObject().transform.localScale = Vector3.zero;
        playerState.GetPlayerCounterObject().SetActive(true);

        // �U���^�O��t����
        playerState.GetPlayerCounterAttackController().EnableAttack();

#if UNITY_EDITOR
        Debug.LogError("CounterStrikeState : �J�n");

        // �ԐF�ɕύX����
        playerState.GetPlayerRenderer().material.color = Color.red;
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        // �_���[�W����
        playerState.CleanupInvalidDamageColliders(playerState.GetPlayerEnemyAttackTag());

        // �J�E���^�[���� Sphere �g�又��
        var sphere = playerState.GetPlayerCounterObject();
        if (sphere != null)
        {
            float maxSize = playerState.GetPlayerCounterRange(); // �g��̍ő�T�C�Y
            float scale = Mathf.Lerp(0f, maxSize, (float)freams / playerState.GetPlayerCounterManager().GetCounterSuccessFrames());
            sphere.transform.localScale = new Vector3(scale, scale, scale);
        }

        // �t���[���X�V
        freams++;
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        // ������
        freams = 0;

        // �U���^�O��߂�
        playerState.GetPlayerCounterAttackController().DisableAttack();

        // Sphere ���A�N�e�B�u�ɖ߂�
        var sphere = playerState.GetPlayerCounterObject();
        if (sphere != null)
        {
            sphere.SetActive(false);
        }

        // ���G��L���ɂ���
        playerState.GetPlayerStatusEffectManager().StartInvicible();

#if UNITY_EDITOR
        playerState.GetPlayerRenderer().material.color = Color.white;
#endif
    }



}
