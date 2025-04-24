using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===================================================
// プレイヤーと敵の怯み状態テンプレートクラス
// ===================================================

public class BaseFlinchState<T> : StateClass<T> where T : BaseState<T>
{
    // インスタンスを取得する
    protected static BaseFlinchState<T> instance;
    // フレームを計測する
    protected int freams = 0;



    // インスタンスを取得する関数
    public static BaseFlinchState<T> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BaseFlinchState<T>();
            }
            return instance;
        }
    }



    // 状態を変更する
    public  override void Change(T currentState)
    {
        // 派生クラスにオーバーライドさせる
    }



    // 状態の開始処理
    public override void Enter(T currentState)
    {

    }

    // 状態中の処理
    public override void Excute(T currentState)
    {
        freams++;
    }

    // 状態中の終了処理
    public override void Exit(T currentState)
    {

    }



}
