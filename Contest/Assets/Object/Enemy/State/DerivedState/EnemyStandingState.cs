using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStandingState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private static EnemyStandingState instance;

    //�����������̂�ۑ�����֐�
    private RaycastHit hit;

    //�Q�[���I�u�W�F�N�g
    private GameObject target;
    private float visionLength; 

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
        //���̋����ȉ������E���ɓ�������
        {
            //y����]
            Quaternion rotation1 = Quaternion.Euler(0f, 45f, 0f);
            Quaternion rotation2 = Quaternion.Euler(0f, -45f, 0f);

            //�ڕW�iPlayer�j�Ɍ����Ă̕����x�N�g���̒P�ʃx�N�g��
            Vector3 targetDir =
                (target.transform.position - enemyState.transform.position).normalized;

            //�G�̐��ʂƃv���C���[�Ɍ����Ă̒P�ʃx�N�g���Ƃ̓���
            float angle = Vector3.Dot(enemyState.transform.forward, targetDir);

            //����p��臒l
            float threshold = Mathf.Cos(90f / 2 * Mathf.Deg2Rad);

            //�v���C���[�Ɍ������ă��C���΂�
            Debug.DrawRay(enemyState.transform.position,
                (target.transform.position - enemyState.transform.position) * 5f, Color.red, 0.1f);

            Debug.DrawRay(enemyState.transform.position, rotation1 * enemyState.transform.forward * 30f, Color.red, 0.1f);
            Debug.DrawRay(enemyState.transform.position, rotation2 * enemyState.transform.forward * 30f, Color.red, 0.1f);

            //����̓����ɓ����Ă��邩
            if (angle > threshold)
            {
                //Debug.Log("������ɂ���");

                if (Physics.Raycast(enemyState.transform.position,
                    (target.transform.position - enemyState.transform.position), out hit, visionLength))
                {
                    //���������Q�[���I�u�W�F�N�g���v���C���[�Ȃ�ǐՃX�e�[�g�ɐ؂�ւ�
                    if (hit.collider.gameObject.name == "Player")
                    {
                        Debug.LogError(hit.collider.gameObject.name + "�ɓ�������");
                        enemyState.ChangeState(EnemyChaseState.Instance);
                    }

                }
            }
        }

       

    }



    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
#if UNITY_EDITOR
        Debug.LogError("EnemyStandingState : �J�n");
#endif

        target = GameObject.Find("Player");
        if(target != null)
        {
            Debug.LogError(target.name + "�𔭌�");
        }

        //����̋����̍ő�l
        visionLength = 30f;
    }



    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {

    }



    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {

    }



}
