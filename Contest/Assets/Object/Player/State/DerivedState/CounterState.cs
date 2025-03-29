using UnityEngine;

// ================================
// プレイヤーのカウンター状態
// ================================

// 必須！！
#if UNITY_EDITOR
[RequireComponent(typeof(Renderer))]
#endif

public class CounterState : StateClass
{
    // インスタンスを入れる変数
    private static CounterState instance;
    // PlayerStateを入れる変数
    PlayerState playerState;
    // フレームを計る
    int freams = 0;
    // デバッグ用
    // 色を変更する
    Renderer objectRenderer;
    Color originalColor; // 元の色を保存



    // インスタンスを取得する
    public static CounterState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CounterState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(GameObject player)
    {
        if (freams >= playerState.CounterActiveFreams)
        {
            playerState.ChangPlayerState(StandingState.Instance);
        }
    }



    // 状態の開始処理
    public override void Enter(GameObject player)
    {
        Debug.LogError("CounterState : 開始");

        playerState = player.GetComponent<PlayerState>();
        if (playerState == null)
        {
            Debug.LogError("CounterState : PlayerStateが見つかりません");
            return;
        }


#if UNITY_EDITOR
        // エディタ実行時に取得して色を変更する
        objectRenderer = player.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color; // 元の色を保存
            objectRenderer.material.color = Color.red;    // カウンター中の色（例: 赤）
        }
#endif
    }



    // 状態中の処理
    public override void Excute(GameObject player)
    {
        // フレーム数を計る
        freams++;
    }



    // 状態中の終了処理
    public override void Exit(GameObject player)
    {
        // 初期化
        freams = 0;


#if UNITY_EDITOR
        // エディタ実行時に色を元に戻す
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor; // 元の色に戻す
        }
#endif
    }



}
