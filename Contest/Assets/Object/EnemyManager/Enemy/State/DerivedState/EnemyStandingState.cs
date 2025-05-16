using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyStandingState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private static EnemyStandingState instance;
    // フレームを計る
    float freams = 0;
    int waitTime = 0;

    //視野角にプレイヤーがいるかを判断する変数
    private RaycastHit hit;
    //private GameObject target;

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
        //y軸回転
        Quaternion rotation1 = Quaternion.Euler(0f, enemyState.GetEnemyFov() / 2, 0f);
        Quaternion rotation2 = Quaternion.Euler(0f, -enemyState.GetEnemyFov() / 2, 0f);

        //目標（Player）に向けての方向ベクトルの単位ベクトル
        Vector3 targetDir =
            (enemyState.GetTargetObject().transform.position - enemyState.transform.position).normalized;

        //敵の正面ベクトルとプレイヤーに向けての単位ベクトルとの内積
        float angle = Vector3.Dot(enemyState.transform.forward, targetDir);

        //視野角の右端方向のベクトル
        Vector3 fovRightVector =
            (rotation1 * enemyState.transform.forward);

        //視野角の左端方向のベクトル
        Vector3 fovLeftVector =
            (rotation2 * enemyState.transform.forward);

        //視野角の右端、左端と正面ベクトルとの内積
        float limitFovAngle_right = Vector3.Dot(enemyState.transform.forward, fovRightVector);
        float limitFovAngle_left = Vector3.Dot(enemyState.transform.forward, fovLeftVector);

        //プレイヤーに向かってレイを飛ばす
        Debug.DrawRay(enemyState.transform.position,
            (enemyState.GetTargetObject().transform.position - enemyState.transform.position) * 2, Color.blue, 0.1f);

        //視野角を視覚化
        Debug.DrawRay(enemyState.transform.position, rotation1 * enemyState.transform.forward * 30f, Color.red, 0.1f);
        Debug.DrawRay(enemyState.transform.position, rotation2 * enemyState.transform.forward * 30f, Color.red, 0.1f);

        //視野角の内側にプレイヤーがいるか
        if (limitFovAngle_right <= angle && limitFovAngle_left <= angle)
        {
            //視認距離の内側にいるか
            if (Physics.Raycast(enemyState.transform.position,
                (enemyState.GetTargetObject().transform.position - enemyState.transform.position), out hit, enemyState.GetEnemyVisionLength()))
            {
                //当たったゲームオブジェクトがプレイヤーかつ、一定の距離より遠いなら追跡ステートに切り替え
                if (hit.collider.gameObject.name == "Player" && hit.distance > enemyState.GetEnemyAttackRange())
                {
                    //Debug.LogError(hit.collider.gameObject.name + "に当たった");
                    enemyState.SetFoundTargetFlg(true);
                    enemyState.ChangeState(Enemy_ChaseState.Instance);
                }
                //一定の距離以下なら攻撃ステートに
                else if (hit.collider.gameObject.name == "Player" && hit.distance <= enemyState.GetEnemyAttackRange())
                {
                    // 攻撃状態に移行する
                    if (freams > waitTime)
                    {
                        enemyState.ChangeState(EnemyAttackState.Instance);
                    }
                }

            }
        }

    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        // デバッグ用に攻撃状態に移行するフレームを決める
        waitTime = Random.Range(30, 120);

#if UNITY_EDITOR
        // Debug.LogError("EnemyStandingState : 開始");
#endif
    }



    // 状態中の処理
    public override void Excute(EnemyState enemyState)
    {
        // ダメージ処理
        enemyState.HandleDamage();

        // フレーム更新
        freams += enemyState.GetEnemySpeed();

        //プレイヤーの方を向かせる
        if(enemyState.GetFoundTargetFlg() == true)
        {
            enemyState.transform.LookAt(enemyState.GetTargetObject().transform.position);
        }
        

    }



    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {
        freams = 0.0f;
    }



}
