using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =====================================
// 投げる武器がないときの状態
// =====================================

public class PlayerThrowFailedState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerThrowFailedState instance;
    // 武器の情報
    private static BaseAttackData weponData;

    float freams = 0.0f;



    // インスタンスを取得する
    public static PlayerThrowFailedState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerThrowFailedState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(PlayerState playerState)
    {
        // 立ち状態に戻す
        if (freams > playerState.GetThrowFailedFreams())
        {
            playerState.ChangeState(PlayerCounterStaggerState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        AnimationClip animClip = playerState.GetThrowFailedAnimation();
        var childAnim = playerState.GetPlayerWeponManager().GetCurrentWeaponAnimator();

        // 武器データを取得する
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

        // アニメーション開始処理
        if (animClip != null && childAnim != null)
        {
            childAnim.CrossFade(animClip.name, 0.2f);
        }

#if UNITY_EDITOR
        Debug.LogError("PlayerWeaponThrowFailedState : 開始");

        // エディタ実行時に取得して色を変更する
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.blue;
        }
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        // ダメージ処理を有効にする
        playerState.HandleDamage();

        freams += playerState.GetPlayerSpeed();
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        freams = 0.0f;

#if UNITY_EDITOR
        // エディタ実行時に色を元に戻す
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.white;
        }
#endif
    }



}
