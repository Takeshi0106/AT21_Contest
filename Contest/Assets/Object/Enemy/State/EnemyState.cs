using System.Collections;
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

    // ���݂̃R���{��
    private int enemyConbo = 0;
    // ���ݎg���Ă��镐��̃i���o�[
    private int weponNumber = 0;
    // �J�E���^�[�U���œ|�ꂽ���̃`�F�b�N
    private bool hitCounter = false;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // Enemy�̃����_���[
    [HideInInspector] public Renderer enemyRenderer;
#endif

    // Start is called before the first frame update
    void Start()
    {
        // ��Ԃ��Z�b�g
        currentState = EnemyStandingState.Instance;

        // ��Ԃ̊J�n����
        currentState.Enter(this);

        // �E�F�|���}�l�[�W���[
        enemyWeponManager = this.gameObject.GetComponent<WeponManager>();
        // hpManager
        hpManager = this.gameObject.GetComponent<HPManager>();
        // PlayerState
        playerState = player.GetComponent<PlayerState>();
        // �G�l�~�[�}�l�[�W���[
        enemyManager = enemyManagerObject.GetComponent<EnemyManager>();

        // HP�}�l�[�W���[��Die�֐���n��
        hpManager.onDeath.AddListener(Die);
        // �G�l�~�[�I�u�W�F�N�g�Ɏ�����n��
        enemyManager.RegisterEnemy(this);


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



    void Update()
    {
        StateUpdate();
    }



    // �_���[�W�����i�ʏ�U���{�J�E���^�[�U���Ή��j
    public void HandleDamage()
    {
        // �ۑ������R���C�_�[�̃^�O�����ɖ߂�̃`�F�b�N
        CleanupInvalidDamageColliders();

        foreach (var info in collidedInfos)
        {
            // ���łɃ_���[�W�����ς�,�^�O�R���|�[�l���g��null�Ȃ�X�L�b�v
            if (info.multiTag == null || damagedColliders.Contains(info.collider)) { continue; }

            // �v���C���[�̍U���^�O�����邩�𒲂ׂ�
            if (info.multiTag.HasTag(playerAttackTag))
            {
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
        // EnemyManager�Ɏ������|�ꂽ���Ƃ�m�点��
        enemyManager.UnregisterEnemy(this);
        // �U���^�O�����ɖ߂�
        enemyWeponManager.DisableAllWeaponAttacks();

        if (playerState != null && dropWeapon != null && hitCounter)
        {
            // Player�ɕ����n��
            playerState.GetPlayerWeponManager().AddWeapon(dropWeapon);
        }

        // �������A�N�e�B�u�ɂ���
        gameObject.SetActive(false);

#if UNITY_EDITOR
        Debug.Log($"{gameObject.name} �����S���܂���");
#endif

    }



    // �Z�b�^�[
    public void SetEnemyCombo(int value) { enemyConbo = value; }

    // �Q�b�^�[
    public  List<Collider>  GetEnemyCollidedObjects()  { return collidedObjects; }
    public  WeponManager    GetEnemyWeponManager()     { return enemyWeponManager; }
    public  int             GetEnemyConbo()            { return enemyConbo; }
    public  int             GetEnemyWeponNumber()      { return weponNumber; }
    public BaseAttackData GetDropWeapon() { return dropWeapon; }
    public string GetEnemyPlayerAttackTag() { return playerAttackTag; }
    public string GetEnemyPlayerCounterAttackTag() { return playerCounterTag; }
#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    public  Renderer        GetEnemyRenderer()         { return enemyRenderer; }
#endif
}
