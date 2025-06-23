using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =====================================
// �G�l�~�[�̏��
// =====================================

public class EnemyState : EnemyBaseState<EnemyState>
{
    DamageResponseManager enemyDamageResponseManager;

    // ����������
    void Start()
    {
        CharacterStart(); // �L�����N�^�[������
        EnemyStart(); // �G�l�~�[�N���X������

        enemyDamageResponseManager = this.gameObject.GetComponent<DamageResponseManager>();

        // HP�}�l�[�W���[��Die�֐���n��
        hpManager.SetOnDeathEvent(Die);

        // �G�l�~�[�}�l�W���[�ɃX���[�C�x���g���Z�b�g
        enemyManager.AddOnEnemySlow(SetEnemySpead);

        // ��Ԃ��Z�b�g
        currentState = new EnemyStandingState();
        // ��Ԃ̊J�n����
        currentState.Enter(this);
        // �G�l�~�[�I�u�W�F�N�g�Ɏ�����n��
        enemyManager.RegisterEnemy(this);

        SetEnemySpead(0.8f);

        this.gameObject.SetActive(false);
    }



    // �X�V����
    void Update()
    {
        StateUpdate();
        enemyDamageResponseManager.FlinchUpdate(enemySpeed);
    }



    // �_���[�W�����i�ʏ�U���{�J�E���^�[�U���Ή��j
    virtual public void HandleDamage()
    {
        damagerFlag = false;
        hitCounter = false;

        for (int i = 0; i < collidedInfos.Count; i++)
        { 
            var info = collidedInfos[i];

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log($"{info.collider.name} :  HitFlag :{info.hitFlag} / HitID{info.hitID}");
            }
#endif

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

                Debug.Log($"Enemy�̃_���[�W: {attackInterface.GetOtherAttackDamage()}�i{(isCounterAttack ? "�J�E���^�[" : "�ʏ�")}�j");
                Debug.Log(Time.frameCount + ": Counter Hit!");
#endif
                // ���ݏ���
                if (enemyDamageResponseManager.FlinchDamage(attackInterface.GetOtherStanAttackDamage()))
                {
                    ChangeState(new EnemyFlinchState());
                }
                // �_���[�W����������
                hpManager.TakeDamage(attackInterface.GetOtherAttackDamage());

                // �_���[�W����������
                info.hitFlag = true;

                if(isCounterAttack) { hitCounter = true; }

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
        Debug.Log($"{gameObject.name} �����S���܂���");
#endif

        enemyRigidbody.useGravity = false; // �d�͂�OFF�ɂ���
        this.GetComponent<Collider>().enabled = false; // �R���C�_�[�𖳌��ɂ���

        ChangeState(new EnemyDeadState()); // Dead��ԂɕύX
    }

    // �Q�b�^�[
    public DamageResponseManager GetEnemyDamageResponseManager() { return enemyDamageResponseManager; }
}
