using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// =================================
// 状態クラス
// =================================

// Stateの基底クラス
public abstract class StateClass<T> where T : BaseState<T>
{
    // 状態を変更する
    public abstract void Change(T state);
    // 状態の開始処理
    public abstract void Enter(T currentState);
    // 状態中の処理
    public abstract void Excute(T currentState);
    // 状態中の終了処理
    public abstract void Exit(T currentState);
}
