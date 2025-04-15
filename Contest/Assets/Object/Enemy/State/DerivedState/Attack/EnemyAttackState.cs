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
        if (freams >= (weponData.GetAttackStartupFrames(enemyState.GetEnemyConbo()) +
    weponData.GetAttackSuccessFrames(enemyState.GetEnemyConbo())))
        {
            enemyState.ChangeState(EnemyAttackRecoveryState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        weponData = enemyState.GetEnemyWeponManager().GetWeaponData(enemyState.GetEnemyWeponNumber());

        // アニメーション再生
        // Animator を取得
        //var anim = enemyState.GetEnemyAnimator();
        // AnimationClip を取得
        var animClip = weponData.GetAttackAnimation(enemyState.GetEnemyConbo());
        var childAnim = enemyState.GetEnemyWeponManager().GetCurrentWeaponAnimator();
        /*
        if (anim != null && animClip != null)
        {
            // anim.CrossFade(animClip.name, 0.2f);
        }
        */
        if (childAnim != null && animClip != null)
        {
            childAnim.CrossFade(animClip.name, 0.2f);
        }

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
        freams = 0;
    }



}
