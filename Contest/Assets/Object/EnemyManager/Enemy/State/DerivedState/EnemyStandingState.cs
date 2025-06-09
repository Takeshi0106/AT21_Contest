using UnityEngine;

public class EnemyStandingState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private EnemyStandingState instance;
    // フレームを計る
    float freams = 0;

    //視野角にプレイヤーがいるかを判断する変数
    private RaycastHit hit;
    //private GameObject target;

    // インスタンスを取得する関数
    public EnemyStandingState Instance
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
        Vector3 vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;

        // 移動状態に移行する
        if (vec.magnitude > 8.0f || enemyState.GetEnemyAttackFlag())
        {
            enemyState.ChangeState(new EnemyMoveState());
        }
        
        /*
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
        //攻撃範囲を視覚化
        Debug.DrawRay(enemyState.transform.position, enemyState.transform.forward * enemyState.GetEnemyAttackRange(), Color.green, 0.1f);

        //攻撃範囲の内側にいるか
        if (Physics.Raycast(enemyState.transform.position,
            (enemyState.GetTargetObject().transform.position - enemyState.transform.position), out hit,
            enemyState.GetEnemyAttackRange()))
        {
            //プレイヤーなら攻撃ステートに
            if (hit.collider.gameObject.name == "Player")
            {
                // 攻撃状態に移行する
                if (freams > waitTime)
                {
                    enemyState.ChangeState(EnemyAttackState.Instance);
                }
            }

        }
        //外側なら追跡ステートに
        else
        {
            enemyState.SetFoundTargetFlg(true);
            enemyState.ChangeState(Enemy_ChaseState.Instance);
        }
        */
    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        // 立ち状態アニメーション開始
        if (enemyState.GetEnemyAnimator() != null && enemyState.GetEnemyStandingAnimation() != null)
        {
            enemyState.GetEnemyAnimator().CrossFade(enemyState.GetEnemyStandingAnimation().name, 0.1f);
        }

        // デバッグ用に攻撃状態に移行するフレームを決める
        // waitTime = Random.Range(30, 120);

#if UNITY_EDITOR
        Debug.LogError("EnemyStandingState : 開始");
#endif
    }



    // 状態中の処理
    public override void Excute(EnemyState enemyState)
    {
        // ダメージ処理
        enemyState.HandleDamage();

        enemyState.Target();

        // フレーム更新
        freams += enemyState.GetEnemySpeed();

        //プレイヤーの方を向かせる

        //スムーズにプレイヤーの方を向かせる処理
        Quaternion targetRotation = Quaternion.LookRotation(
            enemyState.GetTargetObject().transform.position - enemyState.transform.position);

        float angle = Quaternion.Angle(enemyState.transform.rotation, targetRotation);

        float speed = angle / 5f;

        enemyState.transform.rotation = Quaternion.Slerp(
            enemyState.transform.rotation, targetRotation, Time.deltaTime * speed);






    }



    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {
        freams = 0.0f;
    }



}
