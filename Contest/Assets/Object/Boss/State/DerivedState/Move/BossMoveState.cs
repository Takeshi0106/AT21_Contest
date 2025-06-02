using UnityEngine;

public class BossMoveState : StateClass<BossState>
{
    // ��Ԃ̕ύX����
    public override void Change(BossState bossState)
    {
        Vector3 vec = bossState.GetPlayerState().transform.position - bossState.transform.position;

        // �U�����
        if (vec.magnitude < bossState.GetDistanceAttack() && bossState.GetEnemyAttackFlag())
        {
            // bossState.ChangeState(new EnemyAttackState());
            return;
        }
        // �������
        if (vec.magnitude < 7.5f && !bossState.GetEnemyAttackFlag())
        {
            bossState.ChangeState(new BossStandingState());
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(BossState bossState)
    {
        Debug.Log("BossState : Move �J�n");

        // �����ԃA�j���[�V�����J�n
        if (bossState.GetEnemyAnimator() != null && bossState.GetEnemyDashAnimation() != null)
        {
            bossState.GetEnemyAnimator().CrossFade(bossState.GetEnemyDashAnimation().name, 0.1f);
        }
    }



    // ��Ԓ��̏���
    public override void Excute(BossState bossState)
    {
        Vector3 vec = bossState.GetPlayerState().transform.position - bossState.transform.position;


        bossState.Target();

        if (vec.magnitude > 8.0f)
        {
            bossState.GetEnemyRigidbody().velocity = bossState.transform.forward * bossState.GetEnemyDashSpeed();
        }
        else if (vec.magnitude > 1.0f && bossState.GetEnemyAttackFlag())
        {
            bossState.GetEnemyRigidbody().velocity = bossState.transform.forward * bossState.GetEnemyDashSpeed();
        }
    }



    // ��Ԓ��̏I������
    public override void Exit(BossState bossState)
    {

    }
}
