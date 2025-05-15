using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyStandingState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private static EnemyStandingState instance;
    // �t���[�����v��
    float freams = 0;
    int waitTime = 0;

    // �C���X�^���X���擾����֐�
    public static EnemyStandingState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyStandingState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(EnemyState enemyState)
    {
        Vector3 vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;

        // �ړ���ԂɈڍs����
        if (vec.magnitude > 8.0f || enemyState.GetEnemyAttackFlag())
        {
            enemyState.ChangeState(EnemyMoveState.Instance);
        }
        // ���ݏ�ԂɈڍs
        if (enemyState.GetEnemyDamageFlag() && enemyState.GetEnemyFlinchCnt() < 1)
        {
            enemyState.ChangeState(EnemyFlinchState.Instance);
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
        // ������ԃA�j���[�V�����J�n
        if (enemyState.GetEnemyAnimator() != null && enemyState.GetEnemyStandingAnimation() != null)
        {
            enemyState.GetEnemyAnimator().CrossFade(enemyState.GetEnemyStandingAnimation().name, 0.1f);
        }

        // �f�o�b�O�p�ɍU����ԂɈڍs����t���[�������߂�
        // waitTime = Random.Range(30, 120);

#if UNITY_EDITOR
        Debug.LogError("EnemyStandingState : �J�n");
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {
        // �_���[�W����
        enemyState.HandleDamage();

        enemyState.Target();

        // �t���[���X�V
        freams += enemyState.GetEnemySpeed();
    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {
        freams = 0.0f;
    }



}
