using UnityEngine;

// =====================================
// プレイヤーの何もしていない状態
// =====================================

public class PlayerStandingState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerStandingState instance;



    // インスタンスを取得する関数
    public static PlayerStandingState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerStandingState();
            }
            return instance;
        }
    }



    // 状態の変更処理
    public override void Change(PlayerState playerState)
    {
        // 走り移動状態に変更
        if((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)&&
            Input.GetButton("Dash"))
        {
            playerState.ChangeState(PlayerDashState.Instance);
        }
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0&&
            !Input.GetButton("Dash"))   
        {
            playerState.ChangeState(PlayerWalkState.Instance);
        }
        // カウンター状態に変更
        else if(Input.GetButtonDown("Counter"))
        {
            playerState.ChangeState(PlayerCounterStanceState.Instance);
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
#if UNITY_EDITOR
        Debug.LogError("PlayerStandingState : 開始");
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
    
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {

    }



}
