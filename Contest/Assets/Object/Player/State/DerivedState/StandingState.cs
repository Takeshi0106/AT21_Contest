using UnityEngine;

// =====================================
// �v���C���[�̉������Ă��Ȃ����
// =====================================

public class StandingState : StateClass
{
    // �C���X�^���X������ϐ�
    private static StandingState instance;
    // PlayerState������ϐ�
    PlayerState playerState;

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
    public override void Change(GameObject player)
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)   
        {
            playerState.ChangPlayerState(MoveState.Instance);
        }
    }

    // ��Ԃ̊J�n����
    public override void Enter(GameObject player)
    {
        Debug.LogError("StandingState : �J�n");

        playerState = player.GetComponent<PlayerState>();
        if (playerState == null)
        {
            Debug.LogError("StandingState : PlayerState��������܂���");
            return;
        }
    }

    // ��Ԓ��̏���
    public override void Excute(GameObject player)
    {
    
    }

    // ��Ԓ��̏I������
    public override void Exit(GameObject player)
    {

    }
}
