

public class BossDeadState : StateClass<BossState>
{

    // ��Ԃ̕ύX����
    public override void Change(BossState bossState)
    {

    }



    // ��Ԃ̊J�n����
    public override void Enter(BossState bossState)
    {
        bossState.GetEnemyWeponManager().DisableAllWeaponAttacks(); // �U���^�O�����ɖ߂�
        bossState.GetEnemyWeponManager().RemoveAllWeapon(); // ����f�[�^�����ׂč폜

        // ���S�A�j���[�V�����J�n
        if (bossState.GetEnemyAnimator() != null && bossState.GetEnemyDeadAnimation() != null)
        {
            bossState.GetEnemyAnimator().CrossFade(bossState.GetEnemyDeadAnimation().name, 0.1f);
        }
    }



    // ��Ԓ��̏���
    public override void Excute(BossState bossState)
    {

    }



    // ��Ԓ��̏I������
    public override void Exit(BossState bossState)
    {

    }
}
