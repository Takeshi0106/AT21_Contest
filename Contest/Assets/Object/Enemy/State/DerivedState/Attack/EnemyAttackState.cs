using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private static EnemyAttackState instance;
    // weponData
    private static BaseAttackData weponData;
    // フレームを計る
    int freams = 0;

    // インスタンスを取得する関数
    public static EnemyAttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyAttackState();
            }
            return instance;
        }
    }



    // 状態の変更処理
    public override void Change(EnemyState enemyState)
    {
        // 攻撃のフレームが過ぎたら
        if (freams >= weponData.GetAttackStartupFrames(enemyState.GetEnemyConbo()) +
            weponData.GetAttackSuccessFrames(enemyState.GetEnemyConbo()))
        {
            // 後から硬直状態に移行する
            enemyState.ChangeState(EnemyAttackRecoveryState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        weponData = enemyState.GetEnemyWeponManager().GetWeaponData(enemyState.GetEnemyWeponNumber());

#if UNITY_EDITOR
        Debug.LogError($"EnemyAttackState : 開始（Combo数：{enemyState.GetEnemyConbo() + 1}）");

        if (weponData == null)
        {
            Debug.LogError("EnemyAttackState : WeponDataが見つかりません");
            return;
        }
#endif
    }



    // 状態中の処理
    public override void Excute(EnemyState enemyState)
    {
        freams++;

        // 攻撃判定をONにする
        if (freams == weponData.GetAttackStartupFrames(enemyState.GetEnemyConbo()))
        {
            enemyState.GetEnemyWeponManager().EnableAllWeaponAttacks();
        }
    }



    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {
        // 攻撃判定をOFF
        enemyState.GetEnemyWeponManager().DisableAllWeaponAttacks();
        // 初期化
        freams = 0;
    }



}
