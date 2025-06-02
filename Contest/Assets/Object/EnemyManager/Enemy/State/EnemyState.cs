using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =====================================
// �G�l�~�[�̏��
// =====================================

public class EnemyState : EnemyBaseState<EnemyState>
{
    // ����������
    void Start()
    {
        EnemyStart(); // �G�l�~�[�N���X������

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
}
