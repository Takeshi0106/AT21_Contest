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
    public float distance = 5.0f; //�I�u�W�F�N�g�ƃJ�����̋���

    [Header("�}�E�X���x�ݒ�")]
    public float sensitivity = 3.0f; //�}�E�X���x

    [Header("�J�����̒����_�ݒ�(�I�u�W�F�N�g����̑��Έʒu������)")]
    public Vector2 lookPoint = new Vector2(0.0f, 1.0f); //�I�u�W�F�N�g�̒��S����̃J�����̈ʒu

    // �J����������I�u�W�F�N�g
    GameObject obj;
    // �J�����̃g�����X�t�H�[��
    Transform cameraTrans;
    // �J�����̃x�N�g��(����)
    Vector3 targetPos;

    // �}�E�X�̓���
    float mouseInputX;
    float mouseInputY;

    // Start is called before the first frame update
    void Start()
    {
        // �I�u�W�F�N�g����
        obj = GameObject.Find(objectName);

        // ������Ȃ���΃G���[
        if (obj == null)
        {
            Debug.LogError("TPFCamera �F �v���C���[�I�u�W�F�N�g��������܂���");
            return;
        }

        // �J������transform���
        cameraTrans = this.transform;

        // �I�u�W�F�N�g�̍��W��lookPoint���v�Z�����l��������
        targetPos = ExportTargetPos(obj);

        // �J�����̈ʒu�v�Z   ===================================
        // �I�u�W�F�N�g�̃x�N�g��(����)���擾
        Vector3 objOppositeVector = obj.transform.forward;
        // �t�x�N�g�����v�Z
        objOppositeVector = objOppositeVector * -1;
        //�x�N�g���̒�����distance�ɂ���
        objOppositeVector = objOppositeVector.normalized * distance;

        // �J�����̈ʒu����
        cameraTrans.position = targetPos + objOppositeVector;
        //�J�����̃x�N�g��(����)���v�Z
        cameraTrans.LookAt(targetPos);

        //�J�[�\����\��
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // ESC�L�[�������ꂽ�烍�b�N����������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.visible == false)
            {
                // �J�[�\����\��
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                //�J�[�\����\��
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // �I�u�W�F�N�g�̍��W��lookPoint���v�Z�����l��������
        Vector3 tpos = ExportTargetPos(obj);
        // �L�����N�^�[���ړ����Ă�����
        if (tpos != targetPos)
        {
            // �ړ��ʂ��v�Z   
            Vector3 sa = targetPos - tpos;
            // �J�����̈ʒu���ړ�
            cameraTrans.position -= sa;
            // �J�����̒����_���X�V
            targetPos = tpos;
        }

        // �}�E�X���͂��擾
        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = Input.GetAxis("Mouse Y");

        // X�����ɃJ�������ړ�������
        cameraTrans.RotateAround(targetPos, Vector3.up, mouseInputX * sensitivity);

        // Y�����ɃJ�������ړ�������
        Vector3 oldPos = cameraTrans.position;
        Quaternion oldRot = cameraTrans.rotation;

        // �}�E�X���͂𔽓]������
        mouseInputY *= -1;
        // �J�����̉�]���v�Z
        cameraTrans.RotateAround(targetPos, cameraTrans.right, mouseInputY * sensitivity);
        // �J�����̊p�x���v�Z
        float camAngle = Mathf.Abs(Vector3.Angle(Vector3.up, targetPos - cameraTrans.position));

        //�J�����̊p�x�����͈͊O�Ȃ瓮�����Ȃ�
        if (camAngle < 45 || camAngle > 135)
        {
            cameraTrans.position = oldPos;
            cameraTrans.rotation = oldRot;
        }
        // �J������Z���������Œ肷��
        cameraTrans.eulerAngles = new Vector3(cameraTrans.eulerAngles.x, cameraTrans.eulerAngles.y, 0.0f);
    }


    // �I�u�W�F�N�g����̑��ΓI�ʒu�v�Z����
    Vector3 ExportTargetPos(GameObject obj)
    {
        // �I�u�W�F�N�g�̈ʒu
        Vector3 res = obj.transform.position;

        // X�����̌v�Z
        res += obj.transform.right * lookPoint.x;
        // Y�����̌v�Z
        res += obj.transform.up * lookPoint.y;
        return res;
    }
}
