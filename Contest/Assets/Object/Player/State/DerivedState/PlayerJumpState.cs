using UnityEngine;
using UnityEngine.Playables;

// =============================
// �W�����v���
// =============================

public class PlayerJumpState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerJumpState instance;
    // �ړ��x
    Vector3 moveForward;



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

            if (currentState.GetPlayerAnimator() != null && currentState.GetPlayerJumpAnimation() != null)
            {
                currentState.GetPlayerAnimator().CrossFade(currentState.GetPlayerJumpAnimation().name, 0.1f);
            }

            currentState.SetJumpFlag(true);
        }
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
    public override void Exit(PlayerState currentState)
    {

    }
}
