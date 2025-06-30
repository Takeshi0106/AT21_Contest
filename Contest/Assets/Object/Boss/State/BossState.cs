using UnityEngine;

// =================================
// BOSS�̏�ԊǗ��N���X
// =================================


public class BossState : EnemyBaseState<BossState>
{
    private BossStatusEffectManager m_BossStatusEffectManager = null; // Boss�̏�ԊǗ��N���X


    private void Start()
    {
        CharacterStart(); // �L�����N�^�[������
        EnemyStart(); // �G�l�~�[�N���X������

        // HP�}�l�[�W���[��Die�֐���n��
        hpManager.SetOnDeathEvent(Die);

        // �G�l�~�[�}�l�W���[�ɃX���[�C�x���g���Z�b�g
        enemyManager.AddOnEnemySlow(SetEnemySpead);
        // 
        m_BossStatusEffectManager = this.gameObject.GetComponent<BossStatusEffectManager>();

        // ��Ԃ��Z�b�g
        currentState = new BossStandingState();
        // ��Ԃ̊J�n����
        currentState.Enter(this);

        attackFlag = true;

#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        enemyRenderer = this.gameObject.GetComponent<Renderer>();

        if (enemyWeponManager == null)
        {
            Debug.Log("EnemyState : WeponManager��������܂���");
            return;
        }
        if (m_BossStatusEffectManager == null)
        {
            Debug.Log("BossState : EffectManager��������܂���");
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

        // HandleDamage();
    }

    // �_���[�W���� Boss�p
    public void HandleDamage()
    {
        damagerFlag = false;
        hitCounter = false;

        // �ۑ������R���C�_�[�̃^�O�����ɖ߂�̃`�F�b�N
        // CleanupInvalidDamageColliders();

        for (int i = 0; i < collidedInfos.Count; i++)
        {
            var info = collidedInfos[i];

            // ���łɃ_���[�W�����ς�,�^�O�R���|�[�l���g��null�Ȃ�X�L�b�v
            if (info.multiTag == null || info.hitFlag) { continue; }

            // �v���C���[�̍U���^�O�����邩�𒲂ׂ�
            if (info.multiTag.HasTag(playerAttackTag))
            {
                damagerFlag = true;

                // �J�E���^�[�^�O�����邩���ׂ�
                bool isCounterAttack = info.multiTag.HasTag(playerCounterTag);
                // ������ꂽ�{�u�W�F�N�g���𒲂ׂ�
                bool isThrowAttack = info.multiTag.HasTag(playerThrowTag);

                // ��x�_���[�W���������R���C�_�[��ۑ�
                // damagedColliders.Add(info.collider);

                var attackInterface = info.collider.GetComponentInParent<AttackInterface>();

#if UNITY_EDITOR
                if(attackInterface == null)
                {
                    Debug.Log("AttackInterface ��������܂���");
                }

                Debug.Log($"Enemy�̃_���[�W: {attackInterface.GetOtherAttackDamage()}�i{(isCounterAttack ? "�J�E���^�[" : "�ʏ�")}�j");
                Debug.Log(Time.frameCount + ": Counter Hit!");
#endif

                // �X�^���_���[�W����������
                m_BossStatusEffectManager.Damage(attackInterface.GetOtherStanAttackDamage());

                // �X�^����ԂɈڍs���邩�̃`�F�b�N
                if(m_BossStatusEffectManager.GetStanFlag())
                {
                    this.ChangeState(new BossStanState()); // �X�^����ԂɈڍs
                }

                if (isCounterAttack) { hitCounter = true; }
                // �_���[�W����������
                hpManager.TakeDamage(attackInterface.GetOtherAttackDamage());

                // �G�t�F�N�g
                DamageEffect(info.collider);

                info.hitFlag = true;

                // attackInterface.HitAttack();

                break; // ��x�q�b�g�ŏ����I��
            }
        }
    }


    protected void Die()
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
        if(!hitCounter)
        {
            Debug.Log("�J�E���^�[�ȊO�œ|���ꂽ");
        }
        Debug.Log($"{gameObject.name} �����S���܂���");
#endif

        enemyRigidbody.useGravity = false; // �d�͂�OFF�ɂ���
        this.GetComponent<Collider>().enabled = false; // �R���C�_�[�𖳌��ɂ���

        ChangeState(new BossDeadState()); // Dead��ԂɕύX
    }


    // �Q�b�^�[
    public BossStatusEffectManager GetBossStatusEffectManager() { return m_BossStatusEffectManager; }
}
