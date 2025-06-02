using UnityEngine;

// ======================
// 敵の攻撃状態
// ======================

public class BossAttackState : StateClass<BossState>
{
    // weponData
    private static BaseAttackData weponData;
    // フレームを計る
    float freams = 0.0f;


    // 状態の変更処理
    public override void Change(BossState bossState)
    {
        // 攻撃のフレームが過ぎたら
        if (freams >= (weponData.GetAttackStartupFrames(bossState.GetEnemyConbo()) +
    weponData.GetAttackSuccessFrames(bossState.GetEnemyConbo())))
        {
            bossState.ChangeState(new BossAttackRecoveryState());
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(BossState bossState)
    {
        // 武器データ取得
        weponData = bossState.GetEnemyWeponManager().GetWeaponData(bossState.GetEnemyWeponNumber());

        // アニメーション取得
        var animClip = weponData.GetAttackAnimation(bossState.GetEnemyConbo());
        var Anim = bossState.GetEnemyAnimator();
        
        // アニメーション再生
        if (Anim != null && animClip != null)
        {
            Anim.CrossFade(animClip.name, 0.2f);
        }

#if UNITY_EDITOR

        if (weponData == null)
        {
            Debug.LogError("BossAttackState : WeponDataが見つかりません");
            return;
        }
#endif
    }



    // 状態中の処理
    public override void Excute(BossState bossState)
    {
        // ダメージ処理
        bossState.HandleDamage();

        // フレーム更新
        freams += bossState.GetEnemySpeed();

        // 攻撃判定をONにする
        if (freams >= weponData.GetAttackStartupFrames(bossState.GetEnemyConbo()))
        {
            bossState.GetEnemyWeponManager().EnableAllWeaponAttacks();
        }
    }



    // 状態中の終了処理
    public override void Exit(BossState bossState)
    {
        // 攻撃判定をOFF
        bossState.GetEnemyWeponManager().DisableAllWeaponAttacks();
        // 初期化
        freams = 0.0f;
    }



}
