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
    [Header("��𐬌����̓G�̃X�s�[�h�ቺ(�S�̂̃X�s�[�h�ቺ�Əd��)")]
    [SerializeField] private float avoidanceSlowEnemy = 0.8f;
    [Header("������������Ēx���Ȃ�t���[��")]
    [SerializeField] private int slowFreams = 20;
    [Header("�G�̃}�l�[�W���[������")]
    [SerializeField] private GameObject enemyManagerObj = null;

    [Header("��𐬌����̑S�̂̃X�s�[�h�ቺ")]
    [SerializeField] private float avoidanceSlow = 0.8f;


    private EnemyManager enemyManager;
    private PlayerState playerState;
    private bool slowFlag = false;
    private int freams = 0;


    void Start()
    {
        // �G�̃}�l�W���[���擾����
        enemyManager = enemyManagerObj.GetComponent<EnemyManager>();
        // �v���C���[�̏�Ԃ��擾����
        playerState = this.gameObject.GetComponent<PlayerState>();
    }



    public void AvoidUpdate()
    {
        // ������������Ă��Ȃ���Ύ��s���Ȃ�
        if(!slowFlag) { return; }

        if (slowFreams < freams)
        {
            // ����̏I��菈��
            AvoidanceEnd();
        }

        // �t���[���X�V
        freams++;
    }



    public void AvoidanceStart()
    {
#if UNITY_EDITOR
        Debug.Log("��𐬌� �J�n����");
#endif
        // �S�Ă̑��x��x������
        Time.timeScale = avoidanceSlow;
        // �v���C���[�̃t���[��������x������
        playerState.SetPlayerSpeed(avoidanceSlow);
        // �G��x������
        enemyManager.EnemySlow(avoidanceSlowEnemy);
        // �t���O��ON�ɂ���
        slowFlag = true;
    }



    private void AvoidanceEnd()
    {
#if UNITY_EDITOR
        Debug.Log("����I�� �I������");
#endif

        // �S�Ă̑��x�����ɖ߂�
        Time.timeScale = 1.0f;
        // �v���C���[�̃t���[�����������ɖ߂�
        playerState.SetPlayerSpeed(1.0f);
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
}
