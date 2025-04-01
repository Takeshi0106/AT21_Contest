using System.Collections.Generic;
using UnityEngine;

// ================================
// �v���C���[�̃J�E���^�[�\�����
// ================================

public class CounterStanceState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static CounterStanceState instance;
    // �t���[�����v��
    int freams = 0;
    // �J�E���^�[�̐���
    bool counterOutcome = false;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // ���̐F��ۑ�
    Color originalColor;
#endif



    // �C���X�^���X���擾����
    public static CounterStanceState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CounterStanceState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(PlayerState playerState)
    {
        // �J�E���^�[�̗L���t���[�����߂�����
        if (freams >= playerState.GetPlayerCounterManager().GetCounterFrames())
        {
            playerState.ChangeState(CounterStaggerState.Instance);
        }
        // �J�E���^�[������������
        if(counterOutcome)
        {
            playerState.ChangeState(CounterStrikeState.Instance);
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        Debug.LogError("CounterStanceState : �J�n");


#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // ���̐F��ۑ�
            playerState.playerRenderer.material.color = Color.yellow;    // �J�E���^�[�\�����̐F
        }
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        // �J�E���^�[�̐��۔���
        if (freams <= playerState.GetPlayerCounterManager().GetCounterFrames())
        {
            // �Ԃ������I�u�W�F�N�g�̃^�O���`�F�b�N
            foreach (var obj in playerState.GetCollidedObjects())
            {
                if (obj != null && obj.CompareTag("ParryableAttack"))
                {
                    Debug.Log("�J�E���^�[�����I ����: " + obj.gameObject.name);
                    counterOutcome = true;
                }
            }
        }
        // �t���[�������v��
        freams++;
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        // ������
        freams = 0;
        counterOutcome = false;

#if UNITY_EDITOR
        // �G�f�B�^���s���ɐF�����ɖ߂�
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // ���̐F�ɖ߂�
        }
#endif
    }



}
