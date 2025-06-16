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
        AnimationClip clip = currentState.GetPlayerAvoidanceManager().GetAvoidanceAnimation();
        AvoidanceManager avoidanceManager = currentState.GetPlayerAvoidanceManager();

        // アニメーション開始
        if (currentState.GetPlayerAnimator() != null && clip != null)
        {
            currentState.GetPlayerAnimator().CrossFade(clip.name, 0.1f);
        }

        // 武器を見えないようにする
        currentState.GetPlayerWeponManager().WeaponInvisible(currentState.GetPlayerWeponNumber());

        // 後ろの下がる
        currentState.GetPlayerRigidbody().AddForce(-currentState.transform.forward * avoidanceManager.GetAvoidancePower(), ForceMode.Impulse);

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
            var counterTag = currentState.GetPlayerCounterPossibleAttack();
            var collidedInfos = currentState.GetPlayerCollidedInfos();

            // 当たっているオブジェクトを調べる
            for (int i = 0; i < collidedInfos.Count; i++)
            {
                var collidedInfo = collidedInfos[i];
                var collider = collidedInfo.collider;
                var tag = collidedInfo.multiTag;

                if (collider == null) continue;

                // すでにダメージ処理されたものはスキップ
                if (collidedInfo.hitFlag) continue;

                if (tag != null && tag.HasTag(counterTag))
                {
                    // 無敵を有効にする
                    currentState.GetPlayerStatusEffectManager().
                        StartInvicible(currentState.GetPlayerAvoidanceManager().GetAvoidanceInvincibleFreams());

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
        // 武器を見えるようにする
        currentState.GetPlayerWeponManager().WeaponVisible(currentState.GetPlayerWeponNumber());

        freams = 0.0f;
    }



}
