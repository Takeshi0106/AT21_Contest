using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ==================================================
// �������I�u�W�F�N�g�������������
// ==================================================

public class ThrowObjectDeleteState : StateClass<ThrowObjectState>
{

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
