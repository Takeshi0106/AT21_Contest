using UnityEngine;

// ==============================================================
// Boss�̏�ԊǗ��N���X�@���̓X�^����������܂���
// ==============================================================

public class BossStatusEffectManager : MonoBehaviour
{
    [Header("Boss�̃X�^���Q�[�W")]
    [SerializeField] private float m_StanGage = 100.0f; // �X�^����MAX�Q�[�W
    [Header("�X�^�����̃t���[����")]
    [SerializeField] private int m_StanFleams = 600; // �X�^���̃t���[�����i���ԁj
    [Header("�X�^�����̃A�j���[�V����")]
    [SerializeField] private AnimationClip m_StanAnimationClip = null; // �X�^���̃A�j���[�V����

    bool m_StanFlag = false; // �X�^���t���O
    float m_StanCntFleams = 0.0f; //�J�E���g�p�t���O

    // �_���[�W���󂯂����ɌĂԏ���
    public void Damage(float _GageDamage)
    {
        m_StanGage -= _GageDamage;

        if (m_StanGage < 0.0f)
        {
            m_StanFlag = true;
        }
    }

    // �X�^����Ԃ̊J�n����
    public void StartStan()
    {
        // �A�j���[�V������G�t�F�N�g�Ȃǂ��J�n����
    }

    // �X�^�����ɌĂԏ���
    public void UpdateStan(float enemyCnt)
    {
        m_StanCntFleams += enemyCnt; // �t���[���X�V

        if (m_StanCntFleams > m_StanFleams)
        {
            // ������
            m_StanFlag = false;
            m_StanCntFleams = 0.0f;
        }
    }

    // �Q�b�^�[
    public bool GetStanFlag() { return m_StanFlag; }
    public AnimationClip GetStanAnimationClip() { return m_StanAnimationClip; }
}
