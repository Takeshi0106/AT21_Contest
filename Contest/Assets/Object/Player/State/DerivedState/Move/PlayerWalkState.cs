using UnityEngine;

// =====================================
// プレイヤーの歩き移動状態
// =====================================

public class PlayerWalkState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerWalkState instance;
    // 移動度
    Vector3 moveForward;



    // インスタンスを取得する関数
    public static PlayerWalkState Instance
    {
        get
        {
            if(instance==null)
            {
                instance = new PlayerWalkState();
            }
            return instance;
        }
    }



    // 状態の変更処理
    public override void Change(PlayerState playerState)
    {
        // カウンター状態に変更
        if (Input.GetButtonDown("Counter"))
        {
            playerState.ChangeState(PlayerCounterStanceState.Instance);
            return;
        }
        // 攻撃状態に変更
        if (Input.GetButtonDown("Attack"))
        {
            playerState.ChangeState(PlayerAttackState.Instance);
            return;
        }
        // 移動キー入力がないとき、待機状態に変更
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            playerState.ChangeState(PlayerStandingState.Instance);
            return;
        }
        // Dash ボタンを押した場合に Dash 状態へ変更
        if (Input.GetButtonDown("Dash"))
        {
            playerState.ChangeState(PlayerDashState.Instance);
            return;
        }
        // 武器を投げる状態に移行
        if (Input.GetButtonDown("Throw"))
        {
            if (playerState.GetPlayerWeponManager().GetWeaponCount() <= 1)
            {
                // 武器を投げるの失敗状態に移行
                playerState.ChangeState(PlayerThrowFailedState.Instance);
            }
            else
            {
                // 武器を投げる状態に移行
                playerState.ChangeState(PlayerWeaponThrowState.Instance);
            }
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
#if UNITY_EDITOR
        Debug.LogError("WalkState : 開始");
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        playerState.HandleDamage();

        //移動度をリセットする
        moveForward = Vector3.zero;

        //入力を取得
        float inputX = Input.GetAxisRaw("Horizontal"); //横方向
        float inputY = Input.GetAxisRaw("Vertical"); //縦方向

        // カメラのベクトルから移動方向を決める
        moveForward = playerState.GetCameraTransform().forward * inputY + playerState.GetCameraTransform().right * inputX;
        moveForward = Vector3.Scale(moveForward, new Vector3(1, 0, 1)).normalized * playerState.GetWalkSpeed();

        // 移動度に移動速度を掛けて力を加える
        playerState.GetPlayerRigidbody().velocity = new Vector3(moveForward.x, playerState.GetPlayerRigidbody().velocity.y, moveForward.z);

        // 移動方向に向ける
        if (moveForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveForward);
            playerState.transform.rotation = Quaternion.Slerp(playerState.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {

    }



}
