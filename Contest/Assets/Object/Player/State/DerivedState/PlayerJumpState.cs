using UnityEngine;

// =============================
// �W�����v���
// =============================

public class PlayerJumpState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerJumpState instance;


    // �C���X�^���X���擾����֐�
    public static PlayerJumpState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerJumpState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(PlayerState currentstate)
    {
        if(!currentstate.GetPlayerAirFlag())
        {
            currentstate.ChangeState(PlayerStandingState.Instance);
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState currentState)
    {
        if (!currentState.GetPlayerAirFlag())
        {
#if UNITY_EDITOR
            Debug.Log("�W�����v");
#endif

            // �W�����v����
            currentState.GetPlayerRigidbody().AddForce(new Vector3(0.0f, currentState.GetPlayerJumpPower(), 0.0f), ForceMode.Impulse);
        }
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState currentState)
    {
        
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState currentState)
    {

    }
}
