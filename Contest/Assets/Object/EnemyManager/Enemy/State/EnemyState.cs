using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =====================================
// �G�l�~�[�̏��
// =====================================

public class EnemyState : BaseCharacterState<EnemyState>
{
    [Header("Player�̍U���^�O��")]
    [SerializeField] private string playerAttackTag = "PlayerAttack";
    [Header("Player�̃J�E���^�[�^�O")]
    [SerializeField] private string playerCounterTag = "CounterAttack";
    [Header("Player�̓�����^�O")]
    [SerializeField] private string playerThrowTag = "ThrowAttack";

    [Header("Player�ɓ|���ꂽ�Ƃ��ɓn������")]
    [SerializeField] private BaseAttackData dropWeapon;
    [Header("�|�������ɕ����n���v���C���[�̖��O")]
    [SerializeField] private GameObject player;
    [Header("�G���Ǘ�����}�l�[�W���[�̖��O")]
    [SerializeField] private GameObject enemyManagerObject;

    [Header("�f�o�b�O�p�@�G�̑��x(0.01�`1.00)")]
    [SerializeField] private float enemySpeed = 1.0f;
    [Header("���ݎ���")]
    [SerializeField] private int flinchFreams = 10;

    [Header("������ԃA�j���[�V����")]
    [SerializeField] private AnimationClip enemyStandingAnimation = null;
    [Header("���S�A�j���[�V����")]
    [SerializeField] private AnimationClip enemyDeadAnimation = null;
    [Header("���݃A�j���[�V����")]
    [SerializeField] private AnimationClip enemyFlinchAnimation = null;
    [Header("����A�j���[�V����")]
    [SerializeField] private AnimationClip enemyDashAnimation = null;

    [Header("�G�̑���ړ����x")]
    [SerializeField] private float dashSpeed = 4.0f; //�ړ����x

    [Header("�U�����鋗��")]
    [SerializeField] private float attackDistance = 1.0f; //�ړ����x

    [Header("����p")]
    [SerializeField] private float fov;
    [Header("����̒���")]
    [SerializeField] private float visionLength;
    [Header("�U�������W")]
    [SerializeField] private float attackRange;
    [Header("�ړ����x")]
    [SerializeField] private float moveSpeed;

    //�v���C���[�𔭌��������̃t���O
    private bool foundTargetFlg;


    // �Փ˂����I�u�W�F�N�g��ۑ����郊�X�g
    [HideInInspector] private List<Collider> collidedObjects = new List<Collider>();
    // Enemy�̃E�F�|���}�l�[�W���[
    [HideInInspector] private WeponManager enemyWeponManager;
    // Player��State
    [HideInInspector] private PlayerState playerState;
    // Player��State
    [HideInInspector] private EnemyManager enemyManager;
    // Enemy��HP�}�l�[�W���[ 
    private HPManager hpManager;
    // Enemy�̃��W�b�h�{�f�B�[
    private Rigidbody enemyRigidbody;
    // Enemy�̃A�j���[�V�������擾����
    private Animator enemyAnimator;

    private Transform playerTransform;


    // ���݂̃R���{��
    private int enemyConbo = 0;
    // ���ݎg���Ă��镐��̃i���o�[
    private int weponNumber = 0;
    // �J�E���^�[�U���œ|�ꂽ���̃`�F�b�N
    private bool hitCounter = false;
    //
    private bool damagerFlag = false;
    //
    private int flinchCnt = 0;

    private bool attackFlag = false;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // Enemy�̃����_���[
    [HideInInspector] public Renderer enemyRenderer;
#endif



    // ����������
    void Start()
    {
        // �E�F�|���}�l�[�W���[
        enemyWeponManager = this.gameObject.GetComponent<WeponManager>();
        // hpManager
        hpManager = this.gameObject.GetComponent<HPManager>();
        // PlayerState
        playerState = player.GetComponent<PlayerState>();
        // �G�l�~�[�}�l�[�W���[
        enemyManager = enemyManagerObject.GetComponent<EnemyManager>();
        // ���W�b�h�{�f�B�[���擾
        enemyRigidbody = this.gameObject.GetComponent<Rigidbody>();
        // �A�j���[�^�[���擾
        enemyAnimator = this.gameObject.GetComponent<Animator>();

        playerTransform = player.transform;

        // HP�}�l�[�W���[��Die�֐���n��
        hpManager.SetOnDeathEvent(Die);
        
        // �G�l�~�[�}�l�W���[�ɃX���[�C�x���g���Z�b�g
        enemyManager.AddOnEnemySlow(SetEnemySpead);

        // ��Ԃ��Z�b�g
        currentState = new EnemyStandingState();
        // ��Ԃ̊J�n����
        currentState.Enter(this);

#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        enemyRenderer = this.gameObject.GetComponent<Renderer>();

        if (enemyWeponManager == null)
        {
            Debug.Log("EnemyState : WeponManager��������܂���");
            return;
        }
#endif

        // �G�l�~�[�I�u�W�F�N�g�Ɏ�����n��
        enemyManager.RegisterEnemy(this);

        SetEnemySpead(0.8f);

        this.gameObject.SetActive(false);
    }


