using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Camera�R���|�[�l���g���K�{�I�I
[RequireComponent(typeof(Camera))]

public class TPFCamera : MonoBehaviour
{
    [Header("�J������t����I�u�W�F�N�g�̖��O")]
    public string objectName = "Player"; //�I�u�W�F�N�g��

    [Header("�I�u�W�F�N�g�ƃJ�����̋���")]
    public float distance = 3.0f; //�I�u�W�F�N�g�ƃJ�����̋���

    [Header("�}�E�X���x�ݒ�")]
    public float sensitivity = 3.0f; //�}�E�X���x

    [Header("�J�����̒����_�ݒ�(�I�u�W�F�N�g����̑��Έʒu)")]
    public Vector2 lookPoint = new Vector2(0.7f, 1.0f); //�I�u�W�F�N�g�̒��S����̃J�����̈ʒu

    GameObject obj; //�J����������I�u�W�F�N�g
    Transform cameraTrans; //�J�����̃g�����X�t�H�[��
    Vector3 targetPos; //�J�����̃x�N�g��������

    // �}�E�X�̓��͂�������
    float mouseInputX;
    float mouseInputY;

    // Start is called before the first frame update
    void Start()
    {
        //���O�����ŃI�u�W�F�N�g��������
        obj = GameObject.Find(objectName);
        if (obj == null) //�v���C���[�I�u�W�F�N�g��������Ȃ���΃G���[
        {
            Debug.LogError("�v���C���[�I�u�W�F�N�g��������܂���");
            return;
        }

        //���g��transform���擾���Ă���
        cameraTrans = this.transform;

        //�J�����̒����_�����߂�i�L�����N�^�[�̏����E��j
        targetPos = ExportTargetPos(obj); //�L�����N�^�[�̍��W���擾

        //�J�����̈ʒu�����߂�i�L�����N�^�̌�������kyori�����ړ��������n�_�j
        Vector3 k = obj.transform.forward; //�L�����N�^�[�̐��ʕ����̃x�N�g�����擾
        k = k * -1; //-1���|���ăL�����N�^�[�̐^�������̃x�N�g���ɂ���
        k = k.normalized * distance;//�x�N�g���̒�����kyori�ɂ���
        cameraTrans.position = targetPos + k; //�J�����̈ʒu�����肷��

        //�J�����𒍎��_�֌�����
        cameraTrans.LookAt(targetPos);

        //�J�[�\����\��
        Cursor.visible = false;
        //�J�[�\���̃��b�N
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //�L�����N�^�[���ړ����Ă�����
        Vector3 tpos = ExportTargetPos(obj);
        if (tpos != targetPos)
        {
            //�ړ������擾
            Vector3 sa = targetPos - tpos;

            //�J�����̈ʒu����������������
            cameraTrans.position -= sa;

            //�J�����̒����_���X�V
            targetPos = tpos;
        }

        //�}�E�X���͂��擾
        mouseInputX = Input.GetAxis("Mouse X"); //X����
        mouseInputY = Input.GetAxis("Mouse Y"); //Y����

        //X�����ɃJ�������ړ�������
        cameraTrans.RotateAround(targetPos, Vector3.up, mouseInputX * sensitivity);

        //Y�����ɃJ�������ړ�������
        Vector3 oldPos = cameraTrans.position;
        Quaternion oldRot = cameraTrans.rotation;

        mouseInputY *= -1;

        cameraTrans.RotateAround(targetPos, cameraTrans.right, mouseInputY * sensitivity);
        float camAngle = Mathf.Abs(Vector3.Angle(Vector3.up, targetPos - cameraTrans.position)); //�J�����̊p�x�����߂�
        if (camAngle < 45 || camAngle > 135) //�J�����̊p�x�����͈͊O�Ȃ瓮�����Ȃ�
        {
            cameraTrans.position = oldPos;
            cameraTrans.rotation = oldRot;
        }

        //�J������Z�������ɉ�]���Ȃ��悤�ɂ���
        cameraTrans.eulerAngles = new Vector3(cameraTrans.eulerAngles.x, cameraTrans.eulerAngles.y, 0.0f);
    }


    //�J�����̒����_���擾����
    Vector3 ExportTargetPos(GameObject obj)
    {
        Vector3 res = obj.transform.position; //�v���C���[�̈ʒu

        res += obj.transform.right * lookPoint.x; //�����_�����iX�����j
        res += obj.transform.up * lookPoint.y; //�����_�����iY�����j

        return res; //�v�Z���ʂ�Ԃ�
    }
}
