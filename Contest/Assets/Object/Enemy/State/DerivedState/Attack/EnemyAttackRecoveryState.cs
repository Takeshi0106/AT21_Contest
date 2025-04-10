using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyAttackRecoveryState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private static EnemyAttackRecoveryState instance;
    // weponData
    private static BaseAttackData weponData;
    // �t���[�����v��
    int freams = 0;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // ���̐F��ۑ�
    Color originalColor;
#endif

    // �C���X�^���X���擾����֐�
    public static EnemyAttackRecoveryState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyAttackRecoveryState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(EnemyState enemyState)
    {
        // �U���d���t���[�����I���Ɨ�����Ԃɖ߂�
        if (freams >= weponData.GetAttackStaggerFrames(enemyState.GetEnemyConbo()))
        {
            //�@�R���{������������
            enemyState.SetEnemyCombo(0);
            // �ォ��d����ԂɈڍs����
            enemyState.ChangeState(EnemyStandingState.Instance);
            return;
        }
        /*
        // �U����ԂɕύX
        if (enemyState.GetEnemyConbo() < weponData.GetMaxCombo() - 1)
        {
            // �R���{�𑝂₷
            EnemyState.SetEnemyCombo(enemyState.GetEnemyConbo() + 1);
            // �U����ԂɈڍs
            enemyState.ChangeState(EnemyAttackState.Instance);
            return;
        }
        */
    }



    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
        weponData = enemyState.GetEnemyWeponManager().GetWeaponData(enemyState.GetEnemyWeponNumber());

#if UNITY_EDITOR
        Debug.LogError("EnemyAttackRecoveryState : �J�n");

        if (weponData == null)
        {
            Debug.LogError("EnemyAttackState : WeponData��������܂���");
            return;
        }
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        if (enemyState.enemyRenderer != null)
        {
            originalColor = enemyState.enemyRenderer.material.color; // ���̐F��ۑ�
            enemyState.enemyRenderer.material.color = Color.blue;    // �J�E���^�[�������̐F
        }
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {
        freams++;
    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {
        // ������
        freams = 0;
#if UNITY_EDITOR
        // �G�f�B�^���s���ɐF�����ɖ߂�
        if (enemyState.enemyRenderer != null)
        {
            enemyState.enemyRenderer.material.color = originalColor; // ���̐F�ɖ߂�
        }
#endif
    }



}
