using UnityEngine;

// ===============================
// �v���C���[�̑�����
// ===============================

public class DashState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static DashState instance;
    // �ړ��x
    Vector3 moveForward;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // ���̐F��ۑ�
    Color originalColor;
#endif

    // �C���X�^���X���擾����֐�
    public static DashState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DashState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(PlayerState playerState)
    {
        // ������ԂɕύX
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 &&
            !Input.GetButton("Dash"))
        {
            playerState.ChangeState(WalkState.Instance);
        }
        // �ړ��L�[���͂��Ȃ��Ƃ��A�ҋ@��ԂɕύX
        else if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            playerState.ChangeState(StandingState.Instance);
        }
        // �J�E���^�[��ԂɕύX
        else if (Input.GetButtonDown("Counter"))
        {
            playerState.ChangeState(CounterStanceState.Instance);
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        Debug.LogError("DashState : �J�n");

#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // ���̐F��ۑ�
            playerState.playerRenderer.material.color = Color.cyan;    // �_�b�V�����̐F
        }
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        //�ړ��x�����Z�b�g����
        moveForward = Vector3.zero;

        //���͂��擾
        float inputX = Input.GetAxisRaw("Horizontal"); //������
        float inputY = Input.GetAxisRaw("Vertical"); //�c����

        // �J�����̃x�N�g������ړ����������߂�
        moveForward = playerState.GetCameraTransform().forward * inputY + playerState.GetCameraTransform().right * inputX;
        moveForward = Vector3.Scale(moveForward, new Vector3(1, 0, 1)).normalized * playerState.GetDashSpeed();

        // �ړ��x�Ɉړ����x���|���ė͂�������
        playerState.GetPlayerRigidbody().velocity = new Vector3(moveForward.x, playerState.GetPlayerRigidbody().velocity.y, moveForward.z);

        //�L�����N�^�[����]������
        playerState.transform.eulerAngles =
            new Vector3(playerState.transform.eulerAngles.x, playerState.GetCameraTransform().eulerAngles.y, playerState.transform.eulerAngles.z);
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
#if UNITY_EDITOR
        // �G�f�B�^���s���ɐF�����ɖ߂�
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // ���̐F�ɖ߂�
        }
#endif
    }



}
