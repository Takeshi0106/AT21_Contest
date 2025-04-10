using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private static EnemyAttackState instance;
    // weponData
    private static BaseAttackData weponData;
    // �t���[�����v��
    int freams = 0;

    // �C���X�^���X���擾����֐�
    public static EnemyAttackState Instance
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
        if (freams >= weponData.GetAttackStartupFrames(enemyState.GetEnemyConbo()) +
            weponData.GetAttackSuccessFrames(enemyState.GetEnemyConbo()))
        {
            // �ォ��d����ԂɈڍs����
            enemyState.ChangeState(EnemyAttackRecoveryState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
        weponData = enemyState.GetEnemyWeponManager().GetWeaponData(enemyState.GetEnemyWeponNumber());

#if UNITY_EDITOR
        Debug.LogError($"EnemyAttackState : �J�n�iCombo���F{enemyState.GetEnemyConbo() + 1}�j");

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
        freams++;

        // �U�������ON�ɂ���
        if (freams == weponData.GetAttackStartupFrames(enemyState.GetEnemyConbo()))
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
        freams = 0;
    }



}
