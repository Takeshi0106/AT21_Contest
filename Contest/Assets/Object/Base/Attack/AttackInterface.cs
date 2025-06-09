using UnityEngine;

public class AttackInterface : MonoBehaviour
{
    // 攻撃力を持たせる
    float m_AttackDamage = 0.0f;
    // スタン力を持たせる
    float m_StanAttackDamage = 0.0f;

    // 攻撃が当たった後の処理
    public void HitAttack() 
    {
        m_AttackDamage = 0.0f;
        m_StanAttackDamage = 0.0f;
    }

    // セッター
    public void SetSelfAttackDamage(float _attackDamage) {  m_AttackDamage = _attackDamage; } // 攻撃力をセット
    public void SetSelfStanAttackDamage(float _stanAttackDamage) { m_StanAttackDamage = _stanAttackDamage; } // スタン力をセット
    
    // ゲッター
    public float GetOtherAttackDamage() { return m_AttackDamage; } // 攻撃力を取得
    public float GetOtherStanAttackDamage() { return m_StanAttackDamage; } // スタン力を取得
}
