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
        // ����ړ���ԂɕύX
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)&&
            Input.GetButton("Dash"))
        {
            playerState.ChangeState(PlayerDashState.Instance);
            return;
        }
        // �����ړ���ԂɕύX
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0&&
            !Input.GetButton("Dash"))   
        {
            playerState.ChangeState(PlayerWalkState.Instance);
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
        //�󒆂ɕ����Ă�����
        if ((Input.GetButtonDown("Jump") && !playerState.GetPlayerAirFlag()) ||
            playerState.GetPlayerAirFlag())
        {
            playerState.ChangeState(PlayerJumpState.Instance);
            return;
        }
        // �����ԂɈڍs
        if (Input.GetButtonDown("Avoidance"))
        {
            playerState.ChangeState(PlayerAvoidanceState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        // ������ԃA�j���[�V�����J�n
        if (playerState.GetPlayerAnimator() != null && playerState.GetPlayerStandingAnimation() != null)
        {
            playerState.GetPlayerAnimator().CrossFade(playerState.GetPlayerStandingAnimation().name, 0.1f);
        }

#if UNITY_EDITOR
        Debug.LogError("PlayerStandingState : �J�n");
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        playerState.HandleDamage();
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {

    }



}
