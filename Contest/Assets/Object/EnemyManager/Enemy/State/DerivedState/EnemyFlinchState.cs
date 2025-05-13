

public class EnemyFlinchState : StateClass<EnemyState>
{
    // �C���X�^���X���擾����
    protected static EnemyFlinchState instance;
    // �t���[�����v������
    protected float freams = 0.0f;


    // �C���X�^���X���擾����֐�
    public static EnemyFlinchState Instance
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
            enemyState.ChangeState(EnemyStandingState.Instance);
        }
    }

    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {

    }


    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {
        freams += enemyState.GetEnemySpeed();
    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {

    }
}
