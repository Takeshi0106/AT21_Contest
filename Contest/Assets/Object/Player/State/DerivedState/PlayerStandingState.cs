using UnityEngine;

// =====================================
// �v���C���[�̉������Ă��Ȃ����
// =====================================

public class PlayerStandingState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerStandingState instance;



    // �C���X�^���X���擾����֐�
    public static PlayerStandingState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerStandingState();
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
            playerState.ChangeState(PlayerDashState.Instance);
        }
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0&&
            !Input.GetButton("Dash"))   
        {
            playerState.ChangeState(PlayerWalkState.Instance);
        }
        // �J�E���^�[��ԂɕύX
        else if(Input.GetButtonDown("Counter"))
        {
            playerState.ChangeState(PlayerCounterStanceState.Instance);
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
#if UNITY_EDITOR
        Debug.LogError("PlayerStandingState : �J�n");
#endif
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
