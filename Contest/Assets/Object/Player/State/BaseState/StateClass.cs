using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// State�̊��N���X
public abstract class StateClass
{
    // ��Ԃ�ύX����
    public abstract void Change(GameObject player);
    // ��Ԃ̊J�n����
    public abstract void Enter(GameObject player);
    // ��Ԓ��̏���
    public abstract void Excute(GameObject player);
    // ��Ԓ��̏I������
    public abstract void Exit(GameObject player);
}
