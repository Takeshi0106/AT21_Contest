using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyStandingState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private static EnemyStandingState instance;
    // フレームを計る
    float freams = 0;
    int waitTime = 0;

    // インスタンスを取得する関数
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



    // 状態の変更処理
    public override void Change(EnemyState enemyState)
    {
        Vector3 vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;

        // 移動状態に移行する
        if (vec.magnitude > 8.0f || enemyState.GetEnemyAttackFlag())
        {
            enemyState.ChangeState(EnemyMoveState.Instance);
        }
        // 怯み状態に移行
        if (enemyState.GetEnemyDamageFlag() && enemyState.GetEnemyFlinchCnt() < 1)
        {
            enemyState.ChangeState(EnemyFlinchState.Instance);
        }
    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        // 立ち状態アニメーション開始
        if (enemyState.GetEnemyAnimator() != null && enemyState.GetEnemyStandingAnimation() != null)
        {
            enemyState.GetEnemyAnimator().CrossFade(enemyState.GetEnemyStandingAnimation().name, 0.1f);
        }

        // デバッグ用に攻撃状態に移行するフレームを決める
        // waitTime = Random.Range(30, 120);

#if UNITY_EDITOR
        Debug.LogError("EnemyStandingState : 開始");
#endif
    }



    // 状態中の処理
    public override void Excute(EnemyState enemyState)
    {
        // ダメージ処理
        enemyState.HandleDamage();

        enemyState.Target();

        // フレーム更新
        freams += enemyState.GetEnemySpeed();
    }



    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {
        freams = 0.0f;
    }



}
