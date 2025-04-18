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

    // �G�f�B�^���s���Ɏ��s�����
    // ���̐F��ۑ�
    Color originalColor;

#if UNITY_EDITOR

#endif



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

        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // ���̐F��ۑ�
            playerState.playerRenderer.material.color = Color.red;    // �J�E���^�[�������̐F
        }

#if UNITY_EDITOR
        Debug.LogError("CounterStrikeState : �J�n");


#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        playerState.CleanupInvalidDamageColliders(playerState.GetPlayerEnemyAttackTag());

        freams++;

        // �J�E���^�[���� Sphere �g�又��
        var sphere = playerState.GetPlayerCounterObject();
        if (sphere != null)
        {
            float maxSize = playerState.GetPlayerCounterRange(); // �g��̍ő�T�C�Y
            float scale = Mathf.Lerp(0f, maxSize, (float)freams / playerState.GetPlayerCounterManager().GetCounterSuccessFrames());
            sphere.transform.localScale = new Vector3(scale, scale, scale);
        }
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

        // �G�f�B�^���s���ɐF�����ɖ߂�
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // ���̐F�ɖ߂�
        }

#if UNITY_EDITOR

#endif
    }



}
