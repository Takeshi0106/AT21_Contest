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
    [Header("カウンターの持続フレーム（ランクごと/準備フレームとは別）")]
    [SerializeField] private int[] counterFrames = { 30, 40, 50, 60, 70 };
    [Header("カウンター失敗時の硬直フレーム（ランクごと）")]
    [SerializeField] private int[] counterStaggerFrames = { 10, 10, 10, 10, 10 };
    [Header("カウンター成功時のフレーム（ランクごと）")]
    [SerializeField] private int[] counterSuccessFrames = { 10, 10, 10, 10, 10 };
    [Header("カウンター準備のフレーム（ランクごと）")]
    [SerializeField] private int[] counterStartupFrames = { 10, 10, 10, 10, 10 };
    [Header("ランクが落ちるまでのフレーム数(ランクごと)")]
    [SerializeField] private int[] rankDecayFrames = { 300, 300, 300, 300, 300 };
    [Header("成功時に減衰が無効になる猶予フレーム（ランクごと）")]
    [SerializeField] private int[] decayGraceFrames = { 120, 120, 120, 120, 120 };

    // 最新のゲージを入れる
    private float currentGauge = 0f;
    // 最新のランクを入れる
    private int currentRank = 0;
    // カウンターが落ちるフレームを入れる変数
    private int downTime = 0;
    // 猶予フレームを入れる変数
    private int graceTimer = 0;
    // 猶予フレームが過ぎたかのスイッチ
    private bool isInGracePeriod = false;


    // ゲージ量を上げる処理
    public void IncreaseGauge()
    {
        // ゲージ計算
        currentGauge += counterSuccessGain;

        if (currentGauge >= counterGauge[currentRank])
        {
            RankUp();
            currentGauge = 0;
        }

        // 猶予期間をリセット
        isInGracePeriod = true;
        graceTimer = 0;
        downTime = 0;
    }



    // カウンターランクが落ちるかの処理
    public void GaugeDecay()
    {
        if (isInGracePeriod)
        {
            graceTimer++;

            if (graceTimer >= decayGraceFrames[currentRank])
            {
                isInGracePeriod = false;
                graceTimer = 0;
            }
            // 猶予期間中は減衰タイマーを進めない
            return;
        }

        downTime++;

        if (downTime >= rankDecayFrames[currentRank])
        {
            RankDown();
            downTime = 0;
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



    // ランクダウン処理
    private void RankDown()
    {
        if (currentRank > 0)
        {
            currentRank--;
            Debug.Log($"カウンターランクダウン！現在のランク: {currentRank + 1}");
        }
    }



    // ゲッター
    public float GetDamageMultiplier() => damageMultipliers[(int)currentRank];
    public int GetCounterFrames() => counterFrames[(int)currentRank];
    public int GetCounterStaggerFrames() => counterStaggerFrames[(int)currentRank];
    public int GetCounterSuccessFrames() => counterSuccessFrames[(int)currentRank];
    public int GetCounterStartupFrames() => counterStartupFrames[(int)currentRank];
    public float GetCurrentGauge() => currentGauge;
}