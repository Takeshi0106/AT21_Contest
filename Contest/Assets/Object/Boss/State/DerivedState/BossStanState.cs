using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class BossStanState : StateClass<BossState>
{
    // ��Ԃ̕ύX����
    public override void Change(BossState _BossState)
    {
        // �X�^���t���O���߂����痧����Ԃɖ߂�
        if (!_BossState.GetBossStatusEffectManager().GetStanFlag())
        {
            _BossState.ChangeState(new BossStandingState());
        }
    }

    // ��Ԃ̊J�n����
    public override void Enter(BossState _BossState)
    {
        // �X�^���̊J�n����
        _BossState.GetBossStatusEffectManager().StartStan();
    }

    // ��Ԓ��̏���
    public override void Excute(BossState _BossState)
    {
        // �X�^���̏I������
        _BossState.GetBossStatusEffectManager().UpdateStan(_BossState.GetEnemySpeed());
    }

    // ��Ԓ��̏I������
    public override void Exit(BossState _BossState)
    {

    }
}
