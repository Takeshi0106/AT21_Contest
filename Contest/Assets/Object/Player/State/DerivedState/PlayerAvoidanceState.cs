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
            // �U���^�O���߂��Ă��邩���`�F�b�N
            currentState.CleanupInvalidDamageColliders();

            // �������Ă���I�u�W�F�N�g�𒲂ׂ�
            foreach (var collidedInfo in currentState.GetPlayerCollidedInfos())
            {
                if (collidedInfo.collider != null)
                {
                    // �R���C�_�[�����łɃ_���[�W���������Ă����玟�̃I�u�W�F�N�g�𒲂ׂ�
                    if (currentState.GetPlayerDamagedColliders().Contains(collidedInfo.collider)) { continue; }

                    // �^�O���擾����
                    MultiTag tag = collidedInfo.multiTag;

                    if (tag != null && tag.HasTag(currentState.GetPlayerCounterPossibleAttack()))
                    {
                        // ���G��L���ɂ���
                        currentState.GetPlayerStatusEffectManager().
                            StartInvicible(currentState.GetPlayerAvoidanceManager().GetAvoidanceInvincibleFreams());
                        // �R���C�_�[��ۑ�����
                        currentState.AddDamagedCollider(collidedInfo.collider);

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
        freams = 0.0f;
    }



}
