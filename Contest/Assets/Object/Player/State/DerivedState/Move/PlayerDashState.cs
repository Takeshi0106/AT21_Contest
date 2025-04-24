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
        // ����𓊂����ԂɈڍs
        if (Input.GetButtonDown("Throw"))
        {
            if (playerState.GetPlayerWeponManager().GetWeaponCount() < 1)
            {
                // ����𓊂���̎��s��ԂɈڍs
                playerState.ChangeState(PlayerThrowFailedState.Instance);
            }
            else
            {
                // ����𓊂����ԂɈڍs
                playerState.ChangeState(PlayerWeaponThrowState.Instance);
            }
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        Debug.LogError("DashState : �J�n");

#if UNITY_EDITOR
        // �_�b�V�����ɐF�����ɂ���
        playerState.playerRenderer.material.color = Color.magenta;
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        playerState.HandleDamage(playerState.GetPlayerEnemyAttackTag());

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
#if UNITY_EDITOR
        // �F�����ɖ߂�
        playerState.GetPlayerRenderer().material.color = Color.white;
#endif
    }



}
