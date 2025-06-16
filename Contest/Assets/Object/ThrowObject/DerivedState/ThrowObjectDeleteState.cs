using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ==================================================
// 投げたオブジェクトが当たった状態
// ==================================================

public class ThrowObjectDeleteState : StateClass<ThrowObjectState>
{

    // 状態を変更する
    public override void Change(ThrowObjectState state)
    {

    }



    // 状態の開始処理
    public override void Enter(ThrowObjectState state)
    {
        // 自分を削除する
        state.ThrowObjectDelete();
    }



    // 状態中の処理
    public override void Excute(ThrowObjectState state)
    {

    }



    // 状態中の終了処理
    public override void Exit(ThrowObjectState state)
    {

    }



}
