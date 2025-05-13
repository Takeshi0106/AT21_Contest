using UnityEngine;

// =======================================
// �v���C���[�̋��ݏ��
// =======================================

public class PlayerFlinchState : StateClass<PlayerState>
{
    // �C���X�^���X���擾����
    protected static PlayerFlinchState instance;
    // �t���[�����v������
    protected float freams = 0.0f;


    // �C���X�^���X���擾����֐�
    public static PlayerFlinchState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerFlinchState();
            }
            return instance;
        }
    }


    // ��Ԃ�ύX����
    public override void Change(PlayerState playerState)
    {
        if (freams > playerState.GetPlayerFlinchFreams())
        {
            playerState.ChangeState(PlayerStandingState.Instance);
        }
    }


    // ��Ԃ̊J�n����
    public override void Enter(PlayerState currentState)
    {

    }


    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        freams += playerState.GetPlayerSpeed();

#if UNITY_EDITOR
        // �f�o�b�O���F��F�ɕύX����
        playerState.GetPlayerRenderer().material.color = Color.blue;
#endif

        /*
        // ��ԕύX�����Ƃ���Input�𖳌��ɂ���@�U���{�^���������Ă�����A���̍s����\��
        if (freams > 1)
        {
            if (Input.GetButtonDown("Attack"))
            {
                playerState.SetPlayerNextReseved(RESEVEDSTATE.ATTACK);
            }
            if (Input.GetButtonDown("Counter"))
            {
                playerState.SetPlayerNextReseved(RESEVEDSTATE.COUNTER);
            }
            if (Input.GetButtonDown("Throw"))
            {
                playerState.SetPlayerNextReseved(RESEVEDSTATE.COUNTER);
            }
        }
        */
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        /*
        // �X�g�b�N�̏�����
        playerState.SetPlayerNextReseved(RESEVEDSTATE.NOTHING);
        */

#if UNITY_EDITOR
        // �f�o�b�O���F�𔒂ɂ���
        playerState.GetPlayerRenderer().material.color = Color.white;
#endif
    }



}
