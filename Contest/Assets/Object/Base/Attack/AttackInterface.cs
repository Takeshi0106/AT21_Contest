using UnityEngine;

public class AttackInterface : MonoBehaviour
{
    // �U���͂���������
    float m_AttackDamage = 0.0f;
    // �X�^���͂���������
    float m_StanAttackDamage = 0.0f;
    // �U���t���O������
    int m_AttackID = 0;

    // �U��������������̏���
    public void HitAttack() 
    {
        m_AttackDamage = 0.0f;
        m_StanAttackDamage = 0.0f;
    }

    // �Z�b�^�[
    public void SetSelfAttackDamage(float _attackDamage) {  m_AttackDamage = _attackDamage; } // �U���͂��Z�b�g
    public void SetSelfStanAttackDamage(float _stanAttackDamage) { m_StanAttackDamage = _stanAttackDamage; } // �X�^���͂��Z�b�g
    public void SetSelfID() { m_AttackID = Time.frameCount; }
    
    // �Q�b�^�[
    public float GetOtherAttackDamage() { return m_AttackDamage; } // �U���͂��擾
    public float GetOtherStanAttackDamage() { return m_StanAttackDamage; } // �X�^���͂��擾
    public int GetOtherAttackID() { return m_AttackID; }
}
