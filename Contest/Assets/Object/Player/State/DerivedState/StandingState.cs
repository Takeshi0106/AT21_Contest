using UnityEngine;

// =====================================
// �v���C���[�̉������Ă��Ȃ����
// =====================================

public class StandingState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static StandingState instance;



    // �C���X�^���X���擾����֐�
    public static StandingState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StandingState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(PlayerState playerState)
    {
        // ����ړ���ԂɕύX
        if((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)&&
            Input.GetButton("Dash"))
        {
            playerState.ChangeState(DashState.Instance);
        }
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0&&
            !Input.GetButton("Dash"))   
        {
            playerState.ChangeState(WalkState.Instance);
        }
        // �J�E���^�[��ԂɕύX
        else if(Input.GetButtonDown("Counter"))
        {
            playerState.ChangeState(CounterStanceState.Instance);
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        Debug.LogError("StandingState : �J�n");
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
    
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {

    }



}
