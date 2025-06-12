using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

// ================================
// �v���C���[�̃J�E���^�[�\�����
// ================================

public class PlayerCounterStanceState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerCounterStanceState instance;
    // �t���[�����v��
    float freams = 0.0f;
    // �J�E���^�[�̐���
    bool counterOutcome = false;
    // �J�E���^�[���L�����ǂ���
    bool counterActive = false;



    // �C���X�^���X���擾����
    public static PlayerCounterStanceState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerCounterStanceState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(PlayerState playerState)
    {
        // �J�E���^�[�̗L���t���[�����߂�����
        if (freams >= playerState.GetPlayerCounterManager().GetCounterFrames() + 
            playerState.GetPlayerCounterManager().GetCounterStartupFrames())
        {
            playerState.ChangeState(PlayerCounterStaggerState.Instance);
            return;
        }
        // �J�E���^�[������������
        if(counterOutcome)
        {
            playerState.ChangeState(PlayerCounterStrikeState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        // �A�j���[�V�����擾
        var animClip = playerState.GetPlayerCounterManager().GetCounterAnimation();
        var Anim = playerState.GetPlayerAnimator();

        // �A�j���[�V�����Đ�
        if (Anim != null && animClip != null)
        {
            Anim.CrossFade(animClip.name, 0.1f);
        }

        // ������\��
        playerState.GetPlayerWeponManager().WeaponInvisible(playerState.GetPlayerWeponNumber());

#if UNITY_EDITOR
        Debug.LogError("CounterStanceState : �J�n");

        // �J�E���^�[�\�����ΐF�ɂ���
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.green;
        }
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        // **�J�E���^�[�������Ԃ̏���**
        if (freams > playerState.GetPlayerCounterManager().GetCounterStartupFrames())
        {
            // ����������ɃJ�E���^�[�L����
            counterActive = true;

#if UNITY_EDITOR
            playerState.playerRenderer.material.color = Color.yellow;
#endif
        }

        if (counterActive)
        {
            var counterTag = playerState.GetPlayerCounterPossibleAttack();
            var collidedInfos = playerState.GetPlayerCollidedInfos();

            for (int i = 0; i < collidedInfos.Count; i++)
            {
                var collidedInfo = collidedInfos[i];
                var collider = collidedInfo.collider;
                var tag = collidedInfo.multiTag;

                if (collider == null) continue;

                // ���łɃ_���[�W�������ꂽ���̂̓X�L�b�v
                if (collidedInfo.hitFlag) continue;

                if (tag != null && tag.HasTag(counterTag))
                {
                    // �J�E���^�[����
                    counterOutcome = true;

                    // �t���O���X�V���ď����߂�
                    collidedInfo.hitFlag = true;
                    collidedInfos[i] = collidedInfo;

#if UNITY_EDITOR
                    // ��ԏ�̐e�I�u�W�F�N�g���擾
                    Transform parentTransform = collider.transform;
                    while (parentTransform.parent != null)
                    {
                        parentTransform = parentTransform.parent;
                    }

                    Debug.Log("�J�E���^�[�����I����̐e: " + parentTransform.gameObject.name);
                    Debug.Log("�U���I�u�W�F�N�g��: " + collider.gameObject.name);
#endif

                    return;
                }
            }
        }
        else
        {
            // �ʏ�_���[�W����
            playerState.HandleDamage();
        }

        // �t���[�������v��
        freams += playerState.GetPlayerSpeed();
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        // ������
        freams = 0.0f;
        counterOutcome = false;
        counterActive = false;

        playerState.GetPlayerWeponManager().WeaponVisible(playerState.GetPlayerWeponNumber());

#if UNITY_EDITOR
        // ���̐F�ɖ߂�
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.white;
        }
#endif
    }



}
