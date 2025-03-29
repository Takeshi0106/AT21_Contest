using UnityEngine;

// =====================================
// プレイヤーの何もしていない状態
// =====================================

public class StandingState : StateClass
{
    // インスタンスを入れる変数
    private static StandingState instance;
    // PlayerStateを入れる変数
    PlayerState playerState;

    // インスタンスを取得する関数
    public static StandingState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StandingState();
            }
            return instance;
        }
    }

    // 状態の変更処理
    public override void Change(GameObject player)
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)   
        {
            playerState.ChangPlayerState(MoveState.Instance);
        }
    }

    // 状態の開始処理
    public override void Enter(GameObject player)
    {
        Debug.LogError("StandingState : 開始");

        playerState = player.GetComponent<PlayerState>();
        if (playerState == null)
        {
            Debug.LogError("StandingState : PlayerStateが見つかりません");
            return;
        }
    }

    // 状態中の処理
    public override void Excute(GameObject player)
    {
    
    }

    // 状態中の終了処理
    public override void Exit(GameObject player)
    {

    }
}
