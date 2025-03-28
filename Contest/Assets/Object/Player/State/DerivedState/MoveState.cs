using System;
using UnityEngine;

// =====================================
// プレイヤーの移動状態
// =====================================

// Rigidbodyコンポーネントが必須
[RequireComponent(typeof(Rigidbody))]

public class MoveState : StateClass
{
    // インスタンスを入れる変数
    private static MoveState instance;
    // Playerのリジッドボディ
    Rigidbody rb;
    // PlayerStateを入れる変数
    PlayerState playerState;
    // 移動度
    Vector3 moveForward;

    // インスタンスを取得する関数
    public static MoveState Instance
    {
        get
        {
            if(instance==null)
            {
                instance = new MoveState();
            }
            return instance;
        }
    }

    // 状態の変更処理
    public override void Change(GameObject player)
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            playerState.ChangPlayerState(StandingState.Instance);
        }
    }

    // 状態の開始処理
    public override void Enter(GameObject player)
    {
        Debug.LogError("MoveState : 開始");

        // Rigidbodyを探す
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("MoveState : Rigidbodyが見つかりません");
            return;
        }
        // PlayerStateを探す
        playerState = player.GetComponent<PlayerState>();
        if (playerState == null)
        {
            Debug.LogError("MoveState : PlayerStateが見つかりません");
            return;
        }
    }

    // 状態中の処理
    public override void Excute(GameObject player)
    {
        //移動度をリセットする
        moveForward = Vector3.zero;

        //入力を取得
        float inputX = Input.GetAxisRaw("Horizontal"); //横方向
        float inputY = Input.GetAxisRaw("Vertical"); //縦方向

        // カメラのベクトルから移動方向を決める
        moveForward = playerState.cameraTransform.forward * inputY + playerState.cameraTransform.right * inputX;
        moveForward = Vector3.Scale(moveForward, new Vector3(1, 0, 1)).normalized * playerState.speed;

        // 移動度に移動速度を掛けて力を加える
        rb.velocity = new Vector3(moveForward.x, rb.velocity.y, moveForward.z);

        //キャラクターを回転させる
        player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, playerState.cameraTransform.eulerAngles.y, player.transform.eulerAngles.z);
    }

    // 状態中の終了処理
    public override void Exit(GameObject player)
    {

    }
}
