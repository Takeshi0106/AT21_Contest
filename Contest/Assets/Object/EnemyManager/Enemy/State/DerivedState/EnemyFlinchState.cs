

public class EnemyFlinchState : StateClass<EnemyState>
{
    // インスタンスを取得する
    protected static EnemyFlinchState instance;
    // フレームを計測する
    protected float freams = 0.0f;


    // インスタンスを取得する関数
    public static EnemyFlinchState Instance
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
            enemyState.ChangeState(EnemyStandingState.Instance);
        }
    }

    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {

    }


    // 状態中の処理
    public override void Excute(EnemyState enemyState)
    {
        freams += enemyState.GetEnemySpeed();
    }



    // 状態中の終了処理
    public override void Exit(EnemyState enemyState)
    {

    }
}
