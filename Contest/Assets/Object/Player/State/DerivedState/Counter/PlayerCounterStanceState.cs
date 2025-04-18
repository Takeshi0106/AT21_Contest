using System.Collections.Generic;
using UnityEngine;

// ================================
// �v���C���[�̃J�E���^�[�\�����
// ================================

public class PlayerCounterStanceState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerCounterStanceState instance;
    // �t���[�����v��
    int freams = 0;
    // �J�E���^�[�̐���
    bool counterOutcome = false;
    // �J�E���^�[���L�����ǂ���
    bool counterActive = false;

    // �G�f�B�^���s���Ɏ��s�����
    // ���̐F��ۑ�
    Color originalColor;


#if UNITY_EDITOR

#endif



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
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        if (playerState.playerRenderer != null)
        {
            originalColor = playerState.playerRenderer.material.color; // ���̐F��ۑ�
            playerState.playerRenderer.material.color = Color.cyan;    // �J�E���^�[�\�����̐F
        }

#if UNITY_EDITOR
        Debug.LogError("CounterStanceState : �J�n");

        
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

            playerState.playerRenderer.material.color = Color.yellow;

#if UNITY_EDITOR

#endif
        }

        if (counterActive)
        {
            playerState.CleanupInvalidDamageColliders(playerState.GetPlayerEnemyAttackTag());

            foreach (var collidedInfo in playerState.GetPlayerCollidedInfos())
            {
                if (collidedInfo.collider != null)
                {
                    MultiTag tag = collidedInfo.multiTag;
                    if (tag != null && tag.HasTag(playerState.GetPlayerCounterPossibleAttack()))
                    {
                        counterOutcome = true;

#if UNITY_EDITOR
                        Debug.Log("�J�E���^�[�����I����: " + collidedInfo.collider.gameObject.name);
#endif
                    }
                }
            }
        }
        else
        {
            playerState.HandleDamage(playerState.GetPlayerEnemyAttackTag());
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
        counterActive = false;

        // �G�f�B�^���s���ɐF�����ɖ߂�
        if (playerState.playerRenderer != null)
        {
            playerState.playerRenderer.material.color = originalColor; // ���̐F�ɖ߂�
        }

#if UNITY_EDITOR

#endif
    }



}
