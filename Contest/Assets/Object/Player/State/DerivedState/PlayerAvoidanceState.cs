using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

// =============================
// ������
// =============================

public class PlayerAvoidanceState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerAvoidanceState instance;
    // ����̏����t���[��
    private int startUpFreams = 0;
    // ��𒆂̃t���[��
    private int avoidanceFreams = 0;
    // �����̃t���[��
    private int affterFreams = 0;

    private float freams = 0.0f;
    private bool avoidanceFlag = false;

    // �C���X�^���X���擾����֐�
    public static PlayerAvoidanceState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerAvoidanceState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(PlayerState currentstate)
    {
        // ����t���[�����I�������
        if ((startUpFreams + avoidanceFreams + affterFreams) < freams)
        {
            currentstate.ChangeState(PlayerStandingState.Instance);
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState currentState)
    {
        // �������
        startUpFreams = currentState.GetPlayerAvoidanceManager().GetAvoidanceStartUpFreams();
        avoidanceFreams = currentState.GetPlayerAvoidanceManager().GetAvoidanceFreams();
        affterFreams = currentState.GetPlayerAvoidanceManager().GetAvoidanceAfterFreams();
        AnimationClip clip = currentState.GetPlayerAvoidanceManager().GetAvoidanceAnimation();
        AvoidanceManager avoidanceManager = currentState.GetPlayerAvoidanceManager();

        // �A�j���[�V�����J�n
        if (currentState.GetPlayerAnimator() != null && clip != null)
        {
            currentState.GetPlayerAnimator().CrossFade(clip.name, 0.1f);
        }

        // ����������Ȃ��悤�ɂ���
        currentState.GetPlayerWeponManager().WeaponInvisible(currentState.GetPlayerWeponNumber());

        // ���̉�����
        currentState.GetPlayerRigidbody().AddForce(-currentState.transform.forward * avoidanceManager.GetAvoidancePower(), ForceMode.Impulse);

#if UNITY_EDITOR
        Debug.Log("PlayerAvoidanceState �J�n");
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState currentState)
    {
        // ���L�������`�F�b�N
        if ((startUpFreams < freams) &&
            (startUpFreams + avoidanceFreams > freams))
        {
            avoidanceFlag = true;
        }
        else
        {
            avoidanceFlag = false;
        }


        if (avoidanceFlag)
        {
            var counterTag = currentState.GetPlayerCounterPossibleAttack();
            var collidedInfos = currentState.GetPlayerCollidedInfos();

            // �������Ă���I�u�W�F�N�g�𒲂ׂ�
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
                    // ���G��L���ɂ���
                    currentState.GetPlayerStatusEffectManager().
                        StartInvicible(currentState.GetPlayerAvoidanceManager().GetAvoidanceInvincibleFreams());

                    // ��𐬌��������Ăяo��
                    currentState.GetPlayerAvoidanceManager().AvoidanceStart();

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
                        Debug.Log("��𐬌��I����̐e: " + parentTransform.gameObject.name);
                    }
                    Debug.Log("�U���I�u�W�F�N�g��: " + collidedInfo.collider.gameObject.name);
#endif

                    // �J�E���^�[�������ɏ������I������
                    return;
                }
            }
        }
        else
        {
            // �_���[�W������L���ɂ���
            currentState.HandleDamage();
        }

        // �t���[���X�V
        freams += currentState.GetPlayerSpeed();
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState currentState)
    {
        // �����������悤�ɂ���
        currentState.GetPlayerWeponManager().WeaponVisible(currentState.GetPlayerWeponNumber());

        freams = 0.0f;
    }



}
