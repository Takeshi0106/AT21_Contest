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
    private GameObject target;

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
                (target.transform.position - enemyState.transform.position), Color.green, 0.1f);
        //一定の距離にプレイヤーが来たら
        if (Physics.Raycast(enemyState.transform.position, 
            (target.transform.position - enemyState.transform.position), out hit, enemyState.GetEnemyAttackRange()))
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
            pair_enemyChase.Add(enemyState.name,enemyState.GetComponent<ChaseScript>());
            Debug.LogError(enemyState.name + "を追加");
        }

        //プレイヤーを探して取得する
        target = GameObject.Find("Player");

        if (target == null)
        {
            Debug.LogWarning("指定のオブジェクトが見つかりませんでした");
        }
        //Debug.LogError(enemyState.name+":chaseScriptに移行");

        //停止する距離を渡す
        pair_enemyChase[enemyState.name].SetStoppingDistance(enemyState.GetEnemyAttackRange());
    }
    
    //状態中の処理
    public override void Excute(EnemyState enemyState)
    {
        if (pair_enemyChase.ContainsKey(enemyState.name))
        {
            pair_enemyChase[enemyState.name].TargetChase();
            //Debug.LogError(enemyState.name + "が動かしている");
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
