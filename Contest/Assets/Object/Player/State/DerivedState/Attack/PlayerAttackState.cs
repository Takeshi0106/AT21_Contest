using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerAttackState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerAttackState instance;
    // weponData
    private static BaseAttackData weponData;
    // フレームを計る
    int freams = 0;

    // インスタンスを取得する関数
    public static PlayerAttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerAttackState();
            }
            return instance;
        }
    }



    // 状態の変更処理
    public override void Change(PlayerState playerState)
    {
        // 攻撃のフレームが過ぎたら
        if (freams == weponData.GetAttackStartupFrames(playerState.GetPlayerConbo()) +
            weponData.GetAttackSuccessFrames(playerState.GetPlayerConbo()))
        {
            // 後から硬直状態に移行する
            playerState.ChangeState(PlayerAttackRecoveryState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

#if UNITY_EDITOR
        Debug.LogError($"PlayerAttackState : 開始（Combo数：{playerState.GetPlayerConbo() + 1}）");

        if (weponData == null)
        {
            Debug.LogError("PlayerAttackState : WeponDataが見つかりません");
            return;
        }
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        freams++;

        // 攻撃判定をONにする
        if (freams >= weponData.GetAttackStartupFrames(playerState.GetPlayerConbo()))
        {
            playerState.GetPlayerWeponManager().EnableAllWeaponAttacks();
        }
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        // 攻撃判定をOFF
        playerState.GetPlayerWeponManager().DisableAllWeaponAttacks();
        // 初期化
        freams = 0;
    }



}
