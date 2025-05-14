using UnityEngine;
using UnityEngine.Playables;

// =============================
// ジャンプ状態
// =============================

public class PlayerJumpState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerJumpState instance;
    // 移動度
    Vector3 moveForward;



    // インスタンスを取得する関数
    public static PlayerJumpState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerJumpState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(PlayerState currentstate)
    {
        if(!currentstate.GetPlayerAirFlag())
        {
            currentstate.ChangeState(PlayerStandingState.Instance);
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState currentState)
    {
        if (!currentState.GetPlayerAirFlag())
        {
#if UNITY_EDITOR
            Debug.Log("ジャンプ");
#endif

            // ジャンプ処理
            currentState.GetPlayerRigidbody().AddForce(new Vector3(0.0f, currentState.GetPlayerJumpPower(), 0.0f), ForceMode.Impulse);

            if (currentState.GetPlayerAnimator() != null && currentState.GetPlayerJumpAnimation() != null)
            {
                currentState.GetPlayerAnimator().CrossFade(currentState.GetPlayerJumpAnimation().name, 0.1f);
            }

            currentState.SetJumpFlag(true);
        }
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
    public override void Exit(PlayerState currentState)
    {

    }
}
