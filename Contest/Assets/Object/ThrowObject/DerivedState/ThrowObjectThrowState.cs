using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ==================================
// 投げられている状態
// ==================================

public class ThrowObjectThrowState : StateClass<ThrowObjectState>
{
    // インスタンスを入れる変数
    private static ThrowObjectThrowState instance;

    // フレーム
    int freams = 0;


    // インスタンスを取得する関数
    public static ThrowObjectThrowState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ThrowObjectThrowState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(ThrowObjectState state)
    {

    }



    // 状態の開始処理
    public override void Enter(ThrowObjectState state)
    {
        state.GetThrowObjectRigidbody().AddForce(state.GetCameraTransform().forward * state.GetForcePower(),
    ForceMode.Impulse);
    }



    // 状態中の処理
    public override void Excute(ThrowObjectState state)
    {
        // 重力を無効にする
        if(state.GetNoGravityFreams() > freams) 
        {
            state.GetThrowObjectRigidbody().useGravity = false;
        }
        // 重力を有効にする
        else
        {
            state.GetThrowObjectRigidbody().useGravity = true;
        }

        freams++;
    }



    // 状態中の終了処理
    public override void Exit(ThrowObjectState state)
    {

    }



}
