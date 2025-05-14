

public class EnemyDeadState : StateClass<EnemyState>
{
    // インスタンスを入れる変数
    private static EnemyDeadState instance;

    // インスタンスを取得する関数
    public static EnemyDeadState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyDeadState();
            }
            return instance;
        }
    }



    // 状態の変更処理
    public override void Change(EnemyState enemyState)
    {

    }



    // 状態の開始処理
    public override void Enter(EnemyState enemyState)
    {
        enemyState.GetEnemyWeponManager().RemoveAllWeapon(); // 武器データをすべて削除

        // 死亡アニメーション開始
        if (enemyState.GetEnemyAnimator() != null && enemyState.GetEnemyDeadAnimation() != null)
        {
            enemyState.GetEnemyAnimator().CrossFade(enemyState.GetEnemyDeadAnimation().name, 0.1f);
        }
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
