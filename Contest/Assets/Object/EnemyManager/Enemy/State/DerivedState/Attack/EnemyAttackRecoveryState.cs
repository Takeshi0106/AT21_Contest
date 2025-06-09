using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// ======================
// 敵の攻撃後状態
// ======================

public class EnemyAttackRecoveryState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private EnemyAttackRecoveryState instance;
    // weponData
    private static BaseAttackData weponData;
    // フレームを計る
    float freams = 0.0f;

    Vector3 vec;

#if UNITY_EDITOR
    // エディタ実行時に実行される
    // 元の色を保存
    Color originalColor;
#endif



    // インスタンスを取得する関数
    public EnemyAttackRecoveryState Instance
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
        // 硬直フレームが終わったら処理
        if (vec.magnitude < 3.0f && enemyState.GetEnemyAttackFlag())
        {
            if (enemyState.GetEnemyConbo() < weponData.GetMaxCombo() - 1)
            {
                // コンボを進める
                enemyState.SetEnemyCombo(enemyState.GetEnemyConbo() + 1);
                enemyState.ChangeState(new EnemyAttackState());
            }
            else
            {
                // コンボ終了 → 立ち状態へ戻る
                enemyState.SetEnemyCombo(0);
                enemyState.ChangeState(new EnemyStandingState());
            }
            return;
        }
        else
        {
            // コンボ終了 → 立ち状態へ戻る
            enemyState.SetEnemyCombo(0);
            enemyState.ChangeState(new EnemyStandingState());
        }
    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        // 武器データ取得
        weponData = enemyState.GetEnemyWeponManager().GetWeaponData(enemyState.GetEnemyWeponNumber());

        /*
        // アニメーション取得
        var animClip = weponData.GetAttackStaggerAnimation(enemyState.GetEnemyConbo());
        var childAnim = enemyState.GetEnemyWeponManager().GetCurrentWeaponAnimator();

        // アニメーション再生
        if (childAnim != null && animClip != null)
        {
            childAnim.CrossFade(animClip.name, 0.1f);
        }
        */

        vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;

#if UNITY_EDITOR

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
        // ダメージ処理
        enemyState.HandleDamage();

        vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;

        // フレーム更新
        freams += enemyState.GetEnemySpeed();
    }



    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {
        // 初期化
        freams = 0;
        //
        enemyState.SetEnemyFlinchCnt(0);

#if UNITY_EDITOR

        // エディタ実行時に色を元に戻す
        if (enemyState.enemyRenderer != null)
        {
            enemyState.enemyRenderer.material.color = originalColor; // 元の色に戻す
        }
#endif
    }



}
