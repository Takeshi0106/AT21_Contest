using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("�J�����I�u�W�F�N�g��")]
    public string cameraName = "Main Camera"; //�J�����I�u�W�F�N�g��

    [Header("�ړ����x")]
    public float speed = 2.0f; //�ړ����x

    // �v���C���[��State������ϐ�    
    StateClass playerState;
    // �J�����̃g�����X�t�H�[�� ���̃X�N���v�g�ȊO�ŕύX�ł��Ȃ��悤�ɐݒ�
    [HideInInspector] public Transform cameraTransform { get; private set; }
    // Player�̃��W�b�h�{�f�B
    [HideInInspector] public Rigidbody playerRigidbody { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // ��Ԃ��Z�b�g
        playerState = StandingState.Instance;
        // ��Ԃ̊J�n����
        playerState.Enter(this.gameObject);

        //�@�J�����I�u�W�F�N�g����
        cameraTransform = GameObject.Find(cameraName).transform;
        if (cameraTransform == null)
        {
            Debug.LogError("PlayerState : �J�����I�u�W�F�N�g��������܂���");
            return;
        }

        // Player���W�b�h�{�f�B�[
        // Rigidbody��T��
        playerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("PlayerState : Rigidbody��������܂���");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ��Ԃ̕ύX
        playerState.Change(this.gameObject);
        // ��Ԃ̍X�V
        playerState.Excute(this.gameObject);
    }

    // State��Chang���鏈��
    public void ChangPlayerState(StateClass stateClass)
    {
        // �I������
        playerState.Exit(this.gameObject);
        // State�ύX
        playerState = stateClass;
        // �J�n����
        playerState.Enter(this.gameObject);
    }
}
