using UnityEngine;

// ==============================================================
// Bossの状態管理クラス　今はスタンしかありません
// ==============================================================

public class BossStatusEffectManager : MonoBehaviour
{
    [Header("Bossのスタンゲージ")]
    [SerializeField] private float m_StanGage = 100.0f; // スタンのMAXゲージ
    [Header("スタン中のフレーム数")]
    [SerializeField] private int m_StanFleams = 600; // スタンのフレーム数（時間）

    bool m_StanFlag = false; // スタンフラグ
    float m_StanCntFleams = 0.0f; //カウント用フラグ

    // ダメージを受けた時に呼ぶ処理
    public bool Damage(float _GageDamage)
    {
        m_StanGage -= _GageDamage;

        if (m_StanGage < 0.0f)
        {
            m_StanFlag = true;
        }

        return m_StanFlag;
    }

    // スタン状態の開始処理
    public void StartStan()
    {
        // アニメーションやエフェクトなどを開始する
    }

    // スタン中に呼ぶ処理
    public void UpdateStan(float enemyCnt)
    {
        m_StanCntFleams += enemyCnt; // フレーム更新

        if (m_StanCntFleams > m_StanFleams)
        {
            // 初期化
            m_StanFlag = false;
            m_StanCntFleams = 0.0f;
        }
    }

    // ゲッター
    public bool GetStanFlag() { return m_StanFlag; }

}
