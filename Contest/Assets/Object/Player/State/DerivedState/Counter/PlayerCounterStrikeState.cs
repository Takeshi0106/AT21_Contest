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
    float freams = 0.0f;



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
        float attackData = playerState.GetPlayerCounterManager().GetCounterDamage();   // �J�E���^�[�̍U����
        float stanAttackData = playerState.GetPlayerCounterManager().GetCounterStanDamage(); // �X�^���͂��擾 
        float multiData = playerState.GetPlayerCounterManager().GetDamageMultiplier(); // �J�E���^�[�}�l�[�W���[���擾

        // �Q�[�W�ʃA�b�v
        playerState.GetPlayerCounterManager().IncreaseGauge();

        // Sphere �������T�C�Y�ɂ��A�A�N�e�B�u��
        // playerState.GetPlayerCounterObject().transform.localScale = Vector3.zero;
        // playerState.GetPlayerCounterObject().SetActive(true);

        // �U���^�O��t����
        // playerState.GetPlayerCounterAttackController().EnableAttack();

        // �U�������X�V����
        playerState.GetAttackInterface().SetSelfAttackDamage(attackData* multiData);
        // �X�^�������X�V����
        playerState.GetAttackInterface().SetSelfStanAttackDamage(stanAttackData);
        // ID
        playerState.GetAttackInterface().SetSelfID();
        // �J�E���^�[�J�n����
        playerState.GetPlayerCounterObjectManager().Activate();


#if UNITY_EDITOR
        Debug.Log("CounterStrikeState : �J�n");

        Debug.LogError($"CounterStrikeState : {playerState.GetAttackInterface().GetOtherAttackID()}");

        // �ԐF�ɕύX����
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.red;
        }
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        // �_���[�W����
        // playerState.CleanupInvalidDamageColliders();

        // �J�E���^�[���� Sphere �g�又��
        // var sphere = playerState.GetPlayerCounterObject();
        /*
        if (sphere != null)
        {
            float maxSize = playerState.GetPlayerCounterRange(); // �g��̍ő�T�C�Y
            float scale = Mathf.Lerp(0f, maxSize, (float)freams / playerState.GetPlayerCounterManager().GetCounterSuccessFrames());
            sphere.transform.localScale = new Vector3(scale, scale, scale);
        }
        */

        playerState.GetPlayerCounterObjectManager().UpdateScale(freams / (float)playerState.GetPlayerCounterManager().GetCounterSuccessFrames(),
            playerState.GetPlayerCounterObjectManager().GetCounterMaxSize());

        // �t���[���X�V
        freams += playerState.GetPlayerSpeed();
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        // ������
        freams = 0.0f;

        // �U���^�O��߂�
        // playerState.GetPlayerCounterAttackController().DisableAttack();

        playerState.GetPlayerCounterObjectManager().Deactivate();

        // Sphere ���A�N�e�B�u�ɖ߂�
        //var sphere = playerState.GetPlayerCounterObject();
        /*
        if (sphere != null)
        {
            sphere.SetActive(false);
        }
        */

        // ���G��L���ɂ���
        playerState.GetPlayerStatusEffectManager().StartInvicible(playerState.GetPlayerCounterManager().GetCounterInvincibleFreams());

#if UNITY_EDITOR
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.white;
        }
#endif
    }



}
