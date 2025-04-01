using System.Collections.Generic;
using UnityEngine;

// ================================
// プレイヤーのカウンター構え状態
// ================================

public class CounterStanceState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static CounterStanceState instance;
    // フレームを計る
    int freams = 0;
    // カウンターの成否
    bool counterOutcome = false;

#if UNITY_EDITOR
    // エディタ実行時に実行される
    // 元の色を保存
    Color originalColor;
#endif



    // インスタンスを取得する
    public static CounterStanceState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CounterStanceState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(PlayerState playerState)
    {
        // カウンターの有効フレームが過ぎたら
        if (freams >= playerState.GetPlayerCounterManager().GetCounterFrames())
        {
            playerState.ChangeState(CounterStaggerState.Instance);
        }
        // カウンターが成功したら
        if(counterOutcome)
        {
            playerState.ChangeState(CounterStrikeState.Instance);
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        Debug.LogError("CounterStanceState : 開始");


#if UNITY_EDITOR
        // エディタ実行時に取得して色を変更する
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // 元の色を保存
            playerState.playerRenderer.material.color = Color.yellow;    // カウンター構え中の色
        }
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        // カウンターの成否判定
        if (freams <= playerState.GetPlayerCounterManager().GetCounterFrames())
        {
            // ぶつかったオブジェクトのタグをチェック
            foreach (var obj in playerState.GetCollidedObjects())
            {
                if (obj != null && obj.CompareTag("ParryableAttack"))
                {
                    Debug.Log("カウンター成功！ 相手: " + obj.gameObject.name);
                    counterOutcome = true;
                }
            }
        }
        // フレーム数を計る
        freams++;
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        // 初期化
        freams = 0;
        counterOutcome = false;

#if UNITY_EDITOR
        // エディタ実行時に色を元に戻す
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // 元の色に戻す
        }
#endif
    }



}
