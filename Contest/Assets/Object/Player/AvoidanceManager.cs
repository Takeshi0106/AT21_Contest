using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
    [Header("�G��system������")]
    [SerializeField] private GameObject enemySystemObj = null;

    [Header("��𐬌����̑S�̂̃X�s�[�h�ቺ")]
    [SerializeField] private float avoidanceSlow = 0.8f;

    [Header("Color Grading�̃t�B���^�[�̐F")]
    [SerializeField] private Color avoidanceColorFilter = Color.red;

    private EnemySystem enemySystem;
    private PlayerState playerState;
    private PostProcessVolume playerVolume;
    private ColorGrading colorGrading;
    private bool slowFlag = false;
    private int freams = 0;


    void Start()
    {
        // �G�̃}�l�W���[���擾����
        enemySystem = enemySystemObj.GetComponent<EnemySystem>();
        // �v���C���[�̏�Ԃ��擾����
        playerState = this.gameObject.GetComponent<PlayerState>();
        // �G�t�F�N�g��ǉ�
        playerVolume = this.gameObject.GetComponent<PostProcessVolume>();

        // Color Grading �G�t�F�N�g���擾
        if (playerVolume.profile.TryGetSettings(out colorGrading))
        {
            // �����F��ݒ�
            colorGrading.colorFilter.value = Color.white;
        }   

#if UNITY_EDITOR
        Debug.Log("AvoidanceManager �̏����������s����܂����B");

        if (enemySystem == null)
        {
            Debug.LogError("AvoidanceManager : enemySystem�� null �ł�");
        }
        if (playerState == null)
        {
            Debug.LogError("AvoidanceManager : playerState�� null �ł�");
        }
        if (playerVolume == null)
        {
            Debug.LogError("AvoidanceManager : playerVolume�� null �ł�");
        }
        if(colorGrading == null)
        {
            Debug.LogError("AvoidanceManager : colorGrading�� null �ł�");
        }
#endif
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
        // �G�t�F�N�g�����s
        colorGrading.colorFilter.value = avoidanceColorFilter;
        // �S�Ă̑��x��x������
        Time.timeScale = avoidanceSlow;
        // �v���C���[�̃t���[��������x������
        playerState.SetPlayerSpeed(avoidanceSlow);
        // �G��x������
        enemySystem.EnemySlow(avoidanceSlowEnemy);
        // �t���O��ON�ɂ���
        slowFlag = true;
    }



    private void AvoidanceEnd()
    {
#if UNITY_EDITOR
        Debug.Log("����I�� �I������");
#endif
        // �G�t�F�N�g�����ɖ߂�
        colorGrading.colorFilter.value = Color.white;
        // �S�Ă̑��x�����ɖ߂�
        Time.timeScale = 1.0f;
        // �v���C���[�̃t���[�����������ɖ߂�
        playerState.SetPlayerSpeed(1.0f);
        // �G�����ɖ߂�
        enemySystem.EnemySlow(0.8f);
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
    public EnemySystem GetEnemyManager() { return enemySystem;}
}
