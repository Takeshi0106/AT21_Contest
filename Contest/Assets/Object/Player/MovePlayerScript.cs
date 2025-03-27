using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rigidbody�R���|�[�l���g���K�{
[RequireComponent(typeof(Rigidbody))]

public class MovePlayerScript : MonoBehaviour
{
    // Player�̃��W�b�h�{�f�B
    Rigidbody rb;
    // �ړ��x
    Vector3 moveForward;
    // �J�����̃g�����X�t�H�[��
    Transform cameraTransform;

    [Header("�J�����I�u�W�F�N�g��")]
    public string cameraName = "Main Camera"; //�J�����I�u�W�F�N�g��

    [Header("�ړ����x")]
    public float speed = 2.0f; //�ړ����x

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody����
        rb = this.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("MovePlayerScript : Rigidbody��������܂���");
            return;
        }

        //�@�J�����I�u�W�F�N�g����
        cameraTransform = GameObject.Find(cameraName).transform;
        if (cameraTransform == null)
        {
            Debug.LogError("MovePlayerScript : �J�����I�u�W�F�N�g��������܂���");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ��x�����Z�b�g����
        moveForward = Vector3.zero;

        //���͂��擾
        float inputX = Input.GetAxisRaw("Horizontal"); //������
        float inputY = Input.GetAxisRaw("Vertical"); //�c����

        // �J�����̃x�N�g������ړ����������߂�
        moveForward = cameraTransform.forward * inputY + cameraTransform.right * inputX;
        moveForward = Vector3.Scale(moveForward, new Vector3(1, 0, 1)).normalized * speed;

        // �ړ��x�Ɉړ����x���|���ė͂�������
        rb.velocity = new Vector3(moveForward.x, rb.velocity.y, moveForward.z);

        //�L�����N�^�[����]������
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, cameraTransform.eulerAngles.y, this.transform.eulerAngles.z);

    }
}
