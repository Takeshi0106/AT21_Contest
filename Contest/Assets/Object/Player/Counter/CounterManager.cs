using UnityEngine;
using UnityEngine.Events;

public class CounterManager : MonoBehaviour
{
    [Header("�J�E���^�[�̃����N��")]
    [SerializeField] private int counterMaxRank = 5;
    [Header("�J�E���^�[�̃Q�[�W�ʁi�����N���Ɓj")]
    [SerializeField] private float[] counterGauge = { 100.0f, 200.0f, 300.0f, 400.0f, 500.0f };
    [Header("�J�E���^�[�������̃Q�[�W������")]
    [SerializeField] private float counterSuccessGain = 20f;
    [Header("�_���[�W�A�b�v�{���i�����N���Ɓj")]
    [SerializeField] private float[] damageMultipliers = { 1.0f, 1.2f, 1.5f, 2.0f, 5.0f };
    [Header("�J�E���^�[�̎�t�t���[���i�����N����)")]
    [SerializeField] private int[] counterFrames = { 30, 40, 50, 60, 70 };
    [Header("�J�E���^�[���s�d���t���[���i�����N���Ɓj")]
    [SerializeField] private int[] counterStaggerFrames = { 10, 10, 10, 10, 10 };
    [Header("�J�E���^�[�����t���[���i�����N���Ɓj")]
    [SerializeField] private int[] counterSuccessFrames = { 10, 10, 10, 10, 10 };
    [Header("�J�E���^�[���\����t���[���i�����N���Ɓj")]
    [SerializeField] private int[] counterStartupFrames = { 10, 10, 10, 10, 10 };
    [Header("�����N��������܂ł̃t���[��(�����N����)")]
    [SerializeField] private int[] rankDecayFrames = { 300, 300, 300, 300, 300 };
    [Header("�J�E���^�[���������烉���N�_�E���J�n�܂ł̖����t���[���i�����N���Ɓj")]
    [SerializeField] private int[] decayGraceFrames = { 120, 120, 120, 120, 120 };
    [Header("�J�E���^�[�������̍U���� ( �U���� �� �_���[�W�A�b�v�{�� )")]
    [SerializeField] private float[] counterDamages = { 20, 20, 20, 20, 20 };
    [Header("�J�E���^�[�������̃X�^���_���[�W��")]
    [SerializeField] private float[] m_CounterStanDamages = { 20, 20, 20, 20, 20 };
    [Header("�v���C���[�̃J�E���^�[������̖��G���ԁi�J�E���^�[�������̖��G���ԂƂ͕ʁj")]
    [SerializeField] private int[] invincibleTime = { 0, 0, 0, 0, 0 };

    [Header("�J�E���^�[�I�u�W�F�N�g")]
    [SerializeField] private Object m_CounterObj = null;
    [Header("�J�E���^�[�A�j���[�V����")]
    [SerializeField] private AnimationClip m_CounterAnimation = null;

    // �ŐV�̃Q�[�W������
    private float currentGauge = 0f;
    // �ŐV�̃����N������
    private int currentRank = 0;
    // �J�E���^�[��������t���[��������ϐ�
    private int downTime = 0;
    // �P�\�t���[��������ϐ�
    private int graceTimer = 0;
    // �P�\�t���[�����߂������̃X�C�b�`
    private bool isInGracePeriod = false;
    // �J�E���^�[�������̃C�x���g������
    private UnityEvent counterRankUpEvent = new UnityEvent();
    // �J�E���^�[���s���̃C�x���g������
    private UnityEvent counterRankDownEvent=new UnityEvent();



    // �Q�[�W�ʂ��グ�鏈��
    public void IncreaseGauge()
    {
        // �Q�[�W�v�Z
        currentGauge += counterSuccessGain;

        // �����N�A�b�v���������s����
        if (currentGauge >= counterGauge[currentRank])
        {
            RankUp();
            currentGauge = 0;
        }

        // �P�\���Ԃ����Z�b�g
        isInGracePeriod = true;
        graceTimer = 0;
        downTime = 0;
    }



    // �J�E���^�[�����N�������邩�̏���
    public void GaugeDecay()
    {
        // �J�E���^�[�������̗P�\����
        if (isInGracePeriod)
        {
            // �P�\�t���[�����X�V
            graceTimer++;

            // �P�\���I��������̏���
            if (graceTimer >= decayGraceFrames[currentRank])
            {
                isInGracePeriod = false;
                graceTimer = 0;
            }
            // �P�\���Ԓ��͏I���
            return;
        }

        // �����N�_�E���t���[���X�V
        downTime++;

        // �����N�_�E�������Ƃ��̏���
        if (downTime >= rankDecayFrames[currentRank])
        {
            RankDown();
            downTime = 0;
        }
    }



    // �����N�A�b�v����
    private void RankUp()
    {
        if (currentRank < counterMaxRank - 1)
        {
            // �����N�A�b�v
            currentRank++;
            // �����N�A�b�v�C�x���g���Ăяo��
            counterRankUpEvent.Invoke();

#if UNITY_EDITOR
            // �f�o�b�O�p�̃J�E���^�[�����N�\��
            Debug.Log($"�J�E���^�[�����N�A�b�v�I���݂̃����N: {currentRank + 1}");
#endif
        }
    }



    // �����N�_�E������
    private void RankDown()
    {
        if (currentRank > 0)
        {
            // �����N�_�E��
            currentRank--;
            // �����N�_�E���C�x���g���Ăяo��
            counterRankDownEvent.Invoke();

#if UNITY_EDITOR
            // �f�o�b�O�p�̃J���^�[�����N�\��
            Debug.Log($"�J�E���^�[�����N�_�E���I���݂̃����N: {currentRank + 1}");
#endif
        }
    }



    // �V�[���J�ڎ��ɌĂ΂��
    private void OnDestroy()
    {
        // �C�x���g���X�i�[������
        counterRankUpEvent.RemoveAllListeners();
        counterRankDownEvent.RemoveAllListeners();
        // �|�C���^�[������
        counterRankUpEvent = null;
        counterRankDownEvent = null;

#if UNITY_EDITOR
        // �f�o�b�O�p�̃J�E���^�[�����N�\��
        Debug.Log("CounterManager : OnDestroy�����s���܂����B");
#endif
    }



    // �Z�b�^�[
    public void SetCounterRankUpEvent(UnityAction action) { counterRankUpEvent.AddListener(action); }
    public void SetCounterRankDownEvent(UnityAction action) { counterRankDownEvent.AddListener(action); }


    // �Q�b�^�[
    public  float  GetDamageMultiplier()     { return damageMultipliers[(int)currentRank]; }
    public  int    GetCounterFrames()        { return counterFrames[(int)currentRank]; }
    public  int    GetCounterStaggerFrames() { return counterStaggerFrames[(int)currentRank]; }
    public  int    GetCounterSuccessFrames() { return counterSuccessFrames[(int)currentRank]; }
    public  int    GetCounterStartupFrames() { return counterStartupFrames[(int)currentRank]; }
    public int GetCounterInvincibleFreams() { return invincibleTime[(int)currentRank]; }
    public  float  GetCurrentGauge()         { return currentGauge; }
    public float GetCounterDamage() { return counterDamages[(int)currentRank]; }
    public float GetCounterStanDamage() { return m_CounterStanDamages[(int)currentRank]; }
    public int GetCurrentRank() { return currentRank; }
    public AnimationClip GetCounterAnimation() { return m_CounterAnimation; }
    public Object GetCounterObject() { return m_CounterObj; }
}