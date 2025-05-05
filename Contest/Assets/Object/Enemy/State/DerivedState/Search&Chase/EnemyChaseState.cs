using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class EnemyChaseState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private static EnemyChaseState instance;

    //�Q�[���I�u�W�F�N�g
    private GameObject enemy;

    private GameObject target;

    private ChaseScript chaseScript;

    //Ray�֘A
    RaycastHit hit;

    //���C�̒���
    float rayLength;

    // �C���X�^���X���擾����֐�
    public static EnemyChaseState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyChaseState();
            }
            return instance;
        }
    }

    // ��Ԃ̕ύX����
    public override void Change(EnemyState enemyState)
    {
        //�v���C���[�ƓG�Ƃ̋��������̒l�ȉ��ɂȂ�����

        //�v���C���[�Ɍ������ă��C���΂�
        Debug.DrawRay(enemyState.transform.position,
            (target.transform.position - enemyState.transform.position), Color.red, 0.1f);

        //�v���C���[�Ƀ��C������������X�e�[�g��ύX
        if(Physics.Raycast(enemyState.transform.position,
            (target.transform.position - enemyState.transform.position),out hit,rayLength))
        {
            //Debug.Log(hit.distance);

            if (hit.collider.gameObject.name == "Player")
            {
                //�U���X�e�[�g��
                enemyState.ChangeState(EnemySwordAttackState.Instance);
            }
        }


    }

    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
#if UNITY_EDITOR
        Debug.LogError("EnemyChase : �J�n");
#endif
        //�G���̃X�N���v�g�ɃA�N�Z�X���邽�߂ɓG��������
        enemy = GameObject.Find("Enemy");
        if (enemy != null)
        {
            chaseScript = enemy.GetComponent<ChaseScript>();
            if (chaseScript != null)
            {
                Debug.Log("�ǐՊJ�n");
               
            }
        }

        //�ǂ�������Ώہi�v���C���[�j��������
        target = GameObject.Find("Player");
        if (target != null)
        {
            Debug.LogError(target.name + "�𔭌�");
        }

        rayLength = 3f;
    }



    // ��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {
        if (enemy != null)
        {
            //chaseScript = Enemy.GetComponent<ChaseScript>();
            if (chaseScript != null)
            {
                //Debug.Log("�ǐՒ�");
                chaseScript.TargetChase();
            }
        }


    }

    // ��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {

    }


}
