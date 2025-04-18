using System.Collections.Generic;
using UnityEngine;

// ================================
// プレイヤーのカウンター攻撃状態
// ================================

public class PlayerCounterStrikeState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerCounterStrikeState instance;
    // フレームを計る
    int freams = 0;

    // エディタ実行時に実行される
    // 元の色を保存
    Color originalColor;

#if UNITY_EDITOR

#endif



    // インスタンスを取得する
    public static PlayerCounterStrikeState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerCounterStrikeState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(PlayerState playerState)
    {
        // カウンターの有効フレームが過ぎたら
        if (freams >= playerState.GetPlayerCounterManager().GetCounterSuccessFrames())
        {
            playerState.ChangeState(PlayerStandingState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        // ゲージ量アップ
        playerState.GetPlayerCounterManager().IncreaseGauge();

        // Sphere を初期サイズにし、アクティブ化
        playerState.GetPlayerCounterObject().transform.localScale = Vector3.zero;
        playerState.GetPlayerCounterObject().SetActive(true);

        // 攻撃タグを付ける
        playerState.GetPlayerCounterAttackController().EnableAttack();

        // エディタ実行時に取得して色を変更する
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // 元の色を保存
            playerState.playerRenderer.material.color = Color.red;    // カウンター成功時の色
        }

#if UNITY_EDITOR
        Debug.LogError("CounterStrikeState : 開始");


#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        playerState.CleanupInvalidDamageColliders(playerState.GetPlayerEnemyAttackTag());

        freams++;

        // カウンター中の Sphere 拡大処理
        var sphere = playerState.GetPlayerCounterObject();
        if (sphere != null)
        {
            float maxSize = playerState.GetPlayerCounterRange(); // 拡大の最大サイズ
            float scale = Mathf.Lerp(0f, maxSize, (float)freams / playerState.GetPlayerCounterManager().GetCounterSuccessFrames());
            sphere.transform.localScale = new Vector3(scale, scale, scale);
        }
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        // 初期化
        freams = 0;

        // 攻撃タグを戻す
        playerState.GetPlayerCounterAttackController().DisableAttack();

        // Sphere を非アクティブに戻す
        var sphere = playerState.GetPlayerCounterObject();
        if (sphere != null)
        {
            sphere.SetActive(false);
        }

        // エディタ実行時に色を元に戻す
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // 元の色に戻す
        }

#if UNITY_EDITOR

#endif
    }



}
