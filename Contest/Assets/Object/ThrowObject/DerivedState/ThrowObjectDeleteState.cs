using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ==================================================
// �������I�u�W�F�N�g�������������
// ==================================================

public class ThrowObjectDeleteState : StateClass<ThrowObjectState>
{
    // �C���X�^���X������ϐ�
    private static ThrowObjectDeleteState instance;

    // �t���[��
    int freams = 0;


    // �C���X�^���X���擾����֐�
    public static ThrowObjectDeleteState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ThrowObjectDeleteState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(ThrowObjectState state)
    {

    }



    // ��Ԃ̊J�n����
    public override void Enter(ThrowObjectState state)
    {
        // �������폜����
        state.ThrowObjectDelete();
    }



    // ��Ԓ��̏���
    public override void Excute(ThrowObjectState state)
    {

    }



    // ��Ԓ��̏I������
    public override void Exit(ThrowObjectState state)
    {

    }



}
