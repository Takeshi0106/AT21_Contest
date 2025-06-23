using System.Collections.Generic;
using UnityEngine;

// ================================
// プレイヤーのカウンター攻撃状態
// ================================

public class PlayerCounterStrikeState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerCounterStrikeState instance;
    // フレームを計る
    float freams = 0.0f;



    // インスタンスを取得する
    public static PlayerCounterStrikeState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerCounterStrikeState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(PlayerState playerState)
    {
        // カウンターの有効フレームが過ぎたら
        if (freams >= playerState.GetPlayerCounterManager().GetCounterSuccessFrames())
        {
            playerState.ChangeState(PlayerStandingState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        float attackData = playerState.GetPlayerCounterManager().GetCounterDamage();   // カウンターの攻撃力
        float stanAttackData = playerState.GetPlayerCounterManager().GetCounterStanDamage(); // スタン力を取得 
        float multiData = playerState.GetPlayerCounterManager().GetDamageMultiplier(); // カウンターマネージャーを取得

        // ゲージ量アップ
        playerState.GetPlayerCounterManager().IncreaseGauge();

        // Sphere を初期サイズにし、アクティブ化
        // playerState.GetPlayerCounterObject().transform.localScale = Vector3.zero;
        // playerState.GetPlayerCounterObject().SetActive(true);

        // 攻撃タグを付ける
        // playerState.GetPlayerCounterAttackController().EnableAttack();

        // 攻撃情報を更新する
        playerState.GetAttackInterface().SetSelfAttackDamage(attackData* multiData);
        // スタン情報を更新する
        playerState.GetAttackInterface().SetSelfStanAttackDamage(stanAttackData);
        // ID
        playerState.GetAttackInterface().SetSelfID();
        // カウンター開始処理
        playerState.GetPlayerCounterObjectManager().Activate();


#if UNITY_EDITOR
        Debug.Log("CounterStrikeState : 開始");

        Debug.LogError($"CounterStrikeState : {playerState.GetAttackInterface().GetOtherAttackID()}");

        // 赤色に変更する
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.red;
        }
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        // ダメージ処理
        // playerState.CleanupInvalidDamageColliders();

        // カウンター中の Sphere 拡大処理
        // var sphere = playerState.GetPlayerCounterObject();
        /*
        if (sphere != null)
        {
            float maxSize = playerState.GetPlayerCounterRange(); // 拡大の最大サイズ
            float scale = Mathf.Lerp(0f, maxSize, (float)freams / playerState.GetPlayerCounterManager().GetCounterSuccessFrames());
            sphere.transform.localScale = new Vector3(scale, scale, scale);
        }
        */

        playerState.GetPlayerCounterObjectManager().UpdateScale(freams / (float)playerState.GetPlayerCounterManager().GetCounterSuccessFrames(),
            playerState.GetPlayerCounterObjectManager().GetCounterMaxSize());

        // フレーム更新
        freams += playerState.GetPlayerSpeed();
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        // 初期化
        freams = 0.0f;

        // 攻撃タグを戻す
        // playerState.GetPlayerCounterAttackController().DisableAttack();

        playerState.GetPlayerCounterObjectManager().Deactivate();

        // Sphere を非アクティブに戻す
        //var sphere = playerState.GetPlayerCounterObject();
        /*
        if (sphere != null)
        {
            sphere.SetActive(false);
        }
        */

        // 無敵を有効にする
        playerState.GetPlayerStatusEffectManager().StartInvicible(playerState.GetPlayerCounterManager().GetCounterInvincibleFreams());

#if UNITY_EDITOR
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.white;
        }
#endif
    }



}
