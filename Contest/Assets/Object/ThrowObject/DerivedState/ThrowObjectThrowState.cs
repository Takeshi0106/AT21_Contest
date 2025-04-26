using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ==================================
// �������Ă�����
// ==================================

public class ThrowObjectThrowState : StateClass<ThrowObjectState>
{
    // �C���X�^���X������ϐ�
    private static ThrowObjectThrowState instance;

    // �t���[��
    int freams = 0;


    // �C���X�^���X���擾����֐�
    public static ThrowObjectThrowState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ThrowObjectThrowState();
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
        state.GetThrowObjectRigidbody().AddForce(state.GetCameraTransform().forward * state.GetForcePower(),
    ForceMode.Impulse);
    }



    // ��Ԓ��̏���
    public override void Excute(ThrowObjectState state)
    {
        // �d�͂𖳌��ɂ���
        if(state.GetNoGravityFreams() > freams) 
        {
            state.GetThrowObjectRigidbody().useGravity = false;
        }
        // �d�͂�L���ɂ���
        else
        {
            state.GetThrowObjectRigidbody().useGravity = true;
        }

        freams++;
    }



    // ��Ԓ��̏I������
    public override void Exit(ThrowObjectState state)
    {

    }



}
