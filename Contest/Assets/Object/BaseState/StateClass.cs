using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stateの基底クラス
public abstract class StateClass
{
    // 状態を変更する
    public abstract void Change(GameObject player);
    // 状態の開始処理
    public abstract void Enter(GameObject player);
    // 状態中の処理
    public abstract void Excute(GameObject player);
    // 状態中の終了処理
    public abstract void Exit(GameObject player);
}
