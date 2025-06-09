using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ======================
// 敵の攻撃状態
// ======================

public class EnemyAttackState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private EnemyAttackState instance;
    // weponData
    private static BaseAttackData weponData;
    // フレームを計る
    float freams = 0.0f;

    // インスタンスを取得する関数
    public EnemyAttackState Instance
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
        if (freams >= (weponData.GetAttackStartupFrames(enemyState.GetEnemyConbo()) +
    weponData.GetAttackSuccessFrames(enemyState.GetEnemyConbo())))
        {
            enemyState.ChangeState(new EnemyAttackRecoveryState());
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        // 武器データ取得
        weponData = enemyState.GetEnemyWeponManager().GetWeaponData(enemyState.GetEnemyWeponNumber());

        // アニメーション取得
        var animClip = weponData.GetAttackAnimation(enemyState.GetEnemyConbo());
        var Anim = enemyState.GetEnemyAnimator();
        
        // アニメーション再生
        if (Anim != null && animClip != null)
        {
            Anim.CrossFade(animClip.name, 0.2f);
        }

        // 攻撃力を更新
        enemyState.GetAttackInterface().SetSelfAttackDamage(weponData.GetDamage(enemyState.GetEnemyConbo()));
        // スタン力を更新
        enemyState.GetAttackInterface().SetSelfStanAttackDamage(weponData.GetStanDamage(enemyState.GetEnemyConbo()));



#if UNITY_EDITOR

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
        // ダメージ処理
        enemyState.HandleDamage();

        // フレーム更新
        freams += enemyState.GetEnemySpeed();

        // 攻撃判定をONにする
        if (freams >= weponData.GetAttackStartupFrames(enemyState.GetEnemyConbo()))
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
        freams = 0.0f;
    }



}
