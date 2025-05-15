using UnityEngine;
using UnityEngine.Playables;

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
        // �A�j���[�V�����Đ�
        // Animator ���擾
        var anim = currentState.GetPlayerAnimator();
        var animClip = currentState.GetPlayerFlinchAnimation();

        if (anim != null && animClip != null)
        {
            anim.CrossFade(animClip.name, 0.1f);
        }
#if UNITY_EDITOR
        else
        {
            Debug.Log("�A�j���[�V�������J�n����܂���");
        }
        Debug.Log("���ݏ��");
#endif
    }


    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        // �_���[�W����
        playerState.CleanupInvalidDamageColliders();

        freams += playerState.GetPlayerSpeed();

#if UNITY_EDITOR
        // �f�o�b�O���F��F�ɕύX����
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.blue;
        }
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

        freams = 0;

#if UNITY_EDITOR
        // �f�o�b�O���F�𔒂ɂ���
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.white;
        }
#endif
    }



}
