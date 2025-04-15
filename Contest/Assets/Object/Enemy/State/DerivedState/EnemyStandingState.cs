using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandingState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private static EnemyStandingState instance;
    // フレームを計る
    int freams = 0;
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
        if (freams > waitTime)
        {
            enemyState.ChangeState(EnemyAttackState.Instance);
        }
    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        waitTime = Random.Range(30, 120);

#if UNITY_EDITOR
        Debug.LogError("EnemyStandingState : 開始");
#endif
    }



    // 状態中の処理
    public override void Excute(EnemyState enemyState)
    {
        freams++;
    }



    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {
        freams = 0;
    }



}
