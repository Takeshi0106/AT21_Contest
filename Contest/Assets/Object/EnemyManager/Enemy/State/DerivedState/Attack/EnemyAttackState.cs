using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ======================
// �G�̍U�����
// ======================

public class EnemyAttackState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private EnemyAttackState instance;
    // weponData
    private static BaseAttackData weponData;
    // �t���[�����v��
    float freams = 0.0f;

    // �C���X�^���X���擾����֐�
    public EnemyAttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyAttackState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(EnemyState enemyState)
    {
        // �U���̃t���[�����߂�����
        if (freams >= (weponData.GetAttackStartupFrames(enemyState.GetEnemyConbo()) +
    weponData.GetAttackSuccessFrames(enemyState.GetEnemyConbo())))
        {
            enemyState.ChangeState(new EnemyAttackRecoveryState());
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
        // ����f�[�^�擾
        weponData = enemyState.GetEnemyWeponManager().GetWeaponData(enemyState.GetEnemyWeponNumber());

        // �A�j���[�V�����擾
        var animClip = weponData.GetAttackAnimation(enemyState.GetEnemyConbo());
        var Anim = enemyState.GetEnemyAnimator();
        
        // �A�j���[�V�����Đ�
        if (Anim != null && animClip != null)
        {
            Anim.CrossFade(animClip.name, 0.2f);
        }

        // �U���͂��X�V
        enemyState.GetAttackInterface().SetSelfAttackDamage(weponData.GetDamage(enemyState.GetEnemyConbo()));
        // �X�^���͂��X�V
        enemyState.GetAttackInterface().SetSelfStanAttackDamage(weponData.GetStanDamage(enemyState.GetEnemyConbo()));



#if UNITY_EDITOR

        if (weponData == null)
        {
            Debug.LogError("EnemyAttackState : WeponData��������܂���");
            return;
        }
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {
        // �_���[�W����
        enemyState.HandleDamage();

        // �t���[���X�V
        freams += enemyState.GetEnemySpeed();

        // �U�������ON�ɂ���
        if (freams >= weponData.GetAttackStartupFrames(enemyState.GetEnemyConbo()))
        {
            enemyState.GetEnemyWeponManager().EnableAllWeaponAttacks();
        }
    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {
        // �U�������OFF
        enemyState.GetEnemyWeponManager().DisableAllWeaponAttacks();
        // ������
        freams = 0.0f;
    }



}
