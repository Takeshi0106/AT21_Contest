using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyMoveState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private EnemyMoveState instance;

    int i = 0;

    // インスタンスを取得する関数
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

    // 状態の変更処理
    public override void Change(EnemyState enemyState)
    {
        Vector3 vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;

        // 攻撃状態
        if (vec.magnitude < enemyState.GetDistanceAttack() && enemyState.GetEnemyAttackFlag())
        {
            enemyState.ChangeState(new EnemyAttackState());
            return;
        }
        // 立ち状態
        if (vec.magnitude < 7.5f && !enemyState.GetEnemyAttackFlag())
        {
            enemyState.ChangeState(new EnemyStandingState());
            return;
        }
        // 怯み状態
        if (enemyState.GetEnemyDamageFlag() && enemyState.GetEnemyFlinchCnt() < 1)
        {
            enemyState.ChangeState(new EnemyFlinchState());
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        Debug.Log("Move 開始");
    }



    // 状態中の処理
    public override void Excute(EnemyState enemyState)
    {
        Vector3 vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;


        enemyState.Target();

        if (vec.magnitude > 8.0f)
        {
            enemyState.GetEnemyRigidbody().velocity = enemyState.transform.forward * enemyState.GetEnemyDashSpeed();

            if (i == 0)
            {
                // 走り状態アニメーション開始
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



    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {
        i = 0;
    }
}
