using UnityEngine;

// ================================
// �v���C���[�̃J�E���^�[���
// ================================

// �K�{�I�I
#if UNITY_EDITOR
[RequireComponent(typeof(Renderer))]
#endif

public class CounterState : StateClass
{
    // �C���X�^���X������ϐ�
    private static CounterState instance;
    // PlayerState������ϐ�
    PlayerState playerState;
    // �t���[�����v��
    int freams = 0;
    // �f�o�b�O�p
    // �F��ύX����
    Renderer objectRenderer;
    Color originalColor; // ���̐F��ۑ�



    // �C���X�^���X���擾����
    public static CounterState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CounterState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(GameObject player)
    {
        if (freams >= playerState.CounterActiveFreams)
        {
            playerState.ChangPlayerState(StandingState.Instance);
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(GameObject player)
    {
        Debug.LogError("CounterState : �J�n");

        playerState = player.GetComponent<PlayerState>();
        if (playerState == null)
        {
            Debug.LogError("CounterState : PlayerState��������܂���");
            return;
        }


#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        objectRenderer = player.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color; // ���̐F��ۑ�
            objectRenderer.material.color = Color.red;    // �J�E���^�[���̐F�i��: �ԁj
        }
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(GameObject player)
    {
        // �t���[�������v��
        freams++;
    }



    // ��Ԓ��̏I������
    public override void Exit(GameObject player)
    {
        // ������
        freams = 0;


#if UNITY_EDITOR
        // �G�f�B�^���s���ɐF�����ɖ߂�
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor; // ���̐F�ɖ߂�
        }
#endif
    }



}
