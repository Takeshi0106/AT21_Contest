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
    // Player�̃E�F�|���}�l�[�W���[
    [HideInInspector] private WeponManager playerWeponManager;

    // ���݂̃R���{��
    private int playerConbo = 0;
    // ���ݎg���Ă��镐��̃i���o�[
    private int weponNumber = 0;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // Player�̃����_���[
    [HideInInspector] public Renderer playerRenderer;
#endif

    // Start is called before the first frame update
    void Start()
    {
        // ��Ԃ��Z�b�g
        currentState = PlayerStandingState.Instance;

        // ��Ԃ̊J�n����
        currentState.Enter(this);

        //�@�J�����I�u�W�F�N�g����
        cameraTransform = GameObject.Find(cameraName).transform;
        // Player���W�b�h�{�f�B�[
        playerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        // Player�R���C�_�[
        playerCollider = this.gameObject.GetComponent<Collider>();
        // Player�g�����X�t�H�[��
        playerTransform = this.gameObject.GetComponent<Transform>();
        // �J�E���^�[�}�l�[�W���[
        playerCounterManager = this.gameObject.GetComponent<CounterManager>();
        // �E�F�|���}�l�[�W���[
        playerWeponManager = this.gameObject.GetComponent<WeponManager>();

#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        playerRenderer = this.gameObject.GetComponent<Renderer>();


        // �����o���Ă��Ȃ��Ƃ��G���[���o��
        if (cameraTransform == null)
        {
            Debug.LogError("PlayerState : �J�����I�u�W�F�N�g��������܂���");
            return;
        }
        if (playerRigidbody == null)
        {
            Debug.LogError("PlayerState : Rigidbody��������܂���");
            return;
        }
        if (playerCollider == null)
        {
            Debug.LogError("PlayerState : Collider��������܂���");
            return;
        }
        if (playerTransform == null)
        {
            Debug.LogError("PlayerState : Transform��������܂���");
            return;
        }
        if (playerCounterManager == null)
        {
            Debug.LogError("PlayerState : CounterManager��������܂���");
            return;
        }
        if (playerRigidbody == null)
        {
            Debug.LogError("PlayerState : Renderer��������܂���");
            return;
        }
        if(playerWeponManager==null)
        {
            Debug.Log("PlayerState : WeponManager��������܂���");
            return;
        }
#endif
    }



    void Update()
    {
        // ��Ԃ��X�V����
        StateUpdate();
        // �J�E���^�[�����N�������鏈��
        playerCounterManager.GaugeDecay();
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

    // �Z�b�^�[
    public void SetPlayerCombo(int value)
    {
        playerConbo = value;
    }

    // �Q�b�^�[
    public float GetWalkSpeed()                      { return walkSpeed; }
    public float GetDashSpeed()                      { return dashSpeed; }
    public List<Collider> GetPlayerCollidedObjects() { return collidedObjects; }
    public Transform GetCameraTransform()            { return cameraTransform; }
    public Rigidbody GetPlayerRigidbody()            { return playerRigidbody; }
    public Collider GetPlayerCollider()              { return playerCollider; }
    public Transform GetPlayerTransform()            { return playerTransform; }
    public CounterManager GetPlayerCounterManager()  { return playerCounterManager; }
    public WeponManager GetPlayerWeponManager()      { return playerWeponManager; }
    public int GetPlayerConbo()                      { return playerConbo; }
    public int GetPlayerWeponNumber()                { return weponNumber; }
#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    public Renderer GetPlayerRenderer()              { return playerRenderer; }
#endif
}

