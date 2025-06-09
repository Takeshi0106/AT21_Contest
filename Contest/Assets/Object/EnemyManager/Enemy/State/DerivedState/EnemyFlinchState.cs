using UnityEngine;

public class EnemyFlinchState : StateClass<EnemyState>
{
    // インスタンスを取得する
    protected EnemyFlinchState instance;
    // フレームを計測する
    protected float freams = 0.0f;


    // インスタンスを取得する関数
    public EnemyFlinchState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyFlinchState();
            }
            return instance;
        }
    }


    // 状態を変更する
    public override void Change(EnemyState enemyState)
    {
        if (freams > enemyState.GetEnemyFlinchFreams())
        {
            enemyState.ChangeState(new EnemyStandingState());
        }
    }

    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        // アニメーション再生
        // Animator を取得
        var anim = enemyState.GetEnemyAnimator();
        var animClip = enemyState.GetEnemyFlinchAnimation();

        if (anim != null && animClip != null)
        {
            anim.CrossFade(animClip.name, 0.1f);
        }

        enemyState.SetEnemyFlinchCnt(enemyState.GetEnemyFlinchCnt() + 1);
    }


    // 状態中の処理
    public override void Excute(EnemyState enemyState)
    {
        freams += enemyState.GetEnemySpeed();
    }



    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {
        enemyState.GetEnemyDamageResponseManager().RecoverFlinch();
        freams = 0;
    }
}