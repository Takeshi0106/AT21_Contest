using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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
    [Header("�J�E���^�[�������̍U���̑傫��")]
    [SerializeField] private float counterRange = 5.0f;
    [Header("�G�̍U���^�O��")]
    [SerializeField] private string enemyAttackTag = "EnemyAttack";
    [Header("�v���C���[�̃J�E���^�[������̖��G���ԁi�J�E���^�[�������̖��G���ԂƂ͕ʁj")]
    [SerializeField] private int invincibleTime = 0;


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
    // �J�E���^�[��AttackController
    [HideInInspector] private AttackController playerCounterAttackController;
    // Player�̏�ԃ}�l�[�W���[
    [HideInInspector] private StatusEffectManager playerStatusEffectManager;


    // ���݂̃R���{��
    private int playerConbo = 0;
    // ���ݎg���Ă��镐��̃i���o�[
    private int weponNumber = 0;

    // ���͂��X�^�b�N����
    RESEVEDSTATE nextReserved = RESEVEDSTATE.NOTHING;

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
        // �J�E���^�[�̍U���R���g���[���[
        playerCounterAttackController = playerCounterObject.GetComponent<AttackController>();
        // HpManager
        hpManager = this.gameObject.GetComponent<HPManager>(); 
        // ��ԊǗ�
        playerStatusEffectManager = this.gameObject.GetComponent<StatusEffectManager>();

        playerCounterObject.SetActive(false);

        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        playerRenderer = this.gameObject.GetComponent<Renderer>();

        // HP�}�l�[�W���[��Die�֐���n��
        hpManager.onDeath.AddListener(Die);

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
        if (playerCounterAttackController == null)
        {
            Debug.Log("PlayerState : PlayerCounterAttackController��������܂���");
            return;
        }
        if (hpManager == null)
        {
            Debug.Log("PlayerState : HPManager��������܂���");
            return;
        }
        if (playerStatusEffectManager == null)
        {
            Debug.Log("PlayerState : playerStatusEffectManager��������܂���");
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



    // �_���[�W����
    public void HandleDamage(string getAttackTags)
    {
        if (!playerStatusEffectManager.Invincible(invincibleTime))
        {
            playerRenderer.material.color = Color.white;

            foreach (var info in collidedInfos)
            {
                // ���łɃ_���[�W�����ς݁A�܂��̓^�O���Ȃ��Ȃ�X�L�b�v
                if (damagedColliders.Contains(info.collider))
                    continue;

                if (info.multiTag != null && info.multiTag.HasTag(getAttackTags))
                {
                    // �_���[�W�����Ȃǂ������ɒǉ�
                    Debug.Log("�_���[�W�Ώۃq�b�g: " + info.collider.gameObject.name);

                    // ��x�_���[�W��^������A���̃R���C�_�[�͋L�^
                    damagedColliders.Add(info.collider);

                    // �e�I�u�W�F�N�g���� EnemyState ���擾
                    var enemyState = info.collider.GetComponentInParent<EnemyState>();

                    if (enemyState != null)
                    {
                        hpManager.TakeDamage(enemyState.GetEnemyWeponManager().GetWeaponData(0).GetDamage(enemyState.GetEnemyConbo()));
                    }

                    // �_���[�W�����Ȃǂ������ɒǉ�
                    Debug.Log("HP " + hpManager.GetCurrentHP());

                    // ����������甲����Ȃ� break�i�����Ȃ� continue�j
                    break;
                }
            }
        }
#if UNITY_EDITOR
        else
        {
            playerRenderer.material.color = Color.yellow;

        }
#endif

        CleanupInvalidDamageColliders(getAttackTags);
    }



    // �U���^�O�����ɖ߂�܂�
    public void CleanupInvalidDamageColliders(string getAttackTags)
    {
        damagedColliders.RemoveWhere(collider =>
        {
            var tag = collidedInfos.FirstOrDefault(info => info.collider == collider).multiTag;
            return tag == null || !tag.HasTag(getAttackTags);
        });
    }



    private void Die()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameObject.SetActive(false);
        SceneManager.LoadScene("ResultScene");
    }



    // �Z�b�^�[
    public void SetPlayerCombo(int value) { playerConbo = value; }
    public void SetPlayerNextReseved(RESEVEDSTATE next) { nextReserved = next; }



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
    public RESEVEDSTATE GetPlayerNextReseved() { return nextReserved; }
    public AttackController GetPlayerCounterAttackController() { return playerCounterAttackController; }
    public float GetPlayerCounterRange() { return counterRange; }
    public HPManager GetPlayerHPManager() { return hpManager; }
    public string GetPlayerEnemyAttackTag() { return enemyAttackTag; }
    public StatusEffectManager GetPlayerStatusEffectManager() {  return playerStatusEffectManager; }
    // �G�f�B�^���s���Ɏ��s�����
    public Renderer GetPlayerRenderer() { return playerRenderer; }

#if UNITY_EDITOR

#endif
}

