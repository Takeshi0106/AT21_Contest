using UnityEngine;

public class CounterManager : MonoBehaviour
{
    [Header("�J�E���^�[�̃����N��")]
    [SerializeField] private int counterMaxRank = 5;
    [Header("�J�E���^�[�̃Q�[�W�ʁi�����N���Ɓj")]
    [SerializeField] private float[] counterGauge = { 100.0f, 200.0f, 300.0f, 400.0f, 500.0f };
    [Header("�J�E���^�[�������̃Q�[�W������")]
    [SerializeField] private float counterSuccessGain = 20f;
    [Header("�J�E���^�[�̃_���[�W�{���i�����N���Ɓj")]
    [SerializeField] private float[] damageMultipliers = { 1.0f, 1.2f, 1.5f, 2.0f,5.0f };
    [Header("�J�E���^�[�̎����t���[���i�����N���Ɓj")]
    [SerializeField] private int[] counterFrames = { 30, 40, 50, 60, 70 };
    [Header("�J�E���^�[���s���̍d���t���[���i�����N���Ɓj")]
    [SerializeField] private int[] counterStaggerFrames = { 10, 10, 10, 10, 10 };
    [Header("�J�E���^�[�������̃t���[���i�����N���Ɓj")]
    [SerializeField] private int[] counterSuccessFrames = { 10, 10, 10, 10, 10 };   

    // �ŐV�̃Q�[�W������
    private float currentGauge = 0f;
    // �ŐV�̃����N������
    private int currentRank = 0;



    // �Q�[�W�ʂ��グ�鏈��
    public void IncreaseGauge()
    {
        currentGauge += counterSuccessGain;
        if (currentGauge >= counterGauge[currentRank])
        {
            RankUp();
            currentGauge = 0;
        }
    }



    // �����N�A�b�v����
    private void RankUp()
    {
        if (currentRank < counterMaxRank - 1)
        {
            currentRank++;
            Debug.Log($"�J�E���^�[�����N�A�b�v�I���݂̃����N: {currentRank + 1}");
        }
    }


    // �Q�b�^�[
    public float GetDamageMultiplier() => damageMultipliers[(int)currentRank];
    public int GetCounterFrames() => counterFrames[(int)currentRank];
    public int GetCounterStaggerFrames() => counterStaggerFrames[(int)currentRank];
    public int GetCounterSuccessFrames() => counterSuccessFrames[(int)currentRank];
    public float GetCurrentGauge() => currentGauge;
}