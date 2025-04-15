using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyAttackRecoveryState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private static EnemyAttackRecoveryState instance;
    // weponData
    private static BaseAttackData weponData;
    // フレームを計る
    int freams = 0;

#if UNITY_EDITOR
    // エディタ実行時に実行される
    // 元の色を保存
    Color originalColor;
#endif

    // インスタンスを取得する関数
    public static EnemyAttackRecoveryState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyAttackRecoveryState();
            }
            return instance;
        }
    }



    // 状態の変更処理
    public override void Change(EnemyState enemyState)
    {
        // 攻撃硬直フレームが終わると立ち状態に戻る
        if (freams >= weponData.GetAttackStaggerFrames(enemyState.GetEnemyConbo()))
        {
            //　コンボを初期化する
            enemyState.SetEnemyCombo(0);
            // 後から硬直状態に移行する
            enemyState.ChangeState(EnemyStandingState.Instance);
            return;
        }
        // 攻撃状態に変更
        if (freams >= weponData.GetAttackStaggerFrames(enemyState.GetEnemyConbo()) - 1 &&
            enemyState.GetEnemyConbo() < weponData.GetMaxCombo() - 1)
        {
            // コンボを増やす
            enemyState.SetEnemyCombo(enemyState.GetEnemyConbo() + 1);
            // 攻撃状態に移行
            enemyState.ChangeState(EnemyAttackState.Instance);
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
        var animClip = weponData.GetAttackStaggerAnimation(enemyState.GetEnemyConbo());
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
        Debug.LogError("EnemyAttackRecoveryState : 開始");

        if (weponData == null)
        {
            Debug.LogError("EnemyAttackState : WeponDataが見つかりません");
            return;
        }
        // エディタ実行時に取得して色を変更する
        if (enemyState.enemyRenderer != null)
        {
            originalColor = enemyState.enemyRenderer.material.color; // 元の色を保存
            enemyState.enemyRenderer.material.color = Color.blue;    // カウンター成功時の色
        }
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
        // 初期化
        freams = 0;
#if UNITY_EDITOR
        // エディタ実行時に色を元に戻す
        if (enemyState.enemyRenderer != null)
        {
            enemyState.enemyRenderer.material.color = originalColor; // 元の色に戻す
        }
#endif
    }



}
