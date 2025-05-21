using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy_ChaseState : StateClass<EnemyState>
{
    //�C���X�^���X
    private static Enemy_ChaseState instance;

   //�v���C���[���������Ă��邩���m�F����ϐ�
    private RaycastHit hit;
    //private GameObject target;

    //�G�l�~�[���ƂɎ����Ă���X�N���v�g�����ʂ��Ċi�[����ϐ�
    private Dictionary<string, ChaseScript> pair_enemyChase = new Dictionary<string, ChaseScript>();

    //�C���X�^���X��Ԃ��֐�
    public static Enemy_ChaseState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Enemy_ChaseState();
            }

            return instance;
        }
        
    }

    //��Ԃ̕ύX����
    public override void Change(EnemyState enemyState)
    {
        //�m�F�p���C
        Debug.DrawRay(enemyState.transform.position,
                (enemyState.GetTargetObject().transform.position - enemyState.transform.position), Color.green, 0.1f);
        //���̋����Ƀv���C���[��������
        if (Physics.Raycast(enemyState.transform.position, 
            (enemyState.GetTargetObject().transform.position - enemyState.transform.position), out hit, enemyState.GetEnemyAttackRange()))
        {
            if (hit.collider.gameObject.name == "Player")
            {
                //�����X�e�[�g�ɖ߂�
                enemyState.ChangeState(EnemyStandingState.Instance);
            }
        }
    }

    //��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
        //dictionary���ɃL�[���i�[����Ă��Ȃ��Ȃ�ǉ�
        if(!pair_enemyChase.ContainsKey(enemyState.name))
        {
            pair_enemyChase.Add(enemyState.name, enemyState.GetComponent<ChaseScript>());
            //pair_enemyChase.Add(enemyState.name, enemyState.GetComponent<ChaseScript>());
            Debug.LogError(enemyState.name + "��ǉ�");

            //�^�[�Q�b�g�I�u�W�F�N�g��n��
            pair_enemyChase[enemyState.name].SetTarget(enemyState.GetTargetObject());

            //��~���鋗����n��
            pair_enemyChase[enemyState.name].SetStoppingDistance(enemyState.GetEnemyAttackRange());

            //�ړ����x�̒l��n��
            pair_enemyChase[(enemyState.name)].SetMoveSpeed(enemyState.GetEnemyMoveSpeed());
        }

       

        Debug.LogError(enemyState.name+":chaseScript�Ɉڍs");

        
    }
    
    //��Ԓ��̏���
    public override void Excute(EnemyState enemyState)
    {
        //���݂�enemyState�����݂���Ȃ�
        if (pair_enemyChase.ContainsKey(enemyState.name))
        {
            //�ǐՃX�N���v�g�̊֐��Ăяo��
            pair_enemyChase[enemyState.name].TargetChase();
            //Debug.LogError(enemyState.name + "���������Ă���");

//#if UNITY_EDITOR
//            // Renderer�����݂���Ȃ�ԐF�ɕύX
//            var renderer = enemyState.GetComponentInChildren<Renderer>();
//            if (renderer != null && renderer.material != null)
//            {
//                renderer.material.color = Color.red;
//            }
//#endif


        }
        else
        {
            Debug.LogWarning("�Ăт���ł���enemyState��������܂���ł���");
        }

    }

    //��Ԓ��̏I������
    public override void Exit(EnemyState enemyState)
    {
        
    }
}
