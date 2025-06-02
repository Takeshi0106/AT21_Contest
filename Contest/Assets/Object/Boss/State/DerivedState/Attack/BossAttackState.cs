using UnityEngine;

// ======================
// �G�̍U�����
// ======================

public class BossAttackState : StateClass<BossState>
{
    // weponData
    private static BaseAttackData weponData;
    // �t���[�����v��
    float freams = 0.0f;


    // ��Ԃ̕ύX����
    public override void Change(BossState bossState)
    {
        // �U���̃t���[�����߂�����
        if (freams >= (weponData.GetAttackStartupFrames(bossState.GetEnemyConbo()) +
    weponData.GetAttackSuccessFrames(bossState.GetEnemyConbo())))
        {
            bossState.ChangeState(new BossAttackRecoveryState());
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(BossState bossState)
    {
        // ����f�[�^�擾
        weponData = bossState.GetEnemyWeponManager().GetWeaponData(bossState.GetEnemyWeponNumber());

        // �A�j���[�V�����擾
        var animClip = weponData.GetAttackAnimation(bossState.GetEnemyConbo());
        var Anim = bossState.GetEnemyAnimator();
        
        // �A�j���[�V�����Đ�
        if (Anim != null && animClip != null)
        {
            Anim.CrossFade(animClip.name, 0.2f);
        }

#if UNITY_EDITOR

        if (weponData == null)
        {
            Debug.LogError("BossAttackState : WeponData��������܂���");
            return;
        }
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(BossState bossState)
    {
        // �_���[�W����
        bossState.HandleDamage();

        // �t���[���X�V
        freams += bossState.GetEnemySpeed();

        // �U�������ON�ɂ���
        if (freams >= weponData.GetAttackStartupFrames(bossState.GetEnemyConbo()))
        {
            bossState.GetEnemyWeponManager().EnableAllWeaponAttacks();
        }
    }



    // ��Ԓ��̏I������
    public override void Exit(BossState bossState)
    {
        // �U�������OFF
        bossState.GetEnemyWeponManager().DisableAllWeaponAttacks();
        // ������
        freams = 0.0f;
    }



}
