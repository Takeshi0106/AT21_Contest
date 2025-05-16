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
        //y����]
        Quaternion rotation1 = Quaternion.Euler(0f, enemyState.GetEnemyFov() / 2, 0f);
        Quaternion rotation2 = Quaternion.Euler(0f, -enemyState.GetEnemyFov() / 2, 0f);

        //�ڕW�iPlayer�j�Ɍ����Ă̕����x�N�g���̒P�ʃx�N�g��
        Vector3 targetDir =
            (enemyState.GetTargetObject().transform.position - enemyState.transform.position).normalized;

        //�G�̐��ʃx�N�g���ƃv���C���[�Ɍ����Ă̒P�ʃx�N�g���Ƃ̓���
        float angle = Vector3.Dot(enemyState.transform.forward, targetDir);

        //����p�̉E�[�����̃x�N�g��
        Vector3 fovRightVector =
            (rotation1 * enemyState.transform.forward);

        //����p�̍��[�����̃x�N�g��
        Vector3 fovLeftVector =
            (rotation2 * enemyState.transform.forward);

        //����p�̉E�[�A���[�Ɛ��ʃx�N�g���Ƃ̓���
        float limitFovAngle_right = Vector3.Dot(enemyState.transform.forward, fovRightVector);
        float limitFovAngle_left = Vector3.Dot(enemyState.transform.forward, fovLeftVector);

        //�v���C���[�Ɍ������ă��C���΂�
        Debug.DrawRay(enemyState.transform.position,
            (enemyState.GetTargetObject().transform.position - enemyState.transform.position) * 2, Color.blue, 0.1f);

        //����p�����o��
        Debug.DrawRay(enemyState.transform.position, rotation1 * enemyState.transform.forward * 30f, Color.red, 0.1f);
        Debug.DrawRay(enemyState.transform.position, rotation2 * enemyState.transform.forward * 30f, Color.red, 0.1f);

        //����p�̓����Ƀv���C���[�����邩
        if (limitFovAngle_right <= angle && limitFovAngle_left <= angle)
        {
            //���F�����̓����ɂ��邩
            if (Physics.Raycast(enemyState.transform.position,
                (enemyState.GetTargetObject().transform.position - enemyState.transform.position), out hit, enemyState.GetEnemyVisionLength()))
            {
                //���������Q�[���I�u�W�F�N�g���v���C���[���A���̋�����艓���Ȃ�ǐՃX�e�[�g�ɐ؂�ւ�
                if (hit.collider.gameObject.name == "Player" && hit.distance > enemyState.GetEnemyAttackRange())
                {
                    //Debug.LogError(hit.collider.gameObject.name + "�ɓ�������");
                    enemyState.SetFoundTargetFlg(true);
                    enemyState.ChangeState(Enemy_ChaseState.Instance);
                }
                //���̋����ȉ��Ȃ�U���X�e�[�g��
                else if (hit.collider.gameObject.name == "Player" && hit.distance <= enemyState.GetEnemyAttackRange())
                {
                    // �U����ԂɈڍs����
                    if (freams > waitTime)
                    {
                        enemyState.ChangeState(EnemyAttackState.Instance);
                    }
                }

            }
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
        if(enemyState.GetFoundTargetFlg() == true)
        {
            enemyState.transform.LookAt(enemyState.GetTargetObject().transform.position);
        }
        

    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {
        freams = 0.0f;
    }



}
