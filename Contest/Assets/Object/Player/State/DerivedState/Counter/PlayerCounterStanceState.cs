using System.Collections.Generic;
using UnityEngine;

// ================================
// プレイヤーのカウンター構え状態
// ================================

public class PlayerCounterStanceState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerCounterStanceState instance;
    // フレームを計る
    int freams = 0;
    // カウンターの成否
    bool counterOutcome = false;
    // カウンターが有効かどうか
    bool counterActive = false;

    // エディタ実行時に実行される
    // 元の色を保存
    Color originalColor;


#if UNITY_EDITOR

#endif



    // インスタンスを取得する
    public static PlayerCounterStanceState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerCounterStanceState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(PlayerState playerState)
    {
        // カウンターの有効フレームが過ぎたら
        if (freams >= playerState.GetPlayerCounterManager().GetCounterFrames() + 
            playerState.GetPlayerCounterManager().GetCounterStartupFrames())
        {
            playerState.ChangeState(PlayerCounterStaggerState.Instance);
            return;
        }
        // カウンターが成功したら
        if(counterOutcome)
        {
            playerState.ChangeState(PlayerCounterStrikeState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        // エディタ実行時に取得して色を変更する
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // 元の色を保存
            playerState.playerRenderer.material.color = Color.cyan;    // カウンター構え中の色
        }

#if UNITY_EDITOR
        Debug.LogError("CounterStanceState : 開始");

        
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        // **カウンター準備時間の処理**
        if (freams > playerState.GetPlayerCounterManager().GetCounterStartupFrames())
        {
            // 準備完了後にカウンター有効化
            counterActive = true;

            playerState.playerRenderer.material.color = Color.yellow;

#if UNITY_EDITOR

#endif
        }

        if (counterActive)
        {
            playerState.CleanupInvalidDamageColliders(playerState.GetPlayerEnemyAttackTag());

            foreach (var collidedInfo in playerState.GetPlayerCollidedInfos())
            {
                if (collidedInfo.collider != null)
                {
                    MultiTag tag = collidedInfo.multiTag;
                    if (tag != null && tag.HasTag(playerState.GetPlayerCounterPossibleAttack()))
                    {
                        counterOutcome = true;

#if UNITY_EDITOR
                        Debug.Log("カウンター成功！相手: " + collidedInfo.collider.gameObject.name);
#endif
                    }
                }
            }
        }
        else
        {
            playerState.HandleDamage(playerState.GetPlayerEnemyAttackTag());
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
        counterActive = false;

        // エディタ実行時に色を元に戻す
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // 元の色に戻す
        }

#if UNITY_EDITOR

#endif
    }



}
