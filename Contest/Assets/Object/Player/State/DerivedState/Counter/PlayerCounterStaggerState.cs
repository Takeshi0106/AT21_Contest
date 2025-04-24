using System.Collections.Generic;
using UnityEngine;

// ================================
// プレイヤーのカウンター失敗状態
// ================================

public class PlayerCounterStaggerState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerCounterStaggerState instance;
    // フレームを計る
    int freams = 0;



    // インスタンスを取得する
    public static PlayerCounterStaggerState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerCounterStaggerState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(PlayerState playerState)
    {
        // カウンター失敗フレームが過ぎたら
        if (freams >= playerState.GetPlayerCounterManager().GetCounterStaggerFrames())
        {
            playerState.ChangeState(PlayerStandingState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
#if UNITY_EDITOR
        Debug.LogError("CounterStaggerState : 開始");

        playerState.GetPlayerRenderer().material.color = Color.blue;
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {

        playerState.HandleDamage(playerState.GetPlayerEnemyAttackTag());

        freams++;
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        // 初期化
        freams = 0;


#if UNITY_EDITOR
        // エディタ実行時に色を元に戻す
        playerState.GetPlayerRenderer().material.color = Color.white;
#endif
    }



}
