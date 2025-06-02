using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class BossStanState : StateClass<BossState>
{
    // 状態の変更処理
    public override void Change(BossState _BossState)
    {
        // スタンフラグが戻ったら立ち状態に戻る
        if (!_BossState.GetBossStatusEffectManager().GetStanFlag())
        {
            _BossState.ChangeState(new BossStandingState());
        }
    }

    // 状態の開始処理
    public override void Enter(BossState _BossState)
    {
        // スタンの開始処理
        _BossState.GetBossStatusEffectManager().StartStan();
    }

    // 状態中の処理
    public override void Excute(BossState _BossState)
    {
        // スタンの終了処理
        _BossState.GetBossStatusEffectManager().UpdateStan(_BossState.GetEnemySpeed());
    }

    // 状態中の終了処理
    public override void Exit(BossState _BossState)
    {

    }
}
