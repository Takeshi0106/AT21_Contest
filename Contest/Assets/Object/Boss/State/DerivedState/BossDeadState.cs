

public class BossDeadState : StateClass<BossState>
{

    // 状態の変更処理
    public override void Change(BossState bossState)
    {

    }



    // 状態の開始処理
    public override void Enter(BossState bossState)
    {
        bossState.GetEnemyWeponManager().DisableAllWeaponAttacks(); // 攻撃タグを元に戻す
        bossState.GetEnemyWeponManager().RemoveAllWeapon(); // 武器データをすべて削除

        // 死亡アニメーション開始
        if (bossState.GetEnemyAnimator() != null && bossState.GetEnemyDeadAnimation() != null)
        {
            bossState.GetEnemyAnimator().CrossFade(bossState.GetEnemyDeadAnimation().name, 0.1f);
        }
    }



    // 状態中の処理
    public override void Excute(BossState bossState)
    {

    }



    // 状態中の終了処理
    public override void Exit(BossState bossState)
    {

    }
}
