using UnityEngine;

// ===============================
// プレイヤーの走り状態
// ===============================

public class DashState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static DashState instance;
    // 移動度
    Vector3 moveForward;

#if UNITY_EDITOR
    // エディタ実行時に実行される
    // 元の色を保存
    Color originalColor;
#endif

    // インスタンスを取得する関数
    public static DashState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DashState();
            }
            return instance;
        }
    }



    // 状態の変更処理
    public override void Change(PlayerState playerState)
    {
        // 歩き状態に変更
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 &&
            !Input.GetButton("Dash"))
        {
            playerState.ChangeState(WalkState.Instance);
        }
        // 移動キー入力がないとき、待機状態に変更
        else if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            playerState.ChangeState(StandingState.Instance);
        }
        // カウンター状態に変更
        else if (Input.GetButtonDown("Counter"))
        {
            playerState.ChangeState(CounterStanceState.Instance);
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        Debug.LogError("DashState : 開始");

#if UNITY_EDITOR
        // エディタ実行時に取得して色を変更する
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // 元の色を保存
            playerState.playerRenderer.material.color = Color.cyan;    // ダッシュ中の色
        }
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        //移動度をリセットする
        moveForward = Vector3.zero;

        //入力を取得
        float inputX = Input.GetAxisRaw("Horizontal"); //横方向
        float inputY = Input.GetAxisRaw("Vertical"); //縦方向

        // カメラのベクトルから移動方向を決める
        moveForward = playerState.GetCameraTransform().forward * inputY + playerState.GetCameraTransform().right * inputX;
        moveForward = Vector3.Scale(moveForward, new Vector3(1, 0, 1)).normalized * playerState.GetDashSpeed();

        // 移動度に移動速度を掛けて力を加える
        playerState.GetPlayerRigidbody().velocity = new Vector3(moveForward.x, playerState.GetPlayerRigidbody().velocity.y, moveForward.z);

        //キャラクターを回転させる
        playerState.transform.eulerAngles =
            new Vector3(playerState.transform.eulerAngles.x, playerState.GetCameraTransform().eulerAngles.y, playerState.transform.eulerAngles.z);
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
#if UNITY_EDITOR
        // エディタ実行時に色を元に戻す
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // 元の色に戻す
        }
#endif
    }



}
