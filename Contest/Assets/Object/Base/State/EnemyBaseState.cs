using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyBaseState<T> : BaseCharacterState<T> where T : EnemyBaseState<T>
{
    [Header("Player�̍U���^�O��")]
    [SerializeField] protected string playerAttackTag = "PlayerAttack";
    [Header("Player�̃J�E���^�[�^�O")]
    [SerializeField] protected string playerCounterTag = "CounterAttack";
    [Header("Player�̓�����^�O")]
    [SerializeField] protected string playerThrowTag = "ThrowAttack";

    [Header("Player�ɓ|���ꂽ�Ƃ��ɓn������")]
    [SerializeField] protected BaseAttackData dropWeapon;
    [Header("�|�������ɕ����n���v���C���[�̖��O")]
    [SerializeField] protected GameObject player;
    [Header("�G���Ǘ�����}�l�[�W���[�̖��O")]
    [SerializeField] protected GameObject enemyManagerObject;

    [Header("�f�o�b�O�p�@�G�̑��x(0.01�`1.00)")]
    [SerializeField] protected float enemySpeed = 1.0f;
    [Header("���ݎ���")]
    [SerializeField] protected int flinchFreams = 10;

    [Header("������ԃA�j���[�V����")]
    [SerializeField] protected AnimationClip enemyStandingAnimation = null;
    [Header("���S�A�j���[�V����")]
    [SerializeField] protected AnimationClip enemyDeadAnimation = null;
    [Header("���݃A�j���[�V����")]
    [SerializeField] protected AnimationClip enemyFlinchAnimation = null;
    [Header("����A�j���[�V����")]
    [SerializeField] protected AnimationClip enemyDashAnimation = null;

    [Header("�G�̑���ړ����x")]
    [SerializeField] protected float dashSpeed = 4.0f;

    [Header("�U�����鋗��")]
    [SerializeField] protected float attackDistance = 1.0f;

    [Header("����p")]
    [SerializeField] protected float fov;
    [Header("����̒���")]
    [SerializeField] protected float visionLength;
    [Header("�U�������W")]
    [SerializeField] protected float attackRange;
    [Header("�ړ����x")]
    [SerializeField] protected float moveSpeed;

    // �v���C���[�𔭌��������̃t���O
    protected bool foundTargetFlg;

    // �Փ˂����I�u�W�F�N�g��ۑ����郊�X�g
    [HideInInspector] protected List<Collider> collidedObjects = new List<Collider>();
    // Enemy�̃E�F�|���}�l�[�W���[
    [HideInInspector] protected WeponManager enemyWeponManager;
    // Player��State
    [HideInInspector] protected PlayerState playerState;
    // Enemy�}�l�[�W���[
    [HideInInspector] protected EnemyManager enemyManager;
    // Enemy��HP�}�l�[�W���[ 
    protected HPManager hpManager;
    // Enemy�̃��W�b�h�{�f�B�[
    protected Rigidbody enemyRigidbody;
    // Enemy�̃A�j���[�V�������擾����
    protected Animator enemyAnimator;

    protected Transform playerTransform;

    // ���݂̃R���{��
    protected int enemyConbo = 0;
    // ���ݎg���Ă��镐��̃i���o�[
    protected int weponNumber = 0;
    // �J�E���^�[�U���œ|�ꂽ���̃`�F�b�N
    protected bool hitCounter = false;
    protected bool damagerFlag = false;
    protected int flinchCnt = 0;
    protected bool attackFlag = false;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // Enemy�̃����_���[
    [HideInInspector] public Renderer enemyRenderer;
#endif

    // Start is called before the first frame update
    public void EnemyStart()
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



#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        enemyRenderer = this.gameObject.GetComponent<Renderer>();

        if (enemyWeponManager == null)
        {
            Debug.Log("EnemyState : WeponManager��������܂���");
            return;
        }
#endif
    }

    /*
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
    */

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
        if (speed >= 0.00f && speed <= 1.0f)
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


    // Enemy�p�̃`�F���WState�@�I�[�o�[���C�h
    public override void ChangeState(StateClass<T> newState)
    {
        currentState.Exit((T)this);
        currentState = null;
        currentState = newState;
        currentState.Enter((T)this);
    }


    // �Z�b�^�[
    public void SetEnemyCombo(int combo) { enemyConbo = combo; }
    public void SetEnemyFlinchCnt(int cnt) { flinchCnt = cnt; }
    public void SetEnemyAttackFlag(bool flag) { attackFlag = flag; }
    public void SetFoundTargetFlg(bool _foundTargetFlg) { foundTargetFlg = _foundTargetFlg; }

    // �Q�b�^�[
    public List<Collider> GetEnemyCollidedObjects() { return collidedObjects; }
    public WeponManager GetEnemyWeponManager() { return enemyWeponManager; }
    public int GetEnemyConbo() { return enemyConbo; }
    public int GetEnemyWeponNumber() { return weponNumber; }
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
    public Renderer GetEnemyRenderer() { return enemyRenderer; }
#endif
}
