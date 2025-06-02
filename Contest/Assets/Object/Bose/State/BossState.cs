using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// =================================
// BOSS�̏�ԊǗ��N���X
// =================================


public class BossState : EnemyBaseState<BossState>
{
    private BossStatusEffectManager m_BossStatusEffectManager = null; // Boss�̏�ԊǗ��N���X


    private void Start()
    {
        EnemyStart(); // �G�l�~�[�N���X������

        // HP�}�l�[�W���[��Die�֐���n��
        hpManager.SetOnDeathEvent(Die);

        // �G�l�~�[�}�l�W���[�ɃX���[�C�x���g���Z�b�g
        enemyManager.AddOnEnemySlow(SetEnemySpead);
        // 
        m_BossStatusEffectManager = this.gameObject.GetComponent<BossStatusEffectManager>();

        // ��Ԃ��Z�b�g
        // currentState = new EnemyStandingState();
        // ��Ԃ̊J�n����
        // currentState.Enter(this);

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
        /*
        if(m_BossStatusEffectManager.GetStanFlag())
        {
            // ChangeState()
        }
        */

        // StateUpdate();

        HandleDamage();
    }

    // �_���[�W�����i�ʏ�U���{�J�E���^�[�U���Ή��j
    override public void HandleDamage()
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
                // �v���C���[�̃X�^���Q�[�W�_���[�W����������ϐ�
                float stanDamage = 0.0f;
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
                    // �_���[�W���v�Z���邽�߂Ƀp�����[�^���擾
                    baseDamage = playerState.GetPlayerCounterManager().GetCounterDamage();
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                    stanDamage = playerState.GetPlayerCounterManager().GetCounterStanDamage();

                    hitCounter = true;
                }
                // ������U���̏ꍇ�̏���
                else if (isThrowAttack)
                {
                    ThrowObjectState throwState = info.collider.gameObject.GetComponentInParent<ThrowObjectState>();
                    baseDamage = throwState.GetThrowDamage(); // �_���[�W���擾
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                    stanDamage=throwState.GetThrowStanDamage(); // �X�^���_���[�W�擾
                }
                // �ʏ킱�������̏ꍇ�̏���
                else
                {
                    // �_���[�W���v�Z���邽�߂Ƀp�����[�^���擾
                    var weaponManager = playerState.GetPlayerWeponManager();
                    var weaponData = weaponManager.GetWeaponData(playerState.GetPlayerWeponNumber());

                    baseDamage = weaponData.GetDamage(playerState.GetPlayerConbo());
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                    stanDamage = weaponData.GetStanDamage(playerState.GetPlayerConbo());
                }

                // �_���[�W�v�Z
                finalDamage = baseDamage * multiplier;

#if UNITY_EDITOR
                Debug.Log($"Enemy�̃_���[�W: {finalDamage}�i{(isCounterAttack ? "�J�E���^�[" : "�ʏ�")}�j");
                Debug.Log(Time.frameCount + ": Counter Hit!");
#endif

                // �_���[�W����������
                hpManager.TakeDamage(finalDamage);
                // �X�^���_���[�W����������
                m_BossStatusEffectManager.Damage(stanDamage);


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
        Debug.Log($"{gameObject.name} �����S���܂���");
#endif

        enemyRigidbody.useGravity = false; // �d�͂�OFF�ɂ���
        this.GetComponent<Collider>().enabled = false; // �R���C�_�[�𖳌��ɂ���

        // ChangeState(new EnemyDeadState()); // Dead��ԂɕύX
    }
}
