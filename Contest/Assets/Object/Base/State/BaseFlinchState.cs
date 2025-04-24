using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===================================================
// �v���C���[�ƓG�̋��ݏ�ԃe���v���[�g�N���X
// ===================================================

public class BaseFlinchState<T> : StateClass<T> where T : BaseState<T>
{
    // �C���X�^���X���擾����
    protected static BaseFlinchState<T> instance;
    // �t���[�����v������
    protected int freams = 0;



    // �C���X�^���X���擾����֐�
    public static BaseFlinchState<T> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BaseFlinchState<T>();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public  override void Change(T currentState)
    {
        // �h���N���X�ɃI�[�o�[���C�h������
    }



    // ��Ԃ̊J�n����
    public override void Enter(T currentState)
    {

    }

    // ��Ԓ��̏���
    public override void Excute(T currentState)
    {
        freams++;
    }

    // ��Ԓ��̏I������
    public override void Exit(T currentState)
    {

    }



}
