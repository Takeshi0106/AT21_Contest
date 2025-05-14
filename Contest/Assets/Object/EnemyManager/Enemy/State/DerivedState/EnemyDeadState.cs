

public class EnemyDeadState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private static EnemyDeadState instance;

    // �C���X�^���X���擾����֐�
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



    // ��Ԃ̕ύX����
    public override void Change(EnemyState enemyState)
    {

    }



    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
        enemyState.GetEnemyWeponManager().RemoveAllWeapon(); // ����f�[�^�����ׂč폜

        // ���S�A�j���[�V�����J�n
        if (enemyState.GetEnemyAnimator() != null && enemyState.GetEnemyDeadAnimation() != null)
        {
            enemyState.GetEnemyAnimator().CrossFade(enemyState.GetEnemyDeadAnimation().name, 0.1f);
        }
    }



    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {

    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {

    }
}
