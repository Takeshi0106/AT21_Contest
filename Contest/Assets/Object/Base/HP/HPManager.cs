using UnityEngine;
using UnityEngine.Events;

public class HPManager : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField] private float maxHP = 100;
    [Header("現在のHP（デバッグ用）")]
    [SerializeField] private float firstHP;

    private UnityEvent onDeath = new UnityEvent();
    private UnityEvent onDamaged = new UnityEvent();

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
            // 死亡関数を呼び出す
            onDeath.Invoke();
        }
    }



    /// HPを回復する
    public void Heal(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
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
    public void SetOnDeathEvent(UnityAction action)
    {
        Debug.Log("ダメージ処理をセット");
        onDeath.AddListener(action);
    }

    public void SetOnDamagedEvent(UnityAction action)
    {
        Debug.Log("死亡処理をセット");
        onDamaged.AddListener(action);
    }
}
