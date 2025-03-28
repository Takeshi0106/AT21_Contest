using System;
using UnityEngine;

// =====================================
// �v���C���[�̈ړ����
// =====================================

// Rigidbody�R���|�[�l���g���K�{
[RequireComponent(typeof(Rigidbody))]

public class MoveState : StateClass
{
    // �C���X�^���X������ϐ�
    private static MoveState instance;
    // Player�̃��W�b�h�{�f�B
    Rigidbody rb;
    // PlayerState������ϐ�
    PlayerState playerState;
    // �ړ��x
    Vector3 moveForward;

    // �C���X�^���X���擾����֐�
    public static MoveState Instance
    {
        get
        {
            if(instance==null)
            {
                instance = new MoveState();
            }
            return instance;
        }
    }

    // ��Ԃ̕ύX����
    public override void Change(GameObject player)
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            playerState.ChangPlayerState(StandingState.Instance);
        }
    }

    // ��Ԃ̊J�n����
    public override void Enter(GameObject player)
    {
        Debug.LogError("MoveState : �J�n");

        // Rigidbody��T��
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("MoveState : Rigidbody��������܂���");
            return;
        }
        // PlayerState��T��
        playerState = player.GetComponent<PlayerState>();
        if (playerState == null)
        {
            Debug.LogError("MoveState : PlayerState��������܂���");
            return;
        }
    }

    // ��Ԓ��̏���
    public override void Excute(GameObject player)
    {
        //�ړ��x�����Z�b�g����
        moveForward = Vector3.zero;

        //���͂��擾
        float inputX = Input.GetAxisRaw("Horizontal"); //������
        float inputY = Input.GetAxisRaw("Vertical"); //�c����

        // �J�����̃x�N�g������ړ����������߂�
        moveForward = playerState.cameraTransform.forward * inputY + playerState.cameraTransform.right * inputX;
        moveForward = Vector3.Scale(moveForward, new Vector3(1, 0, 1)).normalized * playerState.speed;

        // �ړ��x�Ɉړ����x���|���ė͂�������
        rb.velocity = new Vector3(moveForward.x, rb.velocity.y, moveForward.z);

        //�L�����N�^�[����]������
        player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, playerState.cameraTransform.eulerAngles.y, player.transform.eulerAngles.z);
    }

    // ��Ԓ��̏I������
    public override void Exit(GameObject player)
    {

    }
}
