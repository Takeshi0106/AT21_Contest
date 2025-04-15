using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRecoveryState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerAttackRecoveryState instance;
    // weponData
    private static BaseAttackData weponData;
    // フレームを計る
    int freams = 0;

    // エディタ実行時に実行される
    // 元の色を保存
    Color originalColor;

#if UNITY_EDITOR

#endif



    // インスタンスを取得する関数
    public static PlayerAttackRecoveryState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerAttackRecoveryState();
            }
            return instance;
        }
    }



    // 状態の変更処理
    public override void Change(PlayerState playerState)
    {
        // 攻撃硬直フレームが終わると立ち状態に戻る
        if (freams >= weponData.GetAttackStaggerFrames(playerState.GetPlayerConbo()))
        {
            //　コンボを初期化する
            playerState.SetPlayerCombo(0);
            // 後から硬直状態に移行する
            playerState.ChangeState(PlayerStandingState.Instance);
            return;
        }
        // 攻撃状態に変更
        if ((Input.GetButtonDown("Attack") || playerState.GetPlayerNextAttackReseved())
            && playerState.GetPlayerConbo() < weponData.GetMaxCombo() - 1)
        {
            // コンボを増やす
            playerState.SetPlayerCombo(playerState.GetPlayerConbo() + 1);
            // 攻撃状態に移行
            playerState.ChangeState(PlayerAttackState.Instance);
            // スタックを更新
            playerState.SetPlayerNextAttackReseved(false);
            return;
        }
        // カウンター状態に変更
        if (Input.GetButtonDown("Counter"))
        {
            // コンボを初期化する
            playerState.SetPlayerCombo(0);
            // カウンター状態に移行
            playerState.ChangeState(PlayerCounterStanceState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

        // アニメーション再生
        // Animator を取得
        //var anim = playerState.GetPlayerAnimator();
        // AnimationClip を取得
        var animClip = weponData.GetAttackStaggerAnimation(playerState.GetPlayerConbo());
        var childAnim = playerState.GetPlayerWeponManager().GetCurrentWeaponAnimator();
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

        // エディタ実行時に取得して色を変更する
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // 元の色を保存
            playerState.playerRenderer.material.color = Color.blue;    // カウンター成功時の色
        }

#if UNITY_EDITOR
        Debug.LogError("PlayerAttackRecoveryState : 開始");

        if (weponData == null)
        {
            Debug.LogError("PlayerAttackState : WeponDataが見つかりません");
            return;
        }
        
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        freams++;
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        // 初期化
        freams = 0;

        // エディタ実行時に色を元に戻す
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // 元の色に戻す
        }

#if UNITY_EDITOR

#endif
    }



}
