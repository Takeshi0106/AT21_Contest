using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =====================================
// ���G��X�^���̊Ǘ�
// =====================================


public class StatusEffectManager : MonoBehaviour
{
    // �t���O
    private bool invincibleFlag = false;
    private bool stunFlag = false;

    // �t���[�����v��
    private int invincibleFreams = 0;
    private int stunFreams = 0;



    // ���G���̏���
    public bool Invincible(int invincibleTime)
    {
        if(invincibleFlag)
        {
            invincibleFreams++;

            if(invincibleFreams > invincibleTime)
            {
                invincibleFreams = 0;
                invincibleFlag = false;
            }
        }
        
        return invincibleFlag;
    }



    // �X�^�����̏���
    public bool Stun(int stunTime)
    {
        if (stunFlag)
        {
            stunFreams++;

            if (stunFreams > stunTime)
            {
                stunFreams = 0;
                stunFlag = false;
            }
        }

        return stunFlag;
    }



    // ���G�̊J�n����
    public void StartInvicible()
    {
        invincibleFlag = true;
    }



    // �X�^���̊J�n����
    public void StartStun()
    {
        stunFlag = true;
    }



}
