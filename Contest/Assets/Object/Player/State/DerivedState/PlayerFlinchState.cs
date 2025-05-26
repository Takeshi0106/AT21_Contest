using UnityEngine;
using UnityEngine.Playables;

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
        // アニメーション再生
        // Animator を取得
        var anim = currentState.GetPlayerAnimator();
        var animClip = currentState.GetPlayerFlinchAnimation();

        if (anim != null && animClip != null)
        {
            anim.CrossFade(animClip.name, 0.1f);
        }
#if UNITY_EDITOR
        else
        {
            Debug.Log("アニメーションが開始されません");
        }
        Debug.Log("怯み状態");
#endif
    }


    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        // ダメージ処理
        playerState.CleanupInvalidDamageColliders();

        freams += playerState.GetPlayerSpeed();

#if UNITY_EDITOR
        // デバッグ時色を青色に変更する
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.blue;
        }
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

        freams = 0;

#if UNITY_EDITOR
        // デバッグ時色を白にする
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.white;
        }
#endif
    }



}
