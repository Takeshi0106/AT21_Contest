using UnityEngine;

// =====================================
// �v���C���[�̕����ړ����
// =====================================

public class PlayerWalkState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerWalkState instance;
    // �ړ��x
    Vector3 moveForward;



    // �C���X�^���X���擾����֐�
    public static PlayerWalkState Instance
    {
        get
        {
            if(instance==null)
            {
                instance = new PlayerWalkState();
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
        // Dash �{�^�����������ꍇ�� Dash ��Ԃ֕ύX
        if (Input.GetButtonDown("Dash"))
        {
            playerState.ChangeState(PlayerDashState.Instance);
            return;
        }
        // ����𓊂����ԂɈڍs
        if (Input.GetButtonDown("Throw"))
        {
            if (playerState.GetPlayerWeponManager().GetWeaponCount() <= 1)
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
#if UNITY_EDITOR
        Debug.LogError("WalkState : �J�n");
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        playerState.HandleDamage();

        //�ړ��x�����Z�b�g����
        moveForward = Vector3.zero;

        //���͂��擾
        float inputX = Input.GetAxisRaw("Horizontal"); //������
        float inputY = Input.GetAxisRaw("Vertical"); //�c����

        // �J�����̃x�N�g������ړ����������߂�
        moveForward = playerState.GetCameraTransform().forward * inputY + playerState.GetCameraTransform().right * inputX;
        moveForward = Vector3.Scale(moveForward, new Vector3(1, 0, 1)).normalized * playerState.GetWalkSpeed();

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

    }



}
