using UnityEngine;

public class EnemyFlinchState : StateClass<EnemyState>
{
    // �C���X�^���X���擾����
    protected EnemyFlinchState instance;
    // �t���[�����v������
    protected float freams = 0.0f;


    // �C���X�^���X���擾����֐�
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


    // ��Ԃ�ύX����
    public override void Change(EnemyState enemyState)
    {
        if (freams > enemyState.GetEnemyFlinchFreams())
        {
            enemyState.ChangeState(new EnemyStandingState());
        }
    }

    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
        // �A�j���[�V�����Đ�
        // Animator ���擾
        var anim = enemyState.GetEnemyAnimator();
        var animClip = enemyState.GetEnemyFlinchAnimation();

        if (anim != null && animClip != null)
        {
            anim.CrossFade(animClip.name, 0.1f);
        }

        enemyState.SetEnemyFlinchCnt(enemyState.GetEnemyFlinchCnt() + 1);
    }


    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {
        freams += enemyState.GetEnemySpeed();
    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {
        enemyState.GetEnemyDamageResponseManager().RecoverFlinch();
        freams = 0;
    }
}