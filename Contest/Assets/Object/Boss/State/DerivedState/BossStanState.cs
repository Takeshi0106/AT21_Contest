
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

        // �A�j���[�V�����擾
        var animClip = _BossState.GetBossStatusEffectManager().GetStanAnimationClip();
        var Anim = _BossState.GetEnemyAnimator();

        // �A�j���[�V�����Đ�
        if (Anim != null && animClip != null)
        {
            Anim.CrossFade(animClip.name, 0.2f);
        }
    }

    // ��Ԓ��̏���
    public override void Excute(BossState _BossState)
    {
        _BossState.HandleDamage();

        // �X�^���̏I������
        _BossState.GetBossStatusEffectManager().UpdateStan(_BossState.GetEnemySpeed());
    }

    // ��Ԓ��̏I������
    public override void Exit(BossState _BossState)
    {

    }
}
