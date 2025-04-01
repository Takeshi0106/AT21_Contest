using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =====================================
// �v���C���[�̏��
// =====================================

// Rigidbody�R���|�[�l���g���K�{
[RequireComponent(typeof(Rigidbody))]

#if UNITY_EDITOR
// �G�f�B�^���s���Ɏ��s�����
[RequireComponent(typeof(Renderer))]
#endif

public class PlayerState : BaseState<PlayerState>
{
    // �C���X�y�N�^�[�r���[����ύX�ł���
    [Header("�J�����I�u�W�F�N�g��")]
    [SerializeField] private string cameraName = "Main Camera"; //�J�����I�u�W�F�N�g��
    [Header("�v���C���[�̕����ړ����x")]
    [SerializeField] private float walkSpeed = 2.0f; //�ړ����x
    [Header("�v���C���[�̕����ړ����x")]
    [SerializeField] private float dashSpeed = 4.0f; //�ړ����x

    // �Փ˂����I�u�W�F�N�g��ۑ����郊�X�g
    [HideInInspector] private List<Collider> collidedObjects = new List<Collider>();

    // �J�����̃g�����X�t�H�[�� ���̃X�N���v�g�ȊO�ŕύX�ł��Ȃ��悤�ɐݒ�
    [HideInInspector] private Transform cameraTransform;
    // Player�̃��W�b�h�{�f�B
    [HideInInspector] private Rigidbody playerRigidbody;
    // Player�̃R���C�_�[
    [HideInInspector] private Collider playerCollider;
    // Player�̃g�����X�t�H�[��
    [HideInInspector] private Transform playerTransform;
    // Player�̃J�E���^�[�}�l�[�W���[
    [HideInInspector] private CounterManager playerCounterManager;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // Player�̃����_���[
    [HideInInspector] public Renderer playerRenderer;
#endif

    // Start is called before the first frame update
    void Start()
    {
        // ��Ԃ��Z�b�g
        currentState = StandingState.Instance;

        // ��Ԃ̊J�n����
        currentState.Enter(this);

        //�@�J�����I�u�W�F�N�g����
        cameraTransform = GameObject.Find(cameraName).transform;
        if (cameraTransform == null)
        {
            Debug.LogError("PlayerState : �J�����I�u�W�F�N�g��������܂���");
            return;
        }

        // Player���W�b�h�{�f�B�[
        playerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("PlayerState : Rigidbody��������܂���");
            return;
        }

        // Player�R���C�_�[
        playerCollider = this.gameObject.GetComponent<Collider>();
        if (playerCollider == null)
        {
            Debug.LogError("PlayerState : Collider��������܂���");
            return;
        }

        // Player�g�����X�t�H�[��
        playerTransform = this.gameObject.GetComponent<Transform>();
        if (playerTransform == null)
        {
            Debug.LogError("PlayerState : Transform��������܂���");
            return;
        }

        // �J�E���^�[�}�l�[�W���[
        playerCounterManager = this.gameObject.GetComponent<CounterManager>();
        if(playerCounterManager == null)
        {
            Debug.LogError("PlayerState : CounterManager��������܂���");
            return;
        }

#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        playerRenderer = this.gameObject.GetComponent<Renderer>();
        if (playerRigidbody == null)
        {
            Debug.LogError("PlayerState : Renderer��������܂���");
            return;
        }
#endif
    }



    // �v���C���[���G�ɂԂ��������̏���
    void OnCollisionEnter(Collision other)
    {
        Collider collider = other.collider;
        if (!collidedObjects.Contains(other.collider))
        {
            collidedObjects.Add(other.collider);
        }
#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ��s�����
        Debug.Log("Object�ɓ������� : " + other.gameObject.name);
#endif
    }



    // �v���C���[���G�Ɨ��ꂽ���̏���
    void OnCollisionExit(Collision other)
    {
        Collider collider = other.collider;
        if (collidedObjects.Contains(other.collider))
        {
            collidedObjects.Remove(other.collider);
        }
#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ��s�����
        Debug.Log("Object�����ꂽ : " + other.gameObject.name);
#endif
    }



    // �Q�b�^�[
    public float GetWalkSpeed() => walkSpeed;
    public float GetDashSpeed() => dashSpeed;
    public List<Collider> GetCollidedObjects() => collidedObjects;
    public Transform GetCameraTransform() => cameraTransform;
    public Rigidbody GetPlayerRigidbody() => playerRigidbody;
    public Collider GetPlayerCollider() => playerCollider;
    public Transform GetPlayerTransform() => playerTransform;
    public CounterManager GetPlayerCounterManager() => playerCounterManager;
#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    public Renderer GetPlayerRenderer() => playerRenderer;
#endif
}

