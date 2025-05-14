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
        // 走り移動状態に変更
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)&&
            Input.GetButton("Dash"))
        {
            playerState.ChangeState(PlayerDashState.Instance);
            return;
        }
        // 歩き移動状態に変更
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0&&
            !Input.GetButton("Dash"))   
        {
            playerState.ChangeState(PlayerWalkState.Instance);
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
        //空中に浮いていたら
        if ((Input.GetButtonDown("Jump") && !playerState.GetPlayerAirFlag()) ||
            playerState.GetPlayerAirFlag())
        {
            playerState.ChangeState(PlayerJumpState.Instance);
            return;
        }
        // 回避状態に移行
        if (Input.GetButtonDown("Avoidance"))
        {
            playerState.ChangeState(PlayerAvoidanceState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        // 立ち状態アニメーション開始
        if (playerState.GetPlayerAnimator() != null && playerState.GetPlayerStandingAnimation() != null)
        {
            playerState.GetPlayerAnimator().CrossFade(playerState.GetPlayerStandingAnimation().name, 0.1f);
        }

#if UNITY_EDITOR
        Debug.LogError("PlayerStandingState : 開始");
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        playerState.HandleDamage();
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {

    }



}
