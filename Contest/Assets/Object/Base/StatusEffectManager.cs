using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =====================================
// 無敵やスタンの管理
// =====================================


public class StatusEffectManager : MonoBehaviour
{
    // フラグ
    private bool invincibleFlag = false;
    private bool stunFlag = false;

    // フレームを計る
    private int invincibleFreams = 0;
    private int stunFreams = 0;



    // 無敵中の処理
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



    // スタン中の処理
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



    // 無敵の開始処理
    public void StartInvicible()
    {
        invincibleFlag = true;
    }



    // スタンの開始処理
    public void StartStun()
    {
        stunFlag = true;
    }



}
