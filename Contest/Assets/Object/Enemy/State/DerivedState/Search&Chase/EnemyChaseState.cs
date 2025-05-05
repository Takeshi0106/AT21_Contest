using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class EnemyChaseState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private static EnemyChaseState instance;

    //ゲームオブジェクト
    private GameObject enemy;

    private GameObject target;

    private ChaseScript chaseScript;

    //Ray関連
    RaycastHit hit;

    //レイの長さ
    float rayLength;

    // インスタンスを取得する関数
    public static EnemyChaseState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyChaseState();
            }
            return instance;
        }
    }

    // 状態の変更処理
    public override void Change(EnemyState enemyState)
    {
        //プレイヤーと敵との距離が一定の値以下になったら

        //プレイヤーに向かってレイを飛ばす
        Debug.DrawRay(enemyState.transform.position,
            (target.transform.position - enemyState.transform.position), Color.red, 0.1f);

        //プレイヤーにレイが当たったらステートを変更
        if(Physics.Raycast(enemyState.transform.position,
            (target.transform.position - enemyState.transform.position),out hit,rayLength))
        {
            //Debug.Log(hit.distance);

            if (hit.collider.gameObject.name == "Player")
            {
                //攻撃ステートに
                enemyState.ChangeState(EnemySwordAttackState.Instance);
            }
        }


    }

    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
#if UNITY_EDITOR
        Debug.LogError("EnemyChase : 開始");
#endif
        //敵内のスクリプトにアクセスするために敵を見つける
        enemy = GameObject.Find("Enemy");
        if (enemy != null)
        {
            chaseScript = enemy.GetComponent<ChaseScript>();
            if (chaseScript != null)
            {
                Debug.Log("追跡開始");
               
            }
        }

        //追いかける対象（プレイヤー）を見つける
        target = GameObject.Find("Player");
        if (target != null)
        {
            Debug.LogError(target.name + "を発見");
        }

        rayLength = 3f;
    }



    // 状態中の処理
    public override void Excute(EnemyState enemyState)
    {
        if (enemy != null)
        {
            //chaseScript = Enemy.GetComponent<ChaseScript>();
            if (chaseScript != null)
            {
                //Debug.Log("追跡中");
                chaseScript.TargetChase();
            }
        }


    }

    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {

    }


}
