using UnityEngine;
using UnityEngine.Events;

public class CounterManager : MonoBehaviour
{
    [Header("カウンターのランク数")]
    [SerializeField] private int counterMaxRank = 5;
    [Header("カウンターのゲージ量（ランクごと）")]
    [SerializeField] private float[] counterGauge = { 100.0f, 200.0f, 300.0f, 400.0f, 500.0f };
    [Header("カウンター成功時のゲージ増加量")]
    [SerializeField] private float counterSuccessGain = 20f;
    [Header("ダメージアップ倍率（ランクごと）")]
    [SerializeField] private float[] damageMultipliers = { 1.0f, 1.2f, 1.5f, 2.0f, 5.0f };
    [Header("カウンターの受付フレーム（ランクごと)")]
    [SerializeField] private int[] counterFrames = { 30, 40, 50, 60, 70 };
    [Header("カウンター失敗硬直フレーム（ランクごと）")]
    [SerializeField] private int[] counterStaggerFrames = { 10, 10, 10, 10, 10 };
    [Header("カウンター成功フレーム（ランクごと）")]
    [SerializeField] private int[] counterSuccessFrames = { 10, 10, 10, 10, 10 };
    [Header("カウンターを構えるフレーム（ランクごと）")]
    [SerializeField] private int[] counterStartupFrames = { 10, 10, 10, 10, 10 };
    [Header("ランクが落ちるまでのフレーム(ランクごと)")]
    [SerializeField] private int[] rankDecayFrames = { 300, 300, 300, 300, 300 };
    [Header("カウンター成功時からランクダウン開始までの無効フレーム（ランクごと）")]
    [SerializeField] private int[] decayGraceFrames = { 120, 120, 120, 120, 120 };
    [Header("カウンター成功時の攻撃力 ( 攻撃力 ＊ ダメージアップ倍率 )")]
    [SerializeField] private float[] counterDamages = { 20, 20, 20, 20, 20 };
    [Header("カウンター成功時のスタンダメージ量")]
    [SerializeField] private float[] m_CounterStanDamages = { 20, 20, 20, 20, 20 };
    [Header("プレイヤーのカウンター成功後の無敵時間（カウンター成功中の無敵時間とは別）")]
    [SerializeField] private int[] invincibleTime = { 0, 0, 0, 0, 0 };

    [Header("カウンターオブジェクト")]
    [SerializeField] private Object m_CounterObj = null;
    [Header("カウンターアニメーション")]
    [SerializeField] private AnimationClip m_CounterAnimation = null;

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
    // カウンター成功時のイベントを入れる
    private UnityEvent counterRankUpEvent = new UnityEvent();
    // カウンター失敗時のイベントを入れる
    private UnityEvent counterRankDownEvent=new UnityEvent();



    // ゲージ量を上げる処理
    public void IncreaseGauge()
    {
        // ゲージ計算
        currentGauge += counterSuccessGain;

        // ランクアップ処理を実行する
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
        // カウンター成功時の猶予処理
        if (isInGracePeriod)
        {
            // 猶予フレームを更新
            graceTimer++;

            // 猶予が終わった時の処理
            if (graceTimer >= decayGraceFrames[currentRank])
            {
                isInGracePeriod = false;
                graceTimer = 0;
            }
            // 猶予期間中は終わる
            return;
        }

        // ランクダウンフレーム更新
        downTime++;

        // ランクダウンしたときの処理
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
            // ランクアップ
            currentRank++;
            // ランクアップイベントを呼び出す
            counterRankUpEvent.Invoke();

#if UNITY_EDITOR
            // デバッグ用のカウンターランク表示
            Debug.Log($"カウンターランクアップ！現在のランク: {currentRank + 1}");
#endif
        }
    }



    // ランクダウン処理
    private void RankDown()
    {
        if (currentRank > 0)
        {
            // ランクダウン
            currentRank--;
            // ランクダウンイベントを呼び出す
            counterRankDownEvent.Invoke();

#if UNITY_EDITOR
            // デバッグ用のカンターランク表示
            Debug.Log($"カウンターランクダウン！現在のランク: {currentRank + 1}");
#endif
        }
    }



    // シーン遷移時に呼ばれる
    private void OnDestroy()
    {
        // イベントリスナーを解除
        counterRankUpEvent.RemoveAllListeners();
        counterRankDownEvent.RemoveAllListeners();
        // ポインター初期化
        counterRankUpEvent = null;
        counterRankDownEvent = null;

#if UNITY_EDITOR
        // デバッグ用のカウンターランク表示
        Debug.Log("CounterManager : OnDestroyを実行しました。");
#endif
    }



    // セッター
    public void SetCounterRankUpEvent(UnityAction action) { counterRankUpEvent.AddListener(action); }
    public void SetCounterRankDownEvent(UnityAction action) { counterRankDownEvent.AddListener(action); }


    // ゲッター
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