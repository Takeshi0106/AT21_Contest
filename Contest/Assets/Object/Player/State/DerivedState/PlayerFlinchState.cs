using UnityEngine;

// =======================================
// プレイヤーの怯み状態
// =======================================

public class PlayerFlinchState : StateClass<PlayerState>
{
    // インスタンスを取得する
    protected static PlayerFlinchState instance;
    // フレームを計測する
    protected float freams = 0.0f;


    // インスタンスを取得する関数
    public static PlayerFlinchState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerFlinchState();
            }
            return instance;
        }
    }


    // 状態を変更する
    public override void Change(PlayerState playerState)
    {
        if (freams > playerState.GetPlayerFlinchFreams())
        {
            playerState.ChangeState(PlayerStandingState.Instance);
        }
    }


    // 状態の開始処理
    public override void Enter(PlayerState currentState)
    {

    }


    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        freams += playerState.GetPlayerSpeed();

#if UNITY_EDITOR
        // デバッグ時色を青色に変更する
        playerState.GetPlayerRenderer().material.color = Color.blue;
#endif

        /*
        // 状態変更したときのInputを無効にする　攻撃ボタンを押していたら、次の行動を予約
        if (freams > 1)
        {
            if (Input.GetButtonDown("Attack"))
            {
                playerState.SetPlayerNextReseved(RESEVEDSTATE.ATTACK);
            }
            if (Input.GetButtonDown("Counter"))
            {
                playerState.SetPlayerNextReseved(RESEVEDSTATE.COUNTER);
            }
            if (Input.GetButtonDown("Throw"))
            {
                playerState.SetPlayerNextReseved(RESEVEDSTATE.COUNTER);
            }
        }
        */
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        /*
        // ストックの初期化
        playerState.SetPlayerNextReseved(RESEVEDSTATE.NOTHING);
        */

#if UNITY_EDITOR
        // デバッグ時色を白にする
        playerState.GetPlayerRenderer().material.color = Color.white;
#endif
    }



}
