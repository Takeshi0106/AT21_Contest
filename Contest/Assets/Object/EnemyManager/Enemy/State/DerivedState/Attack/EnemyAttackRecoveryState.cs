using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// ======================
// �G�̍U������
// ======================

public class EnemyAttackRecoveryState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private static EnemyAttackRecoveryState instance;
    // weponData
    private static BaseAttackData weponData;
    // �t���[�����v��
    float freams = 0.0f;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // ���̐F��ۑ�
    Color originalColor;
#endif



    // �C���X�^���X���擾����֐�
    public static EnemyAttackRecoveryState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyAttackRecoveryState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(EnemyState enemyState)
    {
        // �d���t���[�����I������珈��
        if (freams >= weponData.GetAttackStaggerFrames(enemyState.GetEnemyConbo()) - 1)
        {
            if (enemyState.GetEnemyConbo() < weponData.GetMaxCombo() - 1)
            {
                // �R���{��i�߂�
                enemyState.SetEnemyCombo(enemyState.GetEnemyConbo() + 1);
                enemyState.ChangeState(EnemyAttackState.Instance);
            }
            else
            {
                // �R���{�I�� �� ������Ԃ֖߂�
                enemyState.SetEnemyCombo(0);
                enemyState.ChangeState(EnemyStandingState.Instance);
            }
            return;
        }

        // ���ݏ�ԂɈڍs
        if (enemyState.GetEnemyDamageFlag())
        {
            enemyState.ChangeState(EnemyFlinchState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
        // ����f�[�^�擾
        weponData = enemyState.GetEnemyWeponManager().GetWeaponData(enemyState.GetEnemyWeponNumber());

        /*
        // �A�j���[�V�����擾
        var animClip = weponData.GetAttackStaggerAnimation(enemyState.GetEnemyConbo());
        var childAnim = enemyState.GetEnemyWeponManager().GetCurrentWeaponAnimator();

        // �A�j���[�V�����Đ�
        if (childAnim != null && animClip != null)
        {
            childAnim.CrossFade(animClip.name, 0.1f);
        }
        */

#if UNITY_EDITOR

        if (weponData == null)
        {
            Debug.LogError("EnemyAttackState : WeponData��������܂���");
            return;
        }
        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        if (enemyState.enemyRenderer != null)
        {
            originalColor = enemyState.enemyRenderer.material.color; // ���̐F��ۑ�
            enemyState.enemyRenderer.material.color = Color.blue;    // �J�E���^�[�������̐F
        }
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {
        // �_���[�W����
        enemyState.HandleDamage();

        // �t���[���X�V
        freams += enemyState.GetEnemySpeed();
    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {
        // ������
        freams = 0;
        //
        enemyState.SetEnemyFlinchCnt(0);

#if UNITY_EDITOR

        // �G�f�B�^���s���ɐF�����ɖ߂�
        if (enemyState.enemyRenderer != null)
        {
            enemyState.enemyRenderer.material.color = originalColor; // ���̐F�ɖ߂�
        }
#endif
    }



}
