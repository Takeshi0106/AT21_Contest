using UnityEngine;
using UnityEngine.Events;

public class HPManager : MonoBehaviour
{
    [Header("�ő�HP")]
    [SerializeField] private float maxHP = 100;
    [Header("���݂�HP�i�f�o�b�O�p�j")]
    [SerializeField] private float firstHP;

    private UnityEvent onDeath = new UnityEvent();
    private UnityEvent onDamaged = new UnityEvent();

    // ���݂�HP
    public float currentHP;


    private void Awake()
    {
        // HP��������
        currentHP = maxHP;

#if UNITY_EDITOR
        // �f�o�b�O�p
        currentHP = firstHP;
#endif
    }



    /// HP�Ƀ_���[�W��^����
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // HP�C�x���g���Ă�
        onDamaged.Invoke();

        // ���S�C�x���g���Ă�
        if (currentHP <= 0)
        {
            // ���S�֐����Ăяo��
            onDeath.Invoke();
        }
    }



    /// HP���񕜂���
    public void Heal(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }


    // ==========================
    // �Q�b�^�[
    // ==========================
    public float GetCurrentHP() { return currentHP; }
    public float GetMaxHP() { return maxHP; }

    // ==========================
    // �Z�b�^�[�i�I�v�V�����j
    // ==========================
    public void SetMaxHP(float value)
    {
        maxHP = value;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }
    public void SetOnDeathEvent(UnityAction action)
    {
        Debug.Log("�_���[�W�������Z�b�g");
        onDeath.AddListener(action);
    }

    public void SetOnDamagedEvent(UnityAction action)
    {
        Debug.Log("���S�������Z�b�g");
        onDamaged.AddListener(action);
    }
}
