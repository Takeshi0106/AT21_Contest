using UnityEngine;

// =============================
// ジャンプ状態
// =============================

public class PlayerJumpState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerJumpState instance;


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
        }
    }



    // 状態中の処理
    public override void Excute(PlayerState currentState)
    {
        
    }



    // 状態中の終了処理
    public override void Exit(PlayerState currentState)
    {

    }
}
