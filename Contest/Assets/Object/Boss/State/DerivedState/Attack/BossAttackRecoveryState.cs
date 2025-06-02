using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// ======================
// �G�̍U������
// ======================

public class BossAttackRecoveryState : StateClass<BossState>
{
    // weponData
    private static BaseAttackData weponData;
    // �t���[�����v��
    float freams = 0.0f;

    Vector3 vec;

#if UNITY_EDITOR
    // �G�f�B�^���s���Ɏ��s�����
    // ���̐F��ۑ�
    Color originalColor;
#endif


    // ��Ԃ̕ύX����
    public override void Change(BossState enemyState)
    {
        // �d���t���[�����I������珈��
        if (vec.magnitude < 3.0f && enemyState.GetEnemyAttackFlag())
        {
            if (enemyState.GetEnemyConbo() < weponData.GetMaxCombo() - 1)
            {
                // �R���{��i�߂�
                enemyState.SetEnemyCombo(enemyState.GetEnemyConbo() + 1);
                enemyState.ChangeState(new BossAttackState());
            }
            else
            {
                // �R���{�I�� �� ������Ԃ֖߂�
                enemyState.SetEnemyCombo(0);
                enemyState.ChangeState(new BossStandingState());
            }
            return;
        }
        else
        {
            // �R���{�I�� �� ������Ԃ֖߂�
            enemyState.SetEnemyCombo(0);
            enemyState.ChangeState(new BossStandingState());
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(BossState enemyState)
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

        vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;

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
    public override void Excute(BossState enemyState)
    {
        // �_���[�W����
        enemyState.HandleDamage();

        vec = enemyState.GetPlayerState().transform.position - enemyState.transform.position;

        // �t���[���X�V
        freams += enemyState.GetEnemySpeed();
    }



    // ��Ԓ��̏I������
    public override void Exit(BossState enemyState)
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
