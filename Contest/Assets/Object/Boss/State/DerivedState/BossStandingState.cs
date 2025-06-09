using UnityEngine;

public class BossStandingState : StateClass<BossState>
{
    // フレームを計る
    float freams = 0;


    // 状態の変更処理
    public override void Change(BossState bossState)
    {
        Vector3 vec = bossState.GetPlayerState().transform.position - bossState.transform.position;

        // 移動状態に移行する
        if (vec.magnitude > 8.0f || bossState.GetEnemyAttackFlag())
        {
            bossState.ChangeState(new BossMoveState());
        }
    }



    // 状態の開始処理
    public override void Enter(BossState bossState)
    {
        // 立ち状態アニメーション開始
        if (bossState.GetEnemyAnimator() != null && bossState.GetEnemyStandingAnimation() != null)
        {
            bossState.GetEnemyAnimator().CrossFade(bossState.GetEnemyStandingAnimation().name, 0.1f);
        }

        // デバッグ用に攻撃状態に移行するフレームを決める
        // waitTime = Random.Range(30, 120);

#if UNITY_EDITOR
        Debug.LogError("EnemyStandingState : 開始");
#endif
    }



    // 状態中の処理
    public override void Excute(BossState bossState)
    {
        // ダメージ処理
        bossState.HandleDamage();

        bossState.Target();

        // フレーム更新
        freams += bossState.GetEnemySpeed();

        //プレイヤーの方を向かせる

        //スムーズにプレイヤーの方を向かせる処理
        Quaternion targetRotation = Quaternion.LookRotation(
            bossState.GetTargetObject().transform.position - bossState.transform.position);

        float angle = Quaternion.Angle(bossState.transform.rotation, targetRotation);

        float speed = angle / 5f;

        bossState.transform.rotation = Quaternion.Slerp(
            bossState.transform.rotation, targetRotation, Time.deltaTime * speed);
    }



    // 状態中の終了処理
    public override void Exit(BossState bossState)
    {
        freams = 0.0f;
    }



}
