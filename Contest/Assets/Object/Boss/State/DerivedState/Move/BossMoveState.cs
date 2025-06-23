using UnityEngine;

public class BossMoveState : StateClass<BossState>
{
    // 状態の変更処理
    public override void Change(BossState bossState)
    {
        Vector3 vec = bossState.GetPlayerState().transform.position - bossState.transform.position;

        // 攻撃状態
        if (vec.magnitude < bossState.GetDistanceAttack())
        {
            bossState.ChangeState(new BossAttackState());
            return;
        }
        // 立ち状態
        if (vec.magnitude < 7.5f)
        {
            bossState.ChangeState(new BossStandingState());
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(BossState bossState)
    {
        Debug.Log("BossState : Move 開始");

        // 走り状態アニメーション開始
        if (bossState.GetEnemyAnimator() != null && bossState.GetEnemyDashAnimation() != null)
        {
            bossState.GetEnemyAnimator().CrossFade(bossState.GetEnemyDashAnimation().name, 0.1f);
        }
    }



    // 状態中の処理
    public override void Excute(BossState bossState)
    {
        Vector3 vec = bossState.GetPlayerState().transform.position - bossState.transform.position;


        bossState.Target();

        if (vec.magnitude > 1.0f)
        {
            bossState.GetEnemyRigidbody().velocity = bossState.transform.forward * bossState.GetEnemyDashSpeed();
        }
    }



    // 状態中の終了処理
    public override void Exit(BossState bossState)
    {

    }
}
