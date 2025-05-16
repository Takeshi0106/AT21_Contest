using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyMoveState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private EnemyMoveState instance;

    int i = 0;

    // �C���X�^���X���擾����֐�
    public EnemyMoveState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyMoveState();
            }
            return instance;
        }
    }

    // ��Ԃ̕ύX����
    public override void Change(EnemyState enemyState)
    {
        Vector3 vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;

        // �U�����
        if (vec.magnitude < enemyState.GetDistanceAttack() && enemyState.GetEnemyAttackFlag())
        {
            enemyState.ChangeState(new EnemyAttackState());
            return;
        }
        // �������
        if (vec.magnitude < 7.5f && !enemyState.GetEnemyAttackFlag())
        {
            enemyState.ChangeState(new EnemyStandingState());
            return;
        }
        // ���ݏ��
        if (enemyState.GetEnemyDamageFlag() && enemyState.GetEnemyFlinchCnt() < 1)
        {
            enemyState.ChangeState(new EnemyFlinchState());
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
        Debug.Log("Move �J�n");
    }



    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {
        Vector3 vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;


        enemyState.Target();

        if (vec.magnitude > 8.0f)
        {
            enemyState.GetEnemyRigidbody().velocity = enemyState.transform.forward * enemyState.GetEnemyDashSpeed();

            if (i == 0)
            {
                // �����ԃA�j���[�V�����J�n
                if (enemyState.GetEnemyAnimator() != null && enemyState.GetEnemyDashAnimation() != null)
                {
                    enemyState.GetEnemyAnimator().CrossFade(enemyState.GetEnemyDashAnimation().name, 0.1f);
                }
            }
            i++;
        }
        else if (vec.magnitude > 1.0f && enemyState.GetEnemyAttackFlag())
        {
            enemyState.GetEnemyRigidbody().velocity = enemyState.transform.forward * enemyState.GetEnemyDashSpeed();
        }
    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {
        i = 0;
    }
}
