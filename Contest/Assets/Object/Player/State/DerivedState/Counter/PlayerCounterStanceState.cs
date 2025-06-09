using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

// ================================
// プレイヤーのカウンター構え状態
// ================================

public class PlayerCounterStanceState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerCounterStanceState instance;
    // フレームを計る
    float freams = 0.0f;
    // カウンターの成否
    bool counterOutcome = false;
    // カウンターが有効かどうか
    bool counterActive = false;



    // インスタンスを取得する
    public static PlayerCounterStanceState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerCounterStanceState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(PlayerState playerState)
    {
        // カウンターの有効フレームが過ぎたら
        if (freams >= playerState.GetPlayerCounterManager().GetCounterFrames() + 
            playerState.GetPlayerCounterManager().GetCounterStartupFrames())
        {
            playerState.ChangeState(PlayerCounterStaggerState.Instance);
            return;
        }
        // カウンターが成功したら
        if(counterOutcome)
        {
            playerState.ChangeState(PlayerCounterStrikeState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        // アニメーション取得
        var animClip = playerState.GetPlayerCounterManager().GetCounterAnimation();
        var Anim = playerState.GetPlayerAnimator();

        // アニメーション再生
        if (Anim != null && animClip != null)
        {
            Anim.CrossFade(animClip.name, 0.1f);
        }

        // 武器を非表示
        playerState.GetPlayerWeponManager().WeaponInvisible(playerState.GetPlayerWeponNumber());

#if UNITY_EDITOR
        Debug.LogError("CounterStanceState : 開始");

        // カウンター構え時緑色にする
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.green;
        }
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        // **カウンター準備時間の処理**
        if (freams > playerState.GetPlayerCounterManager().GetCounterStartupFrames())
        {
            // 準備完了後にカウンター有効化
            counterActive = true;

#if UNITY_EDITOR
            playerState.playerRenderer.material.color = Color.yellow;
#endif
        }

        if (counterActive)
        {
            // 攻撃タグが戻っているかをチェック
            playerState.CleanupInvalidDamageColliders();

            // 当たっているオブジェクトを調べる
            foreach (var collidedInfo in playerState.GetPlayerCollidedInfos())
            {
                if (collidedInfo.collider != null)
                {
                    // コライダーがすでにダメージ処理をしていたら次のオブジェクトを調べる
                    if (playerState.GetPlayerDamagedColliders().Contains(collidedInfo.collider)) { continue; }

                    // タグを取得する
                    MultiTag tag = collidedInfo.multiTag;

                    if (tag != null && tag.HasTag(playerState.GetPlayerCounterPossibleAttack()))
                    {
                        // カウンター成功
                        counterOutcome = true;
                        // コライダーを保存する
                        playerState.AddDamagedCollider(collidedInfo.collider);


#if UNITY_EDITOR
                        // 親オブジェクトの名前を取得する
                        Transform parentTransform = collidedInfo.collider.transform.parent;
                        // 一番上の親オブジェクトを取得
                        while (parentTransform.parent != null)
                        {
                            parentTransform = parentTransform.parent;
                        }

                        // ログ表示
                        if (parentTransform != null)
                        {
                            Debug.Log("カウンター成功！相手の親: " + parentTransform.gameObject.name);
                        }
                        Debug.Log("攻撃オブジェクト名: " + collidedInfo.collider.gameObject.name);
#endif

                        // カウンター成功時に処理を終了する
                        return;
                    }
                }
            }
        }
        else
        {
            // ダメージ処理を有効にする
            playerState.HandleDamage();
        }

        // フレーム数を計る
        freams += playerState.GetPlayerSpeed();
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        // 初期化
        freams = 0.0f;
        counterOutcome = false;
        counterActive = false;

        playerState.GetPlayerWeponManager().WeaponVisible(playerState.GetPlayerWeponNumber());

#if UNITY_EDITOR
        // 元の色に戻す
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.white;
        }
#endif
    }



}
