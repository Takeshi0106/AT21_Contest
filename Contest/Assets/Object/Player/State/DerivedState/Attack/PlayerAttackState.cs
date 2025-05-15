using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

// ======================================
// プレイヤーの攻撃状態
// ======================================

public class PlayerAttackState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerAttackState instance;
    // weponData
    private static BaseAttackData weponData;
    // フレームを計る
    float freams = 0.0f;

    // インスタンスを取得する関数
    public static PlayerAttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerAttackState();
            }
            return instance;
        }
    }



    // 状態の変更処理
    public override void Change(PlayerState playerState)
    {
        // 攻撃のフレームが過ぎたら
        if (freams >= weponData.GetAttackStartupFrames(playerState.GetPlayerConbo()) +
            weponData.GetAttackSuccessFrames(playerState.GetPlayerConbo()))
        {
            // 後から硬直状態に移行する
            playerState.ChangeState(PlayerAttackRecoveryState.Instance);
            return;
        }
        if (Input.GetButtonDown("Counter"))
        {
            // コンボを初期化する
            playerState.SetPlayerCombo(0);
            // カウンター状態に移行
            playerState.ChangeState(PlayerCounterStanceState.Instance);
            return;
        }
        if (Input.GetButtonDown("Throw"))
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
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

        // アニメーション再生
        // Animator を取得
        var anim = playerState.GetPlayerAnimator();
        var animClip = weponData.GetAttackAnimation(playerState.GetPlayerConbo());

        if (anim != null && animClip != null)
        {
            anim.CrossFade(animClip.name, 0.1f);
        }

#if UNITY_EDITOR
        Debug.LogError($"PlayerAttackState : 開始（Combo数：{playerState.GetPlayerConbo() + 1}）");

        if (weponData == null)
        {
            Debug.LogError("PlayerAttackState : WeponDataが見つかりません");
            return;
        }
        // Debug.Log($"Playするアニメーション: {animClip.name}");
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        playerState.HandleDamage();

        // 攻撃判定をONにする
        if (freams >= weponData.GetAttackStartupFrames(playerState.GetPlayerConbo()))
        {
            playerState.GetPlayerWeponManager().EnableAllWeaponAttacks();
        }

        // 状態変更したときのInputを無効にする　攻撃ボタンを押していたら、次の攻撃を予約
        if (freams > 0.1)
        {
            if (Input.GetButtonDown("Attack"))
            {
                playerState.SetPlayerNextReseved(RESEVEDSTATE.ATTACK);
            }
            
        }

        freams += playerState.GetPlayerSpeed();
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        // 攻撃判定をOFF
        playerState.GetPlayerWeponManager().DisableAllWeaponAttacks();
        // 初期化
        freams = 0.0f;
    }



}
