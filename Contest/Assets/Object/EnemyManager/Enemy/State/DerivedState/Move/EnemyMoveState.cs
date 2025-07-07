using UnityEngine;

public class EnemyMoveState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private EnemyMoveState instance;

    // �C���X�^���X���擾����֐�
    public EnemyMoveState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyMoveState();
            }
            return instance;
        }
    }

    // ��Ԃ̕ύX����
    public override void Change(EnemyState enemyState)
    {
        Vector3 vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;

        // �U�����
        if (vec.magnitude < enemyState.GetDistanceAttack() && enemyState.GetEnemyAttackFlag())
        {
            enemyState.ChangeState(new EnemyAttackState());
            return;
        }
        // �������
        if (vec.magnitude < 7.5f && !enemyState.GetEnemyAttackFlag())
        {
            enemyState.ChangeState(new EnemyStandingState());
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
        Debug.Log("Move �J�n");

        // �����ԃA�j���[�V�����J�n
        if (enemyState.GetEnemyAnimator() != null && enemyState.GetEnemyDashAnimation() != null)
        {
            enemyState.GetEnemyAnimator().CrossFade(enemyState.GetEnemyDashAnimation().name, 0.1f);
        }
    }



    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {
        Vector3 vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;


        enemyState.Target();
        Rigidbody rb = enemyState.GetEnemyRigidbody();

        if (vec.magnitude > 8.0f)
        {
            Vector3 vel = enemyState.transform.forward * enemyState.GetEnemyDashSpeed();
            vel.y = rb.velocity.y; // y�����i�d�́j���ێ�
            rb.velocity = vel;
        }
        else if (vec.magnitude > 1.0f && enemyState.GetEnemyAttackFlag())
        {
            Vector3 vel = enemyState.transform.forward * enemyState.GetEnemyDashSpeed();
            vel.y = rb.velocity.y; // y�����i�d�́j���ێ�
            rb.velocity = vel;
        }
    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {

    }
}
