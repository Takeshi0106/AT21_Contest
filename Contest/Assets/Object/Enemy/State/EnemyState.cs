using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =====================================
// �G�l�~�[�̏��
// =====================================

public class EnemyState : BaseState<EnemyState>
{
    [Header("Player�̍U���^�O��")]
    [SerializeField] private string playerAttackTag = "PlayerAttack";
    [Header("Player�̃J�E���^�[�^�O")]
    [SerializeField] private string playerCounterTag = "CounterAttack";
    [Header("Player�ɓ|���ꂽ�Ƃ��ɓn������")]
    [SerializeField] private BaseAttackData dropWeapon;
    [Header("�|�������ɕ����n���v���C���[")]
    [SerializeField] private GameObject player;


    // �Փ˂����I�u�W�F�N�g��ۑ����郊�X�g
    [HideInInspector] private List<Collider> collidedObjects = new List<Collider>();
    // Enemy�̃E�F�|���}�l�[�W���[
    [HideInInspector] private WeponManager enemyWeponManager;
    // Player��State
    [HideInInspector] private PlayerState playerState;

    // ���݂̃R���{��
    private int enemyConbo = 0;
    // ���ݎg���Ă��镐��̃i���o�[
    private int weponNumber = 0;

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

        // HP�}�l�[�W���[��Die�֐���n��
        hpManager.onDeath.AddListener(Die);

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



    public void HandleDamage(string getAttackTags, string counterAttackTags)
    {
        foreach (var info in collidedInfos)
        {
            // ���łɃ_���[�W�����ς݂̏ꍇ�X�L�b�v
            if (damagedColliders.Contains(info.collider))
                continue;
            // �^�O���Ȃ��ꍇ�X�L�b�v
            if (info.multiTag == null)
                continue;

            // PlayerAttack�����邩�̔���
            if (info.multiTag.HasTag(getAttackTags))
            {
                // ��{�_���[�W������
                float baseDamage = 0;
                // �U���̓A�b�v�{�����擾����
                float multiplier = 0;
                // �ŏI�_���[�W���v�Z����
                float finalDamage = 0;

                // �J�E���^�[�^�O�����邩���ׂ�
                bool isCounterAttack = info.multiTag.HasTag(counterAttackTags);
                
                // ��x�_���[�W���������R���C�_�[��ۑ�����
                damagedColliders.Add(info.collider);

                // �J�E���^�[����
                if (isCounterAttack)
                {
                    // �J�E���^�[�̍U���͂��擾����
                    baseDamage = playerState.GetPlayerCounterManager().GetCounterDamage();
                    // Player�̍U���̓A�b�v�{�����擾����
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                }
                // �ʏ�U������
                else
                {
                    // Player�̍U���͂��擾����
                    baseDamage = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber())
                        .GetDamage(playerState.GetPlayerConbo());
                    // Player�̍U���̓A�b�v�{�����擾����
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                }

                // �_���[�W���v�Z����
                finalDamage = baseDamage * multiplier;
                // �_���[�W����
                hpManager.TakeDamage(finalDamage);
                // ���O���o�͂���
                Debug.Log("HP " + hpManager.GetCurrentHP());

                break;
            }
        }

        CleanupInvalidDamageColliders(getAttackTags, counterAttackTags);
    }




    // �U���^�O�܂��̓J�E���^�[�^�O�����ɖ߂�܂�
    public void CleanupInvalidDamageColliders(string getAttackTags, string counterAttackTags)
    {
        damagedColliders.RemoveWhere(collider =>
        {
            var tag = collidedInfos.FirstOrDefault(info => info.collider == collider).multiTag;
            return tag == null || (!tag.HasTag(getAttackTags) && !tag.HasTag(counterAttackTags));
        });
    }



    private void Die()
    {
        Debug.Log($"{gameObject.name} �����S���܂���");

        if (player != null && dropWeapon != null)
        {
            playerState.GetPlayerWeponManager().AddWeapon(dropWeapon);
        }

        gameObject.SetActive(false);
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
