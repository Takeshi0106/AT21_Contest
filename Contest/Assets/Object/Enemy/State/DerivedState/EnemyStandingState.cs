using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandingState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private static EnemyStandingState instance;



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
        if (enemyState.GetChildTransform() != null)
        {
            enemyState.ChangeState(EnemySwordAttackState.Instance);
        }
    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
#if UNITY_EDITOR
        Debug.LogError("EnemyStandingState : 開始");
#endif
    }



    // 状態中の処理
    public override void Excute(EnemyState enemyState)
    {

    }



    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {

    }



}
