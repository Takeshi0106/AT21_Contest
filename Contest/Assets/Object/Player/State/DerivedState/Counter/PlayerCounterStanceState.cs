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
            // �U���^�O���߂��Ă��邩���`�F�b�N
            playerState.CleanupInvalidDamageColliders();

            // �������Ă���I�u�W�F�N�g�𒲂ׂ�
            foreach (var collidedInfo in playerState.GetPlayerCollidedInfos())
            {
                if (collidedInfo.collider != null)
                {
                    // �R���C�_�[�����łɃ_���[�W���������Ă����玟�̃I�u�W�F�N�g�𒲂ׂ�
                    if (playerState.GetPlayerDamagedColliders().Contains(collidedInfo.collider)) { continue; }

                    // �^�O���擾����
                    MultiTag tag = collidedInfo.multiTag;

                    if (tag != null && tag.HasTag(playerState.GetPlayerCounterPossibleAttack()))
                    {
                        // �J�E���^�[����
                        counterOutcome = true;
                        // �R���C�_�[��ۑ�����
                        playerState.AddDamagedCollider(collidedInfo.collider);


#if UNITY_EDITOR
                        // �e�I�u�W�F�N�g�̖��O���擾����
                        Transform parentTransform = collidedInfo.collider.transform.parent;
                        // ��ԏ�̐e�I�u�W�F�N�g���擾
                        while (parentTransform.parent != null)
                        {
                            parentTransform = parentTransform.parent;
                        }

                        // ���O�\��
                        if (parentTransform != null)
                        {
                            Debug.Log("�J�E���^�[�����I����̐e: " + parentTransform.gameObject.name);
                        }
                        Debug.Log("�U���I�u�W�F�N�g��: " + collidedInfo.collider.gameObject.name);
#endif

                        // �J�E���^�[�������ɏ������I������
                        return;
                    }
                }
            }
        }
        else
        {
            // �_���[�W������L���ɂ���
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
