using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceManager : MonoBehaviour
{
    [Header("����̊J�n�t���[��")]
    [SerializeField] private int avoidanceStartUpFreams = 10;
    [Header("��𒆂̃t���[��")]
    [SerializeField] private int avoidanceFreams = 30;
    [Header("�����̃t���[��")]
    [SerializeField] private int avoidanceAfterFreams = 20;
    [Header("��𐬌����̖��G�t���[��")]
    [SerializeField] private int avoidanceInvincibleFreams = 20;
    [Header("��𐬌����̓G�̃X�s�[�h�ቺ")]
    [SerializeField] private float avoidanceSlow = 0.8f;
    [Header("������������Ēx���Ȃ�t���[��")]
    [SerializeField] private int EnemySlowFreams = 20;
    [Header("�G�̃}�l�[�W���[������")]
    [SerializeField] private GameObject enemyManagerObj = null;

    [Header("��𐬌����̎����̃X�s�[�h�ቺ")]
    [SerializeField] private float avoidanceSlowPlayer = 0.8f;

    private EnemyManager enemyManager;
    private bool slowFlag = false;
    private int freams = 0;


    void Start()
    {
        // �G�̃}�l�W���[���擾����
        enemyManager= enemyManagerObj.GetComponent<EnemyManager>();
    }



    public void AvoidUpdate()
    {
        // ������������Ă��Ȃ���Ύ��s���Ȃ�
        if(!slowFlag) { return; }

        if (EnemySlowFreams < freams)
        {
            // ����̏I��菈��
            AvoidanceEnd();
        }

        // �t���[���X�V
        freams++;
    }



    public void AvoidanceStart()
    {
        // �G��x������
        enemyManager.EnemySlow(avoidanceSlow);
        // �t���O��ON�ɂ���
        slowFlag = true;
    }



    private void AvoidanceEnd()
    {
        // �G�����ɖ߂�
        enemyManager.EnemySlow(1.0f);
        // �t���O��OFF�ɂ���
        slowFlag= false;
        // �t���[��������������
        freams = 0;
    }



    // �Q�b�^�[
    public int GetAvoidanceStartUpFreams() { return avoidanceStartUpFreams; }
    public int GetAvoidanceFreams() { return avoidanceFreams; }
    public int GetAvoidanceAfterFreams() { return avoidanceAfterFreams; }
    public int GetAvoidanceInvincibleFreams() { return avoidanceInvincibleFreams; }
    public EnemyManager GetEnemyManager() { return enemyManager;}
    public float GetAvoidanceSlowPlayer() { return avoidanceSlowPlayer; }
}
