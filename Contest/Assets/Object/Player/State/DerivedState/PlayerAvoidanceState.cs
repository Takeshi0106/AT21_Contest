using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

// =============================
// 回避状態
// =============================

public class PlayerAvoidanceState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerAvoidanceState instance;
    // 回避の準備フレーム
    private int startUpFreams = 0;
    // 回避中のフレーム
    private int avoidanceFreams = 0;
    // 回避後のフレーム
    private int affterFreams = 0;

    private float freams = 0.0f;
    private bool avoidanceFlag = false;

    // インスタンスを取得する関数
    public static PlayerAvoidanceState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerAvoidanceState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(PlayerState currentstate)
    {
        // 回避フレームが終わったら
        if ((startUpFreams + avoidanceFreams + affterFreams) < freams)
        {
            currentstate.ChangeState(PlayerStandingState.Instance);
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState currentState)
    {
        // 代入する
        startUpFreams = currentState.GetPlayerAvoidanceManager().GetAvoidanceStartUpFreams();
        avoidanceFreams = currentState.GetPlayerAvoidanceManager().GetAvoidanceFreams();
        affterFreams = currentState.GetPlayerAvoidanceManager().GetAvoidanceAfterFreams();

#if UNITY_EDITOR
        Debug.Log("PlayerAvoidanceState 開始");
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState currentState)
    {
        // 回避有効中かチェック
        if ((startUpFreams < freams) &&
            (startUpFreams + avoidanceFreams > freams))
        {
            avoidanceFlag = true;
        }
        else
        {
            avoidanceFlag = false;
        }


        if (avoidanceFlag)
        {
            // 攻撃タグが戻っているかをチェック
            currentState.CleanupInvalidDamageColliders();

            // 当たっているオブジェクトを調べる
            foreach (var collidedInfo in currentState.GetPlayerCollidedInfos())
            {
                if (collidedInfo.collider != null)
                {
                    // コライダーがすでにダメージ処理をしていたら次のオブジェクトを調べる
                    if (currentState.GetPlayerDamagedColliders().Contains(collidedInfo.collider)) { continue; }

                    // タグを取得する
                    MultiTag tag = collidedInfo.multiTag;

                    if (tag != null && tag.HasTag(currentState.GetPlayerCounterPossibleAttack()))
                    {
                        // 無敵を有効にする
                        currentState.GetPlayerStatusEffectManager().
                            StartInvicible(currentState.GetPlayerAvoidanceManager().GetAvoidanceInvincibleFreams());
                        // コライダーを保存する
                        currentState.AddDamagedCollider(collidedInfo.collider);

                        // 回避成功処理を呼び出す
                        currentState.GetPlayerAvoidanceManager().AvoidanceStart();

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
                            Debug.Log("回避成功！相手の親: " + parentTransform.gameObject.name);
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
            currentState.HandleDamage();
        }

        // フレーム更新
        freams += currentState.GetPlayerSpeed();
    }



    // 状態中の終了処理
    public override void Exit(PlayerState currentState)
    {
        freams = 0.0f;
    }



}
