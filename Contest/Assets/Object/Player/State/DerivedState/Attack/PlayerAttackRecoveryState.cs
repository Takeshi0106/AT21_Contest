using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =================================
// 攻撃後の硬直状態（コンボ猶予状態）
// =================================

public class PlayerAttackRecoveryState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerAttackRecoveryState instance;
    // weponData
    private static BaseAttackData weponData;
    // フレームを計る
    float freams = 0.0f;



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
        if ((Input.GetButtonDown("Attack") || playerState.GetPlayerNextReseved() == RESEVEDSTATE.ATTACK)
            && playerState.GetPlayerConbo() < weponData.GetMaxCombo() - 1)
        {
            // コンボを増やす
            playerState.SetPlayerCombo(playerState.GetPlayerConbo() + 1);
            // 攻撃状態に移行
            playerState.ChangeState(PlayerAttackState.Instance);
            return;
        }
        // カウンター状態に変更
        if (Input.GetButtonDown("Counter") || playerState.GetPlayerNextReseved() == RESEVEDSTATE.COUNTER)
        {
            // コンボを初期化する
            playerState.SetPlayerCombo(0);
            // カウンター状態に移行
            playerState.ChangeState(PlayerCounterStanceState.Instance);
            return;
        }
        // 武器を投げる状態に変更
        if (Input.GetButtonDown("Throw") || playerState.GetPlayerNextReseved() == RESEVEDSTATE.THROW)
        {
            // コンボを初期化する
            playerState.SetPlayerCombo(0);

            if (playerState.GetPlayerWeponManager().GetWeaponCount() <= 1)
            {
                // 武器を投げるの失敗状態に移行
                playerState.ChangeState(PlayerThrowFailedState.Instance);
            }
            else
            {
                // 武器を投げる状態に移行
                playerState.ChangeState(PlayerWeaponThrowState.Instance);
            }

            return;
        }
        //空中に浮いていたら
        if ((Input.GetButtonDown("Jump") && !playerState.GetPlayerAirFlag()) ||
            playerState.GetPlayerAirFlag())
        {
            // コンボを初期化する
            playerState.SetPlayerCombo(0);

            playerState.ChangeState(PlayerJumpState.Instance);
            return;
        }
        // 回避状態に移行
        if (Input.GetButtonDown("Avoidance"))
        {
            // コンボを初期化する
            playerState.SetPlayerCombo(0);

            playerState.ChangeState(PlayerAvoidanceState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());
        /*
        // アニメーション再生
        // Animator を取得
        var anim = playerState.GetPlayerAnimator();
        var animClip = weponData.GetAttackStaggerAnimation(playerState.GetPlayerConbo());

        if (anim != null && animClip != null)
        {
            anim.CrossFade(animClip.name, 0.1f);
        }
        */
#if UNITY_EDITOR
        Debug.LogError("PlayerAttackRecoveryState : 開始");

        if (weponData == null)
        {
            Debug.LogError("PlayerAttackState : WeponDataが見つかりません");
            return;
        }
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.blue;
        }

#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        playerState.HandleDamage();

        freams += playerState.GetPlayerSpeed();
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        // 初期化
        freams = 0.0f;
        // ストックの初期化
        playerState.SetPlayerNextReseved(RESEVEDSTATE.NOTHING);

#if UNITY_EDITOR
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.white;
        }
#endif
    }



}
