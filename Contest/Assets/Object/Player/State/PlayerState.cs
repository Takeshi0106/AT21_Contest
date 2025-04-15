using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =====================================
// �v���C���[�̏�Ԃ��Ǘ�����
// =====================================

// Rigidbody�R���|�[�l���g���K�{
[RequireComponent(typeof(Rigidbody))]

// �G�f�B�^���s���Ɏ��s�����
[RequireComponent(typeof(Renderer))]

#if UNITY_EDITOR

#endif

public class PlayerState : BaseState<PlayerState>
{
    // �ڐG�����I�u�W�F�N�g�̏�������
    public struct CollidedInfo
    {
        public Collider collider;
        public MultiTag multiTag;

        public CollidedInfo(Collider collider, MultiTag multiTag)
        {
            this.collider = collider;
            this.multiTag = multiTag;
        }
    }

    // �C���X�y�N�^�[�r���[����ύX�ł���
    [Header("�J�����I�u�W�F�N�g��")]
    [SerializeField] private string cameraName = "Main Camera"; //�J�����I�u�W�F�N�g��
    [Header("�v���C���[�̕����ړ����x")]
    [SerializeField] private float walkSpeed = 2.0f; //�ړ����x
    [Header("�v���C���[�̕����ړ����x")]
    [SerializeField] private float dashSpeed = 4.0f; //�ړ����x
    [Header("�v���C���[�̃J�E���^�[�U���͈̓I�u�W�F�N�g")]
    [SerializeField] private GameObject playerCounterObject;
    [Header("�J�E���^�[�\�ȍU���̃^�O")]
    [SerializeField] private string counterPossibleAttack = "EnemyAttack";

    // �ڐG���̏�������z��iCollider + MultiTag�j
    private List<CollidedInfo> collidedInfos = new List<CollidedInfo>();

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
    // Player�̃A�j���[�^�[
    [HideInInspector] private Animator playerAnimator;
    

    // ���݂̃R���{��
    private int playerConbo = 0;
    // ���ݎg���Ă��镐��̃i���o�[
    private int weponNumber = 0;
    // ���͂��X�^�b�N����
    bool nextAttackReserved = false;

    // �G�f�B�^���s���Ɏ��s�����
    // Player�̃����_���[
    [HideInInspector] public Renderer playerRenderer;

#if UNITY_EDITOR

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
        // �A�j���[�^�[
        playerAnimator = this.gameObject.GetComponent<Animator>();

        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        playerRenderer = this.gameObject.GetComponent<Renderer>();


#if UNITY_EDITOR


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
        if (playerAnimator == null)
        {
            Debug.Log("PlayerState : PlayerAnimator��������܂���");
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
    void OnTriggerEnter(Collider other)
    {
        // �z��̒��ɓ������̂����邩�̃`�F�b�N
        if (!collidedInfos.Any(info => info.collider == other))
        {
            // MulltiTag���擾����
            MultiTag multiTag = other.GetComponent<MultiTag>();
            // �z��ɒǉ�
            collidedInfos.Add(new CollidedInfo(other, multiTag));
        }

#if UNITY_EDITOR
        Debug.Log("Trigger�ɓ������� : " + other.gameObject.name);
#endif
    }



    // �v���C���[���G�Ɨ��ꂽ���̏���
    void OnTriggerExit(Collider other)
    {
        // �z�񂩂瓯������T��
        collidedInfos.RemoveAll(info => info.collider == other);

#if UNITY_EDITOR
        Debug.Log("Trigger���痣�ꂽ : " + other.gameObject.name);
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
    public List<CollidedInfo> GetPlayerCollidedInfos() { return collidedInfos; }
    public Transform GetCameraTransform()            { return cameraTransform; }
    public Rigidbody GetPlayerRigidbody()            { return playerRigidbody; }
    public Collider GetPlayerCollider()              { return playerCollider; }
    public Transform GetPlayerTransform()            { return playerTransform; }
    public CounterManager GetPlayerCounterManager()  { return playerCounterManager; }
    public WeponManager GetPlayerWeponManager()      { return playerWeponManager; }
    public int GetPlayerConbo()                      { return playerConbo; }
    public int GetPlayerWeponNumber()                { return weponNumber; }
    public Animator GetPlayerAnimator() { return playerAnimator; }
    public GameObject GetPlayerCounterObject() { return playerCounterObject; }
    public string GetPlayerCounterPossibleAttack() { return counterPossibleAttack; }
    public bool GetPlayerNextAttackReseved() { return nextAttackReserved; }
    public void SetPlayerNextAttackReseved(bool next) { nextAttackReserved = next; }
#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    public Renderer GetPlayerRenderer()              { return playerRenderer; }
#endif
}

