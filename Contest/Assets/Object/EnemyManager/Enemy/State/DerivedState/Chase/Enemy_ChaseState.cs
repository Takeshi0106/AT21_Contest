using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy_ChaseState : StateClass<EnemyState>
{
    //インスタンス
    private static Enemy_ChaseState instance;

   //プレイヤーが当たっているかを確認する変数
    private RaycastHit hit;
    //private GameObject target;

    //エネミーごとに持っているスクリプトを識別して格納する変数
    private Dictionary<string, ChaseScript> pair_enemyChase = new Dictionary<string, ChaseScript>();

    //インスタンスを返す関数
    public static Enemy_ChaseState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Enemy_ChaseState();
            }

            return instance;
        }
        
    }

    //状態の変更処理
    public override void Change(EnemyState enemyState)
    {
        //確認用レイ
        Debug.DrawRay(enemyState.transform.position,
                (enemyState.GetTargetObject().transform.position - enemyState.transform.position), Color.green, 0.1f);
        //一定の距離にプレイヤーが来たら
        if (Physics.Raycast(enemyState.transform.position, 
            (enemyState.GetTargetObject().transform.position - enemyState.transform.position), out hit, enemyState.GetEnemyAttackRange()))
        {
            if (hit.collider.gameObject.name == "Player")
            {
                //直立ステートに戻す
                enemyState.ChangeState(EnemyStandingState.Instance);
            }
        }
    }

    //状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        //dictionary内にキーが格納されていないなら追加
        if(!pair_enemyChase.ContainsKey(enemyState.name))
        {
            pair_enemyChase.Add(enemyState.name, enemyState.GetComponent<ChaseScript>());
            //pair_enemyChase.Add(enemyState.name, enemyState.GetComponent<ChaseScript>());
            Debug.LogError(enemyState.name + "を追加");

            //ターゲットオブジェクトを渡す
            pair_enemyChase[enemyState.name].SetTarget(enemyState.GetTargetObject());

            //停止する距離を渡す
            pair_enemyChase[enemyState.name].SetStoppingDistance(enemyState.GetEnemyAttackRange());

            //移動速度の値を渡す
            pair_enemyChase[(enemyState.name)].SetMoveSpeed(enemyState.GetEnemyMoveSpeed());
        }

       

        Debug.LogError(enemyState.name+":chaseScriptに移行");

        
    }
    
    //状態中の処理
    public override void Excute(EnemyState enemyState)
    {
        //現在のenemyStateが存在するなら
        if (pair_enemyChase.ContainsKey(enemyState.name))
        {
            //追跡スクリプトの関数呼び出し
            pair_enemyChase[enemyState.name].TargetChase();
            //Debug.LogError(enemyState.name + "が動かしている");

//#if UNITY_EDITOR
//            // Rendererが存在するなら赤色に変更
//            var renderer = enemyState.GetComponentInChildren<Renderer>();
//            if (renderer != null && renderer.material != null)
//            {
//                renderer.material.color = Color.red;
//            }
//#endif


        }
        else
        {
            Debug.LogWarning("呼びこんでいるenemyStateが見つかりませんでした");
        }

    }

    //状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {
        
    }
}
