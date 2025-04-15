using UnityEngine;

// ===============================
// �v���C���[�̑�����
// ===============================

public class PlayerDashState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerDashState instance;
    // �ړ��x
    Vector3 moveForward;

    // �G�f�B�^���s���Ɏ��s�����
    // ���̐F��ۑ�
    Color originalColor;

#if UNITY_EDITOR

#endif

    // �C���X�^���X���擾����֐�
    public static PlayerDashState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerDashState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(PlayerState playerState)
    {
        // �J�E���^�[��ԂɕύX
        if (Input.GetButtonDown("Counter"))
        {
            playerState.ChangeState(PlayerCounterStanceState.Instance);
            return;
        }
        // �U����ԂɕύX
        if (Input.GetButtonDown("Attack"))
        {
            playerState.ChangeState(PlayerAttackState.Instance);
            return;
        }
        // �ړ��L�[���͂��Ȃ��Ƃ��A�ҋ@��ԂɕύX
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            playerState.ChangeState(PlayerStandingState.Instance);
            return;
        }
        // ������ԂɕύX
        if (!Input.GetButton("Dash"))
        {
            playerState.ChangeState(PlayerWalkState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // ���̐F��ۑ�
            playerState.playerRenderer.material.color = Color.magenta;    // �_�b�V�����̐F
        }

#if UNITY_EDITOR
        Debug.LogError("DashState : �J�n");

        
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

        // �ړ������Ɍ�����
        if (moveForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveForward);
            playerState.transform.rotation = Quaternion.Slerp(playerState.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        // �G�f�B�^���s���ɐF�����ɖ߂�
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // ���̐F�ɖ߂�
        }

#if UNITY_EDITOR

#endif
    }



}
