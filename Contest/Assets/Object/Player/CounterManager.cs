using UnityEngine;

public class CounterManager : MonoBehaviour
{
    [Header("カウンターのランク数")]
    [SerializeField] private int counterMaxRank = 5;
    [Header("カウンターのゲージ量（ランクごと）")]
    [SerializeField] private float[] counterGauge = { 100.0f, 200.0f, 300.0f, 400.0f, 500.0f };
    [Header("カウンター成功時のゲージ増加量")]
    [SerializeField] private float counterSuccessGain = 20f;
    [Header("カウンターのダメージ倍率（ランクごと）")]
    [SerializeField] private float[] damageMultipliers = { 1.0f, 1.2f, 1.5f, 2.0f,5.0f };
    [Header("カウンターの持続フレーム（ランクごと）")]
    [SerializeField] private int[] counterFrames = { 30, 40, 50, 60, 70 };
    [Header("カウンター失敗時の硬直フレーム（ランクごと）")]
    [SerializeField] private int[] counterStaggerFrames = { 10, 10, 10, 10, 10 };
    [Header("カウンター成功時のフレーム（ランクごと）")]
    [SerializeField] private int[] counterSuccessFrames = { 10, 10, 10, 10, 10 };   

    // 最新のゲージを入れる
    private float currentGauge = 0f;
    // 最新のランクを入れる
    private int currentRank = 0;



    // ゲージ量を上げる処理
    public void IncreaseGauge()
    {
        currentGauge += counterSuccessGain;
        if (currentGauge >= counterGauge[currentRank])
        {
            RankUp();
            currentGauge = 0;
        }
    }



    // ランクアップ処理
    private void RankUp()
    {
        if (currentRank < counterMaxRank - 1)
        {
            currentRank++;
            Debug.Log($"カウンターランクアップ！現在のランク: {currentRank + 1}");
        }
    }


    // ゲッター
    public float GetDamageMultiplier() => damageMultipliers[(int)currentRank];
    public int GetCounterFrames() => counterFrames[(int)currentRank];
    public int GetCounterStaggerFrames() => counterStaggerFrames[(int)currentRank];
    public int GetCounterSuccessFrames() => counterSuccessFrames[(int)currentRank];
    public float GetCurrentGauge() => currentGauge;
}