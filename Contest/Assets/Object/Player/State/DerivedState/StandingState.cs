using UnityEngine;

// =====================================
// プレイヤーの何もしていない状態
// =====================================

public class StandingState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static StandingState instance;



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
    public override void Change(PlayerState playerState)
    {
        // 走り移動状態に変更
        if((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)&&
            Input.GetButton("Dash"))
        {
            playerState.ChangeState(DashState.Instance);
        }
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0&&
            !Input.GetButton("Dash"))   
        {
            playerState.ChangeState(WalkState.Instance);
        }
        // カウンター状態に変更
        else if(Input.GetButtonDown("Counter"))
        {
            playerState.ChangeState(CounterStanceState.Instance);
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        Debug.LogError("StandingState : 開始");
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
