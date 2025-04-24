using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HPManager : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField] private float maxHP = 100;
    [Header("現在のHP（デバッグ用）")]
    [SerializeField] private float firstHP;
    [Header("死亡時に呼ばれるイベント")]
    public UnityEvent onDeath;
    [Header("ダメージを受けたときに呼ばれるイベント")]
    public UnityEvent onDamaged;

    // 現在のHP
    public float currentHP;


    private void Awake()
    {
        // HPを初期化
        currentHP = maxHP;

#if UNITY_EDITOR
        // デバッグ用
        currentHP = firstHP;
#endif
    }



    /// HPにダメージを与える
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // HPイベントを呼ぶ
        onDamaged.Invoke();

        // 死亡イベントを呼ぶ
        if (currentHP <= 0)
        {
            onDeath.Invoke();
        }
    }



    /// HPを回復する
    public void Heal(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }



    /// 死亡時処理
    private void Die()
    {
        Debug.Log($"{gameObject.name} が死亡しました");
        onDeath.Invoke();
    }

    // ==========================
    // ゲッター
    // ==========================
    public float GetCurrentHP() { return currentHP; }
    public float GetMaxHP() { return maxHP; }

    // ==========================
    // セッター（オプション）
    // ==========================
    public void SetMaxHP(float value)
    {
        maxHP = value;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }
}