    // �X�V����
    void Update()
    {
        StateUpdate();
    }


    // �_���[�W�����i�ʏ�U���{�J�E���^�[�U���Ή��j
    public void HandleDamage()
    {
        damagerFlag = false;
        hitCounter = false;

        // �ۑ������R���C�_�[�̃^�O�����ɖ߂�̃`�F�b�N
        CleanupInvalidDamageColliders();

        foreach (var info in collidedInfos)
        {
            // ���łɃ_���[�W�����ς�,�^�O�R���|�[�l���g��null�Ȃ�X�L�b�v
            if (info.multiTag == null || damagedColliders.Contains(info.collider)) { continue; }

            // �v���C���[�̍U���^�O�����邩�𒲂ׂ�
            if (info.multiTag.HasTag(playerAttackTag))
            {
                damagerFlag = true;

                // �v���C���[�̊�{�_���[�W������ϐ�
                float baseDamage = 0.0f;
                // �v���C���[�̍U���A�b�v�{��������ϐ�
                float multiplier = 1.0f;
                // �ŏI�_���[�W������ϐ�
                float finalDamage = 0f;

                // �J�E���^�[�^�O�����邩���ׂ�
                bool isCounterAttack = info.multiTag.HasTag(playerCounterTag);
                // ������ꂽ�{�u�W�F�N�g���𒲂ׂ�
                bool isThrowAttack = info.multiTag.HasTag(playerThrowTag);

                // ��x�_���[�W���������R���C�_�[��ۑ�
                damagedColliders.Add(info.collider); 

                // �J�E���^�[�̏ꍇ�̏���
                if (isCounterAttack)
                {
                    baseDamage = playerState.GetPlayerCounterManager().GetCounterDamage();
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                    hitCounter = true;
                }
                // ������U���̏ꍇ�̏���
                else if(isThrowAttack)
                {
                    var weaponManager = playerState.GetPlayerWeponManager();
                    var weaponData = weaponManager.GetWeaponData(playerState.GetPlayerWeponNumber());

                    baseDamage = weaponData.GetThrowDamage();
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                }
                // �ʏ킱�������̏ꍇ�̏���
                else
                {
                    var weaponManager = playerState.GetPlayerWeponManager();
                    var weaponData = weaponManager.GetWeaponData(playerState.GetPlayerWeponNumber());

                    baseDamage = weaponData.GetDamage(playerState.GetPlayerConbo());
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                }

                // �_���[�W�v�Z
                finalDamage = baseDamage * multiplier;

#if UNITY_EDITOR
                Debug.Log($"Enemy�̃_���[�W: {finalDamage}�i{(isCounterAttack ? "�J�E���^�[" : "�ʏ�")}�j");
                Debug.Log(Time.frameCount + ": Counter Hit!");
#endif

                // �_���[�W����������
                hpManager.TakeDamage(finalDamage);



                break; // ��x�q�b�g�ŏ����I��
            }
        }
    }




    // �U���^�O�܂��̓J�E���^�[�^�O���O�ꂽ��폜
    public void CleanupInvalidDamageColliders()
    {
        // �^�O���U���^�O�ȊO�̕����𒲂ׂ�
        damagedColliders.RemoveWhere(collider =>
        {
            var tag = collidedInfos.FirstOrDefault(info => info.collider == collider).multiTag;
            return tag == null || (!tag.HasTag(playerAttackTag));
        });

        // �R���C�_�[����A�N�e�B�u���𒲂ׂ�
        damagedColliders.RemoveWhere(collider =>
        collider == null || !collider.gameObject.activeInHierarchy || !collider.enabled);

        // �����ȃR���C�_�[���A�N�e�B�u�����ꂽ���̂����O
        collidedInfos.RemoveAll(info =>
            info.collider == null ||
            !info.collider.gameObject.activeInHierarchy ||
            !info.collider.enabled);
    }



    private void Die()
    {
        enemyManager.RemoveOnEnemySlow(SetEnemySpead);
        // EnemyManager�Ɏ������|�ꂽ���Ƃ�m�点��
        enemyManager.UnregisterEnemy(this);

        if (playerState != null && dropWeapon != null && hitCounter)
        {
            // Player�ɕ����n��
            playerState.GetPlayerWeponManager().AddWeapon(dropWeapon);
        }

#if UNITY_EDITOR
        Debug.Log($"{gameObject.name} �����S���܂���");
#endif

        enemyRigidbody.useGravity = false; // �d�͂�OFF�ɂ���
        this.GetComponent<Collider>().enabled = false; // �R���C�_�[�𖳌��ɂ���
        
        ChangeState(new EnemyDeadState()); // Dead��ԂɕύX
    }

    public void Target()
    {
        Vector3 direction = player.transform.position - transform.position;

        // Y�����̉�]�����ɂ������ꍇ�i�㉺�͖����j
        direction.y = 0;

        // ��������������0�x�N�g���łȂ����Ƃ��m�F
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }


    // �X�s�[�h���Z�b�g���鏈��
    public void SetEnemySpead(float speed)
    {
        if (speed >= 0.01f && speed <= 1.0f)
        {
            // �t���[���̐i�ޏ������X�V
            enemySpeed = speed;
            // �A�j���[�V�����̑��x��ύX
            if (enemyAnimator != null)
            {
                enemyAnimator.speed = speed;
            }
        }
        else
        {
            Debug.Log("�X�s�[�h���Z�b�g�ł��܂���ł����B");
        }
    }


    // �Z�b�^�[
    public void SetEnemyCombo(int combo) { enemyConbo = combo; }
    public void SetEnemyFlinchCnt(int cnt) { flinchCnt = cnt; }
    public void SetEnemyAttackFlag(bool flag) { attackFlag = flag; }
    public void SetFoundTargetFlg(bool _foundTargetFlg) { foundTargetFlg = _foundTargetFlg; }

    // �Q�b�^�[
    public  List<Collider>  GetEnemyCollidedObjects()  { return collidedObjects; }
    public  WeponManager    GetEnemyWeponManager()     { return enemyWeponManager; }
    public  int             GetEnemyConbo()            { return enemyConbo; }
    public  int             GetEnemyWeponNumber()      { return weponNumber; }
    public BaseAttackData GetDropWeapon() { return dropWeapon; }
    public string GetEnemyPlayerAttackTag() { return playerAttackTag; }
    public string GetEnemyPlayerCounterAttackTag() { return playerCounterTag; }
    public float GetEnemySpeed() { return enemySpeed; }
    public int GetEnemyFlinchFreams() { return flinchFreams; }
    public Animator GetEnemyAnimator() { return enemyAnimator; }
    public AnimationClip GetEnemyStandingAnimation() { return enemyStandingAnimation; }
    public AnimationClip GetEnemyDeadAnimation() { return enemyDeadAnimation; }
    public AnimationClip GetEnemyFlinchAnimation() { return enemyFlinchAnimation; }
    public bool GetEnemyDamageFlag() { return damagerFlag; }
    public bool GetEnemyHitCounterFlag() { return hitCounter; }
    public int GetEnemyFlinchCnt() { return flinchCnt; }
    public PlayerState GetPlayerState() { return playerState; }
    public float GetEnemyDashSpeed() { return dashSpeed; }
    public Rigidbody GetEnemyRigidbody() { return enemyRigidbody; }
    public bool GetEnemyAttackFlag() { return attackFlag; }
    public Transform GetPlayerTransform() { return playerTransform; }
    public AnimationClip GetEnemyDashAnimation() { return enemyDashAnimation; }
    public float GetDistanceAttack() { return attackDistance; }

    public float GetEnemyFov() { return fov; }
    public float GetEnemyVisionLength() { return visionLength; }
    public float GetEnemyAttackRange() { return attackRange; }
    public float GetEnemyMoveSpeed() { return moveSpeed; }
    public EnemyManager GetEnemyManager() { return enemyManager; }
    public bool GetFoundTargetFlg() { return foundTargetFlg; }
    public GameObject GetTargetObject() { return player; }

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    public  Renderer        GetEnemyRenderer()         { return enemyRenderer; }
#endif
}
