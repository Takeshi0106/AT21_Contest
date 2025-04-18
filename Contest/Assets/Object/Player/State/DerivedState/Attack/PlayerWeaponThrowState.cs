using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponThrowState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerWeaponThrowState instance;

    // エディタ実行時に実行される
    // 元の色を保存
    Color originalColor;

    // 状態変更までの時間
    int changTime = 60;
    int freams = 0;

#if UNITY_EDITOR

#endif



    // インスタンスを取得する
    public static PlayerWeaponThrowState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerWeaponThrowState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(PlayerState playerState)
    {
        // 立ち状態に戻す
        if (freams > changTime)
        {
            playerState.ChangeState(PlayerCounterStaggerState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        if (playerState.GetPlayerWeponManager().GetWeaponCount() > 1)
        {
            // 武器を削除
            playerState.GetPlayerWeponManager().RemoveWeapon(playerState.GetPlayerWeponNumber());
            // 状態移行時間を変更させる
            changTime = 20;
        }
        else
        {
            // 武器が１つしかないときの処理を書く（アニメーションなど）
        }

        // エディタ実行時に取得して色を変更する
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // 元の色を保存
            playerState.playerRenderer.material.color = Color.blue;    // カウンター構え中の色
        }

#if UNITY_EDITOR
        Debug.LogError("PlayerWeaponThrowState : 開始");

        
#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        freams++;
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        freams = 0;
        changTime = 60;

        // エディタ実行時に色を元に戻す
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // 元の色に戻す
        }

#if UNITY_EDITOR

#endif
    }



}
