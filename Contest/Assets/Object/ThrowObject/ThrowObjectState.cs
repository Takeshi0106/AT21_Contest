using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjectState : BaseColliderState<ThrowObjectState>
{
    // �C���X�y�N�^�[�r���[����ύX�ł���
    [Header("������͂�������")]
    [SerializeField] private float forcePower;
    [Header("���������̏d�͖����t���[��")]
    [SerializeField] private int noGravityFreams = 0;


    // �v���C���[�̃g�����X�t�H�[��������ϐ�
    private Transform cameraTransform;
    // �����̃��W�b�h�{�f�B�[���擾���Ă���
    private Rigidbody throwObjectRigidbody;


    // Start is called before the first frame update
    void Start()
    {
        // ���W�b�h�{�f�B�[���擾
        throwObjectRigidbody = this.gameObject.GetComponent<Rigidbody>();


        // ��Ԃ��Z�b�g
        currentState = ThrowObjectThrowState.Instance;
        // ��Ԃ̊J�n����
        currentState.Enter(this);
    }

    // Update is called once per frame
    void Update()
    {
        // ��ԍX�V
        StateUpdate();
    }


    // �Z�b�^�[
    public void SetCameraTransfoem(Transform p_playerTransform) { cameraTransform = p_playerTransform; }

    // �Q�b�^�[
    public Transform GetCameraTransform() { return cameraTransform; }
    public Rigidbody GetThrowObjectRigidbody() { return throwObjectRigidbody; }
    public float GetForcePower() { return forcePower; }
    public int GetNoGravityFreams() { return noGravityFreams; }
}
