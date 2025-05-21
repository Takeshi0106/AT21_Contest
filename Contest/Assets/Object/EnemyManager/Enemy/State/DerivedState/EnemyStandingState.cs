using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyStandingState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private static EnemyStandingState instance;
    // �t���[�����v��
    float freams = 0;
    int waitTime = 0;

    //����p�Ƀv���C���[�����邩�𔻒f����ϐ�
    private RaycastHit hit;
    //private GameObject target;

    // �C���X�^���X���擾����֐�
    public static EnemyStandingState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyStandingState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(EnemyState enemyState)
    {
        //�v���C���[�Ɍ������ă��C���΂�
        Debug.DrawRay(enemyState.transform.position,
            (enemyState.GetTargetObject().transform.position - enemyState.transform.position) * 2, Color.blue, 0.1f);
        //�U���͈͂����o��
        Debug.DrawRay(enemyState.transform.position, enemyState.transform.forward * enemyState.GetEnemyAttackRange(), Color.green, 0.1f);

        //�U���͈͂̓����ɂ��邩
        if (Physics.Raycast(enemyState.transform.position,
            (enemyState.GetTargetObject().transform.position - enemyState.transform.position), out hit,
            enemyState.GetEnemyAttackRange()))
        {
            //�v���C���[�Ȃ�U���X�e�[�g��
            if (hit.collider.gameObject.name == "Player")
            {
                // �U����ԂɈڍs����
                if (freams > waitTime)
                {
                    enemyState.ChangeState(EnemyAttackState.Instance);
                }
            }

        }
        //�O���Ȃ�ǐՃX�e�[�g��
        else
        {
            enemyState.SetFoundTargetFlg(true);
            enemyState.ChangeState(Enemy_ChaseState.Instance);
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
        // �f�o�b�O�p�ɍU����ԂɈڍs����t���[�������߂�
        waitTime = Random.Range(30, 120);

#if UNITY_EDITOR
        // Debug.LogError("EnemyStandingState : �J�n");
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {
        // �_���[�W����
        enemyState.HandleDamage();

        // �t���[���X�V
        freams += enemyState.GetEnemySpeed();

        //�v���C���[�̕�����������

        //�X���[�Y�Ƀv���C���[�̕����������鏈��
        Quaternion targetRotation = Quaternion.LookRotation(
            enemyState.GetTargetObject().transform.position - enemyState.transform.position);

        float angle = Quaternion.Angle(enemyState.transform.rotation, targetRotation);

        float speed = angle / 5f;

        enemyState.transform.rotation = Quaternion.Slerp(
            enemyState.transform.rotation, targetRotation, Time.deltaTime * speed);






    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {
        freams = 0.0f;
    }



}
