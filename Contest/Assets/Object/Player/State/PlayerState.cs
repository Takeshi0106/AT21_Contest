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

public class PlayerState : BaseCharacterState<PlayerState>
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
    [Header("�v���C���[���Ђ�񂾎��̃t���[����")]
    [SerializeField] private int FlinchFreams = 0;
    [Header("�v���C���[��������̂����s�����Ƃ��̃t���[����")]
    [SerializeField] private int ThrowFailedFreams = 0;
    [Header("�v���C���[��������̂����s�����Ƃ��̃A�j���[�V����")]
    [SerializeField] private AnimationClip throwFailedAnimations = null;


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
    // Player��HP�}�l�[�W���[
    private HPManager hpManager;


    // ���݂̃R���{��
    private int playerConbo = 0;
    // ���ݎg���Ă��镐��̃i���o�[
    private int weponNumber = 0;

    // ���͂��X�^�b�N����
    RESEVEDSTATE nextReserved = RESEVEDSTATE.NOTHING;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // Player�̃����_���[
    [HideInInspector] public Renderer playerRenderer;
#endif

    // Start is called before the first frame update
    void Start()
    {
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

        // playerCounterObject.SetActive(false);
        // HP�}�l�[�W���[��Die�֐���n��
        hpManager.onDeath.AddListener(Die);


        // ��Ԃ��Z�b�g
        currentState = PlayerStandingState.Instance;
        // ��Ԃ̊J�n����
        currentState.Enter(this);

#if UNITY_EDITOR

        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        playerRenderer = this.gameObject.GetComponent<Renderer>();

        // �����o���Ă��Ȃ��Ƃ����O���o���i�G���[�ł͂Ȃ����j
        if (cameraTransform == null)
        {
            Debug.Log("PlayerState : �J�����I�u�W�F�N�g��������܂���");
        }
        if (playerRigidbody == null)
        {
            Debug.Log("PlayerState : Rigidbody��������܂���");
        }
        if (playerCollider == null)
        {
            Debug.Log("PlayerState : Collider��������܂���");
        }
        if (playerTransform == null)
        {
            Debug.Log("PlayerState : Transform��������܂���");
        }
        if (playerCounterManager == null)
        {
            Debug.Log("PlayerState : CounterManager��������܂���");
        }
        if (playerRigidbody == null)
        {
            Debug.Log("PlayerState : Renderer��������܂���");
        }
        if (playerWeponManager == null)
        {
            Debug.Log("PlayerState : WeponManager��������܂���");
        }
        if (playerAnimator == null)
        {
            Debug.Log("PlayerState : PlayerAnimator��������܂���");
        }
        if (playerCounterAttackController == null)
        {
            Debug.Log("PlayerState : PlayerCounterAttackController��������܂���");
        }
        if (hpManager == null)
        {
            Debug.Log("PlayerState : HPManager��������܂���");
        }
        if (playerStatusEffectManager == null)
        {
            Debug.Log("PlayerState : playerStatusEffectManager��������܂���");
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
        // �ۑ������R���C�_�[�̃^�O�����ɖ߂�̃`�F�b�N
        CleanupInvalidDamageColliders(getAttackTags);

        // �v���C���[�����G��Ԃ����ׂ�
        if (playerStatusEffectManager.Invincible(invincibleTime))
        {
#if UNITY_EDITOR
            playerRenderer.material.color = Color.yellow;
#endif
            // ���G��ԂȂ�֐����I������
            return;
        }
#if UNITY_EDITOR
        else
        {
            // ���G�I����͐F�����ɖ߂�
            if (playerRenderer.material.color == Color.yellow)
            {
                playerRenderer.material.color = Color.white;
            }
        }
#endif


        // �������Ă���I�u�W�F�N�g�̃^�O�𒲂ׂ�
        foreach (var info in collidedInfos)
        {
            // ���łɃ_���[�W�����ς�,�^�O�R���|�[�l���g��null�Ȃ�X�L�b�v
            if (info.multiTag == null || damagedColliders.Contains(info.collider)) { continue; }

            // �G�̍U���^�O�����邩�̔���
            if (info.multiTag.HasTag(getAttackTags))
            {
#if UNITY_EDITOR
                Debug.Log("�_���[�W�Ώۃq�b�g: " + info.collider.gameObject.name);
#endif

                // �R���C�_�[�͋L�^
                damagedColliders.Add(info.collider);

                // �e�I�u�W�F�N�g���� EnemyState ���擾
                var enemyState = info.collider.GetComponentInParent<EnemyState>();

                if (enemyState != null)
                {
                    // �_���[�W����
                    hpManager.TakeDamage(enemyState.GetEnemyWeponManager().GetWeaponData(0).GetDamage(enemyState.GetEnemyConbo()));
                }

#if UNITY_EDITOR
                // �_���[�W�����Ȃǂ������ɒǉ�
                Debug.Log("HP " + hpManager.GetCurrentHP());
#endif
            }
        }
    }



    // �U���^�O�����ɖ߂�܂�
    public void CleanupInvalidDamageColliders(string getAttackTags)
    {
        // �^�O���U���^�O�ȊO�̕����𒲂ׂ�
        damagedColliders.RemoveWhere(collider =>
        {
            var tag = collidedInfos.FirstOrDefault(info => info.collider == collider).multiTag;
            return tag == null || !tag.HasTag(getAttackTags);
        });

        // �R���C�_�[����A�N�e�B�u���𒲂ׂ�
        damagedColliders.RemoveWhere(collider =>
        collider == null || !collider.gameObject.activeInHierarchy || !collider.enabled);

        // �������Ă���I�u�W�F�N�g����A�N�e�B�u���𒲂ׂ�
        collidedInfos.RemoveAll(info =>
        info.collider == null ||
        !info.collider.gameObject.activeInHierarchy ||
        !info.collider.enabled);
    }



    private void Die()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameObject.SetActive(false);
        SceneManager.LoadScene("ResultScene");
    }



    public void AddDamagedCollider(Collider target)
    {
        if (target != null && !damagedColliders.Contains(target))
        {
            damagedColliders.Add(target);
        }
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
    public HashSet<Collider> GetPlayerDamagedColliders() { return damagedColliders; }
    public int GetPlayerFlinchFreams() { return FlinchFreams; }
    public int GetThrowFailedFreams() { return ThrowFailedFreams; }
    public AnimationClip GetThrowFailedAnimation() { return throwFailedAnimations; }

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    public Renderer GetPlayerRenderer() { return playerRenderer; }
#endif
}

