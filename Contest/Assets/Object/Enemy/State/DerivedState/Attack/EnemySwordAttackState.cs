using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordAttackState : StateClass<EnemyState>
{
    // �C���X�^���X������ϐ�
    private static EnemySwordAttackState instance;



    // �C���X�^���X���擾����֐�
    public static EnemySwordAttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemySwordAttackState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(EnemyState enemyState)
    {

    }



    // ��Ԃ̊J�n����
    public override void Enter(EnemyState enemyState)
    {
#if UNITY_EDITOR
        Debug.LogError("EnemySwordAttackState : �J�n");
#endif
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
