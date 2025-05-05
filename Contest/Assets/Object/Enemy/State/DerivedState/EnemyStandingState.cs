using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStandingState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private static EnemyStandingState instance;

    //当たったものを保存する関数
    private RaycastHit hit;

    //ゲームオブジェクト
    private GameObject target;
    private float visionLength; 

    // インスタンスを取得する関数
    public static EnemyStandingState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyStandingState();
            }
            return instance;
        }
    }



    // 状態の変更処理
    public override void Change(EnemyState enemyState)
    {
        //一定の距離以下かつ視界内に入ったら
        {
            //y軸回転
            Quaternion rotation1 = Quaternion.Euler(0f, 45f, 0f);
            Quaternion rotation2 = Quaternion.Euler(0f, -45f, 0f);

            //目標（Player）に向けての方向ベクトルの単位ベクトル
            Vector3 targetDir =
                (target.transform.position - enemyState.transform.position).normalized;

            //敵の正面とプレイヤーに向けての単位ベクトルとの内積
            float angle = Vector3.Dot(enemyState.transform.forward, targetDir);

            //視野角の閾値
            float threshold = Mathf.Cos(90f / 2 * Mathf.Deg2Rad);

            //プレイヤーに向かってレイを飛ばす
            Debug.DrawRay(enemyState.transform.position,
                (target.transform.position - enemyState.transform.position) * 5f, Color.red, 0.1f);

            Debug.DrawRay(enemyState.transform.position, rotation1 * enemyState.transform.forward * 30f, Color.red, 0.1f);
            Debug.DrawRay(enemyState.transform.position, rotation2 * enemyState.transform.forward * 30f, Color.red, 0.1f);

            //視野の内側に入っているか
            if (angle > threshold)
            {
                //Debug.Log("視野内にいる");

                if (Physics.Raycast(enemyState.transform.position,
                    (target.transform.position - enemyState.transform.position), out hit, visionLength))
                {
                    //当たったゲームオブジェクトがプレイヤーなら追跡ステートに切り替え
                    if (hit.collider.gameObject.name == "Player")
                    {
                        Debug.LogError(hit.collider.gameObject.name + "に当たった");
                        enemyState.ChangeState(EnemyChaseState.Instance);
                    }

                }
            }
        }

       

    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
#if UNITY_EDITOR
        Debug.LogError("EnemyStandingState : 開始");
#endif

        target = GameObject.Find("Player");
        if(target != null)
        {
            Debug.LogError(target.name + "を発見");
        }

        //視野の距離の最大値
        visionLength = 30f;
    }



    // 状態中の処理
    public override void Excute(EnemyState enemyState)
    {

    }



    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {

    }



}
